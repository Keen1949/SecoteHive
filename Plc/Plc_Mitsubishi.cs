using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Communicate;
using CommonTool;

namespace Plc
{
    /// <summary>
    /// 三菱 MC协议
    /// </summary>
    public class McProtocol
    {
        /// <summary>
        /// 软元件代码
        /// </summary>
        private enum McElementCode
        {
            M = 0x90,
            X = 0x9C,
            Y = 0x9D,
            D = 0xA8
        };

        /// <summary>
        /// 设备站号，默认是0
        /// </summary>
        private int m_nStationNo;

        /// <summary>
        /// 接收的等待时间
        /// </summary>
        private int m_nWaitingTime;

        /// <summary>
        /// 通信网口
        /// </summary>
        private TcpLink m_TcpLink;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tl">网口</param>
        public McProtocol(TcpLink tl)
        {
            m_TcpLink = tl;
            m_nStationNo = 0;
            m_nWaitingTime = 1;
        }

        /// <summary>
        /// 设置接收的等待时间
        /// </summary>
        /// <param name="nMs">超时时间，单位:ms</param>
        public void SetWaitingTime(int nMs)
        {
            int nCount = nMs / 255;

            if (nCount <= 0)
                m_nWaitingTime = 1;
            else
                m_nWaitingTime = nCount;            
        }

        /// <summary>
        /// 发送数据帧
        /// </summary>
        /// <param name="data"> 指令 + 子指令 + 请求数据部分 </param>
        /// <returns></returns>
        private bool SendFrame(byte[] data)
        {
            if (data == null)
                return false;
            byte[] frame = new byte[data.Length + 11];

            // 副头部
            frame[0] = 0x50;
            frame[1] = 0x00;

            // 网络编号
            frame[2] = 0x000;

            // 可编程控制器编号
            frame[3] = 0xFF;

            // 请求目标模块IO编号
            frame[4] = 0xFF;
            frame[5] = 0x03;

            // 请求目标模块站号
            frame[6] = (byte)m_nStationNo;

            // 请求长度
            int nLen = data.Length + 2;
            frame[7] = (byte)(nLen & 0xFF);
            frame[8] = (byte)((nLen >> 8) & 0xFF);

            // CPU监视定时器
            frame[9] = (byte)(m_nWaitingTime & 0xFF);
            frame[10] = (byte)((m_nWaitingTime >> 8) & 0xFF);

            // 数据部分
            for (int i = 0; i < data.Length; i++)
            {
                frame[11 + i] = data[i];
            }

            return m_TcpLink.WriteData(frame, frame.Length);
        }

