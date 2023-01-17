using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonTool;
using M400;


namespace MotionIO
{
    /// <summary>
    /// 记录所有的secote card 内部序号与外部序号的关系
    /// </summary>
    public class SctCardCollect
    {
        /// <summary>
        /// 系统支持 secote card 的最大数量
        /// </summary>
        private const int SctCardCountMax = 8;

        /// <summary>
        /// 记录M400运动控制卡：内部序号+外部序号
        /// </summary>
        public List<KeyValuePair<int, int>> MotionM400List = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// 记录M400的IO卡：内部序号+外部序号
        /// </summary>
        public List<KeyValuePair<int, int>> IoM400List = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// 记录P230卡：内部序号+外部序号
        /// </summary>
        public List<KeyValuePair<int, int>> IoP2300List = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// 构造函数，列举所有的M400卡、P230卡
        /// </summary>
        public SctCardCollect()
        {
            try
            {
                for (int nCardNo = 0; nCardNo < SctCardCountMax; nCardNo++)
                {
                    ErrorCode err = (ErrorCode)Function.SCT_Initial(nCardNo);
                    if (err != ErrorCode.Success)
                        break;

                    int nCardType = 0;
                    err = (ErrorCode)Function.SCT_GetCardType(nCardNo, ref nCardType);
                    if (err != ErrorCode.Success)
                        break;

                    if (nCardType == 0)
                    {
                        IoP2300List.Add(new KeyValuePair<int, int>(nCardNo, -1));
                    }
                    else if (nCardType == 1)
                    {
                        MotionM400List.Add(new KeyValuePair<int, int>(nCardNo, -1));
                        IoM400List.Add(new KeyValuePair<int, int>(nCardNo, -1));
                    }
                    else
                    {
                        break;
                    }
                }                
            }
            catch (Exception e)
            {
                string str1 = "M400控制卡初始化失败,信息{0}";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "M400 control card initialization failed, information {0}";
                }
                WarningMgr.GetInstance().Error(string.Format(str1, e.Message));

        //        System.Windows.Forms.MessageBox.Show(e.Message, "M400控制卡初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private static SctCardCollect instance;

        /// <summary>
        /// 实例引用
        /// </summary>
        /// <returns></returns>
        public static SctCardCollect Instance()
        {
            if (instance == null)
            {
                instance = new SctCardCollect();
            }

            return instance;
        }
    }

    /// <summary>
    /// secote 4轴运动控制卡, 类名必须以"Motion_"前导，否则加载不到
    /// </summary>
    public class Motion_M400 : Motion
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
        public Motion_M400(int nCardIndex, string strName,int nMinAxisNo, int nMaxAxisNo):base(nCardIndex, strName, nMinAxisNo, nMaxAxisNo)
        {
            List<KeyValuePair<int, int>> motionM400List = SctCardCollect.Instance().MotionM400List;
            for (int i = 0; i < motionM400List.Count; i++)
            {
                if (motionM400List[i].Value == -1)
                {
                    m_nInternalIndex = motionM400List[i].Key;
                    motionM400List.RemoveAt(i);
                    motionM400List.Insert(i, new KeyValuePair<int, int>(m_nInternalIndex, nCardIndex));
                    break;
                }
            }

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
            string str1 = "运动控制卡M400读取配置文件失败, result = {0}";
            string str2 = "运动控制卡M400初始化失败!";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "The motion control card M400 failed to read the configuration file, result = {0}";
                str2 = "Initialization of motion control card M400 failed!";
            }
            try
            {
                if (m_nInternalIndex != -1)
                {
                    ErrorCode err = (ErrorCode)Function.SCT_LoadConfigFile("M400Cfg.DAT", m_nInternalIndex);
                    if (err == ErrorCode.Success)
                    {
                        m_bEnable = true;
                        return true;
                    }
                    else
                    {
                        m_bEnable = false;
                        //WarningMgr.GetInstance().Error(string.Format("30101,ERR-XYT,运动控制卡M400读取配置文件失败, result = {0}", err));
                        WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Init,m_nCardIndex.ToString(),
                            string.Format(str1, err));

                        return false;
                    }
                }
                else
                {
                    m_bEnable = false;
                    //WarningMgr.GetInstance().Error(string.Format("30100,ERR-XYT, 运动控制卡M400初始化失败!"));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Init,m_nCardIndex.ToString(),
                        string.Format(str2));

