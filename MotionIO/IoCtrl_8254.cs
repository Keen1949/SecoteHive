/********************************************************************
	created:	2013/11/12
	filename: 	IOCTRL_8254
	file ext:	cpp
	author:		zjh
	purpose:	8254运控卡的IO控制实现类
*********************************************************************/

using System;
using System.Threading;
using CommonTool;
using Adlink;

namespace MotionIO
{
    /// <summary>
    /// 凌华8254自带的IO控制,20进20出，类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_8254 : IoCtrl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_8254(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            m_strCardName = "8254";
            m_strArrayIn = new string[20];
            m_strArrayOut = new string[20] ;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool  Init()
        {
            //8254自带IO，无需专门的初始化, 通过函数调用判断是否能初始化成功
            try
            {
                int Data = 0;
                int nRet = APS168.APS_read_d_input(m_nCardNo, 0, ref Data);
                if ((int)APS_Define.ERR_NoError == nRet)
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
        /// 反初始化
        /// </summary>
        public override void DeInit()
        {
        }
        
        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nData">输入信号信息</param>
        /// <returns></returns>
        public override bool  ReadIOIn(ref int nData)
        {
            //Random rnd1 = new Random();
            //m_nInData = nData = rnd1.Next();
            //return true;
            int nInputData = 0;
            int nRet = APS168.APS_read_d_input(m_nCardNo, 0, ref nInputData);
            if ((int)APS_Define.ERR_NoError == nRet)
            {
                 nInputData = (nInputData | (nInputData << 24)) >> 8;
                if (m_nInData != nInputData)
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
                //if (m_bEnable)
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡8254 ReadIOIn Error,ErrorCode = {1}", m_nCardNo, nRet));
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
            //    Random rnd1 = new Random();
            //    return rnd1.Next() % 2   == 0 ;
            int nData = 0;
            int nRet = APS168.APS_read_d_input(m_nCardNo, 0, ref nData);
            if ((int)APS_Define.ERR_NoError == nRet)
            {
                nData = (nData | (nData << 24)) >> 8;
                return (nData & (1 << (nIndex - 1))) != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡8254 ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 8254 ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡8254 ReadIoInBit {1} Error,result is {2}", m_nCardNo, nIndex, nRet));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, nRet));

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
            //   Random rnd1 = new Random();
            //   return rnd1.Next() % 2 == 0;

            int nData = 0;
            int nRet = APS168.APS_read_d_output(m_nCardNo, 0, ref nData);

            if ((int)APS_Define.ERR_NoError == nRet)
            {
                nData = (nData | (nData << 24)) >> 8;
                return (nData & (1 << (nIndex - 1))) != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡8254 ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 8254 ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡8254 ReadIoOutBit {1} Error,result is {2}", m_nCardNo, nIndex, nRet));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, nRet));

                }
                return false;
            }
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool  ReadIOOut(ref int nData)
        {
            int Data = 0;
            int nRet = APS168.APS_read_d_output(m_nCardNo, 0, ref Data);

             if ((int)APS_Define.ERR_NoError == nRet)
            {
                m_nOutData = nData =  (Data | (Data << 24)) >> 8;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡8254 ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 8254 ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡8254 ReadIOOut Error,result is {1}", m_nCardNo, nRet));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, nRet));

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
        public override bool  WriteIoBit(int nIndex, bool bBit)
        {
            if (m_bEnable)
            {
                if (nIndex > 16)
                    nIndex -= 16;
                else
                    nIndex += 8;
                return APS168.APS_write_d_channel_output(m_nCardNo, 0, nIndex - 1, bBit ? 1 : 0) == 0;
            }
            return false;
        }
        
        /// <summary>
        /// 输出行信号
        /// </summary>
        /// <param name="nData">输出信息</param>
        /// <returns></returns>
        public override bool  WriteIo(int nData)
        {
            if (m_bEnable)
            {
                nData = (nData << 8) | (nData >> 24);
                APS168.APS_write_d_output(m_nCardNo, 0, nData);
                return true;
            }
            else
                return false;
        }
    }
}