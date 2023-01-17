using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using P440;

namespace MotionIO
{
    /// <summary>
    /// 记录所有的secote card 内部序号与外部序号的关系
    /// </summary>
    public class P440CardScan
    {
        /// <summary>
        /// 系统支持 P440 Card 的最大数量
        /// </summary>
        private const int CardCountMax = 8;

        /// <summary>
        /// 卡数量
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// 构造函数，列举所有运动控制卡、IO扩展卡
        /// </summary>
        public P440CardScan()
        {
            try
            {
                int nIoCardCount = 0;
                for (int nCardNo = 0; nCardNo < CardCountMax; nCardNo++)
                {
                    ErrorCode err = (ErrorCode)M2400.Function.SCT_Initial(nCardNo);
                    if (err == ErrorCode.Success)
                        nIoCardCount++;
                    else
                        break;
                }
                Count = nIoCardCount;
            }
            catch (Exception e)
            {
                string str1 = "P440卡初始化失败,信息{0}";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "P440 card initialization failed, info {0}";
                }
                //WarningMgr.GetInstance().Error(string.Format("P440卡初始化失败,信息{0}", e.Message));
                WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init, "P440",
                    string.Format(str1, e.Message));

            }
        }

        /// <summary>
        /// 实例引用
        /// </summary>
        private static P440CardScan instance = null;

        /// <summary>
        /// 实例引用
        /// </summary>
        /// <returns></returns>
        public static P440CardScan Instance()
        {
            if (instance == null)
            {
                instance = new P440CardScan();
            }

            return instance;
        }
    }

    /// <summary>
    /// secote IO卡,32进32出, 类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_P440 : IoCtrl
    {
        private int m_nInternalIndex = -1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_P440(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            if ((nCardNo >= 0) && (nCardNo < M2400CardScan.Instance().Count))
                m_nInternalIndex = nCardNo;

            m_strCardName = "P440";
            m_strArrayIn = new string[32];
            m_strArrayOut = new string[32];
            //Init();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            bool bEnable = false;
            try
            {
                if (m_nInternalIndex != -1)
                {
                    uint nInputData = 0;
                    ErrorCode err = (ErrorCode)Function.SCT_ReadInput(m_nInternalIndex, 0, ref nInputData);
                    if (err == ErrorCode.Success)
                    {
                        bEnable = true;
                    }
                }
            }
            catch
            {
                bEnable = false;
            }

            m_bEnable = bEnable;
            if (!bEnable)
            {
                //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡P440初始化失败!", m_nCardNo));
                WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init,m_nCardNo.ToString(),
                    string.Format("第{0}张IO卡P440初始化失败!", m_nCardNo));

            }
            return bEnable;
        }

        /// <summary>
        /// 释放IO卡
        /// </summary>
        public override void DeInit()
        {

        }
        
        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nData">输入信号信息</param>
        /// <returns></returns>
        public override bool ReadIOIn(ref int nData)
        {
            if (m_bEnable == false)
                return false;
            uint nInputData = 0;
            ErrorCode err = (ErrorCode)Function.SCT_ReadInput(m_nInternalIndex, 0, ref nInputData);
            if (err == ErrorCode.Success)
            {
                if (m_nInData != (int)nInputData)
                {
                    m_bDataChange = true;
                    m_nInData = nData = (int)nInputData;
                }
                else
                {
                    m_bDataChange = false;
                    nData = m_nInData;
                }
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}IO卡P440 Read IOIn Error,ErrorCode = {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 Read IOIn Error,ErrorCode = {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}IO卡P440 Read IOIn Error,ErrorCode = {1}", m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, err));

                }
                return false;
            }
        }

        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nIndex">输入信号位</param>
        /// <returns></returns>
        public override bool ReadIoInBit(int nIndex)
        {
            UInt32 uInBit = 0;
            ErrorCode err = (ErrorCode)Function.SCT_ReadInBit(m_nInternalIndex, 0, nIndex - 1, ref uInBit);
            if (err == ErrorCode.Success)
            {
                return uInBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P440 Read ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 Read ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡P440 Read ReadIoInBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, err));

                }
                return false;
            }
        }
        
        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOOut(ref int nData)
        {
            uint nOutputData = 0;
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutput(m_nInternalIndex, 0, ref nOutputData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData = (int)nOutputData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P440 ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20103,ERR-XYT,第{0}张IO卡P440 ReadIOOut Error,result is {1}", m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, err));

                }
                return false;
            }
        }
        
        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nIndex">输出点位</param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            UInt32 uOutBit = 0;
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutBit(m_nInternalIndex, 0, nIndex - 1, ref uOutBit);
            if (err == ErrorCode.Success)
            {
                return uOutBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P440 ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20104,ERR-XYT,第{0}张IO卡P440 ReadIoOutBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, err));

                }
                return false;
            }
        }
        
        
        /// <summary>
        /// 输出信号
        /// </summary>
        /// <param name="nIndex">输出点</param>
        /// <param name="bBit">输出值</param>
        /// <returns></returns>
        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            int nOutBit = bBit ? 1 : 0;
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutBit(m_nInternalIndex, 0, nIndex - 1, nOutBit);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P440 WriteIoBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 WriteIoBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20105,ERR-XYT,第{0}张IO卡P440 WriteIoBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, err));

                }
                return false;
            }
        }
        
        /// <summary>
        /// 输出信号
        /// </summary>
        /// <param name="nData">输出信息</param>
        /// <returns></returns>
        public override bool WriteIo(int nData)
        {
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutput(m_nInternalIndex, 0, (uint)nData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P440 WriteIo Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P440 WriteIo Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20106,ERR-XYT,第{0}张IO卡P440 WriteIo Error,result is {1}", m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, err));

                }
                return false;
            }
        }
    }
}
