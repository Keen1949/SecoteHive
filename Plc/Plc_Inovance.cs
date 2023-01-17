using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Communicate;
using CommonTool;

namespace Plc
{
    /// <summary>
    /// 汇川计算机链接协议
    /// </summary>
    public class InovanceLinkProtocol
    {
        private enum CtrlCode
        {
            STX = 0x02,     //文本起点
            ETX = 0x03,     //文本终点
            EOT = 0x04,     //传送结束
            ENQ = 0x05,     //询问
            ACK = 0x06,     //确认
            LF = 0x0A,     //换行
            CL = 0x0C,     //清除
            CR = 0x0D,     //回车
            NAK = 0x15      //不确认
        };

        /// <summary>
        /// PLC设备站号
        /// </summary>
        private int m_nStationNo;

        /// <summary>
        /// PLC设备在接收命令以后，回应命令之前的等待时间
        /// </summary>
        private int m_nWaitingTime;

        /// <summary>
        /// 通信用的串口
        /// </summary>
        private ComLink m_ComLink;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cl">通信用的串口</param>
        /// <param name="nId">站号</param>
        public InovanceLinkProtocol(ComLink cl, int nId)
        {
            m_nStationNo = nId;
            m_nWaitingTime = 0;
            m_ComLink = cl;
        }

        /// <summary>
        /// 设置接收的等待时间
        /// </summary>
        /// <param name="nMs">超时时间，单位:ms</param>
        public void SetWaitingTime(int nMs)
        {
            m_nWaitingTime = nMs / 10;
            if (m_nWaitingTime > 255)
                m_nWaitingTime = 255;
        }

        /// <summary>
        /// 计算和校验
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        private byte GetCheckSum(byte[] buffer, int index, int cnt)
        {
            byte sum = 0;
            for (int i = 0; i < cnt; i++)
            {
                sum += buffer[index + i];
            }
            return sum;
        }

        /// <summary>
        /// 发送 ACK 回应
        /// </summary>
        /// <returns></returns>
        private bool SendAck()
        {
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ACK });
            line += "05";
            line += "FF";
            line += "\r\n";

