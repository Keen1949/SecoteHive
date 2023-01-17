using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using CommonTool;

namespace AutoFrameVision
{
    /// <summary>
    /// GIGE相机,通过halcon接口采集
    /// </summary>
    public class CameraGige:CameraBase
    {
        /// <summary>
        /// 相机采集句柄
        /// </summary>
        HTuple m_hAcqHandle = null;
        /// <summary>
        /// 当前是否处在异步模式
        /// </summary>
        bool m_bIsGrab = false;

        /// <summary>
        /// 以相机名称进行构造
        /// </summary>
        /// <param name="strName"></param>
        public CameraGige(string strName):base(strName)
        {
            m_hAcqHandle = null;
            m_bIsGrab = false;
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            if (m_hAcqHandle == null)
            {
                try
                {
                    HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "progressive",
                        -1, "default", -1, "false", "default", this.Name, 0, -1, out m_hAcqHandle);
                    return m_hAcqHandle != null;
                }
                catch (HalconException e)
                {
                    m_hAcqHandle = null;
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    WarningMgr.GetInstance().Info(e.Message);

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否已经打开相机
        /// </summary>
        /// <returns></returns>
        public override bool isOpen()
        {
            return m_hAcqHandle != null;
        }
        /// <summary>
        /// 关闭相机,同时释放相机缓存图像
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            if (m_hAcqHandle != null)
            {
                try
                {
                    HOperatorSet.CloseFramegrabber(m_hAcqHandle);
                }
                catch (HalconException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());

                    m_hAcqHandle = null;

                    return false;
                }
                m_hAcqHandle = null;
            }
            DeInit();
            return true;
        }      

        /// <summary>
        /// 同步采集一张图像
        /// </summary>
        /// <returns>0:失败, 1:成功</returns>
        public override int Snap()
        {
            if (m_bIsGrab)
            {
                m_bIsGrab = false;
            }
            if (m_hAcqHandle == null)
                Open();
            if (m_hAcqHandle != null)
            {
                try
                {
                    m_nCurrentIndex++;
                    m_image[m_nCurrentIndex % m_nBufferCount].Dispose();
                    HOperatorSet.GrabImage(out m_image[m_nCurrentIndex % m_nBufferCount], m_hAcqHandle);
                    
                }
                catch (HalconException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    WarningMgr.GetInstance().Info(e.Message);

                    //相机采集失败，断开相机，下次重新连接 2018-08-07 Binggoo
                    Close();
                    return 0;
                }
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 异步采集一张相机图像
        /// </summary>
        /// <returns></returns>
        public override int Grab()
        {
            if (m_hAcqHandle == null)
                Open();
            if (m_hAcqHandle != null)
            {
                try
                {
                    if(m_bIsGrab == false)
                    {
                        m_bIsGrab = true;
                        HOperatorSet.GrabImageStart(m_hAcqHandle, -1);
                    }
                    m_image[m_nCurrentIndex % m_nBufferCount].Dispose();
                    HOperatorSet.GrabImageAsync(out m_image[m_nCurrentIndex % m_nBufferCount], m_hAcqHandle, -1);
                    m_nCurrentIndex++;
                }
                catch (HalconException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return 0;
                }
                return 1;
            }
            return 0;
        }
     
        /// <summary>
        /// 停止异常采集
        /// </summary>
        /// <returns></returns>
        public override bool StopGrab()
        {
            if (m_hAcqHandle != null)
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(m_hAcqHandle, "do_abort_grab", 1);
                }
                catch (HalconException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置参数，不同型号相机不一样。 
        /// balser 曝光 : "ExposureTimeRaw"
        /// 大恒水星曝光："ExposureTime"
        /// </summary>
        public override void SetGrabParam(string strParam ,int nValue)
        {
            try
            {
                if (m_hAcqHandle == null)
                    Open();
                if(m_hAcqHandle != null)
                    HOperatorSet.SetFramegrabberParam(m_hAcqHandle, strParam, nValue);

            }
            catch (HalconException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
