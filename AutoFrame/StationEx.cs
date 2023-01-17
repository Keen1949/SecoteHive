//2019-04-26 Binggoo 1.加入运动暂停恢复机制
//2019-06-03 Binggoo 1.把暂停和恢复机制从StationEx中移到MotionMgr中处理
//2019-06-04 Binggoo 1.Wait函数在超时弹出对话框之前需要CheckContinue，否则会弹出对话框卡死界面。
//2019-06-05 Binggoo 1.在等待正负限位、原点信号的过程中如果报警或急停，抛出错误。
//2019-07-08 Binggoo 1.TcpCmd加入分割符
//2019-07-23 Binggoo 1. AxisGoTo加入轴掩膜功能，用于屏蔽哪个轴不运动
//2019-08-13 Binggoo 1. 加入自定义速度运动
//2019-08-16 Binggoo 1. 加入Error方法，让流程立马报错停止。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using Communicate;
using System.Threading;
using CommonTool;
using System.Windows.Forms;
using ToolEx;

namespace AutoFrame
{
    /// <summary>
    /// 轴运动优先级
    /// </summary>
    public class MotionPriority
    {
        //用于设置X/Y/Z/U/A/B/C/D执行顺序
        public static readonly int X_FIRST = 0x0001;
        public static readonly int Y_FIRST = 0x0002;
        public static readonly int Z_FIRST = 0x0004;
        public static readonly int U_FIRST = 0x0008;
        public static readonly int A_FIRST = 0x0010;
        public static readonly int B_FIRST = 0x0020;
        public static readonly int C_FIRST = 0x0040;
        public static readonly int D_FIRST = 0x0080;

        //用于设置X/Y/Z/U/A/B/C/D掩膜，即哪个轴不运动
        public static readonly int X_MASK = 0x00010000;
        public static readonly int Y_MASK = 0x00020000;
        public static readonly int Z_MASK = 0x00040000;
        public static readonly int U_MASK = 0x00080000;
        public static readonly int A_MASK = 0x00100000;
        public static readonly int B_MASK = 0x00200000;
        public static readonly int C_MASK = 0x00400000;
        public static readonly int D_MASK = 0x00800000;
    }

    public class StationEx : StationBase
    {
        protected Robot m_Robot;

        private const int RESPONSE_TIME_MS = 200;

        /// <summary>
        /// 气缸名称数组
        /// </summary>
        public string[] m_cylinders = new string[0];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strName">站位名称</param>
        public StationEx(string strName) : base(strName)
        {

        }

        /// <summary>
        /// 等待网络回复以某字符串开头和某字符串结尾的字符串数据
        /// </summary>
        /// <param name="tcplink">TCP对象</param>
        /// <param name="strData">接收到的数据</param>
        /// <param name="strStartWith">开头字符串</param>
        /// <param name="strEndWith">结尾字符串</param>
        /// <param name="nTimeOutS">超时时间，单位秒 0:系统时间 -1:一直等待 其他:其他时间</param>
        /// <param name="bShowDialog">是否弹出超时对话框</param>
        /// <param name="bPause">是否响应系统暂停</param>
        /// <returns>0：未超时 1：超时</returns>
        public virtual int wait_recevie_cmd(TcpLink tcplink, out string strData, string strStartWith, string strEndWith = "", int nTimeOutS = -1, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待网络回复以{0}开头并且以{1}结尾字符串数据";
            string str2 = "等待网络回复以{0}结尾的字符串数据";
            string str3 = "等待网络回复以{0}开头的字符串数据";
            string str4 = "收到网络回复{0}";
            string str5 = "收到网络回复{0}，继续接收";
            string str6 = "等待网络回复以{0}开头并且以{1}结尾字符串数据超时";
            string str7 = "等待网络回复以{0}结尾的字符串数据超时";
            string str8 = "等待网络回复以{0}开头的字符串数据超时";
            string str9 = "等待网络回复超过{0}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for network reply to start with {0} and end with {1} string data";
                str2 = "Waiting for network reply to string data ending with {0}";
                str3 = "Waiting for network to reply to the string data starting with {0}";
                str4 = "Received network reply {0}";
                str5 = "Received network reply {0}, continue to receive";
                str6 = "Timeout waiting for network reply to start with {0} and end with {1} string data";
                str7 = "Timeout waiting for network reply to string data ending in {0}";
                str8 = "Timeout waiting for network reply to string data starting with {0}";
                str9 = "Wait for network reply for more than {0} seconds, continue to wait";
            }

            int nRet = 0;
            strData = "";

            if (strStartWith.Length > 0 && strEndWith.Length > 0)
            {
                //等待开头和结尾
                base.ShowLog(String.Format(str1, strStartWith, strEndWith));
            }
            else if (strEndWith.Length > 0)
            {
                //等待结尾
                base.ShowLog(String.Format(str2, strEndWith));
            }
            else
            {
                //等待开头
                base.ShowLog(String.Format(str3, strStartWith));
            }


            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                Thread.Sleep(100);
            }
            else
            {
                DateTime timeStart = DateTime.Now;
                string sRead = "", sReceive = "";
                int nTime = nTimeOutS;

                bool bTips = false;
                int nDefaultTimeS = GetCommTimeOut();

                while (true)
                {
                    //不响应暂停
                    this.CheckContinue(false, false);

                    tcplink.ReadLine(out sRead);

                    sReceive += sRead;

                    if (strStartWith.Length > 0 && strEndWith.Length > 0
                        && sReceive.StartsWith(strStartWith)
                        && sReceive.EndsWith(strEndWith))
                    {
                        //等待开头和结尾
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (strStartWith.Length == 0 && strEndWith.Length > 0
                        && sReceive.EndsWith(strEndWith))
                    {
                        //等待结尾
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (strStartWith.Length > 0 && strEndWith.Length == 0
                        && sReceive.StartsWith(strStartWith))
                    {
                        //等待开头
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (sRead.Length > 0)
                    {
                        base.ShowLog(String.Format(str5, sReceive), LogLevel.Warn);
                    }


                    if (nTimeOutS == 0)
                    {
                        nTime = nDefaultTimeS;
                    }

                    TimeSpan span = DateTime.Now - timeStart;
                    if (nTimeOutS != -1)
                    {
                        if (span.TotalMilliseconds > nTime * 1000)
                        {
                            string sMsg = "";

                            if (strStartWith.Length > 0 && strEndWith.Length > 0)
                            {
                                //等待开头和结尾
                                sMsg = String.Format(str6, strStartWith, strEndWith);
                            }
                            else if (strEndWith.Length > 0)
                            {
                                //等待结尾
                                sMsg = String.Format(str7, strEndWith);
                            }
                            else
                            {
                                //等待开头
                                sMsg = String.Format(str8, strStartWith);
                            }

                            ShowLog(sMsg, LogLevel.Error);

                            if (bShowDialog)
                            {
                                //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                                CheckContinue(false, bPause);//确保流程能够暂停住
                                //if (ShowTimeOutDlg(sMsg, "90001,ERR-XYT,") == 0)//为0时表示继续等待
                                if (ShowTimeOutDlg(sMsg, ErrorType.Err_Tcp_TimeOut,
                                    tcplink.m_strName) == 0)
                                {
                                    timeStart = DateTime.Now;
                                }
                                else
                                {
                                    nRet = 1;
                                    break;
                                }
                            }
                            else
                            {
                                nRet = 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (!bTips && span.TotalMilliseconds > nDefaultTimeS * 1000)
                        {
                            bTips = true;
                            ShowLog(string.Format(str9, nDefaultTimeS), LogLevel.Warn);
                        }
                    }

                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                }

            }

            CheckContinue(false, bPause);//确保流程能够暂停住 
            return nRet;
        }

        /// <summary>
        /// 等待串口回复以某字符串开头和某字符串结尾的字符串数据
        /// </summary>
        /// <param name="comlink">串口对象</param>
        /// <param name="strData">接收到的数据</param>
        /// <param name="strStartWith">开头字符串</param>
        /// <param name="strEndWith">结尾字符串</param>
        /// <param name="nTimeOutS">超时时间，单位秒 0:系统时间 -1:一直等待 其他:其他时间</param>
        /// <param name="bShowDialog">是否弹出超时对话框</param>
        /// <param name="bPause">是否响应系统暂停</param>
        /// <returns>是否超时，超时返回false，不超时返回true</returns>
        public virtual int wait_recevie_cmd(ComLink comlink, out string strData, string strStartWith, string strEndWith = "", int nTimeOutS = -1, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待串口回复以{0}开头并且以{1}结尾字符串数据";
            string str2 = "等待串口回复以{0}结尾的字符串数据";
            string str3 = "等待串口回复以{0}开头的字符串数据";
            string str4 = "收到串口回复{0}";
            string str5 = "收到串口回复{0}，继续接收";
            string str6 = "等待串口回复以{0}开头并且以{1}结尾字符串数据超时";
            string str7 = "等待串口回复以{0}结尾的字符串数据超时";
            string str8 = "等待串口回复以{0}开头的字符串数据超时";
            string str9 = "等待串口回复超过{0}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for serial port reply to start with {0} and end with {1} string data";
                str2 = "Waiting for the serial port to reply the string data ending with {0}";
                str3 = "Waiting for the serial port to reply the string data starting with {0}";
                str4 = "Received serial reply {0}";
                str5 = "Received serial reply {0}, continue to receive";
                str6 = "Timeout waiting for string data with serial port reply beginning with {0} and ending with {1}";
                str7 = "Timeout waiting for serial port to reply to string data ending in {0}";
                str8 = "Timeout waiting for serial port to reply string data starting with {0}";
                str9 = "Wait for the serial port to reply for more than {0} seconds, continue to wait";
            }

            int nRet = 0;
            strData = "";

            if (strStartWith.Length > 0 && strEndWith.Length > 0)
            {
                //等待开头和结尾
                base.ShowLog(String.Format(str1, strStartWith, strEndWith));
            }
            else if (strEndWith.Length > 0)
            {
                //等待结尾
                base.ShowLog(String.Format(str2, strEndWith));
            }
            else
            {
                //等待开头
                base.ShowLog(String.Format(str3, strStartWith));
            }


            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                Thread.Sleep(100);
            }
            else
            {
                DateTime timeStart = DateTime.Now;
                string sRead = "", sReceive = "";
                int nTime = nTimeOutS;

                bool bTips = false;
                int nDefaultTimeS = GetCommTimeOut();

                while (true)
                {
                    //不响应暂停
                    this.CheckContinue(false, false);

                    comlink.ReadLine(out sRead);

                    sReceive += sRead;

                    if (strStartWith.Length > 0 && strEndWith.Length > 0
                        && sReceive.StartsWith(strStartWith)
                        && sReceive.EndsWith(strEndWith))
                    {
                        //等待开头和结尾
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (strStartWith.Length == 0 && strEndWith.Length > 0
                        && sReceive.EndsWith(strEndWith))
                    {
                        //等待结尾
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (strStartWith.Length > 0 && strEndWith.Length == 0
                        && sReceive.StartsWith(strStartWith))
                    {
                        //等待开头
                        base.ShowLog(String.Format(str4, sReceive));
                        strData = sReceive;
                        sReceive = "";
                        break;
                    }
                    else if (sRead.Length > 0)
                    {
                        base.ShowLog(String.Format(str5, sReceive));
                    }


                    if (nTimeOutS == 0)
                    {
                        nTime = nDefaultTimeS;
                    }

                    TimeSpan span = DateTime.Now - timeStart;
                    if (nTimeOutS != -1)
                    {
                        if (span.TotalMilliseconds > nTime * 1000)
                        {
                            string sMsg = "";

                            if (strStartWith.Length > 0 && strEndWith.Length > 0)
                            {
                                //等待开头和结尾
                                sMsg = String.Format(str6, strStartWith, strEndWith);
                            }
                            else if (strEndWith.Length > 0)
                            {
                                //等待结尾
                                sMsg = String.Format(str7, strEndWith);
                            }
                            else
                            {
                                //等待开头
                                sMsg = String.Format(str8, strStartWith);
                            }
                            ShowLog(sMsg, LogLevel.Error);


                            if (bShowDialog)
                            {
                                //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                                CheckContinue(false, bPause);//确保流程能够暂停住
                                //if (ShowTimeOutDlg(sMsg, "90001,ERR-XYT,") == 0)//为0时表示继续等待
                                if (ShowTimeOutDlg(sMsg, ErrorType.Err_Com_TimeOut,
                                    comlink.m_strName) == 0)//为0时表示继续等待
                                {
                                    timeStart = DateTime.Now;
                                }
                                else
                                {
                                    nRet = 1;
                                    break;
                                }
                            }
                            else
                            {
                                nRet = 1;
                                break;
                            }

                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (!bTips && span.TotalMilliseconds > nDefaultTimeS * 1000)
                        {
                            bTips = true;
                            ShowLog(string.Format(str9, nDefaultTimeS), LogLevel.Warn);
                        }
                    }

                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                }

            }

            CheckContinue(false, bPause);//确保流程能够暂停住 
            return nRet;
        }


        public int WaitTcp(TcpLink tcplink, out string strData, string strStartWith, string strEndWith = "", int nTimeOutS = -1, bool bShowDialog = true, bool bPause = true)
        {
            return wait_recevie_cmd(tcplink, out strData, strStartWith, strEndWith, nTimeOutS, bShowDialog, bPause);
        }

        /// <summary>
        /// 等待正限位信号
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="bValid"></param>
        /// <param name="nTimeOutS"></param>
        /// <param name="bShowDialog"></param>
        /// <param name="bPause"></param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitPel(int nAxisNo, bool bValid, int nTimeOutS = 0, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待轴{0} PEL = {1}";
            string str2 = "{0}等待轴{1} PEL = {2}达到{3}秒超时";
            string str3 = "等待轴{0} PEL = {1}超过{2}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for axis {0} PEL = {1}";
                str2 = "{0} timed out waiting for axis {1} PEL = {2} to reach {3} seconds";
                str3 = "Wait for axis {0} PEL = {1} more than {2} seconds,continue to wait";
            }

            ShowLog(string.Format(str1, nAxisNo, bValid));
            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                WaitTimeDelay(500);
                return 0;
            }

            bool bTips = false;
            int nDefaultTimeS = GetIoTimeOut();

            int nTimeCount = 0;
            if (nTimeOutS == 0)
                nTimeOutS = nDefaultTimeS;
            while (true)
            {
                int ret = MotionMgr.GetInstance().IsAxisPEL(nAxisNo);

                if (ret == 2)
                {
                    //报警
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} ALM", nAxisNo));
                    throw new StationException(sb.ToString());
                }
                else if (ret == 3)
                {
                    //急停
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion_EmgStop);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion_EmgStop.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} EMG", nAxisNo));
                    throw new StationException(sb.ToString());
                }

                bool bBit = (ret == 1);
                if (bBit != bValid)   //未到位
                {
                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                    nTimeCount += SystemMgr.GetInstance().ScanTime;

                    if (nTimeOutS != -1 && nTimeCount > nTimeOutS * 1000)
                    {
                        string strInfo = string.Format(str2,
                                 Name, nAxisNo, bValid.ToString(), nTimeOutS.ToString());
                        ShowLog(strInfo, LogLevel.Error);

                        if (bShowDialog)
                        {
                            //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                            CheckContinue(false, bPause);//确保流程能够暂停住
                            //if (ShowTimeOutDlg(strInfo, "20001,ERR-XYT,") == 0)//为0时表示继续等待
                            if (ShowTimeOutDlg(strInfo, ErrorType.Err_Motion_Pel_TimeOut,
                                nAxisNo.ToString()) == 0)//为0时表示继续等待
                            {
                                nTimeCount = 0;
                            }
                            else
                            {
                                CheckContinue(false, bPause);//确保流程能够暂停住 
                                return 1;
                            }
                        }
                        else
                        {
                            CheckContinue(false, bPause);//确保流程能够暂停住 
                            return 1;
                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (nTimeOutS == -1 && nTimeCount > nDefaultTimeS * 1000 && !bTips)
                        {
                            bTips = true;
                            ShowLog(string.Format(str3, nAxisNo, bValid, nDefaultTimeS), LogLevel.Warn);
                        }
                        //不响应暂停
                        CheckContinue(false, false);
                    }
                }
                else //已到位
                {
                    CheckContinue(false, bPause);//确保流程能够暂停住 

                    ShowLog(string.Format("Axis {0} PEL = {1} done", nAxisNo, bValid));

                    return 0;
                }
            }
        }

