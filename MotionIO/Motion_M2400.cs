using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonTool;
using M2400;


namespace MotionIO
{
    /// <summary>
    /// 记录所有的secote card 内部序号与外部序号的关系
    /// </summary>
    public class M2400CardScan
    {
        /// <summary>
        /// 系统支持 M2400 Card 的最大数量
        /// </summary>
        private const int CardCountMax = 8;

        /// <summary>
        /// 运动控制卡数量
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// IO卡的外部序号"控制卡编号， IO卡节点号"
        /// </summary>
        public List<KeyValuePair<int, int>> IoCardId = new List<KeyValuePair<int, int>>();
     

        /// <summary>
        /// 构造函数，列举所有运动控制卡、IO扩展卡
        /// </summary>
        public M2400CardScan()
        {
            try
            {
                int nMotionCardCount = 0;
                IoCardId.Clear();

                for (int nCardNo = 0; nCardNo < CardCountMax; nCardNo++)
                {
                    ErrorCode err = (ErrorCode)M2400.Function.SCT_Initial(nCardNo);
                    if (err == ErrorCode.Success)
                        nMotionCardCount++; 
                    else
                        continue;

                    int[] ioCardIds = new int[CardCountMax];
                    int nIoCardCount = M2400.Function.SCT_GetIoCardId(nCardNo, ioCardIds);
                    if (nIoCardCount > 0)
                    {
                        if (nIoCardCount >= ioCardIds.Length)
                            nIoCardCount = ioCardIds.Length;

                        for (int i = 0; i < nIoCardCount; i++)
                        {
                            IoCardId.Add(new KeyValuePair<int, int>(nCardNo, ioCardIds[i]));
                        }
                    }
                }
                Count = nMotionCardCount;
            }
            catch (Exception e)
            {
                string str1 = "M2400控制卡初始化失败,信息{0}";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "M2400 control card initialization failed, info {0}";
                }
                WarningMgr.GetInstance().Error(string.Format(str1, e.Message));
            }
        }



        private static M2400CardScan instance = null;

