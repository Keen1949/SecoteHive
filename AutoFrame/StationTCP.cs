using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AutoFrameDll;
using CommonTool;
using ToolEx;
using Communicate;
using System.Reflection;
using System.Xml;
using System.IO;

namespace AutoFrame
{
    class StationTcp : StationEx
    {
        enum POINT
        {
            安全点 = 1,
        }

        public struct DataMatch
        {
            public int index;
            public string name;
        }

        private AsyncSocketTCPServer m_tcpServer;
        private AsyncSocketTCPServer machinedata;//3000
        private AsyncSocketTCPServer machinestate;//3001
        private AsyncSocketTCPServer errordata;//3002


        private static List<DataMatch> L_serials = new List<DataMatch>();
        private static List<DataMatch> L_wip_serials = new List<DataMatch>();
        private static List<DataMatch> L_data = new List<DataMatch>();

        private static DataMatch D_Tossing = new DataMatch();
        private static int n_input_time = 999;
        private static int n_output_time = 999;
        private static int n_pass = 999;
        private static int n_Tossing_reason = 999;

        private static int PLC2PC_DataLength = 0;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationTcp(string strName) : base(strName)
        {
            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";
            if (File.Exists(cfg))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);

                TcpServerMgr.GetInstance().ReadCfgFromXml(doc);
            }

            machinedata = TcpServerMgr.GetInstance().GetTcpServer(0);
            machinedata.DataReceived += M_tcpServer_DataReceived_machinedata;

            machinestate = TcpServerMgr.GetInstance().GetTcpServer(1);
            machinestate.DataReceived += M_tcpServer_DataReceived_machinestate;

