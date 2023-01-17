using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoFrameDll;
using CommonTool;
using Communicate;
using System.Windows.Forms;
using System.IO;
using System.Xml;
namespace AutoFrame
{
    /// <summary>
    /// WaitIo超时中的错误信息
    /// </summary>
    public struct ErrIoInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int index;
        /// <summary>
        /// IO名称
        /// </summary>
        public string strIoName;
        /// <summary>
        /// 转换过后的错误信息
        /// </summary>
        public string strErrorInfo;
    }
    /// <summary>
    /// 枚举类型，表示当前机台是哪一站
    /// </summary>
    enum StationNum
    {
        LS,//Laser Strip
        LC,//Laser Cut
        NEH,// CF_CFE,//联机
        ANC,
        AEC,
        Hive,
    }
    /// <summary>
    /// 自定义的Error信息
    /// </summary>
    public struct UNDErrInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int index;
        /// <summary>
        /// 原本报警的错误信息
        /// </summary>
        public string strErrorMsg;
        /// <summary>
        /// 转换过后的错误信息
        /// </summary>
        public string strErrorInfo;
    }
    /// <summary>
    /// 每个IP对应的网口名称
    /// </summary>
    public struct ErrMsgTcp
    {
        /// <summary>
        ///网口号 
        /// </summary>
        public int m_nIndex;
        /// <summary>
        ///网口定义 
        /// </summary>
        public string m_strName;
        /// <summary>
        ///对方IP地址 
        /// </summary>
        public string m_strIP;
        /// <summary>
        ///端口号 
        /// </summary>
        public int m_nPort;
        /// <summary>
        /// 转换成报警信息后使用的错误名称
        /// </summary>
        public string m_ErrorName;
    }

    /// <summary>
    /// 保存pc上传过的错误信息
    /// </summary>
    public struct DownTimeIni
    {
        public DateTime m_dtErrorStartTime;
        public string strErrorCode;
        public string strErrorMsg;
    }
    /// <summary>
    /// 保存plc上传过的错误信息
    /// </summary>
    public struct PlcDownTimeIni
    {
        public DateTime m_dtErrorStartTime;
        public string strErrorCode;
        public string strErrorMsg;
    }

    class GetErrorInfo : SingletonTemplate<GetErrorInfo>
    {
        public delegate void CleanButton();
        public CleanButton CleanButtonEvent;
        string BasePath;
        string strDownTimeIniPath;
        string strPLCDownTimeIniPath;
        /// <summary>
        /// 字典类型，表示序号和WaitIo超时中的错误信息
        /// </summary>
        private Dictionary<int, ErrIoInfo> m_dictErrIo = new Dictionary<int, ErrIoInfo>();
        /// <summary>
        /// 字典类型，表示序号和自定义错误类型
        /// </summary>
        private Dictionary<int, UNDErrInfo> m_dictUNDErr = new Dictionary<int, UNDErrInfo>();
        /// <summary>
        /// 获取网口IP对应的名称
        /// </summary>
        private Dictionary<int, ErrMsgTcp> m_dictTcpErr = new Dictionary<int, ErrMsgTcp>();
        /// <summary>
        /// 存 PC所有的ErrorCode,Key为错误信息，Value为错误代码 
        /// </summary>
        private Dictionary<string, string> m_dictErrorCode = new Dictionary<string, string>();
        /// <summary>
        /// 存 PC所有的ErrorCode list,Key为错误信息，Value为ErrorMessage数据
        /// </summary>
        private List<ErrorMessage> ErrorCodeList = new List<ErrorMessage>();

        /// <summary>
        /// 存 PC所有的ErrorCode,Key为错误代码 ，Value为错误信息
        /// </summary>
        private Dictionary<string, string> m_dictErrorCode2 = new Dictionary<string, string>();
        /// <summary>
        /// 存 PLC所有的ErrorCode,Key为错误地址 ，Value为错误代码
        /// </summary>
        private Dictionary<string, string> m_dictPlcErrorCode = new Dictionary<string, string>();
        /// <summary>
        /// 存 PLC所有的ErrorCode,Key为错误代码 ，Value为错误信息
        /// </summary>
        private Dictionary<string, string> m_dictPlcErrorCode2 = new Dictionary<string, string>();
        /// <summary>
        /// 存储PC所有发生过的Error信息；
        /// </summary>
        private Dictionary<string, DownTimeIni> m_dictDownTimeIni = new Dictionary<string, DownTimeIni>();
        /// <summary>
        /// 存储PLC所有发生过的Error信息；
        /// </summary>
        private Dictionary<string, PlcDownTimeIni> m_dictPlcDownTimeIni = new Dictionary<string, PlcDownTimeIni>();

        /// <summary>
        /// 表示当前机台是哪一站
        /// </summary>
        public StationNum m_nCurrStation;
        protected object m_lock = new object();
        protected object m_plclock = new object();
        private bool bPcHasError = false;
        private bool bPlcHasError = false;
        protected object m_lock1 = new object();
        protected object m_lock4 = new object();
        protected object m_lock5 = new object();
        //protected object m_DownTimeInilock = new object();
        //protected object m_PlcDownTimelock = new object();
        // protected object m_lock5 = new object();

        public GetErrorInfo()
        {
            m_nCurrStation = StationNum.Hive;
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            strDownTimeIniPath = BasePath + "\\" + "DownTimeCfg";
            strPLCDownTimeIniPath = BasePath + "\\" + "DownTimeCfg" + "PLC";
        }

        public bool HavePLC
        {

            get
            {
                bool b_return = false;
                if ((GetErrorInfo.GetInstance().m_nCurrStation == StationNum.LC || GetErrorInfo.GetInstance().m_nCurrStation == StationNum.NEH || GetErrorInfo.GetInstance().m_nCurrStation == StationNum.LS) && (ErrorCodeCfg.GetInstance().HavePlc))
                    b_return = true;
                return b_return;
            }
        }



        public bool PcHasError
        {
            get
            {
                lock (m_lock4)
                {
                    return bPcHasError;
                }
            }
            set
            {
                lock (m_lock4)
                {
                    bPcHasError = value;
                }
            }
        }
        public bool PlcHasError
        {
            get
            {
                lock (m_lock5)
                {
                    return bPlcHasError;
                }
            }
            set
            {
                lock (m_lock5)
                {
                    bPlcHasError = value;
                }
            }
        }


        #region 获取错误代码
        //报警种类                  //报警错误码             //报警等级              //报警信息                                         //报警时间
        //strCategory;//ERR-XYT     //strCode;//20100       //strLevel;//ERROR      //strMsg;//第0张IO卡0640A初始化失败!错误码 =0"     // tm;

        /// <summary>
        /// 返回错误信息
        /// </summary>
        /// <param name="LastMsg"></param>
        /// <returns></returns>
        public string GetErrCode(CommonTool.WARNING_DATA LastMsg)
        {
            try
            {
                string strErrMsg = "";
                string strErrCode = "";
                switch (LastMsg.strCategory)
                {
                    case "ERR-XYT":
                        strErrMsg = XYTErrCode(LastMsg.strCode, LastMsg.strMsg);
                        break;
                    case "ERR-SSW":
                        strErrMsg = SSWErrCode(LastMsg.strCode, LastMsg.strMsg);
                        break;
                    case "ERR-PLC":
                        strErrMsg = PLCErrCode(LastMsg.strCode, LastMsg.strMsg);
                        break;
                    case "ERR-UND":
                        strErrMsg = UNDErrCode(LastMsg.strCode, LastMsg.strMsg);
                        break;
                    case "ERR-ESB01-10001":
                        strErrMsg = "急停按钮被按下";
                        break;
                    default:
                        strErrMsg = "999";
                        break;
                }
                return strErrMsg;
            }
            catch { return ""; }

        }
        private string XYTErrCode(string strCode, string strMsg)
        {
            string[] strStation = { "缓冲站", "卷料站", "托盘站", "NC供料盘站", "NC收料盘站", "成品下料Try盘站", "Coil上料站" };
            string strXYTErrCode = "";
            switch (strCode)
            {
                case "20100":
                    if (strMsg.Contains("P440"))
                        strXYTErrCode = "运动控制卡8254初始化失败";
                    else
                        strXYTErrCode = "IO卡0640A初始化失败";
                    break;
                case "20101":
                    if (strMsg.Contains("P440"))
                        strXYTErrCode = "IO卡8254读写IO失败";
                    else
                        strXYTErrCode = "IO卡0640A读写IO失败";
                    break;
                case "20102":
                    strXYTErrCode = "IO卡8254读写IO失败";
                    break;
                case "20103":
                    strXYTErrCode = "IO卡8254读写IO失败";
                    break;
                case "20104":
                    strXYTErrCode = "IO卡8254读写IO失败";
                    break;
                case "20105":
                    strXYTErrCode = "IO卡8254读写IO失败";
                    break;
                case "20106":
                    strXYTErrCode = "IO卡8254读写IO失败";
                    break;
                case "30100":
                    strXYTErrCode = "运动控制卡8254初始化失败";
                    break;
                case "30101":
                    strXYTErrCode = "运动控制卡8254读取配置文件失败";
                    break;
                case "30102":
                    strXYTErrCode = "8254板卡库文件关闭出错";
                    break;
                case "30103":
                    strXYTErrCode = "8254板卡轴上电失败";
                    break;
                case "30104":
                    strXYTErrCode = "8254板卡轴下电失败";
                    break;
                case "30105":
                    strXYTErrCode = "8254板卡轴回原点失败";
                    break;
                case "30106":
                    strXYTErrCode = "8254板卡轴以绝对位置移动失败";
                    break;
                case "30107":
                    strXYTErrCode = "8254板卡轴相对位置移动失败";
                    break;
                case "30109":
                    strXYTErrCode = "8254板卡轴正常停止失败";
                    break;
                case "30110":
                    strXYTErrCode = "8254板卡轴急停失败";
                    break;
                case "30111":
                    strXYTErrCode = "8254板卡轴速度模式旋转轴失败";
                    break;
                case "30002"://站位出现轴异常报警
                    for (int i = 0; i < strStation.Length; i++)
                    {
                        if (strMsg.Contains(strStation[i]))
                        {
                            strXYTErrCode = strStation[i] + "出现轴异常报警";
                            break;
                        }
                    }
                    break;
                case "30003"://站位等待轴到位超时
                    for (int i = 0; i < strStation.Length; i++)
                    {
                        if (strMsg.Contains(strStation[i]))
                        {
                            strXYTErrCode = strStation[i] + "等待轴到位超时";
                            break;
                        }
                    }
                    break;
                case "20001"://等待IO到位超时
                    strXYTErrCode = getWaitIOMsg(strMsg);
                    break;
                default:
                    strXYTErrCode = "999";
                    break;
            }
            return strXYTErrCode;
        }
        private string SSWErrCode(string strCode, string strMsg)
        {

            string strSSWErrCode = "";
            switch (strCode)
            {
                case "51210":
                    strSSWErrCode = get51210Msg(strMsg);
                    break;
                case "50109":
                    strSSWErrCode = "轴号索引值错误,无法找到对应的运动控制卡";
                    break;
                case "20004":
                    strSSWErrCode = "等待接收命令超时";
                    break;
                case "20003":
                    strSSWErrCode = "网口读取超时";
                    break;
                case "20002":
                    strSSWErrCode = "串口读取超时";
                    break;
                case "50002":
                    strSSWErrCode = "等待系统值寄存器超时";
                    break;
                case "50001":
                    strSSWErrCode = "等待系统寄存器超时";
                    break;
                case "51220":
                    strSSWErrCode = "串口打开失败";
                    break;
                default:
                    strSSWErrCode = "999";
                    break;
            }
            return strSSWErrCode;
        }
        private string PLCErrCode(string strCode, string strMsg)
        {
            string strPLCErrCode = "";
            if (strMsg.Contains("PLC元件地址错误"))
                strPLCErrCode = "PLC元件地址错误";
            else if (strMsg.Contains("PLC读取失败"))
                strPLCErrCode = "PLC读取失败,请检查通信是否正确";
            return strPLCErrCode;
        }
        private string UNDErrCode(string strCode, string strMsg)
        {
            string strUNDErrCode = "";
            foreach (var undError in m_dictUNDErr)
            {
                if (strMsg.Contains(undError.Value.strErrorMsg))
                {
                    strUNDErrCode = undError.Value.strErrorInfo;
                    break;
                }
            }
            return strUNDErrCode;
        }
        private string getWaitIOMsg(string strMsg)
        {//eg.//缓冲站等待IO点等待IO1.2启动为True超时
         //缓冲站等待IO点等待IO2.20供料盘气缸降到位为False超时
         //ABB上下料站等待IO点IO1.9供料顶升气缸升到位有效电平为True达到3秒超时
         // 缓冲站 等待IO点 等待IO2.20 供料盘气缸降到位 为 False超时
            string strWaitIOMsg = "";
            foreach (var errio in m_dictErrIo)
            {
                if (strMsg.Contains(errio.Value.strIoName))
                {
                    strWaitIOMsg = errio.Value.strErrorInfo;
                    break;
                }
            }
            return strWaitIOMsg;
        }
        private string get51210Msg(string strMsg)
        {
            string strErrMsg = "";
            if (strMsg.Contains("未将对象引用设置到对象的实例"))
                strErrMsg = "IP设置错误";
            else
            {
                foreach (var m_TCP in m_dictTcpErr)
                {
                    if (strMsg.Contains(m_TCP.Value.m_strIP))
                    {
                        strErrMsg = m_TCP.Value.m_ErrorName + "网络通讯出现错误";
                    }
                }
            }
            return strErrMsg;
        }
        #endregion
        #region   //打开软件后加载Xml中的信息
        /// <summary>
        /// 从ErrorCode的XML文档读取信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictErrIo.Clear();
            m_dictUNDErr.Clear();
            XmlNodeList xnl = doc.SelectNodes("/ErrCodeIoCfg");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        XmlElement xe = (XmlElement)xn;

                        if (xn.Name == "IoName")
                        {
                            #region IoName
                            foreach (XmlNode item in xn.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }
                                XmlElement xeItem = (XmlElement)item;
                                int index = Convert.ToInt32(xeItem.GetAttribute("序号").Trim());
                                string strIoName = xeItem.GetAttribute("IO名称").Trim();
                                string strErrorInfo = xeItem.GetAttribute("ErrorInfo").Trim();

                                if (string.IsNullOrEmpty(strIoName))
                                {
                                    continue;
                                }
                                ErrIoInfo ErrorIos = new ErrIoInfo();
                                ErrorIos.index = index;
                                ErrorIos.strIoName = strIoName;
                                ErrorIos.strErrorInfo = strErrorInfo;
                                m_dictErrIo.Add(index, ErrorIos);
                            }
                            #endregion
                        }
                        else if (xn.Name == "ErrorMsg")
                        {
                            #region IoName
                            foreach (XmlNode item in xn.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }
                                XmlElement xeItem = (XmlElement)item;
                                int index = Convert.ToInt32(xeItem.GetAttribute("序号").Trim());
                                string strErrorMsg = xeItem.GetAttribute("报警信息").Trim();
                                string strErrorInfo = xeItem.GetAttribute("ErrorInfo").Trim();

                                if (string.IsNullOrEmpty(strErrorMsg))
                                {
                                    continue;
                                }
                                UNDErrInfo UNDError = new UNDErrInfo();
                                UNDError.index = index;
                                UNDError.strErrorMsg = strErrorMsg;
                                UNDError.strErrorInfo = strErrorInfo;
                                m_dictUNDErr.Add(index, UNDError);
                            }
                            #endregion
                        }

                    }
                }
            }
        }

        private static readonly object R_Wlock = new object();

        /// <summary>
        /// 写入机台的状态信息（主要报Error时的）
        /// </summary>
        /// <returns></returns>
        public void WriteMachineStateXML(StateMessage temp, bool Init = false, bool b_writeToXml = false)
        {
            lock (R_Wlock)
            {
                if (!b_writeToXml)
                {
                    HiveMgr.machine_state = temp;
                }


                try
                {
                    string cfg = Application.StartupPath + $"\\ErrorCode_{GetErrorInfo.GetInstance().m_nCurrStation}.xml";
                    XmlDocument doc;
                    doc = new XmlDocument();
                    doc.Load(cfg);

                    XmlNodeList nodeList = doc.SelectSingleNode("ErrorCode").ChildNodes;

                    foreach (XmlNode xn in nodeList)
                    {
                        XmlElement xe = (XmlElement)xn;
                        if (xe.Name == "StateInformation")
                        {
                            xe.SetAttribute("状态", Init ? "2" : temp.MachineState.ToString());
                            xe.SetAttribute("报警代码", Init ? null : temp.code);
                            xe.SetAttribute("报警信息", Init ? null : temp.strMsg);
                            break;
                        }
                    }
                    doc.Save(cfg);
                }
                catch
                {

                }
            }

        }

        /// <summary>
        /// 读取机台之前的状态
        /// </summary>
        /// <returns></returns>
        public StateMessage ReadMachineStateXML(bool b_readfromXml = false)
        {
            lock (R_Wlock)
            {
                if (!b_readfromXml)
                {
                    StateMessage temp = new StateMessage();
                    temp = HiveMgr.machine_state;
                    return temp;
                }


                try
                {
                    string cfg = Application.StartupPath + $"\\ErrorCode_{GetErrorInfo.GetInstance().m_nCurrStation}.xml";
                    XmlDocument doc;
                    doc = new XmlDocument();
                    doc.Load(cfg);
                    StateMessage temp = new StateMessage();

                    XmlNodeList nodeList = doc.SelectSingleNode("ErrorCode").ChildNodes;

                    foreach (XmlNode xn in nodeList)
                    {
                        XmlElement xe = (XmlElement)xn;
                        if (xe.Name == "StateInformation")
                        {
                            temp.MachineState = Convert.ToInt16(xe.GetAttribute("状态"));
                            temp.code = xe.GetAttribute("报警代码");
                            temp.strMsg = xe.GetAttribute("报警信息");
                            break;
                        }
                    }
                    return temp;
                }
                catch
                {
                    StateMessage temp = new StateMessage();
                    return temp;
                }

            }


        }


        public void ReadErrorCodeXML(XmlDocument doc)
        {
            m_dictErrorCode.Clear();
            m_dictErrorCode2.Clear();
            ErrorCodeList.Clear();
            XmlNodeList xnl = doc.SelectNodes("/ErrorCode/" + m_nCurrStation.ToString());
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        int index = Convert.ToInt32(xe.GetAttribute("序号").Trim());
                        string strErrorCode = xe.GetAttribute("报警代码").Trim();
                        string strErrorInfo = xe.GetAttribute("报警信息").Trim();
                        string strErrorInfoEng = xe.GetAttribute("报警英文信息").Trim();
                        string strErrorLevel = xe.GetAttribute("报警级别").Trim();
                        if (string.IsNullOrEmpty(strErrorCode))
                        {
                            continue;
                        }

                        ErrorMessage temp = new ErrorMessage();
                        temp.strMsgEnglish = strErrorInfoEng;
                        temp.strMsgChinese = strErrorInfo;
                        temp.Level = strErrorLevel;
                        temp.code = strErrorCode;
                        ErrorCodeList.Add(temp);

                        m_dictErrorCode.Add(strErrorInfo, strErrorCode);
                        m_dictErrorCode2.Add(strErrorCode, strErrorInfo);
                    }
                }
            }
        }
        public void ReadPlcErrorCodeXML(XmlDocument doc)
        {
            m_dictPlcErrorCode2.Clear();
            m_dictPlcErrorCode.Clear();
            XmlNodeList xnl = doc.SelectNodes("/ErrorCode/" + m_nCurrStation.ToString());
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        int index = Convert.ToInt32(xe.GetAttribute("序号").Trim());
                        string strErrorCode = xe.GetAttribute("报警代码").Trim();
                        string strErrorInfo = xe.GetAttribute("报警信息").Trim();
                        string strErrorAddress = xe.GetAttribute("报警地址").Trim();
                        if (string.IsNullOrEmpty(strErrorCode))
                        {
                            continue;
                        }
                        m_dictPlcErrorCode2.Add(strErrorCode, strErrorInfo);
                        m_dictPlcErrorCode.Add(strErrorAddress, strErrorCode);
                    }
                }
            }
        }
        public void GetStationTplinks()
        {
            m_dictTcpErr.Clear();
            string[] strName;
            string[] ErrorName;
            #region 根据每一站的不同，转换报警信息。
            switch (m_nCurrStation)
            {
                case StationNum.LS:
                    strName = new string[] { "上下料机械手", "PLC" };
                    ErrorName = new string[] { "上料机械手", "PLC" };
                    break;
                case StationNum.LC:
                    strName = new string[] { "上料机器人", "PLC通讯" };
                    ErrorName = new string[] { "上料机械手", "PLC" };
                    break;
                case StationNum.NEH:
                    strName = new string[] { "搬运机械手", "PLC" };
                    ErrorName = new string[] { "搬运机械手", "PLC" };
                    break;
                case StationNum.ANC:
                    strName = new string[] { "CF_Load机器人", "CF_Peel机器人", "CF_Assemble机器人", "康耐视通讯", "康耐视扫码枪" };
                    ErrorName = new string[] { "上料机械手", "撕膜机械手", "组装机械手", "康耐视", "扫码枪" };
                    break;
                case StationNum.AEC:
                    strName = new string[] { "撕膜机器人", "组装机器人", "康耐视通讯", "上料机器人", "条码枪" };
                    ErrorName = new string[] { "撕膜机械手", "组装机械手", "康耐视", "上料机械手", "扫码枪" };
                    break;
                default:
                    strName = new string[] { "上下料机械手", "PLC","上料机器人", "搬运机械手", "CF_Load机器人", "CF_Peel机器人",
                                             "CF_Assemble机器人", "康耐视通讯", "扫码枪", "撕膜机器人", "组装机器人", "上料机器人", "条码枪" };
                    ErrorName = new string[] { "上料机械手", "PLC", "上料机械手", "搬运机械手", "上料机械手", "撕膜机械手",
                                               "组装机械手", "康耐视", "扫码枪", "撕膜机械手", "组装机械手", "上料机械手", "扫码枪" };
                    break;
            }
            #endregion
            int num = TcpMgr.GetInstance().Count;
            for (int i = 0; i < num; i++)
            {
                TcpLink m_tcp = TcpMgr.GetInstance().GetTcpLink(i);
                ErrMsgTcp m_ErrMsgTcp = new ErrMsgTcp();
                m_ErrMsgTcp.m_nIndex = i;
                m_ErrMsgTcp.m_strName = m_tcp.m_strName;
                m_ErrMsgTcp.m_strIP = m_tcp.m_strIP;
                m_ErrMsgTcp.m_nPort = m_tcp.m_nPort;

                for (int NameIndex = 0; NameIndex < strName.Length; NameIndex++)
                {
                    if (m_ErrMsgTcp.m_strName.Contains(strName[NameIndex]))
                    {
                        m_ErrMsgTcp.m_ErrorName = ErrorName[NameIndex];
                        break;
                    }
                }
                m_dictTcpErr.Add(i, m_ErrMsgTcp);
            }

        }
        #endregion
        #region  PC报警
        /// <summary>
        /// 获取产生过的错误ini
        /// </summary>
        public void LoadDownTimeIni()
        {
            //   m_dictDownTimeIni.Clear();
            DownTimeDataClear();

            try
            {
                if (!Directory.Exists(strDownTimeIniPath))
                {
                    Directory.CreateDirectory(strDownTimeIniPath);
                }
                //得到路径下所有文件名
                string[] strFile = Directory.GetFiles(strDownTimeIniPath, "*.ini");
                foreach (string s in strFile)
                {//仅加载一个错误
                    if (PlcHasError || PcHasError)
                    {
                        // File.Delete(s);
                        continue;
                    }
                    //得到不包含后缀类型的的文件名
                    string strTmp = Path.GetFileNameWithoutExtension(s);

                    //    if (m_dictErrorCode.ContainsValue(strTmp) && (!m_dictDownTimeIni.ContainsKey(strTmp)))
                    if (m_dictErrorCode.ContainsValue(strTmp) && (!DownTimeContainsKey(strTmp)))
                    {
                        DownTimeIni mDownTimeIni = new DownTimeIni();
                        mDownTimeIni.strErrorCode = IniOperation.GetStringValue(s, "ErrorLog", "ErrorCode", null);
                        mDownTimeIni.strErrorMsg = m_dictErrorCode2[mDownTimeIni.strErrorCode];
                        try
                        {
                            if (mDownTimeIni.strErrorCode == null || mDownTimeIni.strErrorMsg == null)
                            {
                                File.Delete(s);
                                continue;
                            }
                            string strStart = IniOperation.GetStringValue(s, "ErrorLog", "ErrStartTime", null);
                            if (strStart != null)
                                mDownTimeIni.m_dtErrorStartTime = Convert.ToDateTime(strStart);
                            else
                            {
                                //  File.Delete(s);//J加载成功后不删除配置文件，防止软件再次关闭
                                continue;
                            }
                            DownTimeAddData(mDownTimeIni.strErrorCode, mDownTimeIni);
                            //   m_dictDownTimeIni.Add(mDownTimeIni.strErrorCode, mDownTimeIni);
                            PcHasError = true;
                        }
                        catch
                        {
                            File.Delete(s);
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Delete(s);
                        }
                        catch
                        {
                            WarningMgr.GetInstance().Warning("DownTimeIni加载错误");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 向字典m_dictDownTimeIni中添加错误信息
        /// </summary>
        /// <param name="strErrorCode"></param>
        /// <param name="mDownTimeIni"></param>
        public void DownTimeAddData(string strErrorCode, DownTimeIni mDownTimeIni)
        {
            lock (m_lock)
            {
                if (!m_dictDownTimeIni.ContainsKey(strErrorCode))
                {
                    m_dictDownTimeIni.Add(strErrorCode, mDownTimeIni);
                }
                else
                {
                    m_dictDownTimeIni[strErrorCode] = mDownTimeIni;
                }
            }

        }
        public bool DownTimeData(string strKey, out DownTimeIni DownTimeIni)
        {
            lock (m_lock)
            {
                DownTimeIni mDownTimeIni = new DownTimeIni();
                if (m_dictDownTimeIni.ContainsKey(strKey))
                {
                    mDownTimeIni = m_dictDownTimeIni[strKey];
                    DownTimeIni = mDownTimeIni;
                    return true;
                }
                DownTimeIni = mDownTimeIni;
                return false;
            }
        }
        public bool DownTimeContainsKey(string strKey)
        {
            lock (m_lock)
            {
                return m_dictDownTimeIni.ContainsKey(strKey);
            }

        }
        public int DownTimeHasKey()
        {
            lock (m_lock)
            {
                try { return m_dictDownTimeIni.Count; }
                catch { return 0; }

            }
        }
        public bool DownTimeDataClear()
        {
            lock (m_lock)
            {
                try
                {
                    m_dictDownTimeIni.Clear();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        public Dictionary<string, DownTimeIni> GetAllDownTimeData()
        {
            lock (m_lock)
            {
                return m_dictDownTimeIni;
            }
        }


        public bool ErrorCodeContainsKey(bool b_Key_Value, string strKey_Value)
        {
            if (b_Key_Value)
                return m_dictErrorCode.ContainsKey(strKey_Value);
            else
                return m_dictErrorCode.ContainsValue(strKey_Value);
        }
        public bool ErrorCodeData(string strKey, out string code)
        {

            if (m_dictErrorCode.ContainsKey(strKey))
            {
                code = m_dictErrorCode[strKey];
                return true;
            }
            code = "";
            return false;

        }

        //根据中文描述，查询Error信息
        public bool ErrorMsg(string strKey, out ErrorMessage Msg)
        {
            ErrorCodeList.Where(n => n.Level == strKey);
            var sss = ErrorCodeList.Where(n => n.strMsgChinese == strKey).ToList();
            if (sss.Count!=0)
            {
                Msg = (ErrorMessage)sss[0];
            }
            else
            {
                Msg = new ErrorMessage();
                Msg.code = "O99OOOO-01-03";
                Msg.Level = "error";
                Msg.strMsgChinese = strKey;
                Msg.strMsgEnglish = strKey;
                return false;
            }
            

            if (Msg.code == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  PLC报警
        /// <summary>
        /// 获取产生过的错误ini
        /// </summary>
        public void LoadPlcDownTimeIni()
        {
            // m_dictPlcDownTimeIni.Clear();
            PlcDownTimeDataClear();
            try
            {
                if (!Directory.Exists(strPLCDownTimeIniPath))
                {
                    Directory.CreateDirectory(strPLCDownTimeIniPath);
                }
                //得到路径下所有文件名
                string[] strFile = Directory.GetFiles(strPLCDownTimeIniPath, "*.ini");
                foreach (string s in strFile)
                {
                    if (PlcHasError || PcHasError)
                    {
                        // File.Delete(s);
                        continue;
                    }
                    //得到不包含后缀类型的的文件名
                    string strTmp = Path.GetFileNameWithoutExtension(s);

                    //if (m_dictPlcErrorCode2.ContainsValue(strTmp) && (!m_dictPlcDownTimeIni.ContainsKey(strTmp)))
                    if (m_dictPlcErrorCode2.ContainsValue(strTmp) && (!PlcDownTimeContainsKey(strTmp)))
                    {
                        PlcDownTimeIni mPlcDownTimeIni = new PlcDownTimeIni();
                        mPlcDownTimeIni.strErrorCode = IniOperation.GetStringValue(s, "ErrorLog", "ErrorCode", null);
                        mPlcDownTimeIni.strErrorMsg = m_dictPlcErrorCode2[mPlcDownTimeIni.strErrorCode];
                        try
                        {
                            if (mPlcDownTimeIni.strErrorCode == null || mPlcDownTimeIni.strErrorMsg == null)
                            {
                                File.Delete(s);
                                continue;
                            }

                            string strStart = IniOperation.GetStringValue(s, "ErrorLog", "ErrStartTime", null);
                            if (strStart != null)
                                mPlcDownTimeIni.m_dtErrorStartTime = Convert.ToDateTime(strStart);
                            else
                            {
                                // File.Delete(s);
                                continue;
                            }
                            // m_dictPlcDownTimeIni.Add(mPlcDownTimeIni.strErrorCode, mPlcDownTimeIni);
                            PlcDownTimeAddData(mPlcDownTimeIni.strErrorCode, mPlcDownTimeIni);
                            PlcHasError = true;
                        }
                        catch
                        {
                            File.Delete(s);
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Delete(s);
                        }
                        catch
                        {
                            WarningMgr.GetInstance().Warning("DownTimeIni");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 向字典m_dictDownTimeIni中添加错误信息
        /// </summary>
        /// <param name="strErrorCode"></param>
        /// <param name="mDownTimeIni"></param>
        public void PlcDownTimeAddData(string strErrorCode, PlcDownTimeIni mPlcDownTimeIni)
        {
            lock (m_plclock)
            {
                if (!m_dictPlcDownTimeIni.ContainsKey(strErrorCode))
                {
                    m_dictPlcDownTimeIni.Add(strErrorCode, mPlcDownTimeIni);
                }
                else
                {
                    m_dictPlcDownTimeIni[strErrorCode] = mPlcDownTimeIni;
                }
            }

        }
        public bool PlcDownTimeData(string strKey, out PlcDownTimeIni plcDownTimeIni)
        {
            lock (m_plclock)
            {
                PlcDownTimeIni mPlcDownTimeIni = new PlcDownTimeIni();
                if (m_dictPlcDownTimeIni.ContainsKey(strKey))
                {
                    mPlcDownTimeIni = m_dictPlcDownTimeIni[strKey];
                    plcDownTimeIni = mPlcDownTimeIni;
                    return true;
                }
                plcDownTimeIni = mPlcDownTimeIni;
                return false;
            }
        }
        public bool PlcDownTimeContainsKey(string strKey)
        {
            lock (m_plclock)
            {
                return m_dictPlcDownTimeIni.ContainsKey(strKey);
            }
        }


        public bool PlcDownTimeDataClear()
        {
            lock (m_plclock)
            {
                try
                {
                    m_dictPlcDownTimeIni.Clear();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        public Dictionary<string, PlcDownTimeIni> GetAllPlcDownTimeData()
        {
            lock (m_plclock)
            {
                return m_dictPlcDownTimeIni;
            }
        }



        public bool ErrorCodePlcContainsKey(bool b_Key_Value, string strKey_Value)
        {
            if (b_Key_Value)
                return m_dictPlcErrorCode2.ContainsKey(strKey_Value);
            else
                return m_dictPlcErrorCode2.ContainsValue(strKey_Value);
        }
        public bool ErrorCodePlcData(string strKey, out string code)
        {

            if (m_dictPlcErrorCode.ContainsKey(strKey))
            {
                code = m_dictPlcErrorCode[strKey];
                return true;
            }
            code = "";
            return false;

        }
        public bool ErrorCodePlcData2(string strKey, out string Msg)
        {

            if (m_dictPlcErrorCode2.ContainsKey(strKey))
            {
                Msg = m_dictPlcErrorCode2[strKey];
                return true;
            }
            Msg = "";
            return false;

        }
        #endregion
        public void SaveDownTimeLog(bool b_IsPlc = false)
        {
            lock (m_lock1)
            {
                try
                {
                    string strSavePath = SystemMgr.GetInstance().GetDataPath() + "\\DownTimeLog";
                    string fileName = String.Format("{0}_{1}.csv", "DownTimeLog", DateTime.Now.ToString("yyyyMMdd"));
                    //记录文件
                    CsvOperationEx csv = new CsvOperationEx(strSavePath, fileName);
                    //csv.BInsertQuota = false;
                    csv.BQuota = false;
                    int row = 0, col = 0;
                    DateTime tTimeNow = DateTime.Now;
                    if (!b_IsPlc)
                    {
                        #region PC部分
                        //  foreach (var item in m_dictDownTimeIni)
                        Dictionary<string, DownTimeIni> m_dictNewDownTimeIni = GetAllDownTimeData();
                        foreach (var item in m_dictNewDownTimeIni)
                        {
                            TimeSpan timespan1 = tTimeNow - item.Value.m_dtErrorStartTime;

                            col = 0;
                            csv[row, col] = item.Value.m_dtErrorStartTime.ToString("yyyy/MM/dd");
                            col++;
                            csv[row, col] = item.Value.m_dtErrorStartTime.ToString("HH:mm:ss");
                            col++;
                            csv[row, col] = "Error";
                            col++;
                            csv[row, col] = item.Value.strErrorCode;
                            col++;
                            csv[row, col] = item.Value.strErrorMsg;
                            col++;
                            csv[row, col] = tTimeNow.ToString("yyyy/MM/dd");
                            col++;
                            csv[row, col] = tTimeNow.ToString("HH:mm:ss");
                            col++;
                            csv[row, col] = timespan1.ToString();
                            col++;
                            row++;
                        }
                        csv.Save();
                        //  m_dictDownTimeIni.Clear();
                        DownTimeDataClear();
                        if (!Directory.Exists(strDownTimeIniPath))
                        {
                            Directory.CreateDirectory(strDownTimeIniPath);
                        }
                        string[] strFile = Directory.GetFiles(strDownTimeIniPath, "*.ini");

                        foreach (string s in strFile)
                        {
                            try
                            {
                                File.Delete(s);
                            }
                            catch { }
                        }
                        #endregion
                    }
                    else
                    {
                        #region PLC部分
                        //     foreach (var item in m_dictPlcDownTimeIni)
                        Dictionary<string, PlcDownTimeIni> m_dictNewPlcDownTimeIni = GetAllPlcDownTimeData();
                        foreach (var item in m_dictNewPlcDownTimeIni)
                        {
                            TimeSpan timespan1 = tTimeNow - item.Value.m_dtErrorStartTime;

                            col = 0;
                            csv[row, col] = item.Value.m_dtErrorStartTime.ToString("yyyy/MM/dd");
                            col++;
                            csv[row, col] = item.Value.m_dtErrorStartTime.ToString("HH:mm:ss");
                            col++;
                            csv[row, col] = "Error";
                            col++;
                            csv[row, col] = item.Value.strErrorCode;
                            col++;
                            csv[row, col] = item.Value.strErrorMsg;
                            col++;
                            csv[row, col] = tTimeNow.ToString("yyyy/MM/dd");
                            col++;
                            csv[row, col] = tTimeNow.ToString("HH:mm:ss");
                            col++;
                            csv[row, col] = timespan1.ToString();
                            col++;
                            row++;
                        }
                        csv.Save();
                        //    m_dictPlcDownTimeIni.Clear();
                        PlcDownTimeDataClear();
                        if (!Directory.Exists(strPLCDownTimeIniPath))
                        {
                            Directory.CreateDirectory(strPLCDownTimeIniPath);
                        }
                        string[] strFile = Directory.GetFiles(strPLCDownTimeIniPath, "*.ini");

                        foreach (string s in strFile)
                        {
                            try
                            {
                                File.Delete(s);
                            }
                            catch { }
                        }
                        #endregion
                    }
                }
                catch { }
            }

        }
    }
    public class GetErrIOs : SingletonTemplate<GetErrIOs>
    {
        string BasePath;
        public void ErrCodeIoCfg()
        {
            GetErrorInfo.GetInstance().GetStationTplinks();
            string cfg = Application.StartupPath + "\\ErrCodeIoCfg.xml";
            if (File.Exists(cfg))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);

                GetErrorInfo.GetInstance().ReadCfgFromXml(doc);
            }
        }
        public void ErrorCode()
        {
            string cfg = Application.StartupPath + $"\\ErrorCode_{GetErrorInfo.GetInstance().m_nCurrStation}.xml";

            if (File.Exists(cfg))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);
                GetErrorInfo.GetInstance().ReadErrorCodeXML(doc);
                GetErrorInfo.GetInstance().LoadDownTimeIni();
            }
        }
        public void PlcErrorCode()
        {
            //   if ((GetErrorInfo.GetInstance().m_nCurrStation == StationNum.LC || GetErrorInfo.GetInstance().m_nCurrStation == StationNum.NEH || GetErrorInfo.GetInstance().m_nCurrStation == StationNum.LS) && (ErrorCodeCfg.GetInstance().HavePlc))
            if (GetErrorInfo.GetInstance().HavePLC)
            {
                string cfg = Application.StartupPath + "\\ErrorCodePlc.xml";

                if (File.Exists(cfg))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cfg);
                    GetErrorInfo.GetInstance().ReadPlcErrorCodeXML(doc);
                    GetErrorInfo.GetInstance().LoadPlcDownTimeIni();
                }
            }

        }

    }
    public partial class ErrorCodeCfg : SingletonTemplate<ErrorCodeCfg>
    {
        private string m_strMachineID = "Machine003";
        private string m_strLineID = "Line001";
        private string m_strUrl = "http://10.32.12.43/api/em";
        private string m_strIniFile = AppDomain.CurrentDomain.BaseDirectory + "ErrorCodeCfg.ini";//获取当前路径
        private bool b_HavePlc = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ErrorCodeCfg()
        {
            ReadCfg();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ErrorCodeCfg()
        {
            // WriteCfg();
        }

        /// <summary>
        /// 读配置文件
        /// </summary>
        public void ReadCfg()
        {
            try
            {
                DtLoad();
            }
            catch (Exception)
            {

                // throw;
            }

        }

        private void DtLoad()
        {
            try
            {
                m_strMachineID = IniOperation.GetStringValue(m_strIniFile, "ErrorCodeCfg", "MachineID", "Machine003");
                m_strLineID = IniOperation.GetStringValue(m_strIniFile, "ErrorCodeCfg", "LineID", "Line001");
                m_strUrl = IniOperation.GetStringValue(m_strIniFile, "ErrorCodeCfg", "Url", "http://10.32.12.43/api/em");
                b_HavePlc = Convert.ToBoolean(IniOperation.GetStringValue(m_strIniFile, "ErrorCodeCfg", "HavePlc", "False"));
            }
            catch { }
        }
        public void DtSave()
        {
            try
            {
                IniOperation.WriteValue(m_strIniFile, "ErrorCodeCfg", "MachineID", MachineID);
                IniOperation.WriteValue(m_strIniFile, "ErrorCodeCfg", "LineID", LineID);
                IniOperation.WriteValue(m_strIniFile, "ErrorCodeCfg", "HavePlc", HavePlc.ToString());
            }
            catch { }
        }

        public bool HavePlc
        {
            get
            {
                return b_HavePlc;
            }
            set
            {
                b_HavePlc = value;
            }
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineID
        {
            get
            {
                return m_strMachineID;
            }

            set
            {
                m_strMachineID = value;
            }
        }
        public string LineID
        {
            get
            {
                return m_strLineID;
            }
            set
            {
                m_strLineID = value;
            }
        }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string Url
        {
            get
            {
                return m_strUrl;
            }

            set
            {
                m_strUrl = value;
            }
        }
    }
}