        /// <summary>
        /// 接收响应数据
        /// </summary>
        /// <param name="nEndCode">结束代码</param>
        /// <param name="data">接收的数据</param>
        /// <returns></returns>
        private bool ReceiveFrame(ref int nEndCode, ref byte[] data)
        {
            byte[] head = new byte[11];
            int nReadLen = m_TcpLink.ReadData(head, head.Length);
            if (nReadLen != head.Length)
                return false;

            // 副头部
            if (head[0] != 0xD0)
                return false;
            if (head[1] != 0x00)
                return false;

            // 网络编号
            if (head[2] != 0x00)
                return false;

            // 可编程控制器编号
            if (head[3] != 0xFF)
                return false;

            // 请求目标模块IO编号
            if (head[4] != 0xFF)
                return false;
            if (head[5] != 0x03)
                return false;

            // 请求目标模块站号
            if (head[6] != m_nStationNo)
                return false;

            // 响应数据长度
            int nRspLength = (int)head[7] + ((int)head[8] << 8);

            // 结束代码
            nEndCode = (int)head[9] + ((int)head[10] << 8);

            // 接收响应数据部分
            if (nRspLength < 2)
            {
                return false;
            }
            else if (nRspLength == 2)
            {
                return true;
            }
            else
            {
                data = new byte[nRspLength - 2];
                nReadLen = m_TcpLink.ReadData(data, data.Length);
                if (nReadLen != data.Length)
                {
                    data = null;
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 读取多个位（XYM 元件）
        /// </summary>
        /// <param name="element">元件类型</param>
        /// <param name="nAddr">元件起始地址</param>
        /// <param name="bVals">元件值</param>
        /// <returns></returns>
        public bool ReadMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            if (bVals == null)
                return false;

            byte[] data = new byte[10];

            //==============准备发送数据==============//
            // 指令
            data[0] = 0x01;
            data[1] = 0x04;

            // 子指令
            data[2] = 0x01;
            data[3] = 0x00;

            // 起始地址
            data[4] = (byte)(nAddr & 0xFF);
            data[5] = (byte)((nAddr >> 8) & 0xFF);
            data[6] = (byte)((nAddr >> 16) & 0xFF);

            // 软元件代码
            data[7] = (byte)GetMcElementCode(element);

            // 软元件点数
            int nElementCount = bVals.Length;
            data[8] = (byte)(nElementCount & 0xFF);
            data[9] = (byte)((nElementCount >> 8) & 0xFF);

            //=============发送数据包==============//
            if (!SendFrame(data))
                return false;

            //=============接收数据包==============//
            int nEndCode = 0;
            byte[] rspData = null;
            if (!ReceiveFrame(ref nEndCode, ref rspData))
                return false;

            if (nEndCode != 0)
                return false;

            if (rspData.Length != (bVals.Length + 1) / 2)
                return false;

            byte[] rspDataEx = new byte[rspData.Length * 2];
            for (int i = 0; i < rspData.Length; i++)
            {
                rspDataEx[i * 2] = (byte)((rspData[i] >> 4) & 0x0F);
                rspDataEx[i * 2 + 1] = (byte)(rspData[i] & 0x0F);
            }
            for (int i = 0; i < bVals.Length; i++)
            {
                bVals[i] = (rspDataEx[i] != 0);
            }

            return true;
        }

        /// <summary>
        /// 写入多个位（XYM 元件）
        /// </summary>
        /// <param name="element">元件类型</param>
        /// <param name="nAddr">元件起始地址</param>
        /// <param name="bVals">元件值</param>
        /// <returns></returns>
        public bool WriteMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            if (bVals == null)
                return false;

            byte[] data = new byte[10 + (bVals.Length + 1) / 2];

            //==============准备发送数据==============//
            // 指令
            data[0] = 0x01;
            data[1] = 0x14;

            // 子指令
            data[2] = 0x01;
            data[3] = 0x00;

            // 起始地址
            data[4] = (byte)(nAddr & 0xFF);
            data[5] = (byte)((nAddr >> 8) & 0xFF);
            data[6] = (byte)((nAddr >> 16) & 0xFF);

            // 软元件代码
            data[7] = (byte)GetMcElementCode(element);

            // 软元件点数
            int nElementCount = bVals.Length;
            data[8] = (byte)(nElementCount & 0xFF);
            data[9] = (byte)((nElementCount >> 8) & 0xFF);

            // 软元件数据
            for (int i = 0; i < bVals.Length; i++)
            {
                if ((i / 2) == 0)
                {
                    if (bVals[i])
                        data[10 + i / 2] |= 0x10;
                }
                else
                {
                    if (bVals[i])
                        data[10 + i / 2] |= 0x01;
                }
            }

            //=============发送数据包==============//
            if (!SendFrame(data))
                return false;

            //=============接收数据包==============//
            int nEndCode = 0;
            byte[] rspData = null;
            if (!ReceiveFrame(ref nEndCode, ref rspData))
                return false;

            if (nEndCode != 0)
                return false;
            return true;
        }

        /// <summary>
        /// 读取多个字（读取位元件时，16个位元件作为一个字）
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="nAddr">元件地址</param>
        /// <param name="nVals">保存元件值</param>
        /// <returns></returns>
        public bool ReadMultiWord(SoftElement element, int nAddr, UInt16[] nVals)
        {
            if (nVals == null)
                return false;

            byte[] data = new byte[10];

            //==============准备发送数据==============//
            // 指令
            data[0] = 0x01;
            data[1] = 0x04;

            // 子指令
            data[2] = 0x00;
            data[3] = 0x00;

            // 起始地址
            data[4] = (byte)(nAddr & 0xFF);
            data[5] = (byte)((nAddr >> 8) & 0xFF);
            data[6] = (byte)((nAddr >> 16) & 0xFF);

            // 软元件代码
            data[7] = (byte)GetMcElementCode(element);

            // 软元件点数
            int nElementCount = nVals.Length;
            data[8] = (byte)(nElementCount & 0xFF);
            data[9] = (byte)((nElementCount >> 8) & 0xFF);

            //=============发送数据包==============//
            if (!SendFrame(data))
                return false;

            //=============接收数据包==============//
            int nEndCode = 0;
            byte[] rspData = null;
            if (!ReceiveFrame(ref nEndCode, ref rspData))
                return false;

            if (nEndCode != 0)
                return false;

            if (rspData.Length != (nVals.Length * 2))
                return false;

            for (int i = 0; i < nVals.Length; i++)
            {
                nVals[i] = (UInt16)(rspData[i * 2] + (rspData[i * 2 + 1] << 8));
            }

            return true;
        }


        /// <summary>
        /// 写入多个字（写取位元件时，16个位元件作为一个字）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVals"></param>
        /// <returns></returns>
        public bool WriteMultiWord(SoftElement element, int nAddr, UInt16[] nVals)
        {
            if (nVals == null)
                return false;

            byte[] data = new byte[10 + nVals.Length * 2];

            //==============准备发送数据==============//
            // 指令
            data[0] = 0x01;
            data[1] = 0x14;

            // 子指令
            data[2] = 0x00;
            data[3] = 0x00;

            // 起始地址
            data[4] = (byte)(nAddr & 0xFF);
            data[5] = (byte)((nAddr >> 8) & 0xFF);
            data[6] = (byte)((nAddr >> 16) & 0xFF);

            // 软元件代码
            data[7] = (byte)GetMcElementCode(element);

            // 软元件点数
            int nElementCount = nVals.Length;
            data[8] = (byte)(nElementCount & 0xFF);
            data[9] = (byte)((nElementCount >> 8) & 0xFF);

            // 软元件数据
            for (int i = 0; i < nVals.Length; i++)
            {
                data[10 + i * 2] = (byte)(nVals[i] & 0xFF);
                data[10 + i * 2 + 1] = (byte)((nVals[i] >> 8) & 0xFF);
            }

            //=============发送数据包==============//
            if (!SendFrame(data))
                return false;

            //=============接收数据包==============//
            int nEndCode = 0;
            byte[] rspData = null;
            if (!ReceiveFrame(ref nEndCode, ref rspData))
                return false;

            if (nEndCode != 0)
                return false;
            return true;
        }

        /// <summary>
        /// MC协议中，元件代码
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private int GetMcElementCode(SoftElement element)
        {
            int nCode = 0;
            switch (element)
            {
                case SoftElement.X:
                    nCode = (int)McElementCode.X;
                    break;

                case SoftElement.Y:
                    nCode = (int)McElementCode.Y;
                    break;

                case SoftElement.M:
                    nCode = (int)McElementCode.M;
                    break;

                case SoftElement.D:
                    nCode = (int)McElementCode.D;
                    break;
                default:
                    break;
            }

            return nCode;
        }
    }
   
