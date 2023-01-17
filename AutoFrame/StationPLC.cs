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

namespace AutoFrame
{

    class StationPLC : StationEx
    {
        string strPath;

        private DateTime Form_MessageBeginTime = DateTime.Now;
        private bool Form_MessageFirst = true;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationPLC(string strName) : base(strName)
        {
            //配置站位界面显示输入
            io_in = new string[] { };
            //配置站位界面显示输出
            io_out = new string[] { };
            //配置站位界面显示气缸
            m_cylinders = new string[] { };
            InitFlag_ThreadIsFinish();
            strPath = SystemMgr.GetInstance().GetParamString("PLCDir") + "\\" + this.Name + "\\";
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

        //public bool ThreadIsFinish1 { get; set; }
        //List<string> list_errorNames = new List<string>() { "message", "code", "severity", "data", "occurrence_Time", "resolved_Time" };
        //List<string> list_dataNames = new List<string>() { "sn", "code", "pass", "input_time", "output_time", "data" };
        //List<string> list_stateNames = new List<string>() { "machineState", "state_change_time", "data" };


        private Thread threadPlcErrorData;
        private Thread threadPlcMachineData;
        private Thread threadPlcMachineState;
        private Thread threadUploadErrorData;
        private Thread threadUploadPlcMachineData;
        private Thread threadUploadPlcMachineState;

        /// <summary>
        /// 正常运行
        /// </summary>
        protected override void NormalRun()
        {
            CheckContinue();//检查系统是否会继续运行

            #region 多线程


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
            #endregion
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
        /// 获取当前数据类型的message配置文件
        /// </summary>
        /// <param name="index">1：ErrorData  2：MachineData  3：MachineState 4:EnvironmentData</param>
        /// <returns></returns>
        private string GetPlcMessage(DataStyle index, bool data = true)
        {
            string Message = "";
            switch (index)
            {
                case DataStyle.ErrorData:
                    Message = "%\"message\":\"{0}\",\"code\":\"{1}\",\"severity\":\"{2}\",\"data\":%^,\"occurrence_time\":\"{3}\",\"resolved_time\":\"{4}\"^";
                    //Message = SystemMgr.GetInstance().GetParamString($"{this.Name}ErrorDataMessage");
                    break;
                case DataStyle.MachineData:
                    Message = "%\"sn\":\"{0}\",\"code\":\"{1}\",\"pass\":\"{2}\",\"input_time\":\"{3}\",\"output_time\":\"{4}\",\"data\":\"{5}\"^";
                    //Message = SystemMgr.GetInstance().GetParamString($"{this.Name}MachineDataMessage");
                    break;
                case DataStyle.MachineState:
                    if (data)
                    {
                        Message = "%\"machine_state\":{0},\"state_change_time\":\"{1}\",\"data\":%\"code\":\"{2}\",\"error_message\":\"{3}\",\"previous_state\":\"{4}\",\"State_change_reason\":\"{5}\",\"sw_version\":\"{6}\"^^";
                    }
                    else
                    {
                        Message = "%\"machine_state\":{0},\"state_change_time\":\"{1}\",\"data\":%{2}{3}\"previous_state\":\"{4}\",\"State_change_reason\":\"{5}\",\"sw_version\":\"{6}\"^^";
                    }

                    //Message = SystemMgr.GetInstance().GetParamString($"{this.Name}MachineStateMessage");
                    break;

                default:
                    Message = "";
                    break;
            }
            return Message;
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

        private void PlcToHive(DataStyle DS)
        {
            try
            {
                string parentDir = strPath;
                string path = CreateFilePath(DS, parentDir + "plcdata");
                string errorpath = CreateFilePath(DS, parentDir + "backup");
                //List<string> fildlist = GetAllFiles(path);
                string[] strFile = Directory.GetFiles(path, "*.csv");

                #region 发现文件，开始遍历
                foreach (string n in strFile)/*foreach (var n in fildlist)*/ //遍历文件路径
                {

                    DateTime t = File.GetCreationTime(n);
                    bool bIsOverTime = HiveMgr.SendDataState.Instance.IsOverBindingTime(t, 24 * 3 - 0.1);//判断一下是否是超过3天的数据。
                    if (bIsOverTime || !JSONTime(DS, n))
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
                        catch
                        {


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
                    #region machinedata
                    string Message = "";
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"开始获取{DS}的文件格式");
                    if (DS == DataStyle.MachineData)
                    {
                        string Json_Message;

                        Dictionary<string, object> Serialtemp = new Dictionary<string, object>();
                        Serialtemp.Add("Coil_Carrier_sn", data[1]);

                        Serialtemp.Add("NC_Carrier_sn", data[2]);


                        Serialtemp.Add("Coil_sn", data[3]);
                        Serialtemp.Add("NC_sn", data[4]);

                        Dictionary<string, object> t1 = new Dictionary<string, object>();
                        t1.Add("MachineID", data[8]);
                        t1.Add("LineID", data[9]);
                        t1.Add("LineName", data[10]);
                        t1.Add("sw_version", data[11]);

                        Dictionary<string, string>[] Tossing = new Dictionary<string, string>[] { new Dictionary<string, string>() };
                        if ((data.Length == 14) && (data[12] == "0" || data[12] == " " || data[12] == "null"))
                        {
                            Tossing[0].Add("Coil", "0");
                            t1.Add("Tossing", Tossing[0]);
                            t1.Add("Tossing_reason", "PASS");
                        }
                        else if ((data.Length == 14))
                        {
                            Tossing[0].Add("Coil", data[12]);
                            t1.Add("Tossing", Tossing[0]);
                            t1.Add("Tossing_reason", data[13]);
                        }
                        else
                        {
                            Tossing[0].Add("Coil", "0");
                            t1.Add("Tossing", Tossing[0]);
                            t1.Add("Tossing_reason", "PASS");
                        }

                        HiveMgr.GetInstance().PlcMachineDataUpload(out Json_Message, data[5], data[6], data[7], Serialtemp, t1, null, false, null, true);
                        Message = Json_Message;
                    }
                    else
                    {
                        bool bState = true;
                        if (DS == DataStyle.MachineState)
                        {
                            if (data.Length < 7)
                            {
                                //ok，将该文件删除
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"该文件数据有问题,有问题数据为：{line}。将该文件{n}删除", LogLevel.Warn);
                                try
                                {
                                    File.Delete(n);
                                }
                                catch
                                {
                                    continue;
                                }
                                continue;

                            }

                            if (data[2] == data[3])
                            {
                                bState = false;
                                Message = GetPlcMessage(DS, bState);//获取当前数据类型的message配置文件
                                data[2] = null;
                                data[3] = null;
                            }
                            else
                            {
                                Message = GetPlcMessage(DS, bState);//获取当前数据类型的message配置文件
                            }
                        }
                        else
                        {
                            Message = GetPlcMessage(DS, bState);
                        }
                        var paramNum = Regex.Matches(Message, "{\\d*}"); //正则表达式，匹配格式项

                        if (data.Length != paramNum.Count)
                        {
                            //缺少数据，需要重新匹配
                            ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件数据异常，需要重新匹配，请检查");
                            SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件数据异常，需要重新匹配，请检查\r\n");
                            MoveFile(n, n.Replace("plcdata", "backup"));

                            continue;
                        }

                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "获取Message成功，开始格式化");

                        //将指定字符串中的格式项替换为data数组中相应对象的字符串表示形式
                        Message = string.Format(Message, data);
                        Message = Message.Replace('%', '{').Replace('^', '}').Replace("\\", "");

                    }
                    #endregion
                    try
                    {
                        Message = HiveMgr.GetInstance().ConvertJsonString(Message);
                    }
                    catch
                    {

                    }




                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "格式化成功");


                    //检测是否是标准json
                    bool isJson = CheckIsJson(Message);
                    if (!isJson)
                    {
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"当前字符串不是一个Json格式，请检查{n},Message:{Message}");
                        SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"当前字符串不是一个Json格式，请检查{n},Message:{Message}\r\n");
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


                        }

                    }
                    //ok，将该文件删除
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"保存成功，将该文件{n}删除");
                    File.Delete(n);
                }
                #endregion
                //post方法
                //HiveMgr.GetInstance().postJsonFile(DS);

                //需要记录log日志(hivelog)   
                threadIsFinish[DS] = true;
            }
            catch (Exception ex)
            {
                ShowLog($"PlcToHive 异常:{ex.ToString()}", LogLevel.Warn);
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

        public void MoveFile(string sourceFileName, string destFileName)
        {
            try
            {
                //缺少数据，需要重新匹配                    
                File.Move(sourceFileName, destFileName);
            }
            catch
            {
                //int m = Regex.Matches(DS, "副本").Count;               
                sourceFileName.Replace(".csv", ".CSV");
                //缺少数据，需要重新匹配                    
                MoveFile(sourceFileName, destFileName.Replace(".CSV", "_副本.CSV"));
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
            catch
            {
                return false;
            }
        }

        private void postJsonFile(DataStyle DS)
        {
            string directory = "";
            var _SendDataState = HiveMgr.SendDataState.Instance;
            while (true)
            {

                directory = CreateFilePath(DS, strPath + "uploadfiles");
                Thread.Sleep(50);

                string[] files = Directory.GetFiles(directory);
                #region 正常上传，按时间排序
                Dictionary<string, DateTime> DI = new Dictionary<string, DateTime>();
                foreach (string temp in files)
                {
                    DateTime t = File.GetCreationTime(temp);
                    if (_SendDataState.IsOverBindingTime(t, 24 * 3))
                    {
                        string DF = SystemMgr.GetInstance().GetParamString("HiveDir");
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
                    //if (!_SendDataState.IsCanSend(DS))
                    //{
                    //    Thread.Sleep(1000);
                    //    //SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Forbid to send because of the previous bad return from Hive server:\r\n");
                    //    //continue;
                    //    break;
                    //}

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
                    result = this.PostDemo(GetUrl(DS), content, ref responseString);

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
                    SavePlcHiveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{content}\r\n");

                    if (responseString == null || responseString == "")
                    {
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Connect Hive system failed" + "\r\n");
                        result = false;
                    }
                    if (result)
                    {
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Success {DS} logged" + "\r\n");
                    }
                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload information to the hive system {(result ? "successfully" : "failed")}!\r\n");
                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
                    SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive returns:{responseString}\r\n" + "\r\n");
                    if (result)
                    {
                        if (responseString.Contains("40"))
                        {
                            string DF = SystemMgr.GetInstance().GetParamString("PlcDir");
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
                        //_SendDataState.DelayTime(1000 * 60 * 5, DS);
                        SavePlcHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Delay to send because of sending failed to Hive server\r\n\r\n");

                    }
                }

            }

        }

        public Dictionary<DataStyle, bool> threadIsFinish { get; set; }

        private bool PostDemo(string url, string postData, ref string responseString)
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
    }
}

