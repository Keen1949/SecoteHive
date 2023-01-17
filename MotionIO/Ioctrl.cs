using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;

namespace MotionIO
{
    /// <summary>
    /// IO卡基类
    /// </summary>
    public abstract class IoCtrl
    {
        /// <summary>
        ///卡在系统中的序号 
        /// </summary>
        public int m_nIndex;
        /// <summary>
        ///卡号 
        /// </summary>
        public int m_nCardNo;
        /// <summary>
        ///输入点状态缓存区 
        /// </summary>
        public int m_nInData;
        /// <summary>
        /// 输入缓冲区数据是否有变化
        /// </summary>
        public bool m_bDataChange;
        /// <summary>
        ///输出点状态缓存区 
        /// </summary>
        public int m_nOutData;
        /// <summary>
        ///卡的有无效状态 
        /// </summary>
        public bool m_bEnable = false;       

        /// <summary>
        ///输入点名称向量 
        /// </summary>
        public string[]  m_strArrayIn;
        /// <summary>
        ///输出点名称向量 
        /// </summary>
        public string[]  m_strArrayOut;      

        /// <summary>
        /// 卡名称
        /// </summary>
        public string m_strCardName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl(int nIndex, int nCardNo)
        {
            m_nIndex = nIndex;
            m_nCardNo = nCardNo;
            m_bEnable = true;
        }

    
        /// <summary>
        /// 输入点总个数
        /// </summary>
        public int CountIoIn
        {
            get { return m_strArrayIn.Length; }
        }

        /// <summary>
        /// 输出点总个数 
        /// </summary>
        public int CountIoOut
        {
            get { return m_strArrayOut.Length; }
        }

        /// <summary>
        /// 卡是否启用成功
        /// </summary>
        /// <returns></returns>
        public bool IsEnable()
        {
            return m_bEnable;
        }

        /// <summary>
        /// 判断输入缓冲区是否有变化
        /// </summary>
        /// <returns></returns>
        public bool IsDataChange()
        {
            return m_bDataChange;
        }

        /// <summary>
        ///初始化 
        /// </summary>
        /// <returns></returns>
        public abstract bool Init();

        /// <summary>
        ///释放IO卡 
        /// </summary>
        public abstract void DeInit();

        /// <summary>
        ///获取卡所有的输入信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool ReadIOIn(ref int nData);

        /// <summary>
        ///获取卡所有的输出信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool ReadIOOut(ref int nData);

        /// <summary>
        ///按位获取输入信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public abstract bool ReadIoInBit(int nIndex);

        /// <summary>
        ///按位获取输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public abstract bool ReadIoOutBit(int nIndex);


        /// <summary>
        /// 按位输出信号 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public abstract bool WriteIoBit(int nIndex, bool bBit);

        /// <summary>
        /// 输出整个卡的信号 
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public abstract bool WriteIo(int nData);

        /// <summary>
        ///获取指定IO输入点的缓冲状态 
        /// </summary>
        /// <returns></returns>

        public bool GetIoInState(int nIndex)
        {
            return (m_nInData & (1 << (nIndex - 1))) != 0;
        }
        /// <summary>
        ///获取指定IO输出点的缓冲状态  
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool GetIoOutState( int nIndex)
        {
            return (m_nOutData & (1 << (nIndex - 1))) != 0;
        }

    }
}
