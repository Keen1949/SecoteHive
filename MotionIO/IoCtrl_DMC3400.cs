/********************************************************************
	created:	2017/04/20
	filename: 	IOCTRL_Dmc3400
	file ext:	cs
	author:		gxf
	purpose:	Dmc3400运控卡的IO控制实现类
*********************************************************************/

using System;
using System.Threading;
using CommonTool;
using csLTDMC;

namespace MotionIO
{
    /// <summary>
    /// 雷赛Dmc3400自带的IO控制,16进16出，类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_Dmc3400 : IoCtrl
    {
        private ushort m_nInternalIndex = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_Dmc3400(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            m_strCardName = "Dmc3400";
            m_strArrayIn = new string[20];
            m_strArrayOut = new string[20] ;

            m_nInternalIndex = (ushort)nCardNo;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool  Init()
        {
            //Dmc3400自带IO，无需专门的初始化, 通过函数调用判断是否能初始化成功
            try
            {
                uint uCountValue = 0;
                short ret = LTDMC.dmc_get_io_count_value(m_nInternalIndex, 0, ref uCountValue);
                if (ret == (short)DMC3400_DEFINE.ERR_NOERR)
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
            // 只有bit0~bit15是通用输入端口
            int nInputData = (int)(LTDMC.dmc_read_inport(m_nInternalIndex, 0) & 0xFFFF);
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

        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="nIndex">输入信号位</param>
        /// <returns></returns>
        public override bool ReadIoInBit(int nIndex)
        {
            short nIoState = LTDMC.dmc_read_inbit(m_nInternalIndex, (ushort)(nIndex - 1));
            return nIoState != 0;
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nIndex">输出点位</param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            short nIoState = LTDMC.dmc_read_outbit(m_nInternalIndex, (ushort)(nIndex - 1));
            return nIoState != 0;
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool  ReadIOOut(ref int nData)
        {            
            int nOutputData = (int)LTDMC.dmc_read_outport(m_nInternalIndex, 0);
            m_nOutData = nData = (nOutputData & 0xFFFF);// 只有bit0~bit15是通用输入端口
            return true;
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
                int on_off = bBit ? 1 : 0;
                short ret = LTDMC.dmc_write_outbit(m_nInternalIndex, (ushort)(nIndex - 1), (ushort)on_off);
                if (ret == (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    return true;
                }
                else
                {
                    string str1 = "第{0}张IO卡Dmc3400 WriteIoBit Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card Dmc3400 WriteIoBit Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡Dmc3400 WriteIoBit Error,result is {1}", m_nCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,string.Format("{0}.{1}",m_nCardNo,nIndex),
                        string.Format(str1, m_nCardNo, ret));

                    return false;
                }
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
                int uOutData = (int)LTDMC.dmc_read_outport(m_nInternalIndex, 0);
                uOutData = (uOutData & ~0xFFFF) | (nData & 0xFFFF);

                short ret = LTDMC.dmc_write_outport(m_nInternalIndex, 0, (uint)uOutData);
                if (ret == (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    return true;
                }
                else
                {
                    string str1 = "第{0}张IO卡Dmc3400 WriteIo Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card Dmc3400 WriteIo Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡Dmc3400 WriteIo Error,result is {1}", m_nCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, ret));

                    return false;
                }
            }
            return false;
        }
    }
}