/********************************************************************
	created:	2017/04/20
	filename: 	IoCtrl_DmcCan
	file ext:	cs
	author:		gxf
	purpose:	DmcCan运控卡的IO控制实现类
*********************************************************************/

using System;
using System.Threading;
using CommonTool;
using csLTDMC;

namespace MotionIO
{
    /// <summary>
    /// 雷赛DmcCan自带的IO控制,16进16出，类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_DmcCan : IoCtrl
    {
        /// <summary>
        /// CAN扩展模块连接的主端子板编号，默认是0
        /// </summary>
        private ushort m_nMainCardNo = 0;

        /// <summary>
        /// CAN扩展模块的节点编号
        /// </summary>
        private ushort m_nNodeNo = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_DmcCan(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            m_strCardName = "DmcCan";
            m_strArrayIn = new string[20];
            m_strArrayOut = new string[20];

            m_nNodeNo = (ushort)nCardNo;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool  Init()
        {
            try
            {
                short ret = LTDMC.dmc_set_can_state(m_nMainCardNo, m_nNodeNo, 1, 0);
                if (ret != (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    m_bEnable = false;
                    return false;
                }

                ushort nNodeNum = 0;
                ushort nCanState = 0;
                ret = LTDMC.dmc_get_can_state(m_nMainCardNo, ref nNodeNum, ref nCanState);
                if (ret != (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    m_bEnable = false;
                    return false;
                }
                if ((m_nNodeNo <= 0) || (m_nNodeNo > nNodeNum))
                {
                    m_bEnable = false;
                    return false;
                }

                if (nCanState != 1)
                {
                    m_bEnable = false;
                    return false;
                }

                m_bEnable = true;
                return true;
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
            if (m_bEnable)
            {
                int nInputData = (int)LTDMC.dmc_read_can_inport(m_nMainCardNo, m_nNodeNo, 0);
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
            short nIoState = LTDMC.dmc_read_can_inbit(m_nMainCardNo, m_nNodeNo, (ushort)(nIndex - 1));
            return nIoState != 0;
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nIndex">输出点位</param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            short nIoState = LTDMC.dmc_read_can_outbit(m_nMainCardNo, m_nNodeNo, (ushort)(nIndex - 1));
            return nIoState != 0;
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool  ReadIOOut(ref int nData)
        {            
            if (m_bEnable)
            {
                int nOutputData = (int)LTDMC.dmc_read_can_outport(m_nMainCardNo, m_nNodeNo, 0);
                m_nOutData = nData = nOutputData;
                return true;
            }
            else
            {
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
                int on_off = bBit ? 1 : 0;
                short ret = LTDMC.dmc_write_can_outbit(m_nMainCardNo, m_nNodeNo, (ushort)(nIndex - 1), (ushort)on_off);
                if (ret == (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    return true;
                }
                else
                {
                    string str1 = "第{0}张IO卡DmcCan WriteIoBit Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card DmcCan WriteIoBit Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20100,ERR-XYT,第{0}张IO卡DmcCan WriteIoBit Error,result is {1}", m_nCardNo, ret));
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
                short ret = LTDMC.dmc_write_can_outport(m_nMainCardNo, m_nNodeNo, 0, (uint)nData);
                if (ret == (short)DMC3400_DEFINE.ERR_NOERR)
                {
                    return true;
                }
                else
                {
                    string str1 = "第{0}张IO卡DmcCan WriteIo Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card DmcCan WriteIo Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡DmcCan WriteIo Error,result is {1}", m_nCardNo, ret));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Write,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, ret));

                    return false;
                }
            }

            return false;
        }
    }
}