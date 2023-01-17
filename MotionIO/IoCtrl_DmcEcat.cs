/********************************************************************
	created:	2018/09/14
	filename: 	IOCTRL_DmcEcat
	file ext:	cs
	author:		gxf
	purpose:	DmcEcat运控卡的IO控制实现类
*********************************************************************/
//2019-4-29 Binggoo 1.雷赛EtherCAT控制卡的IO控制的前8个IO为板卡专用IO，不能作为通用IO使用。
//                  2.支持多组IO，通过nCardNo参数区分分组。
using System;
using System.Threading;
using CommonTool;
using csLTDMC;

namespace MotionIO
{
    /// <summary>
    /// 雷赛EtherCAT控制卡的IO控制，类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    public class IoCtrl_DmcEcat : IoCtrl
    {
        /// <summary>
        /// 控制卡ID
        /// </summary>
        private ushort m_nCardId = 0;

        /// <summary>
        /// 板卡的系统IO数量
        /// </summary>
        private static readonly int SYS_IO_IN_COUNT = 8;

        private static readonly int SYS_IO_OUT_COUNT = 8;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_DmcEcat(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            m_strCardName = "DmcEcat";

            if (nCardNo == 0)
            {
                //第一张卡，需要去除8个系统IO
                m_strArrayIn = new string[32 - SYS_IO_IN_COUNT];
                m_strArrayOut = new string[32 - SYS_IO_OUT_COUNT];
            }
            else
            {
                m_strArrayIn = new string[32];
                m_strArrayOut = new string[32];
            }
            //m_strArrayIn = new string[DmcEtherCatCard.Instance().InputCount];
            //m_strArrayOut = new string[DmcEtherCatCard.Instance().OutputCount];
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool  Init()
        {
            //DmcEcat自带IO，无需专门的初始化, 通过函数调用判断是否能初始化成功
            try
            {
                int nCardId = DmcEtherCatCard.Instance().CardId;
                if (nCardId >= 0)
                {
                    m_nCardId = (ushort)nCardId;
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
            // 总线卡可以扩展多个IO模块，当I/O数量超过32个的时候，此函数只能读取前面32个I/O数据
            int nInputData = (int)LTDMC.dmc_read_inport(m_nCardId, (ushort)m_nCardNo);

            if (m_nCardNo == 0)
            {
                //前8个位系统IO板卡内部使用
                nInputData >>= SYS_IO_IN_COUNT;
            }
           
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
            if (m_nCardNo == 0)
            {
                //前8个位系统IO板卡内部使用
                nIndex += SYS_IO_IN_COUNT;
            }
            else
            {
                //超过32
                nIndex += 32 * m_nCardNo;
            }

            int nBitValue = LTDMC.dmc_read_inbit(m_nCardId, (ushort)(nIndex - 1));
            return (nBitValue != 0);
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nIndex">输出点位</param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            if (m_nCardNo == 0)
            {
                //前8个位系统IO板卡内部使用
                nIndex += SYS_IO_OUT_COUNT;
            }
            else
            {
                nIndex += 32 * m_nCardNo;
            }

            int nBitValue = LTDMC.dmc_read_outbit(m_nCardId, (ushort)(nIndex - 1));
            return (nBitValue != 0);
        }

        /// <summary>
        /// 获取输出信号
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool  ReadIOOut(ref int nData)
        {
            nData = (int)LTDMC.dmc_read_outport(m_nCardId, (ushort)m_nCardNo);

            if (m_nCardNo == 0)
            {
                //前8个位系统IO板卡内部使用
                nData >>= SYS_IO_OUT_COUNT;
            }

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
                if (m_nCardNo == 0)
                {
                    //前8个位系统IO板卡内部使用
                    nIndex += SYS_IO_OUT_COUNT;
                }
                else
                {
                    nIndex += 32 * m_nCardNo;
                }

                short ret = LTDMC.dmc_write_outbit(m_nCardId, (ushort)(nIndex - 1), (ushort)(bBit ? 1 : 0));
                if (ret == 0)
                    return true;
                else
                    return false;
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
                //前8个位系统IO板卡内部使用
                int nOutputData = (int)LTDMC.dmc_read_outport(m_nCardId, (ushort)m_nCardNo);

                if (m_nCardNo == 0)
                {
                    //保留前8位的状态
                    nData = (nData >> SYS_IO_OUT_COUNT) | (nOutputData & 0xFF);
                }

                short ret = LTDMC.dmc_write_outport(m_nCardId, (ushort)m_nCardNo, (uint)nData);
                if (ret == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}