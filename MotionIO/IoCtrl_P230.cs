using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using M400;

namespace MotionIO
{
    /// <summary>
    /// secote IO卡,16进16出, 类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_P230 : IoCtrl
    {
        private int m_nInternalIndex = -1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_P230(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            List<KeyValuePair<int, int>> ioP230List = SctCardCollect.Instance().IoP2300List;
            for (int i = 0; i < ioP230List.Count; i++)
            {
                if (ioP230List[i].Value == -1)
                {
                    m_nInternalIndex = ioP230List[i].Key;
                    ioP230List.RemoveAt(i);
                    ioP230List.Insert(i, new KeyValuePair<int, int>(m_nInternalIndex, nIndex));
                    break;
                }
            }

            m_strCardName = "P230";
            m_strArrayIn = new string[16];
            m_strArrayOut = new string[24];
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            if (m_nInternalIndex != -1)
            {
                m_bEnable = true;
                Function.SCT_WriteOutput(m_nInternalIndex, 0);
                return true;    
            }

            m_bEnable = false;

            string str1 = "第{0}张IO卡P230初始化失败!";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "P230 initialization of IO card {0} failed!";
            }
            //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡P230初始化失败!", m_nCardNo));
            WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init,m_nCardNo.ToString(),
                string.Format(str1, m_nCardNo));

            return false;
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadInput(m_nInternalIndex, ref nInputData);
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
                    string str1 = "第{0}IO卡P230 Read IOIn Error,ErrorCode = {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 Read IOIn Error,ErrorCode = {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}IO卡P230 Read IOIn Error,ErrorCode = {1}", m_nCardNo, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadInBit(m_nInternalIndex, nIndex - 1, ref uInBit);
            if (err == ErrorCode.Success)
            {
                return uInBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P230 Read ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 Read ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡P230 Read ReadIoInBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutput(m_nInternalIndex, ref nOutputData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData = (int)nOutputData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P230 ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20103,ERR-XYT,第{0}张IO卡P230 ReadIOOut Error,result is {1}", m_nCardNo, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_ReadOutBit(m_nInternalIndex, nIndex - 1, ref uOutBit);
            if (err == ErrorCode.Success)
            {
                return uOutBit != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P230 ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20104,ERR-XYT,第{0}张IO卡P230 ReadIoOutBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutBit(m_nInternalIndex, nIndex - 1, nOutBit);
            if (err == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P230 WriteIoBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 WriteIoBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20105,ERR-XYT,第{0}张IO卡P230 WriteIoBit {1} Error,result is {2}", m_nCardNo, nIndex, err));
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
            ErrorCode err = (ErrorCode)Function.SCT_WriteOutput(m_nInternalIndex, (uint)nData);
            if (err == ErrorCode.Success)
            {
                m_nOutData = nData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡P230 WriteIo Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card P230 WriteIo Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20106,ERR-XYT,第{0}张IO卡P230 WriteIo Error,result is {1}", m_nCardNo, err));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, err));

                }
                return false;
            }
        }
    }
}