            errordata = TcpServerMgr.GetInstance().GetTcpServer(2);
            errordata.DataReceived += M_tcpServer_DataReceived_errordata;
        }

        private void M_tcpServer_DataReceived_errordata(object sender, AsyncSocketEventArgs e)
        {
            string strData, message;
            ErrorMessage HiveMsg = new ErrorMessage();
            strData = Encoding.Default.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
            //strData= strData.Replace("__","\"");
            //var client = e.m_state.ClientSocket;
            //client.SendTo("OK");

            //    server.Send(state, "errordata_OK");
            e.m_state.ClientSocket.SendTo(Encoding.UTF8.GetBytes("1\r\n"), e.m_state.ClientSocket.RemoteEndPoint);

            string[] strSlits = strData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            ShowLog(e.m_state + " errordata receive:" + strData);

            if (strSlits.Length != 5)
            {
                ShowLog("errordata 收到的数据异常", LogLevel.Warn);
                HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"errordata 收到的数据异常：{strData}\r\n");
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
                        ShowLog($"errordata 收到的数据中时间异常:{strData}", LogLevel.Warn);
                        HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"errordata 收到的数据异常：{strData}\r\n");
                        return;
                    }

                    HiveMgr.GetInstance().UploadInformation(HiveMsg, out message);
                }
                catch (Exception ex)
                {
                    ShowLog($"errordata异常：{ex}", LogLevel.Warn);
                    HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"errordata异常：{ex}\r\n");
                }


            }
        }

        private void M_tcpServer_DataReceived_machinedata(object sender, AsyncSocketEventArgs e)
        {
            string strData;
            strData = Encoding.Default.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
            e.m_state.ClientSocket.SendTo(Encoding.UTF8.GetBytes("1\r\n"), e.m_state.ClientSocket.RemoteEndPoint);

            string[] strSlits = strData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            ShowLog(e.m_state + "machinedata:" + strData);
            if (strSlits.Length != PLC2PC_DataLength)
            {
                ShowLog("machinedata 收到的数据异常", LogLevel.Warn);
                HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"machinedata 收到的数据异常：{strData}\r\n");
            }
            else
            {
                try
                {
                    MachineData(strSlits);
                }
                catch (Exception ex)
                {
                    ShowLog($"machinestate 异常：{ex}", LogLevel.Warn);
                    HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"machinedata异常：{ex}\r\n");
                }
            }


        }

        private void M_tcpServer_DataReceived_machinestate(object sender, AsyncSocketEventArgs e)
        {
            string strData, message;
            ErrorMessage HiveMsg = new ErrorMessage();
            strData = Encoding.Default.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
            e.m_state.ClientSocket.SendTo(Encoding.UTF8.GetBytes("1\r\n"), e.m_state.ClientSocket.RemoteEndPoint);

            string[] strSlits = strData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            ShowLog(e.m_state + " machinestate receive:" + strData);

            if (!((strSlits.Length == 4) || (strSlits.Length == 7)))
            {
                ShowLog("machinestate 收到的数据异常", LogLevel.Warn);
                HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"machinestate 收到的数据异常：{strData}\r\n");
            }
            else
            {
                try
                {
                    if ((DateTime.Now - Convert.ToDateTime(strSlits[1])).TotalHours > 71)
                    {
                        ShowLog($"machinestate 异常：{strSlits[1]}时效大于3天", LogLevel.Warn);
                        HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"machinestate 异常：{strSlits[1]}时效大于3天\r\n");
                        return;
                    }

                    if (strSlits.Length == 4)
                    {
                        if (strSlits[2] == strSlits[3])
                        {
                            HiveMgr.ChangeState(new StateMessage() { MachineState = Convert.ToInt16(strSlits[0]) }, out message, true, strSlits[1]);
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
                            HiveMgr.ChangeState(new StateMessage() { MachineState = Convert.ToInt16(strSlits[0]) }, out message, true, strSlits[1], Convert.ToInt16(strSlits[4]), strSlits[5], strSlits[6]);
                        }
                        else
                        {
                            StateMessage state = new StateMessage();
                            state.MachineState = Convert.ToInt16(strSlits[0]);
                            state.code = strSlits[2];
                            state.strMsg = strSlits[3];
                            HiveMgr.ChangeState(state, out message, true, strSlits[1], Convert.ToInt16(strSlits[4]), strSlits[5], strSlits[6]);
                        }

                    }

                }
                catch (Exception ex)
                {
                    ShowLog($"machinestate 异常：{ex}", LogLevel.Warn);
                    HiveMgr.GetInstance().SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"machinestate异常：{ex}\r\n");
                }
            }
        }
        /// <summary>
        /// 站位初始化，用来添加伺服上电，打开网口，站位回原点等动作
        /// </summary>
        public override void StationInit()
        {
            n_input_time = 999;
            n_output_time = 999;
            n_pass = 999;
            n_Tossing_reason = 999;
            PLC2PC_DataLength = 0;
            D_Tossing = new DataMatch();

            ReadConfigXML();

            errordata.Start();
            machinestate.Start();
            machinedata.Start();

            ShowLog("初始化完成");
        }
        /// <summary>
        /// 站位退出退程时调用，用来关闭伺服，关闭网口等动作
        /// </summary>
        public override void StationDeinit()
        {
            errordata.Stop();
            machinedata.Stop();
            machinestate.Stop();
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
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
            WaitTimeDelay(200);
        }

        /// <summary>
        /// 空跑
        /// </summary>
        protected override void DryRun()
        {
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

        public void MachineData(string[] machinedata)
        {
            if (!SystemMgr.GetInstance().GetParamBool("EnableHive"))
            {
                return;
            }

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
                t1.Add("Tossing", Tossing[0]);
                t1.Add("Tossing_reason", machinedata[n_Tossing_reason].Length < 2 ? "PASS" : machinedata[n_Tossing_reason]);
            }
            else
            {
                //Tossing[0].Add("materials", "0");
                t1.Add("Tossing", Tossing[0]);
                t1.Add("Tossing_reason", "PASS");
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


            HiveMgr.GetInstance().MachineDataUpload(out Json_Message, result, input, output, Serialtemp, t1);

        }



    }
}
