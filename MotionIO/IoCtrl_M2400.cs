using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using M2400;

namespace MotionIO
{
    /// <summary>
    /// secote 运动控制卡自带的IO控制,20进20, 类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_M2400 : IoCtrl
    {
        private int m_nCardId = -1;
        private int m_nNodeId = -1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl_M2400(int nIndex, int nCardNo) : base(nIndex, nCardNo)
        {
            if ((nCardNo >= 0) && (nCardNo < M2400CardScan.Instance().IoCardId.Count))
            {
                m_nCardId = M2400CardScan.Instance().IoCardId[nCardNo].Key;
                m_nNodeId = M2400CardScan.Instance().IoCardId[nCardNo].Value;
            }
            //m_nCardId = 0;
            //m_nNodeId = nCardNo;
            m_strCardName = "M2400";

            m_strArrayIn = new string[20];
            m_strArrayOut = new string[20];

            //if (m_nNodeId == 0)
            //{
            //    m_strArrayIn = new string[20];
            //    m_strArrayOut = new string[20];
            //}
            //else
            //{
            //    m_strArrayIn = new string[32];
            //    m_strArrayOut = new string[32];
            //}
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            try
            {
                uint nInputData = 0;
                ErrorCode err = (ErrorCode)Function.SCT_ReadInput(m_nCardId, m_nNodeId, ref nInputData);
                if (err == ErrorCode.Success)
                {
                    m_bEnable = true;
                    return true;
                }
                else
                {
                    m_bEnable = false;
                    return false;
                }
            }
            catch
            {
                m_bEnable = false;
                return false;
            }
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
            uint nInputData = 0;
            ErrorCode err = (ErrorCode)Function.SCT_ReadInput(m_nCardId, m_nNodeId, ref nInputData);
            if (err == ErrorCode.Success)
            {
                //2019-04-19 Binggoo int 与 uint不能直接比较需要强制转换后比较
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
                    string str1 = "第{0}张IO卡M2400 Read IOIn Error,ErrorCode = {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card M2400 Read IOIn Error,ErrorCode = {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡M2400 Read IOIn Error,ErrorCode = {1}", base.m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,m_nCardNo.ToString(),
                        string.Format(str1, base.m_nCardNo, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_ReadInBit(m_nCardId, m_nNodeId, nIndex - 1, ref uInBit);
            if (err == ErrorCode.Success)
            {
                return uInBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡M2400 Read ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card M2400 Read ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡M2400 Read ReadIoInBit {1} Error,result is {2}", base.m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, base.m_nCardNo, nIndex, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutput(m_nCardId, m_nNodeId, ref nOutputData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData = (int)nOutputData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡M2400 ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "THe {0} IO card M2400 ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20103,ERR-XYT,第{0}张IO卡M2400 ReadIOOut Error,result is {1}", base.m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,m_nCardNo.ToString(),
                        string.Format(str1, base.m_nCardNo, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutBit(m_nCardId, m_nNodeId, nIndex - 1, ref uOutBit);
            if (err == ErrorCode.Success)
            {
                return uOutBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡M2400 ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card M2400 ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20104,ERR-XYT,第{0}张IO卡M2400 ReadIoOutBit {1} Error,result is {2}", base.m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, base.m_nCardNo, nIndex, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutBit(m_nCardId, m_nNodeId, nIndex - 1, nOutBit);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡M2400 WriteIoBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card M2400 WriteIoBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20105,ERR-XYT,第{0}张IO卡M2400 WriteIoBit {1} Error,result is {2}", base.m_nCardNo, nIndex, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, base.m_nCardNo, nIndex, err));

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
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutput(m_nCardId, m_nNodeId, (uint)nData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡M2400 WriteIo Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card M2400 WriteIo Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20106,ERR-XYT,第{0}张IO卡M2400 WriteIo Error,result is {1}", base.m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,m_nCardNo.ToString(),
                        string.Format(str1, base.m_nCardNo, err));

                }
                return false;
            }
        }
    }
}