            byte[] ackData = Encoding.ASCII.GetBytes(line);
            return m_ComLink.WriteData(ackData, ackData.Length);       
        }

        /// <summary>
        /// 检查 ACK 回应
        /// </summary>
        /// <returns></returns>
        private bool CheckAck()
        {
            byte[] ackData = new byte[7];
            int nRcvLen = m_ComLink.ReadData(ackData, ackData.Length);
            if (nRcvLen != ackData.Length)
            {
                m_ComLink.ClearBuffer(true, true);
                return false;
            }
                                       
            if (ackData[0] == (byte)CtrlCode.ACK)
            {
                // 正确响应
                string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ACK });
                line += (m_nStationNo & 0xFF).ToString("X2");
                line += "FF";
                line += "\r\n";
                byte[] rightData = Encoding.ASCII.GetBytes(line);
                for (int i = 0; i < ackData.Length; i++)
                    if (ackData[i] != rightData[i])
                        return false;
            }
            else if (ackData[0] == (byte)CtrlCode.NAK)
            {
                // 错误响应，包含错误码
                byte[] errorData = new byte[2];
                nRcvLen = m_ComLink.ReadData(errorData, errorData.Length);
                if (nRcvLen != errorData.Length)
                {
                    m_ComLink.ClearBuffer(true, true);                    
                }
                string strErrorCode = Encoding.ASCII.GetString(ackData, 5, 2);

                return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        private byte[] GetValidData(byte[] sourceData)
        {
            // 检查前5个字节
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.STX });
            line += m_nStationNo.ToString("X2"); ;
            line += "FF";
            byte[] buffer = Encoding.ASCII.GetBytes(line);
            for (int i = 0; i < buffer.Length; i++)
                if (sourceData[i] != buffer[i])
                    return null;

            // 检查后前5个字节
            byte nSum = GetCheckSum(sourceData, 1, sourceData.Length - 5);
            line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ETX });
            line += nSum.ToString("X2");
            line += "\r\n";
            buffer = Encoding.ASCII.GetBytes(line);
            for (int i = 0; i < buffer.Length; i++)
                if (sourceData[sourceData.Length - 5 + i] != buffer[i])
                    return null;

            byte[] validData = new byte[sourceData.Length - 10];
            Array.Copy(sourceData, 5, validData, 0, validData.Length);
            return validData;
        }

        /// <summary>
        /// 读取多个位元件
        /// </summary>
        /// <param name="element">元件类型：X、Y、M </param>
        /// <param name="nAddr">元件起始地址</param>
        /// <param name="bValues">保存元件值，其长度表示元件数量</param>
        /// <returns></returns>
        public bool ReadMultiBit(SoftElement element, int nAddr, bool[] bValues)
        {
            //===================准备数据==============//
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ENQ });//ENQ
            line += m_nStationNo.ToString("X2");            //站号
            line += "FF";//PC号
            line += "BR";//命令
            line += (m_nWaitingTime & 0x0F).ToString("X1"); //等待时间
            line += element.ToString();                     //元件类型
            line += (nAddr & 0xFFFF).ToString("D4");        //元件首地址
            line += (bValues.Length & 0xFF).ToString("X2"); //元件数量

            byte[] cmdData = Encoding.ASCII.GetBytes(line.Substring(1, line.Length - 1));
            byte nSumCrc = GetCheckSum(cmdData, 0, cmdData.Length);
            line += nSumCrc.ToString("X2"); // 和校验
            line += "\r\n";                 // \CR\LF

            //===================发送数据==============//
            cmdData = Encoding.ASCII.GetBytes(line);
            if (!m_ComLink.WriteData(cmdData, cmdData.Length))
                return false;

            //===================接收数据==============//
            byte[] rspData = new byte[bValues.Length + 10];
            int nRcvLen = m_ComLink.ReadData(rspData, rspData.Length);
            if (nRcvLen != rspData.Length)
            {
                m_ComLink.ClearBuffer(true, true);
                return false;
            }

            // 获取有效数据
            byte[] validData = GetValidData(rspData);
            if (validData == null)
                return false;
            if (validData.Length != bValues.Length)
                return false;
            for (int i = 0; i < validData.Length; i++)
            {
                if (validData[i] == '1')
                    bValues[i] = true;
                else if (validData[i] == '0')
                    bValues[i] = false;
                else
                    return false;
            }

            //===================PC确认=================//
            return SendAck();
        }

        /// <summary>
        /// 读取多个字（16位元件组成一个字）
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="nAddr">起始地址</param>
        /// <param name="nValues">元件值，其长度表示元件数量</param>
        /// <returns></returns>
        public bool ReadMultiWord(SoftElement element, int nAddr, UInt16[] nValues)
        {
            //===================准备数据==============//
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ENQ });//ENQ
            line += m_nStationNo.ToString("X2");            //站号
            line += "FF";//PC号
            line += "WR";//命令
            line += (m_nWaitingTime & 0x0F).ToString("X1"); //等待时间
            line += element.ToString();                     //元件类型
            //line += (nAddr & 0xFFFF).ToString("X4");        //元件首地址
            line += (nAddr & 0xFFFF).ToString("D4");        //元件首地址
            line += (nValues.Length & 0xFF).ToString("X2"); //元件数量

            byte[] cmdData = Encoding.ASCII.GetBytes(line.Substring(1, line.Length - 1));
            byte nSumCrc = GetCheckSum(cmdData, 0, cmdData.Length);
            line += nSumCrc.ToString("X2"); // 和校验
            line += "\r\n";                 // \CR\LF

            //===================发送数据==============//
            cmdData = Encoding.ASCII.GetBytes(line);
            if (!m_ComLink.WriteData(cmdData, cmdData.Length))
                return false;

            //===================接收数据==============//
            byte[] rspData = new byte[nValues.Length * 4 + 10];
            int nRcvLen = m_ComLink.ReadData(rspData, rspData.Length);
            if (nRcvLen != rspData.Length)
            {
                m_ComLink.ClearBuffer(true, true);
                return false;
            }
                

            // 获取有效数据
            byte[] validData = GetValidData(rspData);
            if (validData == null)
            {
                m_ComLink.ClearBuffer(true, true);
                return false;
            }
                
            if (validData.Length != (nValues.Length * 4))
                return false;
            for (int i = 0; i < nValues.Length; i++)
            {
                string strData = Encoding.ASCII.GetString(validData, i * 4, 4);
                int nTemp = 0;
                if (int.TryParse(strData, System.Globalization.NumberStyles.HexNumber, null, out nTemp))
                    nValues[i] = (UInt16)nTemp;
                else
                    return false;
            }

            //===================PC确认=================//
            return SendAck();
        }

        /// <summary>
        /// 写入多个位元件（XYM）
        /// </summary>
        /// <param name="element">元件类型：X、Y、M </param>
        /// <param name="nAddr">元件起始地址</param>
        /// <param name="bValues">元件值，其长度表示元件数量</param>
        /// <returns></returns>
        public bool WriteMultiBit(SoftElement element, int nAddr, bool[] bValues)
        {
            //===================准备数据==============//
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ENQ });//ENQ
            line += m_nStationNo.ToString("X2");            //站号
            line += "FF";//PC号
            line += "BW";//命令
            line += (m_nWaitingTime & 0x0F).ToString("X1"); //等待时间
            line += element.ToString();                     //元件类型
            line += (nAddr & 0xFFFF).ToString("D4");        //元件首地址
            line += (bValues.Length & 0xFF).ToString("X2"); //元件数量
            for (int i = 0; i < bValues.Length; i++)        //元件数据
            {
                if (bValues[i])
                    line += "1";
                else
                    line += "0";
            }

            byte[] cmdData = Encoding.ASCII.GetBytes(line.Substring(1, line.Length - 1));
            byte nSumCrc = GetCheckSum(cmdData, 0, cmdData.Length);
            line += nSumCrc.ToString("X2"); // 和校验
            line += "\r\n";                 // \CR\LF

            //===================发送数据==============//
            cmdData = Encoding.ASCII.GetBytes(line);
            if (!m_ComLink.WriteData(cmdData, cmdData.Length))
                return false;

            //===================接收数据==============//
            return CheckAck();       
        }

        /// <summary>
        /// 写入多个字（16位元件组成一个字）
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="nAddr">元件起始地址</param>
        /// <param name="nValues">写入的数据，其长度表示元件的数量</param>
        /// <returns></returns>
        public bool WriteMultiWord(SoftElement element, int nAddr, UInt16[] nValues)
        {
            //===================准备数据==============//
            string line = Encoding.ASCII.GetString(new byte[] { (byte)CtrlCode.ENQ });//ENQ
            line += m_nStationNo.ToString("X2");            //站号
            line += "FF";//PC号
            line += "WW";//命令
            line += (m_nWaitingTime & 0x0F).ToString("X1"); //等待时间
            line += element.ToString();                     //元件类型
            line += (nAddr & 0xFFFF).ToString("D4");        //元件首地址
            line += (nValues.Length & 0xFF).ToString("X2"); //元件数量
            for (int i = 0; i < nValues.Length; i++)        //元件数据
            {
                line += nValues[i].ToString("X4");
            }

            byte[] cmdData = Encoding.ASCII.GetBytes(line.Substring(1, line.Length - 1));
            byte nSumCrc = GetCheckSum(cmdData, 0, cmdData.Length);
            line += nSumCrc.ToString("X2"); // 和校验
            line += "\r\n";                 // \CR\LF

            //===================发送数据==============//
            cmdData = Encoding.ASCII.GetBytes(line);
            if (!m_ComLink.WriteData(cmdData, cmdData.Length))
                return false;

            //===================接收数据==============//
            return CheckAck();
        }
    }

    /// <summary>
    /// 汇川PLC设备
    /// </summary>
    public class Plc_Inovance : PlcDevice
    {

        /// <summary>
        /// 串口
        /// </summary>
        protected ComLink m_ComLink;

        /// <summary>
        /// 汇川的计算机链接协议
        /// </summary>
        protected InovanceLinkProtocol m_Protocol;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">PLC设备序号</param>
        /// <param name="strName">PLC设备名称</param>
        /// <param name="cl">串口</param>
        /// <param name="nId">设备站号</param>
        /// <param name="mp">元件地址映射表</param>
        public Plc_Inovance(int nIndex, string strName, ComLink cl, int nId, PlcAddressMap mp) : base(nIndex, strName, mp)
        {
            m_ComLink = cl;
            m_bOpen = false;
            m_Protocol = new InovanceLinkProtocol(cl, nId);

            m_CmdLimit = new PlcCmdCountLimit(256, 160, 32, 10);
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
                bool[] bVals = { false };
                if (!m_Protocol.ReadMultiBit(element, nAddr, bVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40102,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                bVal = bVals[0];
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Protocol.ReadMultiBit(element, nAddr, bVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40104,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                        string.Format(str2, m_strName));

                    return false;
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40106,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40108,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Protocol.ReadMultiWord(element, nAddr, nVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40110,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 读取一个双字
        /// 对于字元件：读取一个双字
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[2];
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40112,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                nVal = (UInt32)nValues[0] + ((UInt32)nValues[1] << 16);
                return true;
            }
        }

        /// <summary>
        /// 读取一组双字
        /// 对于字元件：读取多个双字（读取数量 = count）
        /// 对于位元件：读取多个双位（读取数量 = count * 32 ）
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[nVals.Length * 2];
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40114,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                bool[] bVals = { bVal };
                if (!m_Protocol.WriteMultiBit(element, nAddr, bVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40116,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Protocol.WriteMultiBit(element, nAddr, bVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40118,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40120,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                if (bVal)
                    nValues[0] |= (UInt16)(0x01 << nIndex);
                else
                    nValues[0] &= (UInt16)~(0x01 << nIndex);

                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40121,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { nVal };
                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40123,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Protocol.WriteMultiWord(element, nAddr, nData))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40125,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { (UInt16)(nVal & 0xFFFF), (UInt16)(nVal >> 16) };
                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40127,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
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
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool WriteMultiDWord(SoftElement element, int nAddr, UInt32[] nData)
        {
            string str1 = "{0}{1}元件地址错误。";
            string str2 = "{0}写入失败，请检查通信是否正确。";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "{0} {1} component address is wrong. ";
                str2 = "{0} write failed, please check communication is correct. ";
            }

            if (!base.WriteMultiDWord(element, nAddr, nData))
            {
                //WarningMgr.GetInstance().Error(string.Format("40128,ERR-PLC,{0}{1}元件地址错误。", m_strName, element.ToString()));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[nData.Length * 2];
                for (int i = 0; i < nData.Length; i++)
                {
                    nValues[i * 2] = (UInt16)(nData[i] & 0xFFFF);
                    nValues[i * 2 + 1] = (UInt16)(nData[i] >> 16);
                }
                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40129,ERR-PLC,{0}写入失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write,m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }
    }
}