    /// <summary>
    /// 三菱PLC设备
    /// </summary>
    public class Plc_Mitsubishi : PlcDevice
    {
        /// <summary>
        /// MC 协议
        /// </summary>
        protected McProtocol m_Protocol;

        /// <summary>
        /// 通信网口
        /// </summary>
        protected TcpLink m_TcpLink;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">PLC设备序号</param>
        /// <param name="strName">设备名称</param>
        /// <param name="tl">网口</param>
        /// <param name="nId"></param>
        /// <param name="mp">地址映射表</param>
        public Plc_Mitsubishi(int nIndex, string strName, TcpLink tl, int nId, PlcAddressMap mp) : base(nIndex, strName, mp)
        {
            m_TcpLink = tl;
            m_bOpen = false;

            m_Protocol = new McProtocol(tl);
            m_CmdLimit = new PlcCmdCountLimit(256, 160, 32, 10);
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            if (m_TcpLink == null)
            {
                string str1 = "{0}使用的网口没有配置正确。";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "{0} uses a network port that is not configured correctly. ";
                }
                //WarningMgr.GetInstance().Error(string.Format("40100,ERR-PLC,{0}使用的网口没有配置正确。", m_strName));
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Open,m_strName,
                    string.Format(str1, m_strName));

                return false;
            }

            m_bOpen = m_TcpLink.Open();
            return m_bOpen;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        public override void Close()
        {
            m_TcpLink.Close();
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
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                if (!m_Protocol.ReadMultiBit(element, nAddr, bVals))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40104,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = { 0 };
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
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
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
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
                if (!m_Protocol.ReadMultiWord(element, nAddr, nVals))
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[2];
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Read, m_strName,
                    string.Format(str1, m_strName, element.ToString()));

                return false;
            }

            lock (this)
            {
                UInt16[] nValues = new UInt16[nVals.Length * 2];
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
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
                bool[] bVals = { bVal };
                if (!m_Protocol.WriteMultiBit(element, nAddr, bVals))
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
                if (!m_Protocol.WriteMultiBit(element, nAddr, bVals))
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
                if (!m_Protocol.ReadMultiWord(element, nAddr, nValues))
                {
                    //WarningMgr.GetInstance().Error(string.Format("40120,ERR-PLC,{0}读取失败，请检查通信是否正确。", m_strName));
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
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
                UInt16[] nValues = { nVal };
                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
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
                if (!m_Protocol.WriteMultiWord(element, nAddr, nData))
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
                if (!m_Protocol.WriteMultiWord(element, nAddr, nValues))
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
                WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
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
                    WarningMgr.GetInstance().Error(ErrorType.Err_Plc_Write, m_strName,
                        string.Format(str2, m_strName));

                    return false;
                }

                return true;
            }
        }
    }
}
