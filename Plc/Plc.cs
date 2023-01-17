using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicate;

namespace Plc
{
    /// <summary>
    /// PLC软元件的类型
    /// </summary>
    public enum SoftElement
    {
        /// <summary>
        /// X元件
        /// </summary>
        X = 0,

        /// <summary>
        /// 元件
        /// </summary>
        Y,

        /// <summary>
        /// M元件
        /// </summary>
        M,

        /// <summary>
        /// D元件
        /// </summary>
        D
    };

    /// <summary>
    /// 软元件地址映射表，包括：元件类型、起始地址、元件数量
    /// </summary>
    public class PlcAddressMap
    {
        /// <summary>
        /// 软元件地址信息：单位、起始地址、数量
        /// </summary>
        public struct SoftElementAddress
        {
            /// <summary>
            /// 元件类型
            /// </summary>
            public SoftElement Element;

            /// <summary>
            /// 元件起始地址
            /// </summary>
            public int StartAdrress;

            /// <summary>
            /// 元件数量
            /// </summary>
            public int Count;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="tp">元件类型</param>
            /// <param name="addr">元件起始地址</param>
            /// <param name="cnt">元件数量</param>
            public SoftElementAddress(SoftElement tp, int addr, int cnt)
            {
                Element = tp;

                if (addr >= 0)
                    StartAdrress = addr;
                else
                    StartAdrress = 0;

                if (cnt > 0)
                    Count = cnt;
                else
                    Count = 0;
            }

            /// <summary>
            /// 元件地址有效性检查，检查地址是否在指定的范围内
            /// </summary>
            /// <param name="nAddrToUse">元件的起始地址</param>
            /// <param name="nCountToUse">元件的数量</param>
            /// <returns></returns>
            public bool Check(int nAddrToUse, int nCountToUse)
            {
                int nAddrLow = StartAdrress;
                int nAddrHigh = StartAdrress + Count;

                int nAddr = nAddrToUse;
                if ((nAddr < nAddrLow) || (nAddr >= nAddrHigh))
                    return false;

                nAddr = nAddrToUse + nCountToUse - 1;
                if ((nAddr < nAddrLow) || (nAddr >= nAddrHigh))
                    return false;

                return true;
            }

            /// <summary>
            /// 元件地址有效性检查，检查地址是否在指定的范围内
            /// </summary>
            /// <param name="nAddrToUse">元件起始地址</param>
            /// <returns></returns>
            public bool Check(int nAddrToUse)
            {
                if ((nAddrToUse < StartAdrress) || (nAddrToUse >= (StartAdrress + Count)))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// X元件的地址范围
        /// </summary>
        public SoftElementAddress X;

        /// <summary>
        /// Y元件的地址范围
        /// </summary>
        public SoftElementAddress Y;

        /// <summary>
        /// M元件的地址范围
        /// </summary>
        public SoftElementAddress M;

        /// <summary>
        /// D元件的地址范围
        /// </summary>
        public SoftElementAddress D;

        /// <summary>
        /// 构造函数，元件地址范围的默认值是：0~0xFFFF
        /// </summary>
        public PlcAddressMap()
        {
            X = new SoftElementAddress(SoftElement.X, 0, 0xFFFF);
            Y = new SoftElementAddress(SoftElement.Y, 0, 0xFFFF);
            M = new SoftElementAddress(SoftElement.M, 0, 0xFFFF);
            D = new SoftElementAddress(SoftElement.D, 0, 0xFFFF);
        }

        /// <summary>
        /// 检查元件的地址是否有效
        /// </summary>
        /// <param name="element">元件类型</param>
        /// <param name="nAddrToUse">元件起始地址</param>
        /// <param name="nCountToUse">元件的数量</param>
        /// <returns></returns>
        public bool Check(SoftElement element, int nAddrToUse, int nCountToUse)
        {
            bool ret = false;
            switch (element)
            {
                case SoftElement.X:
                    ret = X.Check(nAddrToUse, nCountToUse);
                    break;

                case SoftElement.Y:
                    ret = Y.Check(nAddrToUse, nCountToUse);
                    break;

                case SoftElement.M:
                    ret = M.Check(nAddrToUse, nCountToUse);
                    break;

                case SoftElement.D:
                    ret = D.Check(nAddrToUse, nCountToUse);
                    break;

                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 检查元件的地址是否有效
        /// </summary>
        /// <param name="element">元件类型</param>
        /// <param name="nAddrToUse">元件的地址</param>
        /// <returns></returns>
        public bool Check(SoftElement element, int nAddrToUse)
        {
            bool ret = false;
            switch (element)
            {
                case SoftElement.X:
                    ret = X.Check(nAddrToUse);
                    break;

                case SoftElement.Y:
                    ret = Y.Check(nAddrToUse);
                    break;

                case SoftElement.M:
                    ret = M.Check(nAddrToUse);
                    break;

                case SoftElement.D:
                    ret = D.Check(nAddrToUse);
                    break;

                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 指定元件的有效地址范围
        /// </summary>
        /// <param name="typ">元件类型</param>
        /// <param name="addr">元件的起始地址</param>
        /// <param name="cnt">元件的数量</param>
        public void AddMap(string typ, string addr, string cnt)
        {
            int nAddr = 0;
            if (!int.TryParse(addr, out nAddr))
                return;

            int nCnt = 0;
            if (!int.TryParse(cnt, out nCnt))
                return;

            if (typ.Equals("X") || typ.Equals("x"))
            {
                X.StartAdrress = nAddr;
                X.Count = nCnt;
            }
            else if (typ.Equals("Y") || typ.Equals("y"))
            {
                Y.StartAdrress = nAddr;
                Y.Count = nCnt;
            }
            else if (typ.Equals("M") || typ.Equals("m"))
            {
                M.StartAdrress = nAddr;
                M.Count = nCnt;
            }
            else if (typ.Equals("D") || typ.Equals("d"))
            {
                D.StartAdrress = nAddr;
                D.Count = nCnt;
            }
        }
    }


    /// <summary>
    /// 多个指令打包时，指令个数的限制
    /// </summary>
    public class PlcCmdCountLimit
    {
        /// <summary>
        /// 位批量读取，一次读取数量上限
        /// </summary>
        public int BitsReadCount;

        /// <summary>
        /// 位批量写入，一次写入数量上限
        /// </summary>
        public int BitsWriteCount;

        /// <summary>
        /// 字批量读取，一次读取数量上限
        /// </summary>
        public int WordsReadCount;

        /// <summary>
        /// 字批量写入，一次写入数量上限
        /// </summary>
        public int WordsWriteCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlcCmdCountLimit()
        {
            BitsReadCount = 100;
            BitsWriteCount = 100;
            WordsReadCount = 10;
            WordsWriteCount = 10;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nBitsRead"></param>
        /// <param name="nBitsWrite"></param>
        /// <param name="nWordsRead"></param>
        /// <param name="nWordsWrite"></param>
        public PlcCmdCountLimit(int nBitsRead, int nBitsWrite, int nWordsRead, int nWordsWrite)
        {
            BitsReadCount = nBitsRead;
            BitsWriteCount = nBitsWrite;
            WordsReadCount = nWordsRead;
            WordsWriteCount = nWordsWrite;
        }
    }

    /// <summary>
    /// PLC设备的基类，定义PLC的公用函数
    /// 所有的PLC类型都继承此类，且实现这些接口函数
    /// </summary>
    public abstract class PlcDevice
    {
        /// <summary>
        /// 指示PLC设备是否打开
        /// </summary>
        protected bool m_bOpen;
        
        /// <summary>
        /// PLC设备的序号
        /// </summary>
        protected int m_nIndex;

        /// <summary>
        /// PLC设备的名称
        /// </summary>
        protected string m_strName;

        /// <summary>
        /// 元件的有效地址信息
        /// </summary>
        protected PlcAddressMap m_AddrMap;

        /// <summary>
        /// 元件批量读写时，一次读写的最大数量
        /// </summary>
        public PlcCmdCountLimit m_CmdLimit;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex">序号</param>
        /// <param name="strName">名称</param>
        /// <param name="mp">地址表</param>
        public PlcDevice(int nIndex, string strName, PlcAddressMap mp)
        {
            m_nIndex = nIndex;
            m_strName = strName;
            m_AddrMap = mp;

            m_CmdLimit = new PlcCmdCountLimit();
        }

        /// <summary>
        /// “位元件”判断：X、Y、M
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsBitElement(SoftElement element)
        {
            if ((element == SoftElement.X)
                || (element == SoftElement.Y)
                || (element == SoftElement.M))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// “字元件”判断：D
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsWordElement(SoftElement element)
        {
            if (element == SoftElement.D)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取设备的序号
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            return m_nIndex;
        }

        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return m_strName;
        }

        /// <summary>
        /// 判断设备是否打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return m_bOpen;
        }

        /// <summary>
        /// 打开设备，读写之前需要执行Open操作
        /// </summary>
        /// <returns></returns>
        public abstract bool Open();

        /// <summary>
        /// 关闭设备
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 读取位元件 : XYM
        /// </summary>
        /// <param name="element"> XYM </param>
        /// <param name="nAddr"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public virtual bool ReadBit(SoftElement element, int nAddr, ref bool bVal)
        {
            if (!IsBitElement(element))
                return false;

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 读取位元件 : XYM
        /// </summary>
        /// <param name="element"> XYM </param>
        /// <param name="nAddr"></param>
        /// <param name="bVals"></param>
        /// <returns></returns>
        public virtual bool ReadMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            if (!IsBitElement(element))
                return false;

            if (!m_AddrMap.Check(element, nAddr, bVals.Length))
                return false;

            return true;
        }

        /// <summary>
        /// 读取位元件 : D
        /// </summary>
        /// <param name="element"> D </param>
        /// <param name="nAddr"></param>
        /// <param name="nIndex"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public virtual bool ReadRegBit(SoftElement element, int nAddr, int nIndex, ref bool bVal)
        {
            if (element != SoftElement.D)
                return false;

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 读取字元件 : XYMD （16个位元件组成一个字）
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public virtual bool ReadWord(SoftElement element, int nAddr, ref UInt16 nVal)
        {
            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 16)) //16位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 读取字元件 : XYMD （16个位元件组成一个字）
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nVals"></param>
        /// <returns></returns>
        public virtual bool ReadMultiWord(SoftElement element, int nAddr, UInt16[] nVals)
        {
            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nVals.Length * 16)) //16位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nVals.Length))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 读取双字元件 : XYMD （32个位元件组成一个双字）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public virtual bool ReadDWord(SoftElement element, int nAddr, ref UInt32 nVal)
        {
            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 32)) //32位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 2)) //2个字组成一个双字
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 读取双字元件 : XYMD （32个位元件组成一个字）
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nVals"></param>
        /// <returns></returns>
        public virtual bool ReadMultiDWord(SoftElement element, int nAddr, UInt32[] nVals)
        {
            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nVals.Length * 32)) //32位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nVals.Length * 2))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 写入位元件 : XYM
        /// </summary>
        /// <param name="element"> XYM </param>
        /// <param name="nAddr"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public virtual bool WriteBit(SoftElement element, int nAddr, bool bVal)
        {
            if (element == SoftElement.X)
                return false;

            if (!IsBitElement(element))
                return false;

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 批量写入位元件 : XYM
        /// </summary>
        /// <param name="element"> XYM </param>
        /// <param name="nAddr"></param>
        /// <param name="bVals"></param>
        /// <returns></returns>
        public virtual bool WriteMultiBit(SoftElement element, int nAddr, bool[] bVals)
        {
            if (element == SoftElement.X)
                return false;

            if (!IsBitElement(element))
                return false;

            if (!m_AddrMap.Check(element, nAddr, bVals.Length))
                return false;

            return true;
        }

        /// <summary>
        /// 修改寄存器的一位 : D
        /// </summary>
        /// <param name="element"> D </param>
        /// <param name="nAddr"></param>
        /// <param name="nIndex"></param>
        /// <param name="bVal"></param>
        /// <returns></returns>
        public virtual bool WriteRegBit(SoftElement element, int nAddr, int nIndex, bool bVal)
        {
            if (element != SoftElement.D)
                return false;

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 写入字元件 : XYMD
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public virtual bool WriteWord(SoftElement element, int nAddr, UInt16 nVal)
        {
            if (element == SoftElement.X)
                return false;

            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 16)) //16位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr))
                    return false;
            }
            else
            {
                return false;
            }

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 批量写入字元件 : XYMD（16个位元件组成一个字）
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nData"></param>
        /// <returns></returns>
        public virtual bool WriteMultiWord(SoftElement element, int nAddr, UInt16[] nData)
        {
            if (element == SoftElement.X)
                return false;

            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nData.Length * 16)) //16位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nData.Length))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 写入双字元件 : XYMD
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nVal"></param>
        /// <returns></returns>
        public virtual bool WriteDWord(SoftElement element, int nAddr, UInt32 nVal)
        {
            if (element == SoftElement.X)
                return false;

            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 32)) //32位组成一个双字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, 2))
                    return false;
            }
            else
            {
                return false;
            }

            if (!m_AddrMap.Check(element, nAddr))
                return false;

            return true;
        }

        /// <summary>
        /// 批量写入双字元件 : XYMD（32个位元件组成一个字）
        /// </summary>
        /// <param name="element"> XYMD </param>
        /// <param name="nAddr"></param>
        /// <param name="nData"></param>
        /// <returns></returns>
        public virtual bool WriteMultiDWord(SoftElement element, int nAddr, UInt32[] nData)
        {
            if (element == SoftElement.X)
                return false;

            if (IsBitElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nData.Length * 32)) //32位组成一个字
                    return false;
            }
            else if (IsWordElement(element))
            {
                if (!m_AddrMap.Check(element, nAddr, nData.Length * 2))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
