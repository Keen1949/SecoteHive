using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicate;

namespace Plc
{
    /// <summary>
    /// Modbus协议部分，包含数据帧的格式分析，及读写操作
    /// 物理层的数据包收发，需要派生类实现
    /// </summary>
    public class Modbus
    {
        /// <summary>
        /// Modbus功能码
        /// </summary>
        private enum FunctionCode
        {
            ReadCoilStatus  = 0x01,
            ReadInputStatus = 0x02,
            ReadHoldingReg  = 0x03,
            ReadInputReg    = 0x04,
            WriteSingleCoil = 0x05,
            WriteSingleReg  = 0x06,
            WriteMultiCoil  = 0x0F,
            WriteMultiReg   = 0x10
        };
        
        /// <summary>
        /// 发送Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected virtual bool SendFrame(byte[] frame)
        {
            return false;        
        }

        /// <summary>
        /// 接收Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected virtual bool ReceiveFrame(byte[] frame)
        {
            return false;
        }

        /// <summary>
        /// 生成Modbus数据帧（读取命令）
        /// </summary>
        /// <param name="code">功能码</param>
        /// <param name="nAddr">起始地址</param>
        /// <param name="nCount">读取数量</param>
        /// <returns></returns>
        private byte[] GenerateReadFrame(FunctionCode code, int nAddr, int nCount)
        {
            byte[] frame = new byte[5];
            frame[0] = (byte)code;
            frame[1] = (byte)((nAddr >> 8) & 0xFF);
            frame[2] = (byte)(nAddr & 0xFF);
            frame[3] = (byte)((nCount >> 8) & 0xFF);
            frame[4] = (byte)(nCount & 0xFF);
            return frame;
        }

        /// <summary>
        /// 0x01:线圈读取
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nCount"></param>
        /// <param name="coilData"></param>
        /// <returns></returns>
        public bool ReadCoilStatus(int nAddr, int nCount, byte[] coilData)
        {
            lock(this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateReadFrame(FunctionCode.ReadCoilStatus, nAddr, nCount);         
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧                
                int nCoilStatusSize = (nCount + 7) / 8;
                byte[] rxFrame = new byte[nCoilStatusSize + 2];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                FunctionCode rspCode = (FunctionCode)rxFrame[0];
                if (rspCode != FunctionCode.ReadCoilStatus)
                    return false;           
                if (nCoilStatusSize != (int)rxFrame[1])
                    return false;

                // 读取数据
                for (int i = 0; i < nCoilStatusSize; i++)
                {
                    coilData[i] = rxFrame[2 + i];
                }

                return true;
            }  
        }

        /// <summary>
        /// 0x02:输入读取
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nCount"></param>
        /// <param name="coilData"></param>
        /// <returns></returns>
        public bool ReadInputStatus(int nAddr, int nCount, byte[] coilData)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateReadFrame(FunctionCode.ReadInputStatus, nAddr, nCount);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧             
                int nInputStatusSize = (nCount + 7) / 8;
                byte[] rxFrame = new byte[nInputStatusSize + 2];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                FunctionCode rspCode = (FunctionCode)rxFrame[0];
                if (rspCode != FunctionCode.ReadInputStatus)
                    return false;
                if (nInputStatusSize != (int)rxFrame[1])
                    return false;

                // 读取数据
                for (int i = 0; i < nInputStatusSize; i++)
                {
                    coilData[i] = rxFrame[2 + i];
                }

