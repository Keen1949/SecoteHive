using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace AutoFrameVision
{
    /// <summary>
    /// 相机采集基类,抽像类,可由该类继承生成自己的相机类
    /// </summary>
    public abstract class CameraBase
    {
        /// <summary>
        /// 相机采集到的缓存图像
        /// </summary>
       public const int m_nBufferCount = 5;
        public HObject[] m_image = new HObject[m_nBufferCount];
        public uint m_nCurrentIndex =0;
        /// <summary>
        /// 相机的名称,对于GIGE相机为用户设置,对于文件相机为目录路径
        /// </summary>
        string m_strCamName;

        /// <summary>
        /// 以相机名称进行构造
        /// </summary>
        /// <param name="strName"></param>
        public CameraBase(string strName)
        {
            m_strCamName = strName;
            for(int i=0; i<m_nBufferCount; ++i)
              HOperatorSet.GenEmptyObj(out m_image[i]);


        }
        /// <summary>
        /// 属性:相机名称
        /// </summary>
        public string Name
        {
            get { return m_strCamName; }
            set { m_strCamName = value; }
        }
        /// <summary>
        /// 获取当前采集的图像
        /// </summary>
        /// <returns></returns>
        public HObject GetImage()
        {
            return m_image[m_nCurrentIndex % m_nBufferCount];
        }
        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public abstract bool Open();
        /// <summary>
        /// 判断相机是否打开
        /// </summary>
        /// <returns></returns>
        public abstract bool isOpen();
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns></returns>
        public abstract bool Close();

        public virtual void DeInit()
        {
            for(int i=0; i<m_nBufferCount; ++i)
            {
                if(m_image[i] != null)
                {
                    m_image[i].Dispose();
                    HOperatorSet.GenEmptyObj(out m_image[i]); 
                }
                m_nCurrentIndex = 0;
            }
        }  
        /// <summary>
        /// 软件触发一次同步采集
        /// </summary>
        /// <returns></returns>
        public abstract int Snap();
        public abstract void SetGrabParam(string strParam, int nValue);
        

        /// <summary>
        /// 触发一次异步采集
        /// </summary>
        /// <returns></returns>
        public abstract int Grab();
        /// <summary>
        /// 停止异步采集
        /// </summary>
        /// <returns></returns>
        public abstract bool StopGrab();

    }
}
