using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicate;
using CommonTool;

namespace Plc
{
    /// <summary>
    /// ModbusRTU设备
    /// </summary>
    public class Plc_ModbusRtu : PlcDevice
    {
        /// <summary>
        /// ModbusRTU使用的串口
        /// </summary>
        protected ComLink m_ComLink;

        /// <summary>
        /// Modbus协议
        /// </summary>
        private Modbus m_Modbus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">PLC设备序号</param>
        /// <param name="strName">PLC设备名称</param>
        /// <param name="cl">使用的串口</param>
        /// <param name="nId">Modbus站号</param>
        /// <param name="mp">元件的地址映射表</param>
        public Plc_ModbusRtu(int nIndex, string strName, ComLink cl, int nId, PlcAddressMap mp) : base(nIndex, strName, mp)
        {
            m_ComLink = cl;
            m_bOpen = false;

            m_Modbus = new ModbusRtu(cl, nId);

            // 每一款PLC的限制不一样，此处的设置根据汇川H2U型设置
            m_CmdLimit = new PlcCmdCountLimit(2000, 1968, 125, 120);
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            if (m_ComLink == null)
            {
                string str1 = "{0}使用的串口没有配置正确。";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "{0} uses a serial port that is not configured correctly. ";
                }

                //WarningMgr.GetInstance().Error(string.Format("40100,ERR-PLC,{0}使用的串口没有配置正确。", m_strName));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Open,m_strName,
                    string.Format(str1, m_strName));

                return false;
            }

            m_bOpen = m_ComLink.Open();
            return m_bOpen;
        }


        /// <summary>
        /// 关闭设备
        /// </summary>
        public override void Close()
        {
            m_ComLink.Close();
        }

        /// <summary>
        /// 读取一个位元件
        /// </summary>
        /// <param name="element">元件类型：X、Y、M </param>
        /// <param name="nAddr">元件地址</param>
        /// <param name="bVal">元件的状态值</param>
        /// <returns></returns>
        public override bool ReadBit(SoftElement element, int nAddr, ref bool bVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadBit(element, nAddr, ref bVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40101,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                byte[] coilData = new byte[1];
                if (!m_Modbus.ReadCoilStatus(nAddr, 1, coilData))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40102,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                if ((coilData[0] & 0x01) != 0)
                    bVal = true;
                else
                    bVal = false;
                return true;
            }
        }

        /// <summary>
        /// 读取多个位元件
        /// </summary>
        /// <param name="element">元件类型：X、Y、M </param>
        /// <param name="nAddr">元件地址</param>
        /// <param name="bVals">元件值</param>
        /// <returns></returns>
        public override bool ReadMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadMultiBit(element, nAddr, bVals))
            {
                //WarningMgr.GetInstance().Error(string.Format("40103,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                byte[] coilData = new byte[(bVals.Length + 7) / 8];
                if (!m_Modbus.ReadCoilStatus(nAddr, bVals.Length, coilData))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40104,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                for (int i = 0; i < bVals.Length; i++)
                {
                    if ((coilData[i / 8] & (0x01 << (i % 8))) != 0)
                        bVals[i] = true;
                    else
                        bVals[i] = false;
                }
                return true;
            }
        }

        /// <summary>
        /// 读取字元件的一位
        /// </summary>
        /// <param name="element">元件类型：D </param>
        /// <param name="nAddr">元件地址</param>
        /// <param name="nIndex">位在字内的偏移</param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public override bool ReadRegBit(SoftElement element, int nAddr, int nIndex, ref bool bVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadRegBit(element, nAddr, nIndex, ref bVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40105,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Modbus.ReadHoldingReg(nAddr, 1, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40106,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                bVal = ((nValues[0] & (0x01 << nIndex)) != 0);
                return true;
            }
        }

        /// <summary>
        /// 读取一个字
        /// 对于字元件：读取一个字
        /// 对于位元件：读取连续16个位
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public override bool ReadWord(SoftElement element, int nAddr, ref UInt16 nVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadWord(element, nAddr, ref nVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40107,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Modbus.ReadHoldingReg(nAddr, 1, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40108,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                nVal = nValues[0];
                return true;
            }
        }

        /// <summary>
        /// 读取一组字
        /// 对于字元件：读取多个字（读取数量 = count）
        /// 对于位元件：读取多个位（读取数量 = count * 16 ）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVals"></param>
        /// <returns></returns>
        public override bool ReadMultiWord(SoftElement element, int nAddr, UInt16[] nVals)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadMultiWord(element, nAddr, nVals))
            {
                //WarningMgr.GetInstance().Error(string.Format("40109,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Modbus.ReadHoldingReg(nAddr, nVals.Length, nVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40110,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 读取一个双字
        /// 对于字元件：读取2个字
        /// 对于位元件：读取连续32个位
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public override bool ReadDWord(SoftElement element, int nAddr, ref UInt32 nVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadDWord(element, nAddr, ref nVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40111,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0, 0};
                if (!m_Modbus.ReadHoldingReg(nAddr, 2, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40112,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                nVal = (UInt32)nValues[0] + ((UInt32)nValues[1] << 16);
                return true;
            }
        }

        /// <summary>
        /// 读取一组双字
        /// 对于字元件：读取多个双字（读取数量 = count * 2）
        /// 对于位元件：读取多个位（读取数量 = count * 32 ）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVals"></param>
        /// <returns></returns>
        public override bool ReadMultiDWord(SoftElement element, int nAddr, UInt32[] nVals)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
            }

            if (!base.ReadMultiDWord(element, nAddr, nVals))
            {
                //WarningMgr.GetInstance().Error(string.Format("40113,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[nVals.Length * 2];
                if (!m_Modbus.ReadHoldingReg(nAddr, nValues.Length, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40114,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                for (int i = 0; i < nVals.Length; i++)
                {
                    nVals[i] = (UInt32)nValues[2 * i] + ((UInt32)nValues[2 * i + 1] << 16);
                }

                return true;
            }
        }

        /// <summary>
        /// 写入一个位，只对位元件有效（YM有效，X不能写入）
        /// </summary>
        /// <param name="element">元件类型：Y、M </param>
        /// <param name="nAddr"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public override bool WriteBit(SoftElement element, int nAddr, bool bVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteBit(element, nAddr, bVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40115,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Modbus.WriteSingleCoil(nAddr, bVal))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40116,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 写入多个位，只对位元件有效（YM有效，X不能写入）
        /// </summary>
        /// <param name="element"> Y、M </param>
        /// <param name="nAddr"></param>
        /// <param name="bVals"></param>
        /// <returns></returns>
        public override bool WriteMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteMultiBit(element, nAddr, bVals))
            {
                //WarningMgr.GetInstance().Error(string.Format("40117,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }


            lock (this)
            {
                byte[] coilData = new byte[(bVals.Length + 7) / 8];
                for (int i = 0; i < bVals.Length; i++)
                {
                    if (bVals[i])
                        coilData[i / 8] |= (byte)(0x01 << (i % 8));
                    else
                        coilData[i / 8] &= (byte)~(0x01 << (i % 8));
                }
                if (!m_Modbus.WriteMultiCoil(nAddr, bVals.Length, coilData))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40118,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 向位元件写入一位（只对D元件有效）
        /// </summary>
        /// <param name="element">元件类型：D </param>
        /// <param name="nAddr"></param>
        /// <param name="nIndex"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public override bool WriteRegBit(SoftElement element, int nAddr, int nIndex, bool bVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}读取失败，请检查通信是否正确。";
            string str3 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} read failed, please check communication is correct. ";
                str3 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteRegBit(element, nAddr, nIndex, bVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40119,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Modbus.ReadHoldingReg(nAddr, 1, nValues))
               {
                    //WarningMgr.GetInstance().Error(string.Format("40120,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                if (bVal)
                    nValues[0] |= (byte)(0x01 << nIndex);
                else
                    nValues[0] &= (byte)~(0x01 << nIndex);

                if (!m_Modbus.WriteWriteSingleReg(nAddr, nValues[0]))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40121,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str3, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 写入一个字（为位元件时，写入16位）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public override bool WriteWord(SoftElement element, int nAddr, UInt16 nVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteWord(element, nAddr, nVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40122,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Modbus.WriteWriteSingleReg(nAddr, nVal))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40123,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 写入多个字（为位元件时，写入数量 x 16）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool WriteMultiWord(SoftElement element, int nAddr, UInt16[] nData)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteMultiWord(element, nAddr, nData))
            {
                //WarningMgr.GetInstance().Error(string.Format("40124,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Modbus.WriteMultiReg(nAddr, nData))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40125,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 写入一个双字（为位元件时，写入32位）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public override bool WriteDWord(SoftElement element, int nAddr, UInt32 nVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteDWord(element, nAddr, nVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40126,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { (UInt16)(nVal & 0xFFFF), (UInt16)(nVal >> 16) };
                if (!m_Modbus.WriteMultiReg(nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40127,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 写入多个双字（为位元件时，写入数量 x 32）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public override bool WriteMultiDWord(SoftElement element, int nAddr, UInt32[] nVal)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteMultiDWord(element, nAddr, nVal))
            {
                //WarningMgr.GetInstance().Error(string.Format("40128,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[nVal.Length * 2];
                for (int i = 0; i < nVal.Length; i++)
                {
                    nValues[i * 2] = (UInt16)(nVal[i] & 0xFFFF);
                    nValues[i * 2 + 1] = (UInt16)(nVal[i] >> 16);
                }
                if (!m_Modbus.WriteMultiReg(nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40129,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }
    }
}
