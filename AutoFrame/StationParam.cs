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

    class StationParam : StationEx
    {
        /// <summary>
        /// 处理PLC写入CSV的数据状态标志位（TRUE：可以处理下一组数据   FALSE：正在处理数据）
        /// </summary>
        private Dictionary<string, bool> threadIsFinish { get; set; }

        private Thread threadPlcTossingData;

        public struct DataMatch
        {
            public int index;
            public string name;
        }

        string strPath;

        private List<DataMatch> L_ParamKey = new List<DataMatch>();

        private int PLC2PC_ParamKeyLength = 0;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationParam(string strName) : base(strName)
        {
            //配置站位界面显示输入
            io_in = new string[] { };
            //配置站位界面显示输出
            io_out = new string[] { };
            //配置站位界面显示气缸
            m_cylinders = new string[] { };

            if (threadIsFinish == null)
            {
                threadIsFinish = new Dictionary<string, bool>();
                threadIsFinish.Add("Param", true);
            }

            strPath = SystemMgr.GetInstance().GetParamString("PLCDir") + "\\" + this.Name + "\\";
        }

        /// <summary>
        /// 站位初始化，用来添加伺服上电，打开网口，站位回原点等动作
        /// </summary>
        public override void StationInit()
        {
            //ReadConfigXML();
            threadIsFinish["Param"] = true;
            base.StationInit();
        }
        /// <summary>
        /// 站位退出退程时调用，用来关闭伺服，关闭网口等动作
        /// </summary>
        public override void StationDeinit()
        {
            try
            {
                threadIsFinish["Param"] = true;
                threadPlcTossingData.Abort();
            }
            catch
            {

            }
            base.StationDeinit();
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
            base.InitSecurityState();
        }


        /// <summary>
        /// 正常运行
        /// </summary>
        protected override void NormalRun()
        {
            CheckContinue();
            if (SystemMgr.GetInstance().GetParamBool("EnableParam"))
            {
                #region Tossing
                if (threadIsFinish["Param"])
                {
                    threadIsFinish["Param"] = false;
                    threadPlcTossingData = new Thread(() => UpParam());
                    threadPlcTossingData.IsBackground = true;
                    threadPlcTossingData.Start();
                }
                #endregion
            }
            Thread.Sleep(100);
        }

        private void UpParam()
        {
            try
            {
                //string parentDir = strPath;
                //string path = CreateFilePath(parentDir + "Param");
                string parentDir = strPath;
                string path = strPath;
                string errorpath = CreateFilePath(parentDir + "Backup");
                //List<string> fildlist = GetAllFiles(path);
                string[] strFile = Directory.GetFiles(path, "*.csv");

                //20210727 如果没有文件，没必要执行下面代码
                if (strFile.Length == 0)
                {
                    Thread.Sleep(500);
                    threadIsFinish["Param"] = true;
                    return;
                }

                #region 发现文件，开始遍历
                foreach (string n in strFile) //遍历文件路径
                {
                    DateTime t = File.GetCreationTime(n);
                    bool bIsOverTime = HiveMgr.SendDataState.Instance.IsOverBindingTime(t, 24 * 3 - 0.1);//20210825 判断一下是否是超过3天的数据，加入0.1h的浮动误差。
                    if (bIsOverTime)
                    {

                       // MoveFile(n, n+"\\"+ "Backup");
                        MoveFile(n, n.Replace("Param", "Param" + "\\" + "Backup"));
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},文件时间超过三天,已移到Backup中");
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},文件时间超过三天,已移到Backup中\r\n");
                        continue;
                    }
                    //ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},文件时间未超过三天");

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
                            threadIsFinish["Param"] = true;

                            if (!MoveFile(n, n.Replace("Param", "Param" + "\\" + "Backup")))
                            {
                                File.Copy(n, n.Replace("Param", "Param" + "\\" + "Backup"));
                                File.Delete(n);
                                MessageBox.Show($"{n}被占用，请解除占用文件");
                            }
                            else
                                ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},该文件连续被占用,已移到Backup中");
                                SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"遍历到文件{n},该文件连续被占用,已移到Backup中\r\n");
                            return;
                        }
                    }
                    int i = 0;
                    string line = "";
                    while (sr.Peek() >= 0)
                    {
                        i++;
                        line = sr.ReadLine();
                        // if (i == GetParamInt("ReaderStart"))
                        if (i == 1)
                        {
                            
                            sr.Close();
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(line))
                    {
                        //如果文件是空文件，添加日志
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空，请检查");
                        MoveFile(n, n.Replace("Param", "Param" + "\\" + "Backup"));
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空,已移到Backup中\r\n");
                        continue;
                    }
                    line = line.Replace("\"", "");
                    string[] data = line.Split(',');//读取的内容拆分转为数组

                    if (data.Length < 6)
                    {
                        WarningMgr.GetInstance().Warning($"文件{n}获取的数值数目与配置参数数目不匹配，请检查");
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"文件{n}获取的数值数目与配置参数数目不匹配,已移到Backup中\r\n");
                        return;
                    }

                    string result = ParamUpload(data);
                    if (!result.ToUpper().Contains("点检参数上传成功"))
                    {
                        ShowLog($"Param文件{n}上传异常，反馈结果为{result}");
                        WarningMgr.GetInstance().Warning($"Param文件{n}上传失败，反馈结果为{result},请检查");
                        MoveFile(n, n.Replace("Param", "Param" + "\\" + "Backup"));
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"文件{n}数据上传异常,已移到Backup中\r\n");
                        return;
                    }
                    //ok，将该文件删除
                    ShowLog($"上传Param文件{n},上传内容{line}成功,反馈结果为{result}");
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"数据上传成功，将该文件{n}删除");
                    SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"数据上传成功，将该文件{n}删除\r\n");
                    File.Delete(n);
                }
                #endregion
                threadIsFinish["Param"] = true;
            }
            catch (Exception ex)
            {
                threadIsFinish["Param"] = true;
                ShowLog($"PlcUploadTossing 异常:{ex.ToString()}", LogLevel.Warn);
            }

            Thread.Sleep(10);
        }


        public bool ReadConfigXML()
        {
            string cfg = Application.StartupPath + "\\ParamUploadCfg.xml";
            XmlDocument doc = new XmlDocument();

            try
            {
                if (File.Exists(cfg))
                {
                    doc.Load(cfg);
                }
                else
                {
                    MessageBox.Show("ParamUploadCfg.xml不存在", "参数上传配置文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message, "参数上传配置文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            L_ParamKey.Clear();

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
                        PLC2PC_ParamKeyLength = index;

                        if (string.IsNullOrEmpty(strName) || string.IsNullOrEmpty(strDataIndex))
                            continue;

                        L_ParamKey.Add(new DataMatch { index = Convert.ToInt16(strDataIndex), name = strName });

                    }
                }
            }
            return true;
        }

        private string CreateFilePath(string parentDir)
        {
            string directory = parentDir;

            if (directory.LastIndexOf("\\") != directory.Length - 1)
                directory += "\\";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }

        public bool MoveFile(string sourceFileName, string destFileName)
        {
            try
            {
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


        private bool SaveLog(string Message)
        {

            try
            {
                string strSavePath = strPath + "\\" + "Log";
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

        public string ParamUpload(string[] param)
        {
            string s1 =
                 "{"
                 + "\"category\":\"" + param[0] + "\","
                 + "\"line\":\"" + param[1] + "\","
                 + "\"machine\":\"" + param[2] + "\","
                 + "\"item\":\"" + param[3] + "\","
                 + "\"value\":\"" + param[4] + "\","
                 + "\"status\":\"" + param[5] +"\","
                  + "\"intime\":\"" + param[6] + "\""
                  + "}";

                string cResult = "";
                string strParamDataUrl = SystemMgr.GetInstance().GetParamString("ParamDataUrl");

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(strParamDataUrl);
                    var data = Encoding.ASCII.GetBytes(s1);
                    request.Method = "POST";
                    request.ContentType = "application/json;charset=utf-8";
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "发送数据:\r\n" + $"{s1}\r\n");
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    response.Close();
                    cResult = responseString;
                    SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + "接收数据:\r\n" + $"{cResult}\r\n");
            }
                catch (Exception e)
                {
                    ShowLog($"{DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss")},Post PlcParam Fail");
                    SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"发送数据失败\r\n");
                    return "Fail";
                }
                return cResult;        
            #region 上传参数参考
            /*
             string s1 =
               "{"
               + "\"category\":\"PCA\","
               + "\"line\":\"" + SystemMgr.GetInstance().GetParamString("SfcLineNo") + "\","
               + "\"machine\":\"" + SystemMgr.GetInstance().GetParamString("SfcMachineID") + "\","
               + "\"item\":\" Press force\","
               + "\"value\":\"" + Press1 + ";" + Press2 + ";" + Press3 + ";" + Press4 + "\","
               + "\"status\":\"N\","
               + "\"intime\":\"" + dttime.ToString("yyyy_MM_dd_HH_mm_ss") + "\""
               + "}";

            string s2 =
               "{"
               + "\"category\":\"PCA\","
               + "\"line\":\"" + SystemMgr.GetInstance().GetParamString("SfcLineNo") + "\","
               + "\"machine\":\"" + SystemMgr.GetInstance().GetParamString("SfcMachineID") + "\","
               + "\"item\":\" Press time\","
               + "\"value\":\"" + PressTime + "\","
               + "\"status\":\"N\","
               + "\"intime\":\"" + dttime.ToString("yyyy_MM_dd_HH_mm_ss") + "\""
               + "}";
    */
            #endregion
        }

    }
}

