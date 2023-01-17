/********************************************************************
	created:	2013/11/12
	filename: 	IOCTRL_0640A
	file ext:	cpp
	author:		zjh
	purpose:	0640A的IO数字卡的实现类
*********************************************************************/
using System;
using System.Threading;
using CommonTool;

using csIOC0640;


namespace MotionIO
{
    /// <summary>
    /// 雷赛0640A IO卡
    /// </summary>
    public class IoCtrl_0640A : IoCtrl
    {
        private static int m_nStatInit = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl_0640A(int nIndex, int nCardNo) : base(nIndex, nCardNo)
        {
            m_bEnable = false;
            m_strCardName = "0640A";
            m_strArrayIn = new string[32];
            m_strArrayOut = new string[32];
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            return true;
            if (0 == m_nStatInit)
            {
                int ret = IOC0640.ioc_board_init();
                if (ret > 0)
                {
                    m_nStatInit = 1;
                    m_bEnable = true;
                    return true;
                }
                else
                {
                    string str1 = "第{0}张IO卡0640A初始化失败!错误码 ={1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "Initialization failed for IO card 0640A of {0}! Error code = {1}";
                    }

                    m_bEnable = false;
                    //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡0640A初始化失败!错误码 ={1}", m_nCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init, m_nCardNo.ToString(), 
                        string.Format(str1, m_nCardNo, ret));
                    return false;
                }
            }
            else
            {
                m_bEnable = true;
                return true;
            }
        }
        
        /// <summary>
        /// 释放IO卡
        /// </summary>
        public override void DeInit()
        {
            try
            {
                if (m_nCardNo >= 0)
                {
                    if (1 == m_nStatInit)
                    {
                        m_nStatInit = 0;
                        IOC0640.ioc_board_close();
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            } //忽略IOC0640关闭多张卡时报错
        }
        
        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOIn(ref int nData)
        {
            nData = IOC0640.ioc_read_inport((ushort)m_nCardNo, 0);
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
            nData = ~nData;
            if(m_nInData != nData)
            {
                m_nInData = nData;
                m_bDataChange = true;
            }
            else
            {
                m_bDataChange = false;
            }
            return true;
        }
        
        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nIndex">输入信号位 取值范围1-47</param>
        /// <returns></returns>
        public override bool ReadIoInBit(int nIndex)
        {
            int ret = IOC0640.ioc_read_inbit((ushort)m_nCardNo, (ushort)nIndex);
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
            if (0 == ret)
                return true;
            else
            {
                //if (m_bEnable)
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡0640A ReadIoInBit Error,ErrorCode = {1}", m_nCardNo, ret));
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
            nData = IOC0640.ioc_read_outport((ushort)m_nCardNo, 0);
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
            nData = ~nData;
            return true;
        }
        
        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nIndex">输出点位 1-48</param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            int ret = IOC0640.ioc_read_outbit((ushort)m_nCardNo, (ushort)nIndex);
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
            if (0 == ret)
                return true;
            else
            {
                //if (m_bEnable)
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡0640A ReadIoOutBit Error,ErrorCode = {1}", m_nCardNo, ret));
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
            {
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
                UInt32 ret = IOC0640.ioc_write_outbit((ushort)m_nCardNo, (ushort)nIndex, bBit == false ? 1 : 0);
                if (0 == ret)
                    return true;
                else
                {
                    if (m_bEnable)
                    {
                        string str1 = "第{0}张IO卡0640A WriteIoBit Error,ErrorCode = {1}";
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            str1 = "The {0} IO card 0640A WriteIoBit Error,ErrorCode = {1}";
                        }
                        //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡0640A WriteIoBit Error,ErrorCode = {1}", m_nCardNo, ret));
                        WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,
                            string.Format("{0}.{1}",m_nCardNo,nIndex),
                            string.Format(str1, m_nCardNo, ret));
                    }

                    return false;
                }
            }
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
            {
			//雷赛IO卡为PNP卡，与8254相反，为保持一致，需进行取反
                nData = ~nData;
                UInt32 ret = IOC0640.ioc_write_outport((ushort)m_nCardNo, 0, (UInt32)nData);
                if (0 == ret)
                    return true;
                else
                {
                    if (m_bEnable)
                    {
                        string str1 = "第{0}张IO卡0640A WriteIo Error,ErrorCode = {1}";
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            str1 = "The {0} IO card 0640A WriteIo Error,ErrorCode = {1}";
                        }
                        //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡0640A WriteIo Error,ErrorCode = {1}", m_nCardNo, ret));
                        WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,
                            string.Format("{0}}", m_nCardNo),
                            string.Format(str1, m_nCardNo, ret));
                    }
                        
                    return false;
                }
            }
            else
                return false;
        }

    }

}