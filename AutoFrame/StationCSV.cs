using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using AutoFrameDll;
using CommonTool;
using ToolEx;
using Communicate;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
using System.Web.Script.Serialization;
using RestSharp;
using System.Xml;
using System.Diagnostics;

namespace AutoFrame
{

    class StationCSV : StationEx
    {
        public struct DataMatch
        {
            public int index;
            public string name;
        }

        /// <summary>
        /// 处理PLC写入CSV的数据状态标志位（TRUE：可以处理下一组数据   FALSE：正在处理数据）
        /// </summary>
        public Dictionary<DataStyle, bool> threadIsFinish { get; set; }

        private Thread threadPlcErrorData;
        private Thread threadPlcMachineData;
        private Thread threadPlcMachineState;
        private Thread threadUploadErrorData;
        private Thread threadUploadPlcMachineData;
        private Thread threadUploadPlcMachineState;

        //20210728 添加服务器，用于PLC的心跳检测
        /// <summary>
        /// 端口号 2000  IP：127.0.0.1
        /// </summary>
        private AsyncSocketTCPServer m_plcServer;

        private static List<DataMatch> L_serials = new List<DataMatch>();
        private static List<DataMatch> L_wip_serials = new List<DataMatch>();
        private static List<DataMatch> L_data = new List<DataMatch>();
        private static DataMatch D_Tossing = new DataMatch();
        private static int n_input_time = 999;
        private static int n_output_time = 999;
        private static int n_pass = 999;
        private static int n_Tossing_reason = 999;

        private static int PLC2PC_DataLength = 0;
        string strPath;

        private DateTime Form_MessageBeginTime = DateTime.Now;
        private bool Form_MessageFirst = true;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationCSV(string strName) : base(strName)
        {
            //配置站位界面显示输入
            io_in = new string[] { };
            //配置站位界面显示输出
            io_out = new string[] { };
            //配置站位界面显示气缸
            m_cylinders = new string[] { };
            InitFlag_ThreadIsFinish();
            strPath = SystemMgr.GetInstance().GetParamString("PLCDir") + "\\" + this.Name + "\\";

            //20210728 添加服务器，用于PLC的心跳检测
            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";
            if (File.Exists(cfg))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);

                TcpServerMgr.GetInstance().ReadCfgFromXml(doc);
            }