                    return false;
                }
            }
            catch (Exception e)
            {
                m_bEnable = false;
                //WarningMgr.GetInstance().Error(string.Format("30100,ERR-XYT, 运动控制卡M400初始化失败!"));
                WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Init,m_nCardIndex.ToString(),
                    string.Format(str2));

                System.Diagnostics.Debug.WriteLine(e.Message);
                //         System.Windows.Forms.MessageBox.Show(e.Message, "控制卡M400初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        
        /// <summary>
        /// 关闭轴卡
        /// </summary>
        /// <returns></returns>
        public override bool DeInit()
        {
            ErrorCode err = (ErrorCode)Function.SCT_Initial(m_nInternalIndex);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "M400板卡库文件关闭出错! result = {0}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "M400 board card library file close error! Result = {0}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("30102,ERR-XYT,M400板卡库文件关闭出错! result = {0}", err));
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
            ErrorCode err = (ErrorCode)Function.SCT_SetServoOn(m_nInternalIndex, nAxisNo, 1);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30103,ERR-XYT,M400 Card Aixs {0} servo on Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_ServoOn,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card Aixs {0} servo on Error,result = {1}", nAxisNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_SetServoOn(m_nInternalIndex, nAxisNo, 0);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30103,ERR-XYT,M400 Card Aixs {0} servo off Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_ServoOff,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card Aixs {0} servo off Error,result = {1}", nAxisNo, err));

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
            int nEnable = 0;
            ErrorCode err = (ErrorCode)Function.SCT_GetServoOnState(m_nInternalIndex, nAxisNo, ref nEnable);
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30104,ERR-XYT,M400 Card Aixs {0} get servo state error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_State,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card Aixs {0} get servo state error,result = {1}", nAxisNo, err));

                }
                return false;
            }

            return nEnable == 0 ? false : true;
        }

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="nParam">回原点方式</param>
        /// <returns></returns>
        public override bool Home(int nAxisNo, int nParam)
        {          
            ErrorCode err = (ErrorCode)Function.SCT_HomeStart(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30104,ERR-XYT,M400 Axis {0} Home Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Home,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Axis {0} Home Error,result = {1}", nAxisNo, err));

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

                err = (ErrorCode)Function.SCT_SetSpeed(m_nInternalIndex, nAxisNo, nSpeed);
                if (err == ErrorCode.Success)
                    m_nAxesSpeed[nAxisNo] = nSpeed;

            } while (false);
            
            if (err != ErrorCode.Success)
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30105,ERR-XYT,M400 Axis {0} set speed Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_SetParam,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Axis {0} set speed Error,result = {1}", nAxisNo, err));
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

            err = (ErrorCode)Function.SCT_AbsoluteMove(m_nInternalIndex, nAxisNo, nPos);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30106,ERR-XYT,M400 Axis {0} abs move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Abs,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Axis {0} abs move Error,result = {1}", nAxisNo, err));

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

            err = (ErrorCode)Function.SCT_RelativeMove(m_nInternalIndex, nAxisNo, nPos);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M400 Axis {0} relative move Error,result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Rel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Axis {0} relative move Error,result = {1}", nAxisNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_JogMove(m_nInternalIndex, axis, nDir);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30107,ERR-XYT,M400 Axis {0} jog move Error,result = {1}", axis, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Jog,GetSysAxisNo(axis).ToString(),
                        string.Format("M400 Axis {0} jog move Error,result = {1}", axis, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_DecStop(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30109,ERR-XYT,M400 Card normal stop axis {0} Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Stop,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card normal stop axis {0} Error, result = {1}", nAxisNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_SuddenStop(m_nInternalIndex, nAxisNo);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30110,ERR-XYT,M400 Card Emergency stop axis {0} Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_EmgStop,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card Emergency stop axis {0} Error, result = {1}", nAxisNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_IsMoving(m_nInternalIndex, nAxisNo, ref nIsMoving);
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
            int nMotionState = 0;
            ErrorCode err = (ErrorCode)Function.SCT_GetMotionState(m_nInternalIndex, nAxisNo, ref nMotionState);
            if (err != ErrorCode.Success)
                return -1;

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
            ErrorCode Err = (ErrorCode)Function.SCT_GetRealPos(m_nInternalIndex, nAxisNo, ref nPos);
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
        /// 判定轴是否已经正常停止
        /// </summary>
        /// <param name="nAxisNo"></param>
        /// <returns></returns>
        public override int IsAxisNormalStop(int nAxisNo)
        {
            int nIsMoving = 0;
            ErrorCode err = (ErrorCode)Function.SCT_IsMoving(m_nInternalIndex, nAxisNo, ref nIsMoving);
            if (err != ErrorCode.Success)
            {
                return -2;//调用异常
            }
            if (nIsMoving != 0)
            {
                return -1;//正在运行，还未停止
            }

            int nStopCode = 0;
            err = (ErrorCode)Function.SCT_GetStopData(m_nInternalIndex, nAxisNo, ref nStopCode);
            if (err == ErrorCode.Success)
            {
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
                    Debug.WriteLine("M400 Axis {0} stop code : {1}. \r\n", nAxisNo, nStopCode);
                    return nStopCode+10;//todo
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
                Function.SCT_GetTargetPos(m_nInternalIndex, nAxisNo, ref nTargetPos);
                Function.SCT_GetRealPos(m_nInternalIndex, nAxisNo, ref nFeedbackPos);

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
            ErrorCode err = (ErrorCode)Function.SCT_SetCommandPos(m_nInternalIndex, nAxisNo, 0);
            if (err != ErrorCode.Success)
                return false;

            err = (ErrorCode)Function.SCT_SetRealPos(m_nInternalIndex, nAxisNo, 0);
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
            ErrorCode err = (ErrorCode)Function.SCT_JogMove(m_nInternalIndex, nAxisNo, nDir);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    //WarningMgr.GetInstance().Error(string.Format("30111,ERR-XYT,M400 Card axis {0} VelocityMove Error, result = {1}", nAxisNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Motion_Vel,GetSysAxisNo(nAxisNo).ToString(),
                        string.Format("M400 Card axis {0} VelocityMove Error, result = {1}", nAxisNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_GetHomeState(m_nInternalIndex, nAxisNo, ref nHomeRunning, ref nHomeFinished);
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