        /// <summary>
        /// 实例引用
        /// </summary>
        /// <returns></returns>
        public static M2400CardScan Instance()
        {
            if (instance == null)
            {
                instance = new M2400CardScan();
            }

            return instance;
        }
    }

    /// <summary>
    /// secote 4轴运动控制卡, 类名必须以"Motion_"前导，否则加载不到
    /// </summary>
    public class Motion_M2400 : Motion
    {
        private const int AXIS_COUNT = 4;
        private int[] m_nAxesSpeed;
        private int m_nInternalIndex = -1;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCardIndex"></param>
        /// <param name="strName"></param>
        /// <param name="nMinAxisNo"></param>
        /// <param name="nMaxAxisNo"></param>
        public Motion_M2400(int nCardIndex, string strName,int nMinAxisNo, int nMaxAxisNo):base(nCardIndex, strName, nMinAxisNo, nMaxAxisNo)
        {
            if ( (nCardIndex > 0) && (nCardIndex <= M2400CardScan.Instance().Count) )
                m_nInternalIndex = nCardIndex - 1;

            m_bEnable = false;
            m_nAxesSpeed = new int[AXIS_COUNT];
            for (int i = 0; i < m_nAxesSpeed.Length; i++)
                m_nAxesSpeed[i] = 0;
        }
       
        /// <summary>
        /// 轴卡初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            //TRACE("init card\r\n");
            string str1 = "运动控制卡M2400初始化失败!";
            string str2 = "运动控制卡M2400读取配置文件失败, result = {0}";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "M2400 initialization of motion control card failed!";
                str2 = "The motion control card M2400 failed to read the configuration file, result = {0}";
            }

            try
            {
                if (m_nInternalIndex == -1)
                {
                    m_bEnable = false;
                    //WarningMgr.GetInstance().Error(string.Format("30100,ERR-XYT, 运动控制卡M2400初始化失败!"));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Init,m_nCardIndex.ToString(),
                        string.Format(str1));

                    return false;
                }
                else if (m_nInternalIndex == 0)//配置文件只需要加载一次
                {
                    ErrorCode err = (ErrorCode)M2400.Function.SCT_LoadConfigFile("M2400Cfg.DAT");
                    if (err == ErrorCode.Success)
                    {
                        m_bEnable = true;
                        return true;
                    }
                    else
                    {
                        m_bEnable = false;
                        //WarningMgr.GetInstance().Error(string.Format("30101,ERR-XYT,运动控制卡M2400读取配置文件失败, result = {0}", err));
                        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Init,m_nCardIndex.ToString(),
                            string.Format(str2, err));

                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                m_bEnable = false;
                
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

        }
        
        /// <summary>
        /// 关闭轴卡
        /// </summary>
        /// <returns></returns>
        public override bool DeInit()
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_Initial(m_nInternalIndex);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "M2400板卡库文件关闭出错! result = {0}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "Error closing M2400 card library file! Result = {0}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("30103,ERR-XYT,M2400板卡库文件关闭出错! result = {0}", err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_DeInit,m_nCardIndex.ToString(),
                        string.Format(str1, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 给予使能
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool ServoOn(int nAxisNo)
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SetServoOn(m_nInternalIndex, nAxisNo, 1);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30104,ERR-XYT,M2400 Card Aixs {0} servo on Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_ServoOn,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card Aixs {0} servo on Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 断开使能
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool ServoOff(int nAxisNo)
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SetServoOn(m_nInternalIndex, nAxisNo, 0);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30105,ERR-XYT,M2400 Card Aixs {0} servo off Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_ServoOff,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card Aixs {0} servo off Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 读取伺服使能状态
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool GetServoState(int nAxisNo)
        {
            int nIoData = 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_GetAxisIo(m_nInternalIndex, nAxisNo, ref nIoData);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30106,ERR-XYT,M2400 Card Aixs {0} get servo state error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_State, GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card Aixs {0} get servo state error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            if ( (nIoData & (0x01 << 16)) == 0 )
                return false;
            else
                return true;
        }

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nParam">回原点方式</param>
        /// <returns></returns>
        public override bool Home(int nAxisNo, int nParam)
        {
            //ErrorCode err = (ErrorCode)M2400.Function.SCT_HomeConfig(m_nInternalIndex, nAxisNo, nParam);

            M2400.AxisConfigBase Cfg = new AxisConfigBase();

            ErrorCode err = (ErrorCode)M2400.Function.SCT_ReadConfig(m_nInternalIndex, nAxisNo, ref Cfg);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Read Config Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} Home Read Config Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            // 回原点模式 
            // 0：使用原点信号、正向硬限位有效、负向硬限位有效 
            // 1：使用原点信号 
            // 2：正向硬限位有效 
            // 3：负向硬限位有效 

            // 回原点方向，0：负方向，1：正方向 

            // 是否使用编码器零点信号，如果使用，原点定位更精确。
            // 0：不使用，1：使用 
            switch (nParam)
            {
                case (int)HomeMode.ORG_P:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 0;
                    break;

                case (int)HomeMode.ORG_N:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 0;
                    break;

                case (int)HomeMode.PEL:
                    Cfg.HomeMode = 2;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 0;
                    break;

                case (int)HomeMode.MEL:
                    Cfg.HomeMode = 3;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 0;
                    break;

                case (int)HomeMode.ORG_P_EZ:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 1;
                    break;

                case (int)HomeMode.ORG_N_EZ:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 1;
                    break;

                case (int)HomeMode.PEL_EZ:
                    Cfg.HomeMode = 2;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 1;
                    break;

                case (int)HomeMode.MEL_EZ:
                    Cfg.HomeMode = 3;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 1;
                    break;
                default:
                    if (m_bEnable)
                    {
                        //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Mode Error", nAxisNo));
                        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home, GetSysAxisNo(nAxisNo).ToString(),
                            string.Format("M2400 Axis {0} Home Mode Error", nAxisNo));

                    }
                    return false;
            }

            //2019-05-28 写不进去会报错，暂时屏蔽掉，待查看底层代码分析原因
            //err = (ErrorCode)M2400.Function.SCT_WriteConfig(m_nInternalIndex, nAxisNo, ref Cfg);
            //if (err != ErrorCode.Success)
            //{
            //    if (m_bEnable)
            //    {
            //        //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Write Config Error,result = {1}", nAxisNo, err));
            //        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home,GetSysAxisNo(nAxisNo).ToString(),
            //            string.Format("M2400 Axis {0} Home Write Config Error,result = {1}", nAxisNo, err));

            //    }
            //    return false;
            //}

            err = (ErrorCode)M2400.Function.SCT_HomeStart(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30108,ERR-XYT,M2400 Axis {0} Home Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} Home Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="nMode"></param>
        /// <param name="vm"></param>
        /// <param name="vo"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="offset"></param>
        /// <param name="sFac"></param>
        /// <returns></returns>
        public override bool Home(int nAxisNo, int nMode, double vm, double vo, double acc, double dec, double offset = 0, double sFac = 0)
        {
            M2400.AxisConfigBase Cfg = new AxisConfigBase();

            ErrorCode err = (ErrorCode)M2400.Function.SCT_ReadConfig(m_nInternalIndex, nAxisNo, ref Cfg);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Read Config Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home, GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} Home Read Config Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            // 回原点模式 
            // 0：使用原点信号、正向硬限位有效、负向硬限位有效 
            // 1：使用原点信号 
            // 2：正向硬限位有效 
            // 3：负向硬限位有效 

            // 回原点方向，0：负方向，1：正方向 

            // 是否使用编码器零点信号，如果使用，原点定位更精确。
            // 0：不使用，1：使用 
            switch ((HomeMode)nMode)
            {
                case HomeMode.ORG_P:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 0;
                    break;

                case HomeMode.ORG_N:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 0;
                    break;

                case HomeMode.PEL:
                    Cfg.HomeMode = 2;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 0;
                    break;

                case HomeMode.MEL:
                    Cfg.HomeMode = 3;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 0;
                    break;

                case HomeMode.ORG_P_EZ:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 1;
                    break;

                case HomeMode.ORG_N_EZ:
                    Cfg.HomeMode = 0;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 1;
                    break;

                case HomeMode.PEL_EZ:
                    Cfg.HomeMode = 2;
                    Cfg.HomeDir = 1;
                    Cfg.HomeUseEz = 1;
                    break;

                case HomeMode.MEL_EZ:
                    Cfg.HomeMode = 3;
                    Cfg.HomeDir = 0;
                    Cfg.HomeUseEz = 1;
                    break;

                default:
                    if (m_bEnable)
                    {
                        //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Mode Error", nAxisNo));
                        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home,GetSysAxisNo(nAxisNo).ToString(),
                            string.Format("M2400 Axis {0} Home Mode Error", nAxisNo));

                    }
                    return false;
            }

            Cfg.HomeOffset = (int)offset;
            Cfg.HomeSpeedHigh = (int)vm;
            Cfg.HomeSpeedLow = (int)vo;

            //2019-05-28 写不进去会报错，暂时屏蔽掉，待查看底层代码分析原因
            //err = (ErrorCode)M2400.Function.SCT_WriteConfig(m_nInternalIndex, nAxisNo, ref Cfg);
            //if (err != ErrorCode.Success)
            //{
            //    if (m_bEnable)
            //    {
            //        //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M2400 Axis {0} Home Write Config Error,result = {1}", nAxisNo, err));
            //        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home, GetSysAxisNo(nAxisNo).ToString(),
            //            string.Format("M2400 Axis {0} Home Write Config Error,result = {1}", nAxisNo, err));

            //    }
            //    return false;
            //}

            err = (ErrorCode)M2400.Function.SCT_HomeStart(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30108,ERR-XYT,M2400 Axis {0} Home Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home, GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} Home Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 设置轴的移动速度
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nSpeed">速度</param>
        /// <returns></returns>
        private ErrorCode SetSpeed(int nAxisNo, int nSpeed)
        {
            ErrorCode err = ErrorCode.Success;
            do
            {
                if ((nAxisNo < 0) || (nAxisNo >= AXIS_COUNT))
                {
                    err = ErrorCode.ParameterError;
                    break;
                }

                if (nSpeed == m_nAxesSpeed[nAxisNo])
                    break;

                err = (ErrorCode)M2400.Function.SCT_SetSpeed(m_nInternalIndex, nAxisNo, nSpeed);
                if (err == ErrorCode.Success)
                    m_nAxesSpeed[nAxisNo] = nSpeed;

            } while (false);
            
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30109,ERR-XYT,M2400 Axis {0} set speed Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_SetParam,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set speed Error,result = {1}", nAxisNo, err));

                }
            }                 

            return err;
        }
        
        /// <summary>
        /// 以绝对位置移动
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nPos">位置</param>
        /// <param name="nSpeed">速度</param>
        /// <returns></returns>
        public override bool AbsMove(int nAxisNo, int nPos, int nSpeed)
        {
            ErrorCode err = SetSpeed(nAxisNo, nSpeed);
            if (err != ErrorCode.Success)
            {
                return false;
            }

            err = (ErrorCode)M2400.Function.SCT_AbsoluteMove(m_nInternalIndex, nAxisNo, nPos);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} abs move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} abs move Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 以绝对位置移动
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="fPos"></param>
        /// <param name="vm"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="vs"></param>
        /// <param name="ve"></param>
        /// <param name="sFac"></param>
        /// <returns></returns>
        public override bool AbsMove(int nAxisNo, double fPos, double vm, double acc, double dec, double vs = 0, double ve = 0, double sFac = 0)
        {
            //设置加速度
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SetAcc(m_nInternalIndex,nAxisNo,(int) (vm / acc));
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            //设置减速度
            err = (ErrorCode)M2400.Function.SCT_SetDec(m_nInternalIndex, nAxisNo, (int)((vm - vs) / dec));
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            //2019-05-28 初始速度设置为0时会报错，暂时屏蔽掉，待查看底层代码分析原因
            //设置起始速度
            //err = (ErrorCode)M2400.Function.SCT_SetStartSpeed(m_nInternalIndex, nAxisNo, (int)vs);
            //if (err != ErrorCode.Success)
            //{
            //    if (m_bEnable)
            //    {
            //        //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));
            //        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
            //            string.Format("M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));

            //    }
            //    return false;
            //}

            //设置运行速度
            err = (ErrorCode)M2400.Function.SCT_SetSpeed(m_nInternalIndex, nAxisNo, (int)vm);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            err = (ErrorCode)M2400.Function.SCT_AbsoluteMove(m_nInternalIndex, nAxisNo, (int)fPos);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} abs move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} abs move Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 相对位置移动
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nPos">位置</param>
        /// <param name="nSpeed">速度</param>
        /// <returns></returns>
        public override bool RelativeMove(int nAxisNo, int nPos, int nSpeed)
        {
            ErrorCode err = SetSpeed(nAxisNo, nSpeed);
            if (err != ErrorCode.Success)
            {
                return false;
            }

            err = (ErrorCode)M2400.Function.SCT_RelativeMove(m_nInternalIndex, nAxisNo, nPos);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30111,ERR-XYT,M2400 Axis {0} relative move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} relative move Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 相对运动
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="fOffset"></param>
        /// <param name="vm"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="vs"></param>
        /// <param name="ve"></param>
        /// <param name="sFac"></param>
        /// <returns></returns>
        public override bool RelativeMove(int nAxisNo, double fOffset, double vm, double acc, double dec, double vs = 0, double ve = 0, double sFac = 0)
        {
            //设置加速度
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SetAcc(m_nInternalIndex, nAxisNo, (int)(vm / acc));
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel, GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            //设置减速度
            err = (ErrorCode)M2400.Function.SCT_SetDec(m_nInternalIndex, nAxisNo, (int)((vm - vs) / dec));
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel, GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set acc Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            //2019-05-28 初始速度设置为0时会报错，暂时屏蔽掉，待查看底层代码分析原因
            //设置起始速度
            //err = (ErrorCode)M2400.Function.SCT_SetStartSpeed(m_nInternalIndex, nAxisNo, (int)vs);
            //if (err != ErrorCode.Success)
            //{
            //    if (m_bEnable)
            //    {
            //        //WarningMgr.GetInstance().Error(string.Format("30111,ERR-XYT,M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));
            //        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel,GetSysAxisNo(nAxisNo).ToString(),
            //            string.Format("M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));

            //    }
            //    return false;
            //}

            //设置运行速度
            err = (ErrorCode)M2400.Function.SCT_SetSpeed(m_nInternalIndex, nAxisNo, (int)vm);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30111,ERR-XYT,M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} set start speed Error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            err = (ErrorCode)M2400.Function.SCT_RelativeMove(m_nInternalIndex, nAxisNo, (int)fOffset);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30111,ERR-XYT,M2400 Axis {0} relative move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Axis {0} relative move Error,result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// JOG运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="bPositive">方向</param>
        /// <param name="bStart">未使用</param>
        /// <param name="nSpeed">速度</param>
        /// <returns></returns>
        public override bool JogMove(int axis, bool bPositive, int bStart, int nSpeed)
        {
            int nDir = bPositive ? 1 : 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_JogMove(m_nInternalIndex, axis, nDir);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30112,ERR-XYT,M2400 Axis {0} jog move Error,result = {1}", axis, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Jog,GetSysAxisNo(axis).ToString(),
                        string.Format("M2400 Axis {0} jog move Error,result = {1}", axis, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 轴正常停止
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool StopAxis(int nAxisNo)
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_DecStop(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30113,ERR-XYT,M2400 Card normal stop axis {0} Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Stop,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card normal stop axis {0} Error, result = {1}", nAxisNo, err));

                }
                return false;
            }  
        }

        /// <summary>
        /// 急停
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool StopEmg(int nAxisNo)
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SuddenStop(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30114,ERR-XYT,M2400 Card Emergency stop axis {0} Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_EmgStop,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card Emergency stop axis {0} Error, result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 获取轴卡运动状态
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override long GetMotionState(int nAxisNo)
        {
            int nIsMoving = 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_IsMoving(m_nInternalIndex, nAxisNo, ref nIsMoving);
            if (err == ErrorCode.Success)
            {
                return nIsMoving;  //1:moving,0:stop
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取轴卡运动IO信号
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override long GetMotionIoState(int nAxisNo)
        {
            // M2400 motion io table
            // |-bit5--|--4--|----3----|----2----|----1----|----0----|
            // |  EMG  | ALM | HLMT(-) | HLMT(+) | SLMT(-) | SLMT(+) |
            // |-bit11-|--10--|--9--|--8--|
            // |  ORG  |  EZ  | INP | RDY |
            // |-bit17-|--16--|
            // |  CLR  |SVR-ON|
            int nIoData = 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_GetAxisIo(m_nInternalIndex, nAxisNo, ref nIoData);
            if (err != ErrorCode.Success)
                return -1;

            // 8254 motion io table
            // |-bit0-|--1--|--2--|--3--|--4--|--5--|--6--|--7--|--8--|...|--11--|--12--|
            // |-ALM--|-PEL-|-MEL-|-ORG-|-EMG-|-EZ--|-INP-|-SVO-|-RDY-|...|-SPEL-|-SMEL-|
            int nMotionState = 0;
            if ((nIoData & (0x01 << 0)) != 0)
                nMotionState |= (0x01 << 11);

            if ((nIoData & (0x01 << 1)) != 0)
                nMotionState |= (0x01 << 12);

            if ((nIoData & (0x01 << 2)) != 0)
                nMotionState |= (0x01 << 1);

            if ((nIoData & (0x01 << 3)) != 0)
                nMotionState |= (0x01 << 2);

            if ((nIoData & (0x01 << 4)) != 0)
                nMotionState |= (0x01 << 0);

            if ((nIoData & (0x01 << 5)) != 0)
                nMotionState |= (0x01 << 4);

            if ((nIoData & (0x01 << 8)) != 0)
                nMotionState |= (0x01 << 8);

            if ((nIoData & (0x01 << 9)) != 0)
                nMotionState |= (0x01 << 6);

            if ((nIoData & (0x01 << 10)) != 0)
                nMotionState |= (0x01 << 5);

            if ((nIoData & (0x01 << 11)) != 0)
                nMotionState |= (0x01 << 3);

            if ((nIoData & (0x01 << 16)) != 0)
                nMotionState |= (0x01 << 7);

            return nMotionState;
        }

        /// <summary>
        /// 获取轴的当前位置
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override double GetAixsPos(int nAxisNo)
        {
            int nPos = 0;
            ErrorCode Err = (ErrorCode)M2400.Function.SCT_GetRealPos(m_nInternalIndex, nAxisNo, ref nPos);
            if (Err == ErrorCode.Success)
            {
                return nPos;
            }
            else
            {
                return 0xFFFFFFFF;
            }
        }

        /// <summary>
        /// 判定轴是否正常停止
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public override int IsAxisNormalStop(int nAxisNo)
        {
            int nStopCode = 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_GetStopData(m_nInternalIndex, nAxisNo, ref nStopCode);
            if (err == ErrorCode.Success)
            {
                if (nStopCode == -1)//正在运行，还未停止
                {
                    return -1;
                }

                StopReason reason = (StopReason)nStopCode;
                if (reason == StopReason.Normal)//正常停止
                {
                    return 0;
                }
                else if (reason == StopReason.OnEmergencyAlarm)//急停
                {
                    Debug.WriteLine("Axis {0} has emergency alarm signal. \r\n", nAxisNo);
                    return 1;
                }
                else if (reason == StopReason.OnAlarm)//报警
                {
                    Debug.WriteLine("Axis {0} has servo alarm signal. \r\n", nAxisNo);
                    return 2;
                }
                else if (reason == StopReason.OnServoOff)// 伺服使能没有打开
                {
                    Debug.WriteLine("Axis {0} servo off. \r\n", nAxisNo);
                    return 3;
                }
                else if (reason == StopReason.OnHardLimitPos)//正限位
                {
                    Debug.WriteLine("Axis {0} has PEL signal. \r\n", nAxisNo);
                    return 4;
                }
                else if (reason == StopReason.OnHardLimitNeg)// 负限位
                {
                    Debug.WriteLine("Axis {0} has MEL signal. \r\n", nAxisNo);
                    return 5;
                }
                else if (reason == StopReason.OnSoftLimitPos)//正向软限位
                {
                    Debug.WriteLine("Axis {0} arrive at maximum soft limit. \r\n", nAxisNo);
                    return 4;
                }
                else if (reason == StopReason.OnSoftLimitNeg)//负向软限位
                {
                    Debug.WriteLine("Axis {0} arrive at minimum soft limit. \r\n", nAxisNo);
                    return 5;
                }
                else
                {
                    Debug.WriteLine("M2400 Axis {0} stop code : {1}. \r\n", nAxisNo, nStopCode);
                    return 6+10;//todo
                }
            }
            else
            {
                return -2;//调用异常
            }
        }
        /// <summary>
        /// 判定轴是否到位
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <param name="nInPosError"></param>
        /// <returns></returns>
        public override int IsAxisInPos(int nAxisNo, int nInPosError = 1000)
        {
            int nRet = IsAxisNormalStop(nAxisNo);
            if(nRet == 0)
            {
                int nTargetPos = 0;
                int nFeedbackPos = 0;
                M2400.Function.SCT_GetTargetPos(m_nInternalIndex, nAxisNo, ref nTargetPos);
                M2400.Function.SCT_GetRealPos(m_nInternalIndex, nAxisNo, ref nFeedbackPos);

                if (Math.Abs(nFeedbackPos - nTargetPos) > nInPosError)
                    return 6;  //轴停止后位置超限
            }
            return nRet;
        }

        /// <summary>
        /// 位置清零
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <returns></returns>
        public override bool SetPosZero(int nAxisNo)
        {
            ErrorCode err = (ErrorCode)M2400.Function.SCT_SetCommandPos(m_nInternalIndex, nAxisNo, 0);
            if (err != ErrorCode.Success)
                return false;

            err = (ErrorCode)M2400.Function.SCT_SetRealPos(m_nInternalIndex, nAxisNo, 0);
            if (err != ErrorCode.Success)
                return false;

            return true;
        }
        
        /// <summary>
        /// 速度模式旋转轴
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nSpeed">速度</param>
        /// <returns></returns>
        public override bool VelocityMove(int nAxisNo, int nSpeed)
        {
            int nDir = nSpeed > 0 ? 1 : 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_JogMove(m_nInternalIndex, nAxisNo, nDir);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30115,ERR-XYT,M2400 Card axis {0} VelocityMove Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Vel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M2400 Card axis {0} VelocityMove Error, result = {1}", nAxisNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 回原点是否正常停止
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public override int IsHomeNormalStop(int nAxisNo)
        {
            int nHomeFinished = 0;
            int nHomeRunning = 0;
            ErrorCode err = (ErrorCode)M2400.Function.SCT_GetHomeState(m_nInternalIndex, nAxisNo, ref nHomeRunning, ref nHomeFinished);
            if (err != ErrorCode.Success)
                return -1;

            if (nHomeRunning != 0)
            {
                // 正在运行
                return -1;
            }
            else
            {
                if (nHomeFinished != 0)
                {
                    // 回原点完成
                    return 0;           
                }
                else
                {
                    // 回原点没有完成
                    int nRet = IsAxisNormalStop(nAxisNo);
                    return nRet;
                }
            }
        }
    }
}
