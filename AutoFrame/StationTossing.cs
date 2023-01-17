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

    class StationTossing : StationEx
    {
        /// <summary>
        /// 处理PLC写入CSV的数据状态标志位（TRUE：可以处理下一组数据   FALSE：正在处理数据）
        /// </summary>
        private Dictionary<string, bool> threadIsFinish { get; set; }

        private Thread threadPlcTossingData;

        string strPath;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationTossing(string strName) : base(strName)
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
                threadIsFinish.Add("Tossing", true);
            }

            strPath = SystemMgr.GetInstance().GetParamString("PLCDir") + "\\" + this.Name + "\\";
        }

        /// <summary>
        /// 站位初始化，用来添加伺服上电，打开网口，站位回原点等动作
        /// </summary>
        public override void StationInit()
        {
            threadIsFinish["Tossing"] = true;
            base.StationInit();
        }
        /// <summary>
        /// 站位退出退程时调用，用来关闭伺服，关闭网口等动作
        /// </summary>
        public override void StationDeinit()
        {
            try
            {
                threadIsFinish["Tossing"] = true;
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
            if (SystemMgr.GetInstance().GetParamBool("EnableTossing"))
            {
                #region Tossing
                if (threadIsFinish["Tossing"])
                {
                    threadIsFinish["Tossing"] = false;
                    threadPlcTossingData = new Thread(() => TossingData());
                    threadPlcTossingData.IsBackground = true;
                    threadPlcTossingData.Start();
                }
                #endregion
            }
            Thread.Sleep(100);
        }

        private void TossingData()
        {
            try
            {
                string parentDir = strPath;
                string path = strPath;

                string errorpath = CreateFilePath(parentDir + "Backup");
                //List<string> fildlist = GetAllFiles(path);
                string[] strFile = Directory.GetFiles(path, "*.csv");

                //20210727 如果没有文件，没必要执行下面代码
                if (strFile.Length == 0)
                {
                    Thread.Sleep(500);
                    threadIsFinish["Tossing"] = true;
                    return;
                }

                #region 发现文件，开始遍历
                foreach (string n in strFile) //遍历文件路径
                {
                    DateTime t = File.GetCreationTime(n);
                    bool bIsOverTime = HiveMgr.SendDataState.Instance.IsOverBindingTime(t, 24 * 3 - 0.1);//20210825 判断一下是否是超过3天的数据，加入0.1h的浮动误差。
                    if (bIsOverTime)
                    {
                        MoveFile(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup"));//n.Replace("Tossing", "Tossing" + "\\" + "Backup")
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
                            threadIsFinish["Tossing"] = true;

                            if (!MoveFile(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup")))
                            {
                                File.Copy(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup"));
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
                            //line = sr.ReadLine();
                            sr.Close();
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(line))
                    {
                        //如果文件是空文件，添加日志
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空，请检查");
                        MoveFile(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup"));
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"{n}该文件为空,已移到Backup中\r\n");
                        continue;
                    }
                    line = line.Replace("\"", "");
                    string[] data = line.Split(',');//读取的内容拆分转为数组
                    if (data.Length < 11)
                    {
                        ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"文件{n}获取的数值数目与配置参数数目不匹配，请检查");
                        MoveFile(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup"));
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"文件{n}获取的数值数目与配置参数数目不匹配,已移到Backup中\r\n");
                        continue;
                    }

                    string result = Uptossing(data);
                    if (!result.ToUpper().Contains("OK"))
                    {
                        ShowLog($"Tossing文件{n}上传异常，反馈结果为{result}");
                        WarningMgr.GetInstance().Warning($"Tossing文件{n}上传失败，反馈结果为{result},请检查");
                        MoveFile(n, n.Replace("Tossing", "Tossing" + "\\" + "Backup"));
                        SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"文件{n}数据上传异常,已移到Backup中\r\n");
                        continue;
                    }
                    //ok，将该文件删除
                    ShowLog($"上传Tossing文件{n},上传内容{line}成功,反馈结果为{result}");
                    ShowLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"保存成功，将该文件{n}删除");
                    SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"数据上传成功，将该文件{n}删除\r\n");
                    File.Delete(n);
                }
                #endregion
                threadIsFinish["Tossing"] = true;
            }
            catch (Exception ex)
            {
                threadIsFinish["Tossing"] = true;
                ShowLog($"PlcUploadTossing 异常:{ex.ToString()}", LogLevel.Warn);
            }

            Thread.Sleep(10);
        }

        public string Uptossing(string[] Tossing)
        {
            string cResult = "";

            //园区URL: http://10.33.24.21/bobcat/sfc_response.aspx
            //http://10.32.18.27:5000/api/DataAnalysis/Error_log_registration_LT --园区
            string strTossingUrl = SystemMgr.GetInstance().GetParamString("TossingDataUrl");

            string unit_sn = Tossing[0];//主条码，扫码失败上传虚拟条码
            string part_1_sn = Tossing[1];//部件1条码，扫码失败上传虚拟条码，无部件则传“NULL”
            string part_2_sn = Tossing[2];//部件2条码，扫码失败上传虚拟条码，无部件则传“NULL”
            string pass = "false";//固定值，代表只有抛料时才需上传数据（此项不同于HIVE）
            string input_time = Tossing[3];//开始时间，如2022-03-22 08:00:00 
            string output_time = Tossing[4];//结束时间，如2022-03-22 08:00:10
            string machineId = Tossing[5];//设备编号（同DT,如P193-L1-CDCB-01）
            string lineId = Tossing[6];//线别编号（同DT,如BU21-B001）
            string workOrder = Tossing[7];//工单号，无则传“NULL”
            string sw_version = "V1.0";//软件版本号，如V1.5

            string shift = "";//班别，白班（08：30~20：30）上传D，夜班（20：30~08：30）上传N
            if (ComPare_Time(Convert.ToDateTime(input_time), "8:30") && !ComPare_Time(Convert.ToDateTime(input_time), "20:30"))
            {
                shift = "D";
            }
            else
            {
                shift = "N";
            }

            string unit = Tossing[8];//主料抛料数量（同一SN多次抛料，带coil的为主料，磁铁段磁铁为主料），无抛料则传0
            string part = Tossing[9];//辅料抛料数量（同一SN多次抛料），无抛料则传0，无部件则传“NULL”
            string tossing_reason = Tossing[10];//抛料原因，同HIVE，无HIVE工站可自拟，尽可能简单明了


            string s1 =
                "{"
                 + "\"unit_sn\":\"" + unit_sn + "\","
                 + "\"serials\":{"
                    + "\"part_1_sn\":\"" + part_1_sn + "\","
                    + "\"part_2_sn\":\"" + part_2_sn + "\""
                 +"},"
                 + "\"pass\":\"" + pass + "\","
                 + "\"input_time\":\"" + input_time + "\","
                 + "\"output_time\":\"" + output_time + "\","
                 + "\"data\":{"
                    + "\"machineId\":\"" + machineId + "\","
                    + "\"lineId\":\"" + lineId + "\","
                    + "\"workOrder\":\"" + workOrder + "\","
                    + "\"sw_version\":\"" + sw_version + "\","
                    + "\"shift\":\"" + shift + "\","
                    + "\"tossing\":{"
                        + "\"unit\":\"" + unit + "\","
                        + "\"part\":\"" + part + "\","
                        + "},"
                    + "\"tossing_reason\":\"" + tossing_reason + "\","
                 + "}"
                 + "}";

            try
            {
                //p = Tossingcount & c = QUERY_RECORD & tsid = testline,testmachineid,stationid,2022 - 03 - 22,10,100,10 & sn = Tossingcount
                var request = (HttpWebRequest)WebRequest.Create(strTossingUrl);
                //var postData = "p=Tossingcount&c=QUERY_RECORD&tsid=" + testline + "," + testmachineid + "," + stationid + "," + time + "," + time1 + "," + Tossing[4] + "," + Tossing[5] + "&sn=Tossingcount";
                var data = Encoding.ASCII.GetBytes(s1);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
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
                cResult = "Post Tossing Fail";
                ShowLog($"{DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss")},Post Tossing Fail");
                SaveLog(DateTime.Now.ToString("HH:mm:ss") + "  " + $"发送数据失败\r\n");
                return cResult;
            }
            return cResult;
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

        public bool ComPare_Time(DateTime input_time, string ToComPareTime)
        {
            bool result;
            try
            {
                string input = input_time.ToString("HH:mm");
                DateTime a = Convert.ToDateTime(input);
                DateTime b = Convert.ToDateTime(ToComPareTime);
                if (DateTime.Compare(a, b) > 0)
                {
                    result = true;
                }
                result = false;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
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



    }
}