            m_plcServer = TcpServerMgr.GetInstance().GetTcpServer(3);
            m_plcServer.DataReceived += M_tcpServer_DataReceived_m_plcServer;
        }

        /// <summary>
        /// PLC发送2\r\n,我们回复1\r\n
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_tcpServer_DataReceived_m_plcServer(object sender, AsyncSocketEventArgs e)
        {
            string strData;
            strData = Encoding.Default.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
            //e.m_state.ClientSocket.SendTo(Encoding.UTF8.GetBytes("1\r\n"), e.m_state.ClientSocket.RemoteEndPoint);
            string[] strSlits = strData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            ShowLog(e.m_state + "PLC 心跳检测数据:" + strData);

            if (StationMgr.GetInstance().IsAutoRunning() && (!StationMgr.GetInstance().IsPause()) && strData == "2\r\n")
            {
                e.m_state.ClientSocket.SendTo(Encoding.UTF8.GetBytes("1\r\n"), e.m_state.ClientSocket.RemoteEndPoint);
                ShowLog(e.m_state + "回复PLC心跳检测数据:" + "1");
            }
        }

        private void InitFlag_ThreadIsFinish()
        {
            if (threadIsFinish == null)
            {
                threadIsFinish = new Dictionary<DataStyle, bool>();
                threadIsFinish.Add(DataStyle.ErrorData, true);
                threadIsFinish.Add(DataStyle.MachineData, true);
                threadIsFinish.Add(DataStyle.MachineState, true);
            }
        }

        /// <summary>
        /// 站位初始化，用来添加伺服上电，打开网口，站位回原点等动作
        /// </summary>
        public override void StationInit()
        {
            HiveMgr.GetInstance().CloseUpload();
            n_input_time = 999;
            n_output_time = 999;
            n_pass = 999;
            n_Tossing_reason = 999;
            PLC2PC_DataLength = 0;
            D_Tossing = new DataMatch();

            ReadConfigXML();

            #region EnableUploadJson
            //if (SystemMgr.GetInstance().GetParamBool("EnableUploadJson"))
            {
                threadUploadErrorData = new Thread(() => postJsonFile(DataStyle.ErrorData));
                threadUploadErrorData.IsBackground = true;
                threadUploadErrorData.Start();

                threadUploadPlcMachineData = new Thread(() => postJsonFile(DataStyle.MachineData));
                threadUploadPlcMachineData.IsBackground = true;
                threadUploadPlcMachineData.Start();


                threadUploadPlcMachineState = new Thread(() => postJsonFile(DataStyle.MachineState));
                threadUploadPlcMachineState.IsBackground = true;
                threadUploadPlcMachineState.Start();
            }
            #endregion

            #region 置位处理三种数据CSV文件标志位
            //20210825
            threadIsFinish[DataStyle.ErrorData] = true;
            threadIsFinish[DataStyle.MachineData] = true;
            threadIsFinish[DataStyle.MachineState] = true;
            #endregion 置位处理三种数据CSV文件标志位

            ShowLog("初始化完成");

            //调用post方法
            //HiveMgr.GetInstance().StartUpload();
        }
        /// <summary>
        /// 站位退出退程时调用，用来关闭伺服，关闭网口等动作
        /// </summary>
        public override void StationDeinit()
        {
            try
            {
                #region 置位处理三种数据CSV文件标志位
                threadIsFinish[DataStyle.ErrorData] = true;
                threadIsFinish[DataStyle.MachineData] = true;
                threadIsFinish[DataStyle.MachineState] = true;
                #endregion 置位处理三种数据CSV文件标志位

                threadPlcMachineData.Abort();
                threadPlcMachineState.Abort();
                threadPlcErrorData.Abort();
                threadUploadErrorData.Abort();
                threadUploadPlcMachineData.Abort();
                threadUploadPlcMachineState.Abort();
            }
            catch
            {

            }
        }

        //当所有站位均为全自动运行模式时，不需要重载该函数
        //当所有站位为半自动运行模式时，也不需要重载该函数， 只需要在站位流程开始时插入WaitBegin()即可保证所有站位同步开始。
        //当所有站位中，有的为半自动，有的为全自动时，半自动的站位不重载该函数，使用WaitBegin()控制同步，全自动的站位重载该函数返回true即可。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public override bool IsReady()
        //{
        //    return true;
        //}


        /// <summary>
        /// 初始化时设置站位安全状态，用来设备初始化时各站位之间相互卡站，
        /// 控制各站位先后动作顺序用，比如流水线组装，肯定需要组装的Z轴升
        /// 起后，流水线才能动作这种情况
        /// </summary>
        public override void InitSecurityState()
        {

        }


        /// <summary>
        /// 正常运行
        /// </summary>
        protected override void NormalRun()
        {
            CheckContinue();//检查系统是否会继续运行

            #region EnableHive
            if (SystemMgr.GetInstance().GetParamBool("EnableHive"))
            {
                #region ErrorData
                if (threadIsFinish[DataStyle.ErrorData])
                {
                    threadIsFinish[DataStyle.ErrorData] = false;
                    threadPlcErrorData = new Thread(() => PlcToHive(DataStyle.ErrorData));
                    threadPlcErrorData.IsBackground = true;
                    threadPlcErrorData.Start();
                }
                #endregion

                #region MachineData
                if (threadIsFinish[DataStyle.MachineData])
                {
                    threadIsFinish[DataStyle.MachineData] = false;
                    threadPlcMachineData = new Thread(() => PlcToHive(DataStyle.MachineData));
                    threadPlcMachineData.IsBackground = true;
                    threadPlcMachineData.Start();
                }
                #endregion

                #region MachineState
                if (threadIsFinish[DataStyle.MachineState])
                {
                    threadIsFinish[DataStyle.MachineState] = false;
                    threadPlcMachineState = new Thread(() => PlcToHive(DataStyle.MachineState));
                    threadPlcMachineState.IsBackground = true;
                    threadPlcMachineState.Start();
                }
                #endregion
            }
            #endregion

            Thread.Sleep(50);

        }
        /// <summary>
        /// 获取当前文件夹下所有文件的地址
        /// </summary>
        /// <param name="errordata_path">文件夹路径</param>
        /// <returns>当前文件夹下所有的文件路径</returns>
        private List<string> GetAllFiles(string path)
        {

            List<string> list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] file = dir.GetFiles();//获取当前路径下的所有文件
            file.OrderBy(n => n.CreationTime);
            foreach (FileInfo f in file)
            {
                list.Add(f.FullName);//添加文件的路径到列表
            }
            return list;
        }

        /// <summary>
        /// 根据名称获取地址
        /// </summary>
        /// <param name="name">plc名称</param>
        /// <returns></returns>
        private string GetPlcDir(string name)
        {

            return "";
        }

        /// <summary>
        /// 空跑
        /// </summary>
        protected override void DryRun()
        {
            NormalRun();
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
        }


        /// <summary>
        /// 根据datastyle（数据类型）获取路径
        /// </summary>
        /// <param name="index">1：ErrorData  2：MachineData  3：MachineState 4:EnvironmentData</param>
        /// <param name="parentDir">根目录</param>
        /// <returns>当前数据类型的文件夹路径</returns>
        private string CreateFilePath(DataStyle index, string parentDir)
        {
            string directory = parentDir;

            if (directory.LastIndexOf("\\") != directory.Length - 1)
                directory += "\\";

            switch (index)
            {
                case DataStyle.ErrorData:
                    directory += "errordata\\";
                    break;
                case DataStyle.MachineData:
                    directory += "machinedata\\";
                    break;
                case DataStyle.MachineState:
                    directory += "machinestate\\";
                    break;
                default:
                    directory += "environmentdata\\";
                    break;
            }
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }


        private void PlcToHive(DataStyle DS)
        {
            try
            {
                string parentDir = strPath;
                string path = CreateFilePath(DS, parentDir + "plcdata");
                string errorpath = CreateFilePath(DS, parentDir + "backup");
                //List<string> fildlist = GetAllFiles(path);
                string[] strFile = Directory.GetFiles(path, "*.csv");

                //20210727 如果没有文件，没必要执行下面代码
                if (strFile.Length == 0)
                {
                    Thread.Sleep(500);
                    threadIsFinish[DS] = true;
                    return;
                }

                #region 发现文件，开始遍历
                foreach (string n in strFile) //遍历文件路径
                {
                    DateTime t = File.GetCreationTime(n);
                    bool bIsOverTime = HiveMgr.SendDataState.Instance.IsOverBindingTime(t, 24 * 3 - 0.1);//20210825 判断一下是否是超过3天的数据，加入0.1h的浮动误差。
                    if (bIsOverTime)
                    {
                        MoveFile(n, n.Replace("plcdata", "backup"));
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},文件时间超过三天,已移到backup中");
                        continue;
                    }
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},文件时间未超过三天");

                    FileInfo fi = new FileInfo(n);
                    if (!fi.Exists) //判断文件是否存在
                        continue;
                    Thread.Sleep(200);
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"开始读取文件{n}");
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(n, Encoding.Default);//读取文件
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        sr = null;
                        try
                        {
                            sr = new StreamReader(n, Encoding.Default);//读取文件
                        }
                        catch (Exception ex)
                        {
                            //20210727 记录因为读取文件失败次数
                            ShowLog($"{n} 读取失败,{ex}", LogLevel.Warn);
                            threadIsFinish[DS] = true;

                            if (!MoveFile(n, n.Replace("plcdata", "backup")))
                            {
                                File.Copy(n, n.Replace("plcdata", "backup"));
                                File.Delete(n);
                                MessageBox.Show($"{n}被占用，请解除占用文件");
                            }
                            else
                            {
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},该文件连续被占用,已移到backup中");
                            }
                            return;
                        }
                    }

                    string line = sr.ReadLine();
                    sr.Close();
                    if (string.IsNullOrEmpty(line))
                    {
                        //如果文件是空文件，添加日志
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空，请检查");
                        SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空，请检查:\r\n");
                        MoveFile(n, n.Replace("plcdata", "backup"));
                        continue;
                    }
                    line = line.Replace("\"", "");
                    string[] data = line.Split(',');//读取的内容拆分转为数组

                    string Message = "";
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"开始获取{DS}的文件格式");

                    switch (DS)
                    {
                        case DataStyle.MachineData:
                            if (!MachineDataMessage(data, out Message))
                            {
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件数据异常，需要重新匹配，请检查");
                                MoveFile(n, n.Replace("plcdata", "backup"));
                                continue;
                            }

                            break;
                        case DataStyle.MachineState:
                            if (!MachineStateMessage(data, out Message))
                            {
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件数据异常，需要重新匹配，请检查");
                                MoveFile(n, n.Replace("plcdata", "backup"));
                                continue;
                            }
                            break;
                        case DataStyle.ErrorData:
                            if (!ErrorDataMessage(data, out Message))
                            {
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件数据异常，需要重新匹配，请检查");
                                MoveFile(n, n.Replace("plcdata", "backup"));
                                continue;
                            }

                            break;
                        default:
                            break;
                    }


                    try
                    {
                        Message = HiveMgr.GetInstance().ConvertJsonString(Message);
                    }
                    catch
                    {
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "格式化失败,将移动到backup文件夹");
                        if (!MoveFile(n, n.Replace("plcdata", "backup")))
                        {
                            File.Copy(n, n.Replace("plcdata", "backup"));
                            File.Delete(n);
                        }
                        threadIsFinish[DS] = true;
                        return;
                    }

                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "格式化成功");

                    //检测是否是标准json
                    bool isJson = CheckIsJson(Message);
                    if (!isJson)
                    {
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"当前字符串不是一个Json格式，请检查{n},Message:{Message}");
                        SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"当前字符串不是一个Json格式，请检查{n},Message:{Message}\r\n");

                        if (!MoveFile(n, n.Replace("plcdata", "backup")))
                        {
                            File.Copy(n, n.Replace("plcdata", "backup"));
                            File.Delete(n);
                        }

                        continue;
                    }
                    //获取保存路径
                    string strFileSavePath = CreateFilePath(DS, parentDir + "uploadfiles\\") + System.Guid.NewGuid().ToString() + ".json";
                    if (!File.Exists(strFileSavePath))
                    {  //路径不存在，新建路径并关闭
                        using (new FileStream(strFileSavePath, FileMode.Create, FileAccess.ReadWrite)) { }
                    }
                    //指定路径新建并保存文件
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"指定路径新建并保存文件");
                    try
                    {
                        File.AppendAllText(strFileSavePath, Message);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        try
                        {
                            File.AppendAllText(strFileSavePath, Message);
                        }
                        catch
                        {
                            ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "保存Json文本失败,将移动到backup文件夹");
                            if (!MoveFile(n, n.Replace("plcdata", "backup")))
                            {
                                File.Copy(n, n.Replace("plcdata", "backup"));
                                File.Delete(n);
                            }
                            threadIsFinish[DS] = true;
                            return;
                        }

                    }
                    //ok，将该文件删除
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"保存成功，将该文件{n}删除");
                    File.Delete(n);
                }
                #endregion

                threadIsFinish[DS] = true;
            }
            catch (Exception ex)
            {
                //20210825 退出plcToHive()时，需要把threadIsFinish标志位置true
                threadIsFinish[DS] = true;
                ShowLog($"PlcToHive 异常:{ex.ToString()}", LogLevel.Warn);
            }

            Thread.Sleep(10);
        }


        public bool ErrorDataMessage(string[] strSlits, out string message)
        {
            message = null;
            ErrorMessage HiveMsg = new ErrorMessage();

            if (strSlits.Length != 5)
            {
                ShowLog("errordata 收到的数据异常", LogLevel.Warn);
                return false;
            }
            else
            {
                try
                {
                    HiveMsg.strMsgEnglish = strSlits[0];
                    HiveMsg.code = strSlits[1];
                    HiveMsg.Level = strSlits[2];
                    HiveMsg.tm_begin = Convert.ToDateTime(strSlits[3]);
                    HiveMsg.tm_end = Convert.ToDateTime(strSlits[4]);

                    if (((HiveMsg.tm_end - HiveMsg.tm_begin).TotalMilliseconds <= 0) || ((DateTime.Now - HiveMsg.tm_end).TotalHours > 71) || ((DateTime.Now - HiveMsg.tm_begin).TotalHours > 71))
                    {
                        ShowLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"errordata 收到的数据时间异常\r\n");
                        return false;
                    }

                    HiveMgr.GetInstance().UploadInformation(HiveMsg, out message, false);
                    return true;
                }
                catch (Exception ex)
                {
                    ShowLog($"errordata异常：{ex}", LogLevel.Warn);
                    return false;
                }
            }





        }

        public bool MachineStateMessage(string[] strSlits, out string message)
        {
            message = null;
            if (!((strSlits.Length == 4) || (strSlits.Length == 7)))
            {
                ShowLog("machinestate 收到的数据异常", LogLevel.Warn);
                return false;
            }
            else
            {
                try
                {
                    if ((DateTime.Now - Convert.ToDateTime(strSlits[1])).TotalHours > 71)
                    {
                        ShowLog($"machinestate 异常：{strSlits[1]}时效大于3天", LogLevel.Warn);
                        return false;
                    }

                    if (strSlits.Length == 4)
                    {
                        if (strSlits[2] == strSlits[3])
                        {
                            HiveMgr.ChangeState(new StateMessage() { MachineState = Convert.ToInt16(strSlits[0]) }, out message, false, strSlits[1]);
                        }
                        else
                        {
                            StateMessage state = new StateMessage();
                            state.MachineState = Convert.ToInt16(strSlits[0]);
                            state.code = strSlits[2];
                            state.strMsg = strSlits[3];
                            HiveMgr.ChangeState(state, out message, true, strSlits[1]);
                        }
                    }
                    else if (strSlits.Length == 7)
                    {
                        if (strSlits[2] == strSlits[3])
                        {
                            HiveMgr.ChangeState(new StateMessage() { MachineState = Convert.ToInt16(strSlits[0]) }, out message, false, strSlits[1], Convert.ToInt16(strSlits[4]), strSlits[5], strSlits[6]);
                        }
                        else
                        {
                            StateMessage state = new StateMessage();
                            state.MachineState = Convert.ToInt16(strSlits[0]);
                            state.code = strSlits[2];
                            state.strMsg = strSlits[3];
                            HiveMgr.ChangeState(state, out message, false, strSlits[1], Convert.ToInt16(strSlits[4]), strSlits[5], strSlits[6]);
                        }
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    ShowLog($"machinestate 异常：{ex}", LogLevel.Warn);
                    return false;
                }
            }
        }

        public bool MachineDataMessage(string[] strSlits, out string message)
        {
            message = null;
            if (strSlits.Length != PLC2PC_DataLength)
            {
                ShowLog("machinedata 收到的数据异常", LogLevel.Warn);
                return false;
            }
            else
            {
                try
                {
                    message = MachineData(strSlits);
                    return true;
                }
                catch (Exception ex)
                {
                    ShowLog($"machinestate 异常：{ex}", LogLevel.Warn);
                    return false;
                }
            }
        }




        private string GetUrl(DataStyle DS)
        {
            string url = "";
            switch (DS)
            {
                case DataStyle.ErrorData:
                    url = SystemMgr.GetInstance().GetParamString("ErrorDataUrl");
                    break;
                case DataStyle.MachineData:
                    url = SystemMgr.GetInstance().GetParamString("MachineDataUrl");
                    break;
                case DataStyle.MachineState:
                    url = SystemMgr.GetInstance().GetParamString("MachineStateUrl");
                    break;
                default:
                    break;
            }
            return url;
        }

        public bool MoveFile(string sourceFileName, string destFileName)
        {
            try
            {
                //缺少数据，需要重新匹配                    
                File.Move(sourceFileName, destFileName);
                return true;
            }
            catch
            {
                //20210727 无法移动的文件，进行删除操作                
                ShowLog($"{sourceFileName}无法被移动", LogLevel.Warn);
                return false;
            }
        }

        private bool SavePlcHiveLog(string Message)
        {

            try
            {
                string strSavePath = strPath + "\\" + "plchivelog";
                string strFileSavePath = "";
                DateTime TimeNow = DateTime.Now;
                if (!Directory.Exists(strSavePath))
                    Directory.CreateDirectory(strSavePath);

                strFileSavePath = strSavePath + "\\" + TimeNow.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(strFileSavePath))
                {
                    FileStream fs1 = new FileStream(strFileSavePath, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                try
                {
                    File.AppendAllText(strFileSavePath, Message);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }


        private bool CheckIsJson(string message)
        {
            try
            {
                JObject o = JObject.Parse(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Json时间有效性防呆
        /// </summary>
        /// <param name="DS"></param>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        private bool JSONTime(DataStyle DS, string jsonFileName)
        {
            try
            {
                string content = jsonFileName.ReadJsonContentFromFile(); //(post json)读取一个文件。
                JObject jo = JObject.Parse(content);
                switch (DS)
                {
                    case DataStyle.MachineData:
                        if (((DateTime.Now - Convert.ToDateTime(jo["output_time"].ToString())).TotalHours > 71) || ((DateTime.Now - Convert.ToDateTime(jo["input_time"].ToString())).TotalHours > 71))
                        {
                            return false;
                        }
                        break;
                    case DataStyle.MachineState:                       
                        if ((DateTime.Now - Convert.ToDateTime(jo["state_change_time"].ToString())).TotalHours > 71)
                        {
                            return false;
                        }
                        break;
                    case DataStyle.ErrorData:
                        if ((DateTime.Now - Convert.ToDateTime(jo["occurrence_time"].ToString())).TotalHours > 71)
                        {
                            return false;
                        }
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }            
        }



        /// <summary>
        /// 捞取Json文件，并上传
        /// </summary>
        /// <param name="DS"></param>
        private void postJsonFile(DataStyle DS)
        {
            string directory = "";
            var _SendDataState = HiveMgr.SendDataState.Instance;
            while (true)
            {
                directory = CreateFilePath(DS, strPath + "uploadfiles");
                Thread.Sleep(50);

                string[] files = Directory.GetFiles(directory);
                //20210727 空文件不执行后续代码
                if (files.Length == 0)
                {
                    Thread.Sleep(500);
                    continue;
                }

                #region 正常上传，按时间排序
                Dictionary<string, DateTime> DI = new Dictionary<string, DateTime>();
                foreach (string temp in files)
                {
                    DateTime t = File.GetCreationTime(temp);
                    if (_SendDataState.IsOverBindingTime(t, 24 * 3 - 0.5)|| !JSONTime(DS, temp))
                    {
                        string DF = SystemMgr.GetInstance().GetParamString("PLCDir");
                        DF = DF + "\\backup\\" + DS.ToString() + "\\";

                        if (!Directory.Exists(DF))
                        {
                            Directory.CreateDirectory(DF);

                        }
                        MoveFile(temp, temp.Replace("uploadfiles", "backup"));
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{DS}文件超过三天，已移到backup {DS}中");
                        //File.Move(temp, temp.Replace("uploadfiles", "backup"));
                        continue;
                    }
                    else
                    {
                        DI.Add(temp, t);
                    }

                }
                var disSort = from objDic in DI orderby objDic.Value select new { objDic.Key };
                files = disSort.Select(n => n.Key).ToArray();
                #endregion 正常上传，按时间排序

                List<string> jsonFiles = new List<string>();
                foreach (string fileName in files)
                {
                    if (Path.GetExtension(fileName).Equals(".json"))
                    {
                        jsonFiles.Add(fileName);
                    }
                }
                bool result = false;
                foreach (string jsonFileName in jsonFiles)
                {
                    if (!_SendDataState.IsCanSend(DS))
                    {
                        Thread.Sleep(1000);

                        break;
                    }

                    //string content = File.ReadAllText(jsonFileName);
                    Thread.Sleep(100);
                    string content = jsonFileName.ReadJsonContentFromFile();
                    if (string.IsNullOrEmpty(content))//若未读取到内容，删除当前文档
                    {
                        File.Delete(jsonFileName);
                        continue;//删除当前文档；
                    }
                    string responseString = "";
                    result = false;
                    //post
                    result = this.Post(GetUrl(DS), content, ref responseString);

                    if (!result)
                    {
                        if (Form_MessageFirst)
                        {
                            Form_MessageFirst = false;
                            Form_MessageBeginTime = DateTime.Now;
                            Form_HiveMessage frm = new Form_HiveMessage("Hive数据上传失败，请检查网络连接是否正常");
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.TopLevel = true;
                            frm.ShowDialog();
                        }
                        else if ((DateTime.Now - Form_MessageBeginTime).TotalMinutes > 30)
                        {
                            Form_MessageBeginTime = DateTime.Now;
                            Form_HiveMessage frm = new Form_HiveMessage("Hive数据上传失败，请检查网络连接是否正常");
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.TopLevel = true;
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        Form_MessageFirst = true;
                    }

                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"上传{DS}{(result ? "successfully" : "failed")}");
                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
                    SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{content}\r\n");

                    if (responseString == null || responseString == "" || responseString == "操作超时")
                    {
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"服务器返回失败原因：{responseString}", LogLevel.Warn);
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Connect Hive system failed" + "\r\n");
                        result = false;
                    }
                    if (result)
                    {
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Success {DS} logged" + "\r\n");
                    }
                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload information to the hive system {(result ? "successfully" : "failed")}!\r\n");

                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive returns:{responseString}\r\n" + "\r\n");
                    if (result)
                    {
                        if (responseString.Contains("40"))
                        {
                            string DF = SystemMgr.GetInstance().GetParamString("PLCDir");
                            DF = strPath + "\\code4files\\" + DS.ToString() + "\\";

                            if (!Directory.Exists(DF))
                                Directory.CreateDirectory(DF);

                            File.Move(jsonFileName, DF + Path.GetFileName(jsonFileName));

                        }
                        else//success
                        {
                            Thread.Sleep(100);
                            File.Delete(jsonFileName);

                        }

                    }
                    else
                    {
                        //网络异常
                        _SendDataState.DelayTime(1000 * 60 * 5, DS);
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Delay to send because of sending failed to Hive server\r\n\r\n");

                    }
                }

            }

        }

        /// <summary>
        /// 把数据post到Hive服务器
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="postData">数据</param>
        /// <param name="responseString">服务器返回结果</param>
        /// <returns></returns>
        private bool Post(string url, string postData, ref string responseString)
        {
            try
            {
                var client = new RestClient(url);
                client.Timeout = 1000 * 10;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", postData, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                responseString = response.Content;
                if (responseString == "")
                {
                    responseString = response.ErrorMessage;
                    return false;
                }
            }
            catch (Exception ex)
            {
                responseString = ex.ToString();
                return false;
            }
            return true;
        }


        public string MachineData(string[] machinedata)
        {
            string Json_Message;

            Dictionary<string, object> Serialtemp = new Dictionary<string, object>();
            foreach (DataMatch item in L_serials)
            {
                Serialtemp.Add(item.name, machinedata[item.index]);
            }

            List<string> temp = new List<string>();
            foreach (DataMatch item in L_wip_serials)
            {
                temp.Add(machinedata[item.index]);
            }
            if (temp.Count > 0)
            {
                Serialtemp.Add("wip_serials", temp);
            }


            Dictionary<string, object> t1 = new Dictionary<string, object>();
            foreach (DataMatch item in L_data)
            {
                t1.Add(item.name, machinedata[item.index]);
            }

            //二级目录
            //Tossing
            Dictionary<string, string>[] Tossing = new Dictionary<string, string>[] { new Dictionary<string, string>() };
            if (D_Tossing.name != null)
            {
                Tossing[0].Add(D_Tossing.name, machinedata[D_Tossing.index]);
                t1.Add("tossing", Tossing[0]);
                t1.Add("tossing_reason", machinedata[n_Tossing_reason].Length < 2 ? "PASS" : machinedata[n_Tossing_reason]);
            }
            else
            {
                //Tossing[0].Add("materials", "0");
                t1.Add("tossing", Tossing[0]);
                t1.Add("tossing_reason", "PASS");
            }

            string result = machinedata[n_pass];
            if (!(result.ToUpper().Equals("TRUE") || result.ToUpper().Equals("FALSE")))
            {
                result = "true";
            }

            DateTime input = Convert.ToDateTime(machinedata[n_input_time]);
            DateTime output = Convert.ToDateTime(machinedata[n_output_time]);

            #region 防止输入和输出时间异常
            TimeSpan t = output - input;
            if (t.TotalMilliseconds <= 0)
            {
                input = output.AddSeconds(-5);
            }

            if (((DateTime.Now - output).TotalHours > 71) || ((DateTime.Now - input).TotalHours > 71))
            {
                output = DateTime.Now;
                input = output.AddSeconds(-6);
            }
            #endregion 防止输入和输出时间异常


            HiveMgr.GetInstance().MachineDataUpload(out Json_Message, result, input, output, Serialtemp, t1, null, false, null, true);
            return Json_Message;
        }

        /// <summary>
        /// 读取配置文件数据
        /// </summary>
        /// <param name="doc"></param>
        public bool ReadConfigXML()
        {
            string cfg = Application.StartupPath + "\\MachineDataCfg.xml";
            XmlDocument doc = new XmlDocument();

            try
            {
                if (File.Exists(cfg))
                {
                    doc.Load(cfg);
                }
                else
                {
                    MessageBox.Show("MachineDataCfg.xml不存在", "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            L_serials.Clear();
            L_wip_serials.Clear();
            L_data.Clear();

            XmlNodeList xnl = doc.SelectNodes("/Config");

            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        int index = Convert.ToInt32(xe.GetAttribute("序号").Trim());
                        string strName = xe.GetAttribute("名称").Trim();
                        string strDataIndex = xe.GetAttribute("数据索引").Trim();
                        string strDataStyle = xe.GetAttribute("数据类型").Trim();
                        PLC2PC_DataLength = index;

                        if (string.IsNullOrEmpty(strName) || string.IsNullOrEmpty(strDataIndex) || string.IsNullOrEmpty(strDataStyle))
                        {
                            continue;
                        }

                        //serials
                        //wip_serials
                        //input_time
                        //output_time
                        //pass
                        //data
                        //Tossing
                        //Tossing_reason

                        switch (strDataStyle)
                        {
                            case "serials":
                                L_serials.Add(new DataMatch { index = Convert.ToInt16(strDataIndex), name = strName });
                                break;
                            case "wip_serials":
                                L_wip_serials.Add(new DataMatch { index = Convert.ToInt16(strDataIndex), name = strName });
                                break;
                            case "data":
                                L_data.Add(new DataMatch { index = Convert.ToInt16(strDataIndex), name = strName });
                                break;
                            case "input_time":
                                n_input_time = Convert.ToInt16(strDataIndex);
                                break;
                            case "output_time":
                                n_output_time = Convert.ToInt16(strDataIndex);
                                break;
                            case "pass":
                                n_pass = Convert.ToInt16(strDataIndex);
                                break;
                            case "Tossing":
                                D_Tossing.index = Convert.ToInt16(strDataIndex);
                                D_Tossing.name = strName;
                                break;
                            case "Tossing_reason":
                                n_Tossing_reason = Convert.ToInt16(strDataIndex);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            return true;
        }

    }
}