        /// <summary>
        /// 等待负限位信号
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="bValid"></param>
        /// <param name="nTimeOutS"></param>
        /// <param name="bShowDialog"></param>
        /// <param name="bPause"></param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitMel(int nAxisNo, bool bValid, int nTimeOutS = 0, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待轴{0} MEL = {1}";
            string str2 = "{0}等待轴{1} MEL = {2}达到{3}秒超时";
            string str3 = "等待轴{0} MEL = {1}超过{2}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for axis {0} MEL = {1}";
                str2 = "{0} timed out waiting for axis {1} MEL = {2} to reach {3} seconds";
                str3 = "Wait for axis {0} MEL = {1} more than {2} seconds,continue to wait";
            }

            ShowLog(string.Format(str1, nAxisNo, bValid));
            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                WaitTimeDelay(500);
                return 0;
            }

            bool bTips = false;
            int nDefaultTimeS = GetIoTimeOut();

            int nTimeCount = 0;
            if (nTimeOutS == 0)
                nTimeOutS = nDefaultTimeS;
            while (true)
            {
                int ret = MotionMgr.GetInstance().IsAxisMEL(nAxisNo);

                if (ret == 2)
                {
                    //报警
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} ALM", nAxisNo));
                    throw new StationException(sb.ToString());
                }
                else if (ret == 3)
                {
                    //急停
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion_EmgStop);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion_EmgStop.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} EMG", nAxisNo));
                    throw new StationException(sb.ToString());
                }

                bool bBit = (ret == 1);
                if (bBit != bValid)   //未到位
                {
                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                    nTimeCount += SystemMgr.GetInstance().ScanTime;

                    if (nTimeOutS != -1 && nTimeCount > nTimeOutS * 1000)
                    {
                        string strInfo = string.Format(str2,
                                 Name, nAxisNo, bValid.ToString(), nTimeOutS.ToString());
                        ShowLog(strInfo, LogLevel.Error);

                        if (bShowDialog)
                        {
                            //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                            CheckContinue(false, bPause);//确保流程能够暂停住
                            //if (ShowTimeOutDlg(strInfo, "20001,ERR-XYT,") == 0)//为0时表示继续等待
                            if (ShowTimeOutDlg(strInfo, ErrorType.Err_Motion_Mel_TimeOut,
                                nAxisNo.ToString()) == 0)//为0时表示继续等待
                            {
                                nTimeCount = 0;
                            }
                            else
                            {
                                CheckContinue(false, bPause);//确保流程能够暂停住 
                                return 1;
                            }
                        }
                        else
                        {
                            CheckContinue(false, bPause);//确保流程能够暂停住 
                            return 1;
                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (nTimeOutS == -1 && nTimeCount > nDefaultTimeS * 1000 && !bTips)
                        {
                            bTips = true;
                            ShowLog(string.Format(str3, nAxisNo, bValid, nDefaultTimeS), LogLevel.Warn);
                        }
                        //不响应暂停
                        CheckContinue(false, false);
                    }
                }
                else //已到位
                {
                    CheckContinue(false, bPause);//确保流程能够暂停住 

                    ShowLog(string.Format("Axis {0} MEL = {1} done", nAxisNo, bValid));
                    return 0;
                }
            }
        }

        /// <summary>
        /// 等待极限信号
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="bValid"></param>
        /// <param name="nTimeOutS"></param>
        /// <param name="bShowDialog"></param>
        /// <param name="bPause"></param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitEL(int nAxisNo, bool bValid, int nTimeOutS = 0, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待轴{0} EL = {1}";
            string str2 = "{0}等待轴{1} EL = {2}达到{3}秒超时";
            string str3 = "等待轴{0} EL = {1}超过{2}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for axis {0} EL = {1}";
                str2 = "{0} timed out waiting for axis {1} EL = {2} to reach {3} seconds";
                str3 = "Wait for axis {0} EL = {1} more than {2} seconds,continue to wait";
            }

            ShowLog(string.Format(str1, nAxisNo, bValid));
            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                WaitTimeDelay(500);
                return 0;
            }

            bool bTips = false;
            int nDefaultTimeS = GetIoTimeOut();

            int nTimeCount = 0;
            if (nTimeOutS == 0)
                nTimeOutS = nDefaultTimeS;
            while (true)
            {
                int ret1 = MotionMgr.GetInstance().IsAxisMEL(nAxisNo);
                int ret2 = MotionMgr.GetInstance().IsAxisPEL(nAxisNo);

                if (ret1 == 2)
                {
                    //报警
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} ALM", nAxisNo));
                    throw new StationException(sb.ToString());
                }
                else if (ret1 == 3)
                {
                    //急停
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion_EmgStop);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion_EmgStop.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} EMG", nAxisNo));
                    throw new StationException(sb.ToString());
                }


                bool bBit1 = ret1 == 1;
                bool bBit2 = ret2 == 1;
                if (bBit1 != bValid && bBit2 != bValid)   //未到位
                {
                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                    nTimeCount += SystemMgr.GetInstance().ScanTime;

                    if (nTimeOutS != -1 && nTimeCount > nTimeOutS * 1000)
                    {
                        string strInfo = string.Format(str2,
                                 Name, nAxisNo, bValid.ToString(), nTimeOutS.ToString());
                        ShowLog(strInfo, LogLevel.Error);

                        if (bShowDialog)
                        {
                            //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                            CheckContinue(false, bPause);//确保流程能够暂停住
                            //if (ShowTimeOutDlg(strInfo, "20001,ERR-XYT,") == 0)//为0时表示继续等待
                            if (ShowTimeOutDlg(strInfo, ErrorType.Err_Motion_El_TimeOut,
                                nAxisNo.ToString()) == 0)//为0时表示继续等待
                            {
                                nTimeCount = 0;
                            }
                            else
                            {
                                CheckContinue(false, bPause);//确保流程能够暂停住 
                                return 1;
                            }
                        }
                        else
                        {
                            CheckContinue(false, bPause);//确保流程能够暂停住 
                            return 1;
                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (nTimeOutS == -1 && nTimeCount > nDefaultTimeS * 1000 && !bTips)
                        {
                            bTips = true;
                            ShowLog(string.Format(str3, nAxisNo, bValid, nDefaultTimeS), LogLevel.Warn);
                        }
                        //不响应暂停
                        CheckContinue(false, false);
                    }
                }
                else //已到位
                {
                    CheckContinue(false, bPause);//确保流程能够暂停住 

                    ShowLog(string.Format("Axis {0} EL = {1} done", nAxisNo, bValid));
                    return 0;
                }
            }
        }

        /// <summary>
        /// 等待原点信号
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="bValid"></param>
        /// <param name="nTimeOutS"></param>
        /// <param name="bShowDialog"></param>
        /// <param name="bPause"></param>
        /// <returns></returns>
        public int WaitOrg(int nAxisNo, bool bValid, int nTimeOutS = 0, bool bShowDialog = true, bool bPause = true)
        {
            string str1 = "等待轴{0} ORG = {1}";
            string str2 = "{0}等待轴{1} ORG = {2}达到{3}秒超时";
            string str3 = "等待轴{0} ORG = {1}超过{2}秒,继续等待";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Waiting for axis {0} ORG = {1}";
                str2 = "{0} timed out waiting for axis {1} ORG = {2} to reach {3} seconds";
                str3 = "Wait for axis {0} ORG = {1} more than {2} seconds,continue to wait";
            }

            ShowLog(string.Format(str1, nAxisNo, bValid));
            if (SystemMgr.GetInstance().IsSimulateRunMode())
            {
                WaitTimeDelay(500);
                return 0;
            }

            bool bTips = false;
            int nDefaultTimeS = GetIoTimeOut();

            int nTimeCount = 0;
            if (nTimeOutS == 0)
                nTimeOutS = nDefaultTimeS;
            while (true)
            {
                int ret = MotionMgr.GetInstance().IsAxisPEL(nAxisNo);

                if (ret == 2)
                {
                    //报警
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} ALM", nAxisNo));
                    throw new StationException(sb.ToString());
                }
                else if (ret == 3)
                {
                    //急停
                    StringBuilder sb = new StringBuilder();
                    sb.Append((int)ErrorType.Err_Motion_EmgStop);
                    sb.Append(",");
                    sb.Append(ErrorType.Err_Motion_EmgStop.ToString());
                    sb.Append(",");
                    sb.Append(nAxisNo);
                    sb.Append(",");
                    sb.Append(string.Format("Axis {0} EMG", nAxisNo));
                    throw new StationException(sb.ToString());
                }

                bool bBit = (ret == 1);
                if (bBit != bValid)   //未到位
                {
                    Thread.Sleep(SystemMgr.GetInstance().ScanTime);
                    nTimeCount += SystemMgr.GetInstance().ScanTime;

                    if (nTimeOutS != -1 && nTimeCount > nTimeOutS * 1000)
                    {
                        string strInfo = string.Format(str2,
                                 Name, nAxisNo, bValid.ToString(), nTimeOutS.ToString());
                        ShowLog(strInfo, LogLevel.Error);

                        if (bShowDialog)
                        {
                            //弹对话框之前检查是否暂停，如果是暂停需要卡住，防止对话框弹出来卡死界面
                            CheckContinue(false, bPause);//确保流程能够暂停住
                            //if (ShowTimeOutDlg(strInfo, "20001,ERR-XYT,") == 0)//为0时表示继续等待
                            if (ShowTimeOutDlg(strInfo, ErrorType.Err_Motion_Org_TimeOut,
                                nAxisNo.ToString()) == 0)//为0时表示继续等待
                            {
                                nTimeCount = 0;
                            }
                            else
                            {
                                CheckContinue(false, bPause);//确保流程能够暂停住 
                                return 1;
                            }
                        }
                        else
                        {
                            CheckContinue(false, bPause);//确保流程能够暂停住 
                            return 1;
                        }
                    }
                    else
                    {
                        //20200217 Binggoo 当设置一直等待时，超过默认时间后提示，方便检查
                        if (nTimeOutS == -1 && nTimeCount > nDefaultTimeS * 1000 && !bTips)
                        {
                            bTips = true;
                            ShowLog(string.Format(str3, nAxisNo, bValid, nDefaultTimeS), LogLevel.Warn);
                        }
                        //不响应暂停
                        CheckContinue(false, false);
                    }
                }
                else //已到位
                {
                    CheckContinue(false, bPause);//确保流程能够暂停住 
                    ShowLog(string.Format("Axis {0} ORG = {1} done", nAxisNo, bValid));
                    return 0;
                }
            }
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="nPercent"></param>
        public virtual void UpdateProgress(int nPercent)
        {
            SystemMgr.GetInstance().WriteRegInt((int)SysIntReg.Int_Process_Step, nPercent);
        }

        /// <summary>
        /// 给机械手发命令
        /// </summary>
        /// <param name="strCmd">命令</param>
        /// <param name="strRespond">等待返回结果</param>
        /// <param name="length">机械手要求的参数长度，当参数长度不够时用0补全</param>
        /// <param name="strParams">参数列表</param>
        /// <returns>接收到的数据</returns>
        public string[] RobotCmd(string strCmd, string strRespond, int length, params string[] strParams)
        {
            string strSend, strData;

            strSend = strCmd;

            int size = length;

            if (strParams.Length < length)
            {
                size = strParams.Length;
            }

            for (int i = 0; i < size; i++)
            {
                strSend += "," + strParams[i];
            }

            for (int i = size; i < length; i++)
            {
                strSend += ",0";
            }

            m_Robot.Lock();

            string str1 = "发送命令给{0}：{1}";
            string str2 = "等待{0}回复:{1}";
            string str3 = "收到机器人回复：";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Send command to {0}: {1}";
                str2 = "Waiting for {0} reply: {1}";
                str3 = "Reply received from robot:";
            }
            try
            {
                //发送给机械手
                ShowLog(String.Format(str1, m_Robot.RobotName, strSend));
                m_Robot.WriteLine(strSend);

                ShowLog(String.Format(str2, m_Robot.RobotName, strRespond));

                if (m_Robot.Mode == CommMode.Com)
                {
                    wait_recevie_cmd((ComLink)m_Robot.LinkRemote, out strData, strRespond);
                }
                else
                {
                    wait_recevie_cmd((TcpLink)m_Robot.LinkRemote, out strData, strRespond);
                }
            }
            finally
            {
                m_Robot.UnLock();
            }

            ShowLog(str3 + strData);
            return strData.Split(',');
        }

        /// <summary>
        /// 发送TCP命令并等待回复
        /// </summary>
        /// <param name="tcpLink">TCPLink</param>
        /// <param name="strCmd">命令</param>
        /// <param name="strRespond">回复开头</param>
        /// <param name="length">参数长度，不包含命令</param>
        /// <param name="bWait">是否等待</param>
        /// <param name="splitChar">分割符</param>
        /// <param name="strParams">传入的参数</param>
        /// <returns></returns>
        public string[] TcpCmd(TcpLink tcpLink, string strCmd, string strRespond, int length, bool bWait, char splitChar = ',', params string[] strParams)
        {
            string strSend, strData;

            strSend = strCmd;

            int size = length;

            if (strParams.Length < length)
            {
                size = strParams.Length;
            }

            for (int i = 0; i < size; i++)
            {
                strSend += "," + strParams[i];
            }

            for (int i = size; i < length; i++)
            {
                strSend += ",0";
            }

            string str1 = "发送命令给{0}:{1} - {2}";
            string str2 = "等待{0}:{1}回复:{2}";
            string str3 = "收到网络回复：";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Send command to {0}: {1} - {2}";
                str2 = "Waiting for {0}: {1} reply: {2}";
                str3 = "Received network reply:";
            }

            strData = "";
            lock (tcpLink)
            {
                //发送给机械手
                ShowLog(String.Format(str1, tcpLink.m_strIP, tcpLink.m_nPort, strSend));
                tcpLink.WriteLine(strSend);

                ShowLog(String.Format(str2, tcpLink.m_strIP, tcpLink.m_nPort, strRespond));

                if (bWait)
                {
                    wait_recevie_cmd(tcpLink, out strData, strRespond);

                    ShowLog(str3 + strData);
                }
            }

            return strData.Split(splitChar);
        }

        /// <summary>
        /// 获取气缸中的IO
        /// </summary>
        /// <param name="io_in">输入io</param>
        /// <param name="io_out">输出io</param>
        protected void GetCylinderIo(ref string[] io_in, ref string[] io_out)
        {
            List<string> listIoIn = io_in.ToList();
            List<string> listIoOut = io_out.ToList();

            foreach (string name in m_cylinders)
            {
                Cylinder cyl = CylinderMgr.GetInstance().GetCyLinder(name);

                //输入
                foreach (string inName in cyl.m_strIoIns)
                {
                    if (inName.Length > 0)
                    {
                        if (listIoIn.Find(s => s == inName) == null)
                        {
                            //不包含这个io，加入
                            listIoIn.Add(inName);
                        }
                    }
                }

                //输出
                foreach (string outName in cyl.m_strIoOuts)
                {
                    if (outName.Length > 0)
                    {
                        if (listIoOut.Find(s => s == outName) == null)
                        {
                            //不包含这个io，加入
                            listIoOut.Add(outName);
                        }
                    }
                }
            }

            io_in = listIoIn.ToArray();

            io_out = listIoOut.ToArray();
        }

        /// <summary>
        /// 直接运动到点
        /// </summary>
        /// <param name="pt">点位索引</param>
        /// <param name="mask">优先级及掩膜，二进制表示</param>
        /// <param name="wait">是否等待</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisGoTo(int pt, int mask = 0, bool wait = true, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            PointInfo pi;
            if (m_dicPoint.TryGetValue(pt, out pi))
            {
                bool bResponseSleep = false;

                //先判断哪些轴优先运动
                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if ((mask & (MotionPriority.X_FIRST << i)) != 0
                    && (mask & (MotionPriority.X_MASK << i)) == 0
                    && m_nAxisArray[i] > 0)
                    {
                        MotionMgr.GetInstance().AbsMoveWithCfg(m_nAxisArray[i], pi.Pos[i]);

                        bResponseSleep = true;
                    }
                }

                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if ((mask & (MotionPriority.X_FIRST << i)) != 0
                    && (mask & (MotionPriority.X_MASK << i)) == 0
                    && m_nAxisArray[i] > 0
                    && wait)
                    {
                        if (bResponseSleep)
                        {
                            Thread.Sleep(responseTimeMs);
                            bResponseSleep = false;
                        }
                        WaitMotion(m_nAxisArray[i], timeout);
                    }
                }

                //再判断哪些轴后运动
                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if ((mask & (MotionPriority.X_FIRST << i)) == 0
                    && (mask & (MotionPriority.X_MASK << i)) == 0
                    && m_nAxisArray[i] > 0)
                    {
                        MotionMgr.GetInstance().AbsMoveWithCfg(m_nAxisArray[i], pi.Pos[i]);

                        bResponseSleep = true;
                    }
                }

                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if ((mask & (MotionPriority.X_FIRST << i)) == 0
                    && (mask & (MotionPriority.X_MASK << i)) == 0
                    && m_nAxisArray[i] > 0
                    && wait)
                    {
                        if (bResponseSleep)
                        {
                            Thread.Sleep(responseTimeMs);
                            bResponseSleep = false;
                        }

                        WaitMotion(m_nAxisArray[i], timeout);
                    }
                }

                #region 旧方法
                /*
                if ((mask & MotionPriority.X_FIRST) != 0 
                    && (mask & MotionPriority.X_MASK) == 0
                    && AxisX > 0)
                { 
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisX, pi.x);
                }

                if ((mask & MotionPriority.Y_FIRST) != 0
                    && (mask & MotionPriority.Y_MASK) == 0
                    && AxisY > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisY, pi.y);
                }

                if ((mask & MotionPriority.Z_FIRST) != 0
                    && (mask & MotionPriority.Z_MASK) == 0
                    && AxisZ > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisZ,pi.z);
                }

                if ((mask & MotionPriority.U_FIRST) != 0
                    && (mask & MotionPriority.U_MASK) == 0
                    && AxisU > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisU,pi.u);
                }

                #region //2020-2-24 Binggoo 新增ABCD轴
                if ((mask & MotionPriority.A_FIRST) != 0
                    && (mask & MotionPriority.A_MASK) == 0
                    && AxisA > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisA, pi.a);
                }

                if ((mask & MotionPriority.B_FIRST) != 0
                    && (mask & MotionPriority.B_MASK) == 0
                    && AxisB > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisB, pi.b);
                }

                if ((mask & MotionPriority.C_FIRST) != 0
                    && (mask & MotionPriority.C_MASK) == 0
                    && AxisC > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisC, pi.c);
                }

                if ((mask & MotionPriority.D_FIRST) != 0
                    && (mask & MotionPriority.D_MASK) == 0
                    && AxisD > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisD, pi.d);
                }
                #endregion

                if ((mask & MotionPriority.X_FIRST) != 0
                    && (mask & MotionPriority.X_MASK) == 0
                    && AxisX > 0)
                {
                    WaitMotion(AxisX, timeout);
                }
                    
                if ((mask & MotionPriority.Y_FIRST) != 0
                    && (mask & MotionPriority.Y_MASK) == 0
                    && AxisY > 0)
                {
                    WaitMotion(AxisY, timeout);
                }

                if ((mask & MotionPriority.Z_FIRST) != 0
                    && (mask & MotionPriority.Z_MASK) == 0
                    && AxisZ > 0)
                {
                    WaitMotion(AxisZ, timeout);
                }

                if ((mask & MotionPriority.U_FIRST) != 0
                    && (mask & MotionPriority.U_MASK) == 0
                    && AxisU > 0)
                {
                    WaitMotion(AxisU, timeout);
                }

                #region //2020-2-24 Binggoo 新增ABCD轴
                if ((mask & MotionPriority.A_FIRST) != 0
                    && (mask & MotionPriority.A_MASK) == 0
                    && AxisA > 0)
                {
                    WaitMotion(AxisA, timeout);
                }
                    
                if ((mask & MotionPriority.B_FIRST) != 0
                    && (mask & MotionPriority.B_MASK) == 0
                    && AxisB > 0)
                {
                    WaitMotion(AxisB, timeout);
                }

                if ((mask & MotionPriority.C_FIRST) != 0
                    && (mask & MotionPriority.C_MASK) == 0
                    && AxisC > 0)
                {
                    WaitMotion(AxisC, timeout);
                }

                if ((mask & MotionPriority.D_FIRST) != 0
                    && (mask & MotionPriority.D_MASK) == 0
                    && AxisD > 0)
                {
                    WaitMotion(AxisD, timeout);
                }

                #endregion

                if ((mask & MotionPriority.X_FIRST) == 0
                    && (mask & MotionPriority.X_MASK) == 0
                    && AxisX > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisX, pi.x);
                }

                if ((mask & MotionPriority.Y_FIRST) == 0
                    && (mask & MotionPriority.Y_MASK) == 0
                    && AxisY > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisY,pi.y);
                }


                if ((mask & MotionPriority.Z_FIRST) == 0
                    && (mask & MotionPriority.Z_MASK) == 0
                    && AxisZ > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisZ,pi.z);
                }

                if ((mask & MotionPriority.U_FIRST) == 0
                    && (mask & MotionPriority.U_MASK) == 0
                    && AxisU > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisU,pi.u);
                }

                #region //2020-2-24 Binggoo 新增ABCD轴
                if ((mask & MotionPriority.A_FIRST) == 0
                    && (mask & MotionPriority.A_MASK) == 0
                    && AxisA > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisA, pi.a);
                }

                if ((mask & MotionPriority.B_FIRST) == 0
                    && (mask & MotionPriority.B_MASK) == 0
                    && AxisB > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisB, pi.b);
                }


                if ((mask & MotionPriority.C_FIRST) == 0
                    && (mask & MotionPriority.C_MASK) == 0
                    && AxisC > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisC, pi.c);
                }

                if ((mask & MotionPriority.D_FIRST) == 0
                    && (mask & MotionPriority.D_MASK) == 0
                    && AxisD > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisD, pi.d);
                }
                #endregion

                if ((mask & MotionPriority.X_FIRST) == 0
                    && (mask & MotionPriority.X_MASK) == 0
                    && AxisX > 0 && wait)
                {
                    WaitMotion(AxisX, timeout);
                }

                if ((mask & MotionPriority.Y_FIRST) == 0
                    && (mask & MotionPriority.Y_MASK) == 0
                    && AxisY > 0 && wait)
                {
                    WaitMotion(AxisY, timeout);
                }
                    
                if ((mask & MotionPriority.Z_FIRST) == 0
                    && (mask & MotionPriority.Z_MASK) == 0
                    && AxisZ > 0 && wait)
                {
                    WaitMotion(AxisZ, timeout);
                }
                    
                if ((mask & MotionPriority.U_FIRST) == 0
                    && (mask & MotionPriority.U_MASK) == 0
                    && AxisU > 0 && wait)
                {
                    WaitMotion(AxisU, timeout);
                }

                #region //2020-2-24 Binggoo 新增ABCD轴
                if ((mask & MotionPriority.A_FIRST) == 0
                    && (mask & MotionPriority.A_MASK) == 0
                    && AxisA > 0 && wait)
                {
                    WaitMotion(AxisA, timeout);
                }

                if ((mask & MotionPriority.B_FIRST) == 0
                    && (mask & MotionPriority.B_MASK) == 0
                    && AxisB > 0 && wait)
                {
                    WaitMotion(AxisB, timeout);
                }

                if ((mask & MotionPriority.C_FIRST) == 0
                    && (mask & MotionPriority.C_MASK) == 0
                    && AxisC > 0 && wait)
                {
                    WaitMotion(AxisC, timeout);
                }

                if ((mask & MotionPriority.D_FIRST) == 0
                    && (mask & MotionPriority.D_MASK) == 0
                    && AxisD > 0 && wait)
                {
                    WaitMotion(AxisD, timeout);
                }
                #endregion
                */
                #endregion
            }
            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(String.Format("{0} point position {1} does not exist", this.Name, pt));
                }
                else
                {
                    MessageBox.Show(String.Format("{0}点位{1}不存在", this.Name, pt));
                }
            }

        }

        /// <summary>
        /// 跳转到点
        /// </summary>
        /// <param name="pt">点位索引</param>
        /// <param name="nLimitZ">Z轴先运动到的位置</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisJumpTo(int pt, double nLimitZ, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            PointInfo pi;
            if (m_dicPoint.TryGetValue(pt, out pi))
            {
                if (AxisZ > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisZ, nLimitZ);

                    Thread.Sleep(responseTimeMs);
                    WaitMotion(AxisZ, timeout);
                }

                bool bResponseSleep = false;
                if (AxisX > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisX, pi.x);
                    bResponseSleep = true;
                }

                if (AxisY > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisY, pi.y);
                    bResponseSleep = true;
                }

                if (AxisU > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisU, pi.u);
                    bResponseSleep = true;
                }

                if (AxisX > 0)
                {
                    if (bResponseSleep)
                    {
                        Thread.Sleep(responseTimeMs);
                        bResponseSleep = false;
                    }
                    WaitMotion(AxisX, timeout);
                }

                if (AxisY > 0)
                {
                    if (bResponseSleep)
                    {
                        Thread.Sleep(responseTimeMs);
                        bResponseSleep = false;
                    }

                    WaitMotion(AxisY, timeout);
                }

                if (AxisU > 0)
                {
                    if (bResponseSleep)
                    {
                        Thread.Sleep(responseTimeMs);
                        bResponseSleep = false;
                    }

                    WaitMotion(AxisU, timeout);
                }

                if (AxisZ > 0)
                {
                    MotionMgr.GetInstance().AbsMoveWithCfg(AxisZ, pi.z);

                    Thread.Sleep(responseTimeMs);

                    WaitMotion(AxisZ, timeout);
                }

            }
            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(String.Format("{0} point position {1} does not exist", this.Name, pt));
                }
                else
                {
                    MessageBox.Show(String.Format("{0}点位{1}不存在", this.Name, pt));
                }
            }
        }

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisHome(int priority = 0, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            bool bResponseSleep = false;

            //先判断哪些轴优先回原点
            for (int i = 0; i < m_nAxisArray.Length; i++)
            {
                if ((priority & (0x01 << i)) != 0 && m_nAxisArray[i] > 0)
                {
                    MotionMgr.GetInstance().Home(m_nAxisArray[i]);

                    bResponseSleep = true;
                }
            }

            for (int i = 0; i < m_nAxisArray.Length; i++)
            {
                if ((priority & (0x01 << i)) != 0 && m_nAxisArray[i] > 0)
                {
                    if (bResponseSleep)
                    {
                        Thread.Sleep(responseTimeMs);

                        bResponseSleep = false;
                    }
                    WaitHome(m_nAxisArray[i], timeout);
                }
            }

            //再判断哪些轴后回原点
            for (int i = 0; i < m_nAxisArray.Length; i++)
            {
                if ((priority & (0x01 << i)) == 0 && m_nAxisArray[i] > 0)
                {
                    MotionMgr.GetInstance().Home(m_nAxisArray[i]);

                    bResponseSleep = true;
                }
            }

            for (int i = 0; i < m_nAxisArray.Length; i++)
            {
                if ((priority & (0x01 << i)) == 0 && m_nAxisArray[i] > 0)
                {
                    if (bResponseSleep)
                    {
                        Thread.Sleep(responseTimeMs);

                        bResponseSleep = false;
                    }

                    WaitHome(m_nAxisArray[i], timeout);
                }
            }

            #region 旧方法
            /*
            if ((priority & MotionPriority.X_FIRST) != 0 && AxisX > 0)
            {
                MotionMgr.GetInstance().Home(AxisX);
            }

            if ((priority & MotionPriority.Y_FIRST) != 0 && AxisY > 0)
            {
                MotionMgr.GetInstance().Home(AxisY);
            }

            if ((priority & MotionPriority.Z_FIRST) != 0 && AxisZ > 0)
            {
                MotionMgr.GetInstance().Home(AxisZ);
            }

            if ((priority & MotionPriority.U_FIRST) != 0 && AxisU > 0)
            {
                MotionMgr.GetInstance().Home(AxisU);
            }

            #region //2020-2-24 Binggoo 新增ABCD轴
            if ((priority & MotionPriority.A_FIRST) != 0 && AxisA > 0)
            {
                MotionMgr.GetInstance().Home(AxisA);
            }

            if ((priority & MotionPriority.B_FIRST) != 0 && AxisB > 0)
            {
                MotionMgr.GetInstance().Home(AxisB);
            }

            if ((priority & MotionPriority.C_FIRST) != 0 && AxisC > 0)
            {
                MotionMgr.GetInstance().Home(AxisC);
            }

            if ((priority & MotionPriority.D_FIRST) != 0 && AxisD > 0)
            {
                MotionMgr.GetInstance().Home(AxisD);
            }
            #endregion

            if ((priority & MotionPriority.X_FIRST) != 0 && AxisX > 0)
            {
                WaitHome(AxisX, timeout);
            }

            if ((priority & MotionPriority.Y_FIRST) != 0 && AxisY > 0)
            {
                WaitHome(AxisY, timeout);
            }

            if ((priority & MotionPriority.Z_FIRST) != 0 && AxisZ > 0)
            {
                WaitHome(AxisZ, timeout);
            }

            if ((priority & MotionPriority.U_FIRST) != 0 && AxisU > 0)
            {
                WaitHome(AxisU, timeout);
            }

            #region //2020-2-24 Binggoo 新增ABCD轴
            if ((priority & MotionPriority.A_FIRST) != 0 && AxisA > 0)
            {
                WaitHome(AxisA, timeout);
            }

            if ((priority & MotionPriority.B_FIRST) != 0 && AxisB > 0)
            {
                WaitHome(AxisB, timeout);
            }

            if ((priority & MotionPriority.C_FIRST) != 0 && AxisC > 0)
            {
                WaitHome(AxisC, timeout);
            }

            if ((priority & MotionPriority.D_FIRST) != 0 && AxisD > 0)
            {
                WaitHome(AxisD, timeout);
            }
            #endregion

            if ((priority & MotionPriority.X_FIRST) == 0 && AxisX > 0)
            {
                MotionMgr.GetInstance().Home(AxisX);
            }

            if ((priority & MotionPriority.Y_FIRST) == 0 && AxisY > 0)
            {
                MotionMgr.GetInstance().Home(AxisY);
            }

            if ((priority & MotionPriority.Z_FIRST) == 0 && AxisZ > 0)
            {
                MotionMgr.GetInstance().Home(AxisZ);
            }

            if ((priority & MotionPriority.U_FIRST) == 0 && AxisU > 0)
            {
                MotionMgr.GetInstance().Home(AxisU);
            }

            #region //2020-2-24 Binggoo 新增ABCD轴
            if ((priority & MotionPriority.A_FIRST) == 0 && AxisA > 0)
            {
                MotionMgr.GetInstance().Home(AxisA);
            }

            if ((priority & MotionPriority.B_FIRST) == 0 && AxisB > 0)
            {
                MotionMgr.GetInstance().Home(AxisB);
            }

            if ((priority & MotionPriority.C_FIRST) == 0 && AxisC > 0)
            {
                MotionMgr.GetInstance().Home(AxisC);
            }

            if ((priority & MotionPriority.D_FIRST) == 0 && AxisD > 0)
            {
                MotionMgr.GetInstance().Home(AxisD);
            }
            #endregion

            if ((priority & MotionPriority.X_FIRST) == 0 && AxisX > 0)
            {
                WaitHome(AxisX, timeout);
            }

            if ((priority & MotionPriority.Y_FIRST) == 0 && AxisY > 0)
            {
                WaitHome(AxisY, timeout);
            }

            if ((priority & MotionPriority.Z_FIRST) == 0 && AxisZ > 0)
            {
                WaitHome(AxisZ, timeout);
            }

            if ((priority & MotionPriority.U_FIRST) == 0 && AxisU > 0)
            {
                WaitHome(AxisU, timeout);
            }

            #region //2020-2-24 Binggoo 新增ABCD轴
            if ((priority & MotionPriority.A_FIRST) == 0 && AxisA > 0)
            {
                WaitHome(AxisA, timeout);
            }

            if ((priority & MotionPriority.B_FIRST) == 0 && AxisB > 0)
            {
                WaitHome(AxisB, timeout);
            }

            if ((priority & MotionPriority.C_FIRST) == 0 && AxisC > 0)
            {
                WaitHome(AxisC, timeout);
            }

            if ((priority & MotionPriority.D_FIRST) == 0 && AxisD > 0)
            {
                WaitHome(AxisD, timeout);
            }
            #endregion
            */
            #endregion
        }

        /// <summary>
        /// 单轴回原点
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="wait">是否等待</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisSingleHome(int nAxis, bool wait = true, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            MotionMgr.GetInstance().Home(nAxis);

            if (wait)
            {
                Thread.Sleep(responseTimeMs);

                WaitHome(nAxis, timeout);
            }
        }


        /// <summary>
        /// 单个轴运动至点位
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="pt">点位</param>
        /// <param name="wait">是否等待</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisSingleTo(int nAxis, int pt, bool wait = true, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            PointInfo pi;

            if (!m_dicPoint.TryGetValue(pt, out pi))
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(String.Format("{0} point position {1} does not exist", this.Name, pt));
                }
                else
                {
                    MessageBox.Show(String.Format("{0}点位{1}不存在", this.Name, pt));
                }

                return;
            }

            double fPos = 0;
            int nIndex = Array.IndexOf(m_nAxisArray, nAxis);
            if (nIndex >= 0)
            {
                fPos = pi.Pos[nIndex];
            }

            MotionMgr.GetInstance().AbsMoveWithCfg(nAxis, fPos);

            if (wait)
            {
                Thread.Sleep(responseTimeMs);

                WaitMotion(nAxis, timeout);
            }

        }

        /// <summary>
        /// 单个轴运动至点位,使用系统配置速度
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="fPos">位置，单位mm or deg</param>
        /// <param name="speedpercent">速度百分比</param>
        /// <param name="wait">是否等待</param>
        /// <param name="timeout">超时时间：0 - 系统时间；-1 - 一直等待</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisSingleTo(int nAxis, double fPos, double speedpercent = 100, bool wait = true, int timeout = -1, int responseTimeMs = RESPONSE_TIME_MS)
        {
            MotionMgr.GetInstance().AbsMoveWithCfg(nAxis, fPos, speedpercent);

            if (wait)
            {
                Thread.Sleep(responseTimeMs);
                WaitMotion(nAxis, timeout);
            }
        }

        /// <summary>
        /// 单轴相对运动,使用系统配置速度
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="offset">相对位置</param>
        /// <param name="speedpercent">速度百分比</param>
        /// <param name="wait">是否等待</param>
        /// <param name="timeout">超时时间秒</param>
        /// <param name="bShowLog">是否显示log，默认显示，当用于逐步顶升时可以不用显示</param>
        /// <param name="responseTimeMs">命令反应时间</param>
        public void AxisSingleRelTo(int nAxis, double offset, double speedpercent = 100, bool wait = true, int timeout = -1, bool bShowLog = true, int responseTimeMs = RESPONSE_TIME_MS)
        {
            MotionMgr.GetInstance().RelativeMoveWithCfg(nAxis, offset, speedpercent);

            if (wait)
            {
                Thread.Sleep(responseTimeMs);
                WaitMotion(nAxis, timeout, true, true, bShowLog);
            }
        }


        /// <summary>
        /// 伺服使能
        /// </summary>
        /// <param name="bServoOn">使能或断使能</param>
        public void AxisEnable(bool bServoOn)
        {
            if (bServoOn)
            {
                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if (m_nAxisArray[i] > 0)
                    {
                        MotionMgr.GetInstance().ServoOn(m_nAxisArray[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_nAxisArray.Length; i++)
                {
                    if (m_nAxisArray[i] > 0)
                    {
                        MotionMgr.GetInstance().ServoOff(m_nAxisArray[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 伺服正常停止
        /// </summary>
        public void AxisStop()
        {
            foreach (int axis in m_nAxisArray)
            {
                if (axis > 0)
                {
                    MotionMgr.GetInstance().StopAxis(axis);
                }
            }
        }

        /// <summary>
        /// 伺服正常停止
        /// </summary>
        /// <param name="nAxis">轴号</param>
        public void AxisStop(int nAxis)
        {
            if (nAxis > 0)
            {
                MotionMgr.GetInstance().StopAxis(nAxis);
            }
        }

        /// <summary>
        /// 伺服急停
        /// </summary>
        public void AxisEStop()
        {
            foreach (int axis in m_nAxisArray)
            {
                if (axis > 0)
                {
                    MotionMgr.GetInstance().StopEmg(axis);
                }
            }
        }

        /// <summary>
        /// 气缸伸出
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <param name="bWait">是否等待，在Deinit中调用时一定要为false</param>
        /// <returns>0：未超时 1：超时</returns>
        public int CylOut(string strName, bool bWait = true)
        {
            if (bWait)
            {
                return CylinderMgr.GetInstance().GetCyLinder(strName).CylOut(this.ShowLog, this.WaitIo);
            }
            else
            {
                return CylinderMgr.GetInstance().GetCyLinder(strName).CylOut(this.ShowLog, null);
            }
        }

        /// <summary>
        /// 等待气缸伸出到位
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitCylOut(string strName)
        {
            return CylinderMgr.GetInstance().GetCyLinder(strName).WaitOut(this.ShowLog, this.WaitIo);
        }

        /// <summary>
        /// 气缸缩回
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <param name="bWait">是否等待,在Deinit中调用时一定要为false</param>
        /// <returns>0：未超时 1：超时</returns>
        public int CylBack(string strName, bool bWait = true)
        {
            if (bWait)
            {
                return CylinderMgr.GetInstance().GetCyLinder(strName).CylBack(this.ShowLog, this.WaitIo);
            }
            else
            {
                return CylinderMgr.GetInstance().GetCyLinder(strName).CylBack(this.ShowLog, null);
            }

        }

        /// <summary>
        /// 等待气缸缩回到位
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitCylBack(string strName)
        {
            return CylinderMgr.GetInstance().GetCyLinder(strName).WaitBack(this.ShowLog, this.WaitIo);
        }

        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="strIoName">输入IO名称</param>
        /// <param name="bUseCache">是否使用缓存状态</param>
        /// <returns>输入信号状态</returns>
        public bool GetDI(string strIoName, bool bUseCache = false)
        {
            if (bUseCache)
            {
                return IoMgr.GetInstance().GetIoInState(strIoName);
            }
            else
            {
                return IoMgr.GetInstance().ReadIoInBit(strIoName);
            }
        }

        /// <summary>
        /// 设置输出信号
        /// </summary>
        /// <param name="strIoName">输出IO名称</param>
        /// <param name="bBit">输出状态</param>
        public void SetDO(string strIoName, bool bBit)
        {
            IoMgr.GetInstance().WriteIoBit(strIoName, bBit);
        }

        /// <summary>
        /// 设置状态寄存器
        /// </summary>
        /// <param name="reg">寄存器</param>
        /// <param name="bBit">值</param>
        public void SetBit(SysBitReg reg, bool bBit)
        {
            SystemMgr.GetInstance().WriteRegBit((int)reg, bBit);
        }

        /// <summary>
        /// 获取状态寄存器
        /// </summary>
        /// <param name="reg">寄存器</param>
        /// <returns>值</returns>
        public bool GetBit(SysBitReg reg)
        {
            return SystemMgr.GetInstance().GetRegBit((int)reg);
        }

        /// <summary>
        /// 获取整形系统寄存器值
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public int GetInt(SysIntReg reg)
        {
            return SystemMgr.GetInstance().GetRegInt((int)reg);
        }

        /// <summary>
        /// 设置整形系统寄存器的值
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="nValue"></param>
        public void SetInt(SysIntReg reg, int nValue)
        {
            SystemMgr.GetInstance().WriteRegInt((int)reg, nValue);
        }

        /// <summary>
        /// 获取浮点型系统寄存器值
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public double GetDouble(SysDoubleReg reg)
        {
            return SystemMgr.GetInstance().GetRegDouble((int)reg);
        }

        /// <summary>
        /// 设置浮点型系统寄存器的值
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="dValue"></param>
        public void SetDouble(SysDoubleReg reg, double dValue)
        {
            SystemMgr.GetInstance().WriteRegDouble((int)reg, dValue);
        }

        /// <summary>
        /// 获取字符串系统寄存器值
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public string GetString(SysStrReg reg)
        {
            return SystemMgr.GetInstance().GetRegString((int)reg);
        }

        /// <summary>
        /// 设置字符串系统寄存器的值
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="strValue"></param>
        public void SetString(SysStrReg reg, string strValue)
        {
            SystemMgr.GetInstance().WriteRegString((int)reg, strValue);
        }

        /// <summary>
        /// 正常模式
        /// </summary>
        protected virtual void NormalRun()
        {

        }

        /// <summary>
        /// 自动标定
        /// </summary>
        protected virtual void AutoCalib()
        {

        }

        /// <summary>
        /// 空跑
        /// </summary>
        protected virtual void DryRun()
        {

        }

        /// <summary>
        /// GRR
        /// </summary>
        protected virtual void GrrRun()
        {

        }

        public override void StationProcess()
        {
            try
            {
                switch (SystemMgr.GetInstance().Mode)
                {
                    case SystemMode.Normal_Run_Mode:
                    case SystemMode.Simulate_Run_Mode:
                        {
                            NormalRun();
                        }
                        break;

                    case SystemMode.Calib_Run_Mode:
                        {
                            AutoCalib();
                        }
                        break;

                    case SystemMode.Dry_Run_Mode:
                        {
                            DryRun();
                        }
                        break;

                    case SystemMode.Other_Mode:
                        {
                            GrrRun();
                        }
                        break;
                }
            }
            catch (SafeException ex)
            {
                //WarningMgr.GetInstance().Info(ex.ToString());

                throw ex;
            }
            catch (StationException ex)
            {
                //WarningMgr.GetInstance().Info(ex.ToString());

                throw ex;
            }
            catch (Exception ex)
            {
                WarningMgr.GetInstance().Info(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 站位急停时调用，如果站位急停时只需要停轴，不需要重载
        /// 如果需要关闭流水线，停止机器人等IO操作，则必须重载此
        /// 函数响应急停的处理
        /// </summary>
        public override void EmgStop()
        {
            AxisEStop();

            if (m_Robot != null)
            {
                m_Robot.TriggerIo("停止");
            }

        }

        /// <summary>
        /// 响应流程暂停的处理，比如流水线在暂停时需要停止,如果不需要可以不用重载
        /// </summary>
        public override void OnPause()
        {
            if (m_Robot != null)
            {
                m_Robot.TriggerIo("暂停");
            }
        }
        /// <summary>
        /// 响应流程恢复的处理，比如流水线在恢复时需要继续运行，如果不需要可以不用重载
        /// </summary>
        public override void OnResume()
        {
            if (m_Robot != null)
            {
                m_Robot.TriggerIo("继续");
            }
        }

        /// <summary>
        /// 启用本站位所有轴响应暂停
        /// </summary>
        /// <param name="bEnable">true - 响应暂停  false - 不响应暂停</param>
        protected void EnablePause(bool bEnable)
        {
            foreach (var axis in m_nAxisArray)
            {
                if (axis > 0)
                {
                    MotionMgr.GetInstance().EnableAxisPause(axis, bEnable);
                }
            }
        }

        /// <summary>
        /// 启用具体轴响应暂停
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="bEnable">true - 响应暂停  false - 不响应暂停</param>
        protected void EnablePause(int nAxisNo, bool bEnable)
        {
            if (nAxisNo > 0)
            {
                MotionMgr.GetInstance().EnableAxisPause(nAxisNo, bEnable);
            }
        }

        /// <summary>
        /// 配置轴的软件限位
        /// </summary>
        protected void ConfigSoftLimit()
        {
            foreach (var axis in m_nAxisArray)
            {
                if (axis > 0)
                {
                    AxisCfg cfg;

                    if (MotionMgr.GetInstance().GetAxisCfg(axis, out cfg))
                    {
                        MotionMgr.GetInstance().SetSPELEnable(axis, cfg.bEnableSPEL);
                        MotionMgr.GetInstance().SetSMELEnable(axis, cfg.bEnableSMEL);

                        if (cfg.bEnableSPEL)
                        {
                            MotionMgr.GetInstance().SetSPELPos(axis, cfg.dbSPELPos);
                        }

                        if (cfg.bEnableSMEL)
                        {
                            MotionMgr.GetInstance().SetSMELPos(axis, cfg.dbSMELPos);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取bool型参数
        /// </summary>
        /// <param name="strKey">参数名称</param>
        /// <returns></returns>
        public bool GetParamBool(string strKey)
        {
            return SystemMgr.GetInstance().GetParamBool(strKey);
        }

        /// <summary>
        /// 获取Int型参数
        /// </summary>
        /// <param name="strKey">参数名称</param>
        /// <returns></returns>
        public int GetParamInt(string strKey)
        {
            return SystemMgr.GetInstance().GetParamInt(strKey);
        }

        /// <summary>
        /// 获取double型参数
        /// </summary>
        /// <param name="strKey">参数名称</param>
        /// <returns></returns>
        public double GetParamDouble(string strKey)
        {
            return SystemMgr.GetInstance().GetParamDouble(strKey);
        }

        /// <summary>
        /// 获取string型参数
        /// </summary>
        /// <param name="strKey">参数名称</param>
        /// <returns></returns>
        public string GetParamString(string strKey)
        {
            return SystemMgr.GetInstance().GetParamString(strKey);
        }

        /// <summary>
        /// 通过此方法可以让流程立马停止而不往下运行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strObject"></param>
        /// <param name="strMessage"></param>
        public void Error(ErrorType type, string strObject, string strMessage)
        {
            WarningMgr.GetInstance().Error(type, strObject, strMessage);

            WaitTimeDelay(10);
        }

        public double GetDataValue(string strDataGroup, string strData)
        {
            double fD = DataMgr.GetInstance().m_dictDataGroup[strDataGroup].m_dictData[strData].m_dbOffset.D;
            if (Math.Abs(fD) > 10)
            {
                ShowLog(strDataGroup + "-" + strData + "补偿值超限");
                fD = 0;
            }
            return fD;
        }
    }
}