                return true;
            }
        }

        /// <summary>
        /// 0x03:保持寄存器读取
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nCount"></param>
        /// <param name="regData"></param>
        /// <returns></returns>
        public bool ReadHoldingReg(int nAddr, int nCount, UInt16[] regData)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateReadFrame(FunctionCode.ReadHoldingReg, nAddr, nCount);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[nCount * 2 + 2];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                FunctionCode rspCode = (FunctionCode)rxFrame[0];
                if (rspCode != FunctionCode.ReadHoldingReg)
                    return false;
                if (nCount * 2 != (int)rxFrame[1])
                    return false;

                // 读取数据
                for (int i = 0; i < nCount; i++)
                {
                    regData[i] = (UInt16)((rxFrame[i * 2 + 2] << 8) + rxFrame[i * 2 + 3]);
                }

                return true;
            }
        }

        /// <summary>
        /// 0x04:输入寄存器读取
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nCount"></param>
        /// <param name="regData"></param>
        /// <returns></returns>
        public bool ReadInputReg(int nAddr, int nCount, UInt16[] regData)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateReadFrame(FunctionCode.ReadInputReg, nAddr, nCount);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[nCount * 2 + 2];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                FunctionCode rspCode = (FunctionCode)rxFrame[0];
                if (rspCode != FunctionCode.ReadInputReg)
                    return false;
                if (nCount * 2 != (int)rxFrame[1])
                    return false;

                // 读取数据
                for (int i = 0; i < nCount; i++)
                {
                    regData[i] = (UInt16)((rxFrame[i * 2 + 2] << 8) + rxFrame[i * 2 + 3]);
                }

                return true;
            }
        }

        /// <summary>
        /// 生成Modbus数据帧（写入单个数据）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nAddr"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        private byte[] GenerateWriteFrame_Single(FunctionCode code, int nAddr, int nValue)
        {
            byte[] frame = new byte[5];
            frame[0] = (byte)code;
            frame[1] = (byte)((nAddr >> 8) & 0xFF);
            frame[2] = (byte)(nAddr & 0xFF);
            frame[3] = (byte)((nValue >> 8) & 0xFF);
            frame[4] = (byte)(nValue & 0xFF);
            return frame;
        }

        /// <summary>
        /// 0x05:写入一个线圈
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public bool WriteSingleCoil(int nAddr, bool bVal)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateWriteFrame_Single(FunctionCode.WriteSingleCoil, nAddr, bVal ? 1 : 0);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[5];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧（服务器直接返回客户端发出的指令）
                if (txFrame.Length != rxFrame.Length)
                    return false;
                for (int i = 0; i < txFrame.Length; i++)
                {
                    if (rxFrame[i] != txFrame[i])
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 0x06:写入一个保存寄存器
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public bool WriteWriteSingleReg(int nAddr, int nVal)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateWriteFrame_Single(FunctionCode.WriteSingleReg, nAddr, nVal);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[5];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧（服务器直接返回客户端发出的指令）
                if (txFrame.Length != rxFrame.Length)
                    return false;
                for (int i = 0; i < txFrame.Length; i++)
                {
                    if (rxFrame[i] != txFrame[i])
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 生成Modbus数据帧（写入多个线圈）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nAddr"></param>
        /// <param name="nCoilCount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] GenerateWriteFrame_MultiCoil(FunctionCode code, int nAddr, int nCoilCount, byte[] data)
        {
            byte[] frame = new byte[6 + ((nCoilCount +7) / 8)];
            frame[0] = (byte)code;
            frame[1] = (byte)((nAddr >> 8) & 0xFF);
            frame[2] = (byte)(nAddr & 0xFF);
            frame[3] = (byte)((nCoilCount >> 8) & 0xFF);
            frame[4] = (byte)(nCoilCount & 0xFF);

            frame[5] = (byte)((nCoilCount + 7) / 8);

            for (int i = 0; i < data.Length; i++)
            {
                frame[6 + i] = data[i];
            }

            return frame;
        }

        /// <summary>
        /// 0x0F:线圈连续写入
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="nCoilCount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteMultiCoil(int nAddr, int nCoilCount, byte[] data)
        {
            lock (this)
            {
                if (((nCoilCount + 7) / 8) > data.Length)
                    return false;

                // 发送数据帧
                byte[] txFrame = GenerateWriteFrame_MultiCoil(FunctionCode.WriteMultiCoil, nAddr, nCoilCount, data);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[5];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                if (rxFrame.Length != 5)
                    return false;
                for (int i = 0; i < rxFrame.Length; i++)
                {
                    if (rxFrame[i] != txFrame[i])
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 生成Modbus数据帧（写入多个寄存器）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nAddr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] GenerateWriteFrame_MultiReg(FunctionCode code, int nAddr, UInt16[] data)
        {
            byte[] frame = new byte[6 + data.Length * 2];
            frame[0] = (byte)code;
            frame[1] = (byte)((nAddr >> 8) & 0xFF);
            frame[2] = (byte)(nAddr & 0xFF);
            frame[3] = (byte)((data.Length >> 8) & 0xFF);
            frame[4] = (byte)(data.Length & 0xFF);
            frame[5] = (byte)(data.Length * 2);

            for (int i = 0; i < data.Length; i++)
            {
                frame[6 + i * 2] = (byte)(data[i] >> 8);
                frame[7 + i * 2] = (byte)(data[i] & 0xFF);
            }

            return frame;
        }

        /// <summary>
        /// 0x10:写入多个寄存器
        /// </summary>
        /// <param name="nAddr"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteMultiReg(int nAddr, UInt16[] data)
        {
            lock (this)
            {
                // 发送数据帧
                byte[] txFrame = GenerateWriteFrame_MultiReg(FunctionCode.WriteMultiReg, nAddr, data);
                if (!SendFrame(txFrame))
                    return false;

                // 接收数据帧
                byte[] rxFrame = new byte[5];
                if (!ReceiveFrame(rxFrame))
                    return false;

                // 检查数据帧
                if (rxFrame.Length != 5)
                    return false;
                for (int i = 0; i < rxFrame.Length; i++)
                {
                    if (rxFrame[i] != txFrame[i])
                        return false;
                }

                return true;
            }
        }
    }

    /// <summary>
    /// ModbusRtu协议，在Modbus的基础上实现数据包收发功能
    /// </summary>
    public class ModbusRtu : Modbus
    {
        /// <summary>
        /// 站号
        /// </summary>
        private int m_nId;

        /// <summary>
        /// 串口
        /// </summary>
        private ComLink m_ComLink;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cl">串口，用于数据包收发</param>
        /// <param name="nId">站号，ModbusRTU 必须有站号</param>
        public ModbusRtu(ComLink cl, int nId)
        {
            m_ComLink = cl;
            m_nId = nId;
        }

        /// <summary>
        /// 发送Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected override bool SendFrame(byte[] frame)
        {
            byte[] buffer = new byte[frame.Length + 3];

            // 从机地址
            buffer[0] = (byte)m_nId;

            // Modbus数据
            for (int i = 0; i < frame.Length; i++)
            {
                buffer[i + 1] = frame[i];
            }

            // CRC
            UInt16 nCrc16 = GetCrc16(buffer, 0, buffer.Length - 2);
            buffer[frame.Length + 1] = (byte)(nCrc16 & 0xFF);
            buffer[frame.Length + 2] = (byte)((nCrc16 >> 8) & 0xFF);

            // 发送
            return m_ComLink.WriteData(buffer, buffer.Length);
        }

        /// <summary>
        /// 接收Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected override bool ReceiveFrame(byte[] frame)
        {
            // 接收数据
            byte[] buffer = new byte[frame.Length + 3];
            int nRxLen = m_ComLink.ReadData(buffer, buffer.Length);
            if (nRxLen != buffer.Length)
                return false;

            // 检查站号
            if ((int)buffer[0] != m_nId)
                return false;

            // 检查CRC
            UInt16 nCrc16 = GetCrc16(buffer, 0, buffer.Length - 2);
            UInt16 nRxCrc16 = (UInt16)(buffer[buffer.Length - 2] + (buffer[buffer.Length - 1] << 8));
            if (nCrc16 != nRxCrc16)
                return false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = buffer[i + 1];
            }

            return true;
        }

        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="pucFrame"></param>
        /// <param name="nOffset"></param>
        /// <param name="nCount"></param>
        /// <returns></returns>
        private UInt16 GetCrc16(byte[] pucFrame, int nOffset, int nCount)
        {
            byte ucCRCHi = 0xFF;
            byte ucCRCLo = 0xFF;
            int nIndex = 0;
            for (int i = 0; i < nCount; i++)
            {
                nIndex = ucCRCLo ^ pucFrame[nOffset + i];
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[nIndex]);
                ucCRCHi = aucCRCLo[nIndex];
            }
            return (UInt16)(ucCRCHi << 8 | ucCRCLo);
        }

        /// <summary>
        /// CRC校验表
        /// </summary>
        private byte[] aucCRCHi = {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40
        };

        /// <summary>
        /// CRC校验表
        /// </summary>
        private byte[] aucCRCLo = {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
            0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
            0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
            0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
            0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
            0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
            0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
            0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
            0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
            0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
            0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
            0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
            0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
            0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
            0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
            0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
            0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
            0x41, 0x81, 0x80, 0x40
        };
    }


    /// <summary>
    /// ModbusTcp协议，在Modbus的基础上实现数据包收发功能
    /// </summary>
    public class ModbusTcp : Modbus
    {
        /// <summary>
        /// 站号
        /// </summary>
        private int m_nId;

        /// <summary>
        /// 网口
        /// </summary>
        private TcpLink m_TcpLink;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tl">网口，用于数据包收发</param>
        /// <param name="nId">站号，对于 ModbusTcp 站号不是必须的，一般默认为0</param>
        public ModbusTcp(TcpLink tl, int nId)
        {
            m_TcpLink = tl;
            m_nId = nId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tl"></param>
        public ModbusTcp(TcpLink tl)
        {
            m_TcpLink = tl;
            m_nId = 9;
        }

        /// <summary>
        /// 发送Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected override bool SendFrame(byte[] frame)
        {
            byte[] buffer = new byte[frame.Length + 7];

            // 事务ID
            buffer[0] = 0;
            buffer[1] = 0;

            // 协议ID
            buffer[2] = 0;
            buffer[3] = 0;

            // 长度
            buffer[4] = (byte)((frame.Length + 1) >> 8);
            buffer[5] = (byte)((frame.Length + 1) & 0xFF);
            
            // 从站ID
            buffer[6] = (byte)m_nId;

            for (int i = 0; i < frame.Length; i++)
            {
                buffer[i + 7] = frame[i];
            }
            return m_TcpLink.WriteData(buffer, buffer.Length);
        }

        /// <summary>
        /// 接收Modbus数据帧
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected override bool ReceiveFrame(byte[] frame)
        {
            // 接收数据
            byte[] buffer = new byte[frame.Length + 7];
            int nRxLen = m_TcpLink.ReadData(buffer, buffer.Length,true);
            if (nRxLen != buffer.Length)
                return false;

            // 事务ID
            if ((buffer[0] != 0) || (buffer[1] != 0))
                return false;

            // 协议ID
            if ((buffer[2] != 0) || (buffer[3] != 0))
                return false;

            // 长度
            int nLen = buffer[5] + (buffer[4] << 8);
            if (nLen != (frame.Length + 1))
                return false;

            // 从站ID
            if ((int)buffer[6] != m_nId)
                return false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = buffer[i + 7];
            }

            return true;
        }
    }
}
