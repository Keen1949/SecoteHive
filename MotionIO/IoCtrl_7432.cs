//2019-08-21 Binggoo 1.加入内部卡号，内部卡号通过板卡初始化后获取，防止配置卡号和实际初始化后卡号不匹配。
/********************************************************************
	created:	2013/11/12
	filename: 	IOCTRL_7432
	file ext:	cpp
	author:		zjh
	purpose:	7432的IO数字卡的实现类
*********************************************************************/
using System;
using System.Threading;
using CommonTool;

using Adlink;


namespace MotionIO
{
    /// <summary>
    /// 凌华7432 IO卡, 类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_7432 : IoCtrl
    {
        private int m_nInternalCardNo = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl_7432(int nIndex, int nCardNo) : base(nIndex, nCardNo)
        {
            m_bEnable = false;
            m_strCardName = "7432";
            m_strArrayIn = new string[32];
            m_strArrayOut = new string[32];
        }

          /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            short ret = DASK.Register_Card(DASK.PCI_7432, (ushort)m_nCardNo);
            if (DASK.NoError <= ret)
            {
                m_nInternalCardNo = ret;
                m_bEnable = true;
                DASK.DO_WritePort((ushort)m_nInternalCardNo, 0, 0);
                return true;
            }
            else
            {
                m_bEnable = false;
                string str1 = "第{0}张IO卡7432初始化失败!错误码 ={1}";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Initialization failed for IO card 7432 of {0}! Error code = {1}";
                }
                //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡7432初始化失败!错误码 ={1}", m_nInternalCardNo, ret));
                WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init,m_nInternalCardNo.ToString(),
                    string.Format(str1, m_nCardNo, ret));
                return false;
            }
        }
        
        /// <summary>
        /// 释放IO卡
        /// </summary>
        public override void DeInit()
        {
            if (m_nInternalCardNo >= 0)
                DASK.Release_Card((ushort)m_nInternalCardNo);
        }
        
        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOIn(ref int nData)
        {
            uint nInputData = 0;
            short ret = DASK.DI_ReadPort((ushort)m_nInternalCardNo, 0, out nInputData);
            if (DASK.NoError == ret)
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
                    string str1 = "第{0}IO卡7432 ReadIOIn Error,ErrorCode = {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 7432 ReadIOIn Error,ErrorCode = {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}IO卡7432 ReadIOIn Error,ErrorCode = {1}", m_nInternalCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,m_nInternalCardNo.ToString(),
                        string.Format(str1, m_nCardNo, ret));

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
            ushort nData = 0;
            short ret = DASK.DI_ReadLine((ushort)m_nInternalCardNo, 0, (ushort)(nIndex - 1), out nData);
            if (DASK.NoError == ret)
            {
                return nData != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡7432 ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 7432 ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡7432 ReadIoInBit {1} Error,result is {2}", m_nInternalCardNo, nIndex, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,string.Format("{0}.{1}",m_nInternalCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, ret));

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
            uint tmp = 0;
            short ret = DASK.DO_ReadPort((ushort)m_nInternalCardNo, 0, out tmp);
            if (DASK.NoError == ret)
            {
                nData = m_nOutData = (int)tmp;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡7432 ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 7432 ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20103,ERR-XYT,第{0}张IO卡7432 ReadIOOut Error,result is {1}", m_nInternalCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,m_nInternalCardNo.ToString(),
                        string.Format(str1, m_nCardNo, ret));

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
            ushort nData = 0;
            short ret = DASK.DO_ReadLine((ushort)m_nInternalCardNo, 0, (ushort)(nIndex - 1), out nData);
            if (DASK.NoError == ret)
            {
                return nData != 0;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡7432 ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card 7432 ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20104,ERR-XYT,第{0}张IO卡7432 ReadIoOutBit {1} Error,result is {2}", m_nInternalCardNo, nIndex, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,string.Format("{0}.{1}",m_nInternalCardNo,nIndex),
                        string.Format(str1, m_nCardNo, nIndex, ret));

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
            if (m_bEnable)
                return DASK.DO_WriteLine((ushort)m_nInternalCardNo, 0, (ushort)(nIndex - 1), bBit == true ? ((ushort)(1)) : ((ushort)(0))) == 0;
            else
                return false;
        }
        
        /// <summary>
        /// 输出信号
        /// </summary>
        /// <param name="nData">输出信息</param>
        /// <returns></returns>
        public override bool WriteIo(int nData)
        {
            if (m_bEnable)
                return DASK.DO_WritePort((ushort)m_nInternalCardNo, 0, (uint)nData) == 0;
            else
                return false;
        }

    }

}