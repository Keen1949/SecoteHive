using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plc
{
    /// <summary>
    /// 数据块定义
    /// 制定起始地址，多个数据块可合并
    /// </summary>
    public class DataBlock : IComparable<DataBlock>
    {
        /// <summary>
        /// 数据块的起始位置
        /// </summary>
        public int Start { private set; get; }

        /// <summary>
        /// 数据块的结束位置
        /// </summary>
        public int End { private set; get; }

        /// <summary>
        /// 两个数据块合并时，允许的间隔
        /// </summary>
        public int IntervalMax { protected set; get; }

        /// <summary>
        /// 保存数据块的数值
        /// </summary>
        public UInt16[] DataBuffer { protected set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataBlock()
        {
            Start = 0;
            End = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="st">起始位置</param>
        /// <param name="end">结束位置</param>
        public DataBlock(int st, int end)
        {
            Set(st, end);
        }

        /// <summary>
        /// 设置起始位置、结束位置
        /// </summary>
        /// <param name="st"></param>
        /// <param name="end"></param>
        protected void Set(int st, int end)
        {
            if (st < 0)
                st = 0;
            if (end < 0)
                end = 0;

            Start = Math.Min(st, end);
            End = Math.Max(st, end);
        }

        /// <summary>
        /// 设置数据块合并时的最大间隔
        /// </summary>
        /// <param name="nInterval"></param>
        public void SetIntervalMax(int nInterval)
        {
            if (nInterval > 0)
                IntervalMax = nInterval;
        }

        /// <summary>
        /// 申请空间，保存数据块的内容
        /// </summary>
        protected virtual void GenerateBuffer()
        {
            DataBuffer = null;
        }

        /// <summary>
        /// 判断数据块是否可以合并
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AllowMerge(DataBlock item)
        {
            bool ret = false;
            if (item.Start == item.End)
                return true;
            if (this.Start == this.End)
                return true;

            // item在前
            //                  |____this____|
            // |____item____|
            if (item.End < this.Start)
            {
                // 两个项目相距较近可以合并
                if ((this.Start - item.End) < IntervalMax)
                    ret = true;
            }
            // item在前，且相交
            // |________this________|
            //    <--item____|
            else if (item.End < this.End)
            {
                ret = true;
            }
            // item在后
            // |______this______|
            //                      <--item____|
            else
            {
                if ((item.Start - this.End) < IntervalMax)
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 判断数据块是否可以合并，并对数据块的大小进行限制
        /// </summary>
        /// <param name="item"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public bool AllowMerge(DataBlock item, int maxSize)
        {
            if (AllowMerge(item))
            {
                int min = Math.Min(this.Start, item.Start);
                int max = Math.Max(this.End, item.End);
                if ((max - min) < maxSize)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 数据块合并
        /// </summary>
        /// <param name="item"></param>
        public void Merge(DataBlock item)
        {
            if (this.Start == this.End)
            {
                this.Start = item.Start;
                this.End = item.End;
                return;
            }

            if (item.Start == item.End)
            {
                return;
            }

            this.Start = Math.Min(this.Start, item.Start);
            this.End = Math.Max(this.End, item.End);

            GenerateBuffer();
        }

        /// <summary>
        /// 比较两个数据块的大小，根据起始位置进行比较
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public int CompareTo(DataBlock db)
        {
            if (this.Start > db.Start)
                return 1;
            else if (this.Start == db.Start)
                return 0;
            else
                return -1;
        }

        /// <summary>
        /// 判断指定的位置，是否在数据块以内
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Contain(int num)
        {
            if ((num >= Start) && (num < End))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断指定的数据块，是否被包含
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Contain(DataBlock db)
        {
            if ((db.Start >= this.Start) && (db.End <= this.End))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 读取数据块指定位置的数据
        /// </summary>
        /// <param name="num">读取位置</param>
        /// <param name="val">指定位置的数值</param>
        /// <returns>读取是否成功</returns>
        public virtual bool Read(int num, ref UInt16 val)
        {
            return false;
        }

        /// <summary>
        ///  读取数据块中连续的一段数据
        /// </summary>
        /// <param name="head">起始位置</param>
        /// <param name="count">读取数量</param>
        /// <returns></returns>
        public virtual UInt16[] Read(int head, int count)
        {
            return null;
        }
    }

    /// <summary>
    /// 位元件数据块，每一个元素是一个Bit
    /// </summary>
    public class BitBlock : DataBlock
    {
        /// <summary>
        /// 两个数据块合并时，允许的最大间隔
        /// </summary>
        private const int BitIntervalMax = 16 * 8;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BitBlock()
        {
            this.IntervalMax = BitIntervalMax;
        }

        /// <summary>
        /// 构造函数，包含一个数据
        /// </summary>
        /// <param name="num"></param>
        public BitBlock(int num)
        {
            int nStart = num - (num % 16);
            Set(nStart, nStart + 16);
            GenerateBuffer();
            this.IntervalMax = BitIntervalMax;         
        }

        /// <summary>
        /// 构造函数，包含一组数据
        /// </summary>
        /// <param name="nHead">起始位置</param>
        /// <param name="nCount">数量</param>
        public BitBlock(int nHead, int nCount)
        {
            int nStart = nHead - (nHead % 16);
            int nTail = nHead + nCount + 15;
            int nEnd = nTail - (nTail % 16);

            Set(nStart, nEnd);
            GenerateBuffer();
            this.IntervalMax = BitIntervalMax;            
        }

        /// <summary>
        /// 分配空间，保存元素的数值
        /// </summary>
        protected override void GenerateBuffer()
        {
            if (this.Start == this.End)
            {
                DataBuffer = null;
            }
            else
            {
                int cnt = (this.End - this.Start) / 16;
                if (cnt == 0)
                    cnt = 1;
                DataBuffer = new UInt16[cnt];
            }               
        }

        /// <summary>
        /// 读取一个元素
        /// </summary>
        /// <param name="num">位置</param>
        /// <param name="val">元素值</param>
        /// <returns></returns>
        public override bool Read(int num, ref UInt16 val)
        {
            if (!Contain(num))
                return false;

            int nIndex = num - this.Start;
            if ((DataBuffer[nIndex / 16] & (0x01 << (nIndex % 16))) != 0)
                val = 1;
            else
                val = 0;

            return true;
        }

        /// <summary>
        /// 读取一组
        /// </summary>
        /// <param name="head">元素</param>
        /// <param name="count">元素数量</param>
        /// <returns></returns>
        public override UInt16[] Read(int head, int count)
        {
            BitBlock bb = new BitBlock(head, count);
            if (!Contain(bb))
                return null;
            if (bb.DataBuffer == null)
                return null;

            int nIndex = (bb.Start - this.Start) / 16;
            Array.Copy(this.DataBuffer, nIndex, bb.DataBuffer, 0, bb.DataBuffer.Length);
            return bb.DataBuffer;
        }
    }

    /// <summary>
    /// 字元件数据块，每一个元素是一个Word
    /// </summary>
    public class WordBlock : DataBlock
    {
        /// <summary>
        /// 两个数据块合并时，允许的最大间隔
        /// </summary>
        private const int WordIntervalMax = 16;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WordBlock()
        {
            this.IntervalMax = WordIntervalMax;
        }

        /// <summary>
        /// 构造函数，包含一个元素
        /// </summary>
        /// <param name="num"></param>
        public WordBlock(int num)
        {
            Set(num, num + 1);
            GenerateBuffer();
            this.IntervalMax = WordIntervalMax;
        }

        /// <summary>
        /// 构造函数，包含一组元素
        /// </summary>
        /// <param name="nHead"></param>
        /// <param name="nCount"></param>
        public WordBlock(int nHead, int nCount)
        {
            Set(nHead, nHead + nCount);
            GenerateBuffer();
            this.IntervalMax = WordIntervalMax;
        }

        /// <summary>
        /// 申请一个数组，用于保存元素的数值
        /// </summary>
        protected override void GenerateBuffer()
        {
            if (this.Start == this.End)
            {
                DataBuffer = null;
            }
            else
            {
                DataBuffer = new UInt16[End - Start];
            }
        }

        /// <summary>
        /// 读取一个元素
        /// </summary>
        /// <param name="num">元素位置</param>
        /// <param name="val">元素值</param>
        /// <returns></returns>
        public override bool Read(int num, ref UInt16 val)
        {
            if (!Contain(num))
                return false;

            val = DataBuffer[num - this.Start];
            return true;
        }

        /// <summary>
        /// 读取一组元素值
        /// </summary>
        /// <param name="head">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public override UInt16[] Read(int head, int count)
        {
            WordBlock wb = new WordBlock(head, count);
            if (!Contain(wb))
                return null;
            if (wb.DataBuffer != null)
            {
                int nIndex = wb.Start - this.Start;
                Array.Copy(this.DataBuffer, nIndex, wb.DataBuffer, 0, wb.DataBuffer.Length);
            }

            return wb.DataBuffer;
        }
    }

    /// <summary>
    /// 监控类，实现对一种软元件监控
    /// </summary>
    public class PlcElementMonitor
    {
        /// <summary>
        /// 软元件类型
        /// </summary>
        public SoftElement Element { private set; get; }

        /// <summary>
        /// 监控列表，未合并
        /// </summary>
        private List<DataBlock> OriginalItems;

        /// <summary>
        /// 监控列表，合并以后的列表
        /// </summary>
        private List<DataBlock> MergeItems;

        /// <summary>
        /// 是否是位元件
        /// </summary>
        private bool IsBitData;

        /// <summary>
        /// 原始项目是否已经合并
        /// </summary>
        private bool HaveMerged;

        /// <summary>
        /// 构造函数，指定元件类型
        /// </summary>
        /// <param name="element"></param>
        public PlcElementMonitor(SoftElement element)
        {            
            Element = element;
            HaveMerged = false;
            OriginalItems = new List<DataBlock>();
            MergeItems = new List<DataBlock>();

            if (PlcDevice.IsBitElement(element))
                IsBitData = true;
            else
                IsBitData = false;
        }

        /// <summary>
        /// 添加监视项目
        /// </summary>
        /// <param name="addr">监视元素的位置</param>
        public void Add(int addr)
        {
            if (Element == SoftElement.D)
            {
                WordBlock wb = new WordBlock(addr);
                OriginalItems.Add(wb);           
            }
            else
            {
                BitBlock bb = new BitBlock(addr);
                OriginalItems.Add(bb);
            }

            HaveMerged = false;
        }

        /// <summary>
        /// 添加监视项目
        /// </summary>
        /// <param name="addr">监视的起始位置</param>
        /// <param name="count">元素数量</param>
        public void Add(int addr, int count)
        {
            if (Element == SoftElement.D)
            {
                WordBlock wb = new WordBlock(addr, count);
                OriginalItems.Add(wb);
            }
            else
            {
                BitBlock bb = new BitBlock(addr, count);
                OriginalItems.Add(bb);
            }

            HaveMerged = false;
        }

        /// <summary>
        /// 读取一个“位元件”的值（只对位元件有效）
        /// </summary>
        /// <param name="addr">元件位置</param>
        /// <returns></returns>
        public bool ReadBit(int addr)
        {
            if (!IsBitData)
                return false;

            UInt16 nVal = 0;
            foreach (var db in MergeItems)
            {
                if (db.Contain(db))
                {
                    if (db.Read(addr, ref nVal))
                    {
                        return (nVal != 0);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 读取字元件的其中一位（只对字元件有效）
        /// </summary>
        /// <param name="addr">字元件地址</param>
        /// <param name="index">字内偏移量</param>
        /// <returns></returns>
        public bool ReadBit(int addr, int index)
        {
            if (IsBitData)
                return false;

            UInt16 nVal = 0;
            foreach (var db in MergeItems)
            {
                if (db.Contain(db))
                {
                    if (db.Read(addr, ref nVal))
                    {
                        return (nVal & (0x01 << index)) != 0;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 读取一个字
        /// 对于字元件：读取一个字
        /// 对于位元件：读取连续16个位
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="val">元素值</param>
        /// <returns>读取是否成功，元素地址超限，会读取失败</returns>
        public bool ReadWord(int addr, ref UInt16 val)
        {
            if (IsBitData)
            {
                int bitAddr = addr - (addr % 16);
                foreach (var db in MergeItems)
                {
                    if (db.Contain(bitAddr))
                    {
                        UInt16[] bitData = db.Read(bitAddr, 16);
                        if (bitData == null)
                        {
                            return false;
                        }
                        else
                        {
                            val = bitData[0];
                            return true;
                        }
                    }
                }
            }
            else
            {
                foreach (var db in MergeItems)
                {
                    if (db.Contain(addr))
                    {
                        if (db.Read(addr, ref val))
                            return true;
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 读取一组元素
        /// 对于字元件：读取多个字
        /// 对于位元件：读取多个位（位元件数量 = count * 16）
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public UInt16[] ReadWords(int addr, int count)
        {
            if (IsBitData)
            {
                BitBlock bitBlock = new BitBlock(addr, count * 16);
                foreach (var db in MergeItems)
                {
                    if (db.Contain(bitBlock))
                    {
                        return db.Read(bitBlock.Start, bitBlock.End - bitBlock.Start);
                    }
                }
            }
            else
            {
                foreach (var db in MergeItems)
                {
                    WordBlock wordBlock = new WordBlock(addr, count);
                    if (db.Contain(wordBlock))
                    {
                        return db.Read(wordBlock.Start, wordBlock.End - wordBlock.Start);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 清除监视列表
        /// </summary>
        public void Clear()
        {
            OriginalItems.Clear();
            HaveMerged = false;
        }

        /// <summary>
        /// 监视列表合并
        /// </summary>
        /// <param name="countMax">数据块合并的最大限制</param>
        private void Merge(int countMax)
        {
            if (HaveMerged)
                return;

            if (OriginalItems.Count == 0)
            {
                MergeItems.Clear();
                HaveMerged = true;
                return;
            }
           
            OriginalItems.Sort();
            MergeItems.Clear();
            MergeItems.Add(OriginalItems[0]);
            foreach(var db in OriginalItems)
            {
                if (MergeItems.Last().AllowMerge(db, countMax))
                {
                    MergeItems.Last().Merge(db);
                }
                else
                {
                    MergeItems.Add(db);
                }
            }
            HaveMerged = true;
        }

        /// <summary>
        /// 更新监控项目的最新值
        /// </summary>
        /// <param name="dev">PLC设备</param>
        /// <returns></returns>
        public bool Update(PlcDevice dev)
        {
            if (dev == null)
                return false;
            if (!dev.IsOpen())
                return false;

            int nWordsReadCount = dev.m_CmdLimit.WordsReadCount;
            if (Element != SoftElement.D)
                nWordsReadCount = dev.m_CmdLimit.WordsReadCount * 16;
            Merge(nWordsReadCount);
            if (MergeItems.Count == 0)
                return true;

            foreach(var item in MergeItems)
            {
                if (!dev.ReadMultiWord(Element, item.Start, item.DataBuffer))
                    return false;
            }

            return true;
        }       
    }

    /// <summary>
    /// 监控类，实现对一个PLC设备监控
    /// </summary>
    public class PlcDeviceMonitor
    {
        /// <summary>
        /// PLC设备
        /// </summary>
        private PlcDevice m_plcDevice;

        /// <summary>
        /// 监控项目集合
        /// Key:监控的元件类型
        /// Value：监控的元件列表
        /// </summary>
        private Dictionary<SoftElement, PlcElementMonitor> m_dictElement = new Dictionary<SoftElement, PlcElementMonitor>();
        
        /// <summary>
        /// 类构造函数
        /// </summary>
        public PlcDeviceMonitor(PlcDevice dev)
        {
            m_dictElement.Add(SoftElement.X, new PlcElementMonitor(SoftElement.X));
            m_dictElement.Add(SoftElement.Y, new PlcElementMonitor(SoftElement.Y));
            m_dictElement.Add(SoftElement.M, new PlcElementMonitor(SoftElement.M));
            m_dictElement.Add(SoftElement.D, new PlcElementMonitor(SoftElement.D));

            m_plcDevice = dev;
        }

        /// <summary>
        /// 添加监视的项目
        /// </summary>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        public void AddMonitorItem(SoftElement element, int addr)
        {
            m_dictElement[element].Add(addr);
        }

        /// <summary>
        /// 添加监视的项目
        /// </summary>
        /// <param name="element">元件类型</param>
        /// <param name="addr">元件起始地址</param>
        /// <param name="count">元件数量</param>
        public void AddMonitorItem(SoftElement element, int addr, int count)
        {
            m_dictElement[element].Add(addr, count);
        }

        /// <summary>
        /// 读取一个Bit，只对“位元件”有效
        /// </summary>
        /// <param name="element"> 元件类型：XYM </param>
        /// <param name="addr">元件地址</param>
        /// <returns></returns>
        public bool ReadBit(SoftElement element, int addr)
        {
            return m_dictElement[element].ReadBit(addr);
        }

        /// <summary>
        /// 读取一个Bit，只对“字元件”有效
        /// </summary>
        /// <param name="element"> 元件类型：D </param>
        /// <param name="addr"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ReadBit(SoftElement element, int addr, int index)
        {
            return m_dictElement[element].ReadBit(addr, index);
        }

        /// <summary>
        /// 读取一个字
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="addr">起始地址</param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool ReadWord(SoftElement element, int addr, ref UInt16 val)
        {
            return m_dictElement[element].ReadWord(addr, ref val);
        }

        /// <summary>
        /// 批量读取字
        /// </summary>
        /// <param name="element">元件类型：X、Y、M、D </param>
        /// <param name="addr">起始地址</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public UInt16[] ReadWords(SoftElement element, int addr, int count)
        {
            return m_dictElement[element].ReadWords(addr, count);
        }
        
        /// <summary>
        /// 更新PLC设备监视项目，监视的元件类型包括：X、Y、M、D
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            foreach(var ele in m_dictElement)
            {
                if (!ele.Value.Update(m_plcDevice))
                    return false;
            }

            return true;
        } 
    }

    /// <summary>
    /// 对一组PLC设备监控
    /// </summary>
    public class PlcMonitor
    {
        /// <summary>
        /// PLC设备列表
        /// </summary>
        private List<PlcDevice> m_PlcList;

        /// <summary>
        /// PLC设备监控类列表，每一项是对一个PLC设备的监控类
        /// </summary>
        private List<PlcDeviceMonitor> m_MonitorList;

        /// <summary>
        /// 对一组PLC设备监控
        /// </summary>
        /// <param name="plcList">PLC设备列表</param>
        public PlcMonitor(List<PlcDevice> plcList)
        {
            m_MonitorList = new List<PlcDeviceMonitor>();

            if (plcList == null)
            {
                m_PlcList = new List<PlcDevice>();             
            }
            else
            {
                m_PlcList = plcList;
                foreach (var dev in plcList)
                {
                    m_MonitorList.Add(new PlcDeviceMonitor(dev));
                }
            }                
        }

        /// <summary>
        /// PLC设备序号检查
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool PlcNumberCheck(int num)
        {
            if (num < 0)
                return false;
            if (num >= m_PlcList.Count)
                return false;
            return true;
        }

        /// <summary>
        /// 添加监视项目
        /// </summary>
        /// <param name="plcIndex">PLC序号</param>
        /// <param name="element">监控的元件类型</param>
        /// <param name="addr">元件的地址</param>
        /// <returns></returns>
        public bool Add(int plcIndex, SoftElement element, int addr)
        {
            if (!PlcNumberCheck(plcIndex))
                return false;

            m_MonitorList[plcIndex].AddMonitorItem(element, addr);
            return true;
        }

        /// <summary>
        /// 添加监视项目
        /// </summary>
        /// <param name="plcIndex"></param>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool Add(int plcIndex, SoftElement element, int addr, int count)
        {
            if (!PlcNumberCheck(plcIndex))
                return false;

            m_MonitorList[plcIndex].AddMonitorItem(element, addr, count);
            return true;
        }

        /// <summary>
        /// 读取“位元件”值，元件类型包括：X、Y、M
        /// </summary>
        /// <param name="plcIndex"></param>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public bool ReadBit(int plcIndex, SoftElement element, int addr)
        {
            if (!PlcNumberCheck(plcIndex))
                return false;

            return m_MonitorList[plcIndex].ReadBit(element, addr);
        }

        /// <summary>
        /// 读取“位元件”值，元件类型包括：D
        /// </summary>
        /// <param name="plcIndex"></param>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ReadBit(int plcIndex, SoftElement element, int addr, int index)
        {
            if (!PlcNumberCheck(plcIndex))
                return false;

            return m_MonitorList[plcIndex].ReadBit(element, addr, index);
        }

        /// <summary>
        /// 读取一个“字元件”值，元件类型包括：X、Y、M、D
        /// </summary>
        /// <param name="plcIndex"></param>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool ReadWord(int plcIndex, SoftElement element, int addr, ref UInt16 val)
        {
            if (!PlcNumberCheck(plcIndex))
                return false;

            return m_MonitorList[plcIndex].ReadWord(element, addr, ref val);
        }

        /// <summary>
        /// 读取一组“字元件”值，元件类型包括：X、Y、M、D
        /// </summary>
        /// <param name="plcIndex"></param>
        /// <param name="element"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public UInt16[] ReadWords(int plcIndex, SoftElement element, int addr, int count)
        {
            if (!PlcNumberCheck(plcIndex))
                return null;

            return m_MonitorList[plcIndex].ReadWords(element, addr, count);
        }

        /// <summary>
        /// 更新监视项目值，监视项目包括：PLC列表中（每一种元件类型），已添加的项目
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            foreach(var devMonitor in m_MonitorList)
            {
                if (!devMonitor.Update())
                    return false;
            }

            return true;
        }
    }
}
