using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolEx
{
    /// <summary>
    /// 光源基类
    /// </summary>
    public abstract class LightBase
    {
        /// <summary>
        /// 光源名称
        /// </summary>
        protected string m_strName = "";

        /// <summary>
        /// 通信接口索引
        /// </summary>
        protected int m_nCommIndex = -1;

        /// <summary>
        /// 通道
        /// </summary>
        protected int m_nChannel = -1;

        /// <summary>
        /// 是否初始化
        /// </summary>
        protected bool m_bInit = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="nChannel"></param>
        /// <param name="nCommIndex"></param>
        public LightBase(string strName,int nChannel,int nCommIndex)
        {
            m_strName = strName;
            m_nCommIndex = nCommIndex;
            m_nChannel = nChannel;
            m_bInit = Init();
        }

        /// <summary>
        /// 光源名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_strName;
            }
        }

        /// <summary>
        /// 通信索引
        /// </summary>
        public int CommIndex
        {
            get
            {
                return m_nCommIndex;
            }
        }

        /// <summary>
        /// 通道
        /// </summary>
        public int Channel
        {
            get
            {
                return m_nChannel;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public abstract bool Init();

        /// <summary>
        /// 反初始化
        /// </summary>
        public abstract void DeInit();

        /// <summary>
        /// 打开光源
        /// </summary>
        /// <param name="nLight">亮度</param>
        /// <returns></returns>
        public abstract bool LightOn(int nLight = 255);
        
        /// <summary>
        /// 关闭光源
        /// </summary>
        public abstract void LightOff();
    }
}
