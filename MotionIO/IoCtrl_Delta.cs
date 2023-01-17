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
using PCI_DMC;
using PCI_DMC_ERR;

namespace MotionIO
{
    /// <summary>
    /// 类名必须以"IoCtrl_"前导，否则加载不到
    /// </summary>
    /// // <IoCard 卡序号="4" 卡号="10021001" 卡类型="Detla" />
    public class IoCtrl_Delta : IoCtrl
    {
        /// <summary>
        /// 
        /// </summary>
        public struct DeltaAddr
        {
            /// <summary>
            /// 
            /// </summary>
            public uint nType;
            /// <summary>
            /// 
            /// </summary>
            public ushort nCardID;
            /// <summary>
            /// 
            /// </summary>
            public ushort nNodeID;
            /// <summary>
            /// 
            /// </summary>
            public ushort nSlotID;
            /// <summary>
            /// 
            /// </summary>
            public ushort nPortID;
            /// <summary>
            /// 
            /// </summary>
            public ushort nIndex;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="nCardID"></param>
            /// <param name="nNodeID"></param>
            /// <param name="nPortID"></param>
            /// <param name="nSlotID"></param>
            /// <param name="nIndex"></param>
            /// <param name="nType"></param>
            public DeltaAddr(ushort nCardID, ushort nNodeID, ushort nPortID = 0, ushort nSlotID= 0, ushort nIndex= 0, uint nType = 0)
            {
                this.nCardID = nCardID;
                this.nNodeID = nNodeID;
                this.nSlotID = nSlotID;
                this.nPortID = nPortID;
                this.nIndex = nIndex;
                this.nType = nType;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DeltaAddr addrDI;
        /// <summary>
        /// 
        /// </summary>
        public DeltaAddr addrDO;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">索引号</param>
        /// <param name="nCardNo">卡号</param>
        public IoCtrl_Delta(int nIndex, int nCardNo) :base(nIndex,nCardNo)
        {
            m_strCardName = "Delta";
            m_strArrayIn = new string[16];
            m_strArrayOut = new string[16];

            this.addrDI = new DeltaAddr((ushort)(nCardNo/10000/1000),
                (ushort)(nCardNo / 10000 % 1000/10),
                (ushort)(nCardNo / 10000 % 10));
            this.addrDO = new DeltaAddr((ushort)(nCardNo % 10000 / 1000),
                (ushort)(nCardNo % 10000 % 1000 / 10),
                (ushort)(nCardNo % 10000 % 10));            
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool  Init()
        {
            //Delta 总线在Motion中已经初始化
            try
            {
                uint nIdentity = 0;
                short rc = CPCI_DMC.CS_DMC_01_get_devicetype((short)this.addrDI.nCardID, this.addrDI.nNodeID, this.addrDI.nSlotID, ref this.addrDI.nType, ref nIdentity);
                rc = CPCI_DMC.CS_DMC_01_get_devicetype((short)this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, ref this.addrDO.nType, ref nIdentity);
                //Enable
                rc = CPCI_DMC.CS_DMC_01_set_rm_output_active(this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, 1);

                if (CPCI_DMC_ERR.ERR_NoError == rc)
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
            ushort nInputData = 0;

            short nRtn = CPCI_DMC.CS_DMC_01_get_rm_input_value(this.addrDI.nCardID, this.addrDI.nNodeID, this.addrDI.nSlotID, this.addrDI.nPortID, ref nInputData);
            if (CPCI_DMC_ERR.ERR_NoError == nRtn)
            {
                //nInputData = (nInputData | (nInputData << 24)) >> 8;
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
            ushort nData = 0;
            
            short nRtn = CPCI_DMC.CS_DMC_01_get_rm_input_single_value(this.addrDI.nCardID, this.addrDI.nNodeID, this.addrDI.nSlotID, this.addrDI.nPortID, (ushort)(nIndex-1),ref nData);
            if (CPCI_DMC_ERR.ERR_NoError == nRtn)
            {
                return nData==0 ? false : true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡Delta ReadIoInBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card Delta ReadIoInBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡Delta ReadIoInBit {1} Error,result is {2}", this.addrDI.nCardID, nIndex, nRtn));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,string.Format("{0}.{1}", this.addrDI.nCardID, nIndex),
                        string.Format(str1, this.addrDI.nCardID, nIndex, nRtn));

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

            ushort nData = 0;
            short nRtn = CPCI_DMC.CS_DMC_01_get_rm_output_single_value(this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, this.addrDO.nPortID, (ushort)(nIndex - 1), ref nData);

            if (CPCI_DMC_ERR.ERR_NoError == nRtn)
            {
                return nData == 0 ? false : true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡Delta ReadIoOutBit {1} Error,result is {2}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card Delta ReadIoOutBit {1} Error,result is {2}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡Delta ReadIoOutBit {1} Error,result is {2}", this.addrDO.nCardID, nIndex, nRtn));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,string.Format("{0}.{1}", this.addrDO.nCardID, nIndex),
                        string.Format(str1, this.addrDO.nCardID, nIndex, nRtn));

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
            ushort nValue = 0;
            short nRtn = CPCI_DMC.CS_DMC_01_get_rm_output_value(this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, this.addrDO.nPortID, ref nValue);

            if (CPCI_DMC_ERR.ERR_NoError == nRtn)
            {
                nData = nValue;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡Delta ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card Delta ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡Delta ReadIOOut Error,result is {1}", this.addrDO.nCardID, nRtn));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out, this.addrDO.nCardID.ToString(),
                        string.Format(str1, this.addrDO.nCardID, nRtn));

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
                short nRtn = CPCI_DMC.CS_DMC_01_set_rm_output_single_value(this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, this.addrDO.nPortID, (ushort)(nIndex - 1), (ushort)(bBit ? 1 : 0));
                if (CPCI_DMC_ERR.ERR_NoError == nRtn)
                    return true;
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
                short nRtn = CPCI_DMC.CS_DMC_01_set_rm_output_value(this.addrDO.nCardID, this.addrDO.nNodeID, this.addrDO.nSlotID, this.addrDO.nPortID, (ushort)nData);
                if (CPCI_DMC_ERR.ERR_NoError == nRtn)
                    return true;
            }
            return false;
        }
    }
}