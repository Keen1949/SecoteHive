//2018-07-16 Binggoo 解决Baumer相机采集的当前图像是上一次的图像

using CommonTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGAPI;
using HalconDotNet;
using System.Threading;

namespace AutoFrameVision
{
    public class BaumerSys : SingletonTemplate<BaumerSys>
    {
        private BGAPI.System m_System = null;
        private int m_nCamCount = 0;

        public BaumerSys()
        {
            CreateSys();
        }

        ~BaumerSys()
        {
            ReleaseSys();
        }

        public bool CreateSys()
        {
            ReleaseSys();

            int sysno = 0;
            int syscount = 0;
            int result;
            result = BGAPI.EntryPoint.countSystems(ref syscount);
            if (result != BGAPI.Result.OK)
            {
                return false;
            }
            result = BGAPI.EntryPoint.createSystem(sysno, ref m_System);
            if (result != BGAPI.Result.OK)
            {
                return false;
            }
            result = m_System.open();
            if (result != BGAPI.Result.OK)
            {
                return false;
            }

            result = m_System.countCameras(ref m_nCamCount);
            if (result != BGAPI.Result.OK)
            {
                return false;
            }
            return true;
        }

        public BGAPI.System System
        {
            get
            {
                return m_System;
            }
        }

        public int CameraCount
        {
            get
            {
                return m_nCamCount;
            }
        }

        public bool ReleaseSys()
        {
            if (m_System != null)
            {
                int res = m_System.release();
                if (res != BGAPI.Result.OK)
                    return false;

                m_System = null;

                return true;
            }
            else
                return false;
        }

    }

    public class BaumerCam
    {
        private BGAPI.System m_parentSys = BaumerSys.GetInstance().System;
        private BGAPI.Camera m_pCamera = null;
        private BGAPI.Image m_pImage = null;
        private IntPtr m_imagebuffer;
        private string m_strCamUserName = "";
        private string m_strCamSN = "";
        private int m_nWidth = 0;
        private int m_nHeight = 0;
        private bool m_bNewImage = false;

        public BGAPI.System ParentSystem
        {
            set
            {
                m_parentSys = value;
            }

            get
            {
                return m_parentSys;
            }
        }

        /// <summary>
        /// 图像的宽度
        /// </summary>
        public int ImageWidth
        {
            get
            {
                return m_nWidth;
            }
        }

        /// <summary>
        /// 图像的高度
        /// </summary>
        public int ImageHeight
        {
            get
            {
                return m_nHeight;
            }
        }

        /// <summary>
        /// 图像数据
        /// </summary>
        public IntPtr ImageData
        {
            get
            {
                return m_imagebuffer;
            }
        }


        /// <summary>
        /// 通过UserName打开相机，打开相机之后自动开始采集图像
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CreateCamByUserName(string username)
        {
            if (m_parentSys == null)
                return false;

            ReleaseCam();

            int camno = 0;
            int result;
            m_imagebuffer = new IntPtr();

            BGAPI_FeatureState state = new BGAPI_FeatureState();
            BGAPIX_CameraInfo caminfo = new BGAPI.BGAPIX_CameraInfo();

            result = m_parentSys.open();
            if (result != BGAPI.Result.OK && result != BGAPI.Result.ALREADYDONE)
                return false;

            result = m_parentSys.countCameras(ref camno);
            if (result != BGAPI.Result.OK)
                return false;

            for (int i = 0; i < camno; i++)
            {
                BGAPI.Camera tcam = null;
                result = m_parentSys.createCamera(i, ref tcam);
                if (result != BGAPI.Result.OK)
                {
                    continue;
                }

                result = tcam.getDeviceInformation(ref state, ref caminfo);
                if (result != BGAPI.Result.OK)
                    return false;

                if (caminfo.label == username)
                {
                    m_strCamUserName = caminfo.label;
                    m_strCamSN = caminfo.serialNumber;
                    m_pCamera = tcam;

                    result = m_pCamera.open();
                    if (result != BGAPI.Result.OK)
                        return false;

                    //2018-07-16 解决Baumer相机采集的当前图像是上一次的图像
                    m_bNewImage = false;
                    m_pCamera.setTrigger(true);

                    m_pCamera.setTriggerSource(BGAPI_TriggerSource.BGAPI_TRIGGERSOURCE_SOFTWARE);

                    m_pCamera.setGVSHeartBeatTimeout(36000000);

                    m_pCamera.registerNotifyCallback(this, ImageCallBack);

                    result = BGAPI.EntryPoint.createImage(ref m_pImage);
                    if (result != BGAPI.Result.OK)
                        return false;

                    result = m_pCamera.setImage(ref m_pImage);
                    if (result != BGAPI.Result.OK)
                        return false;

                    //用软件触发的方式不能用
                    //result = m_pCamera.setImagePolling(true);
                    //if (result != BGAPI.Result.OK)
                    //    return false;

                    result = m_pCamera.setStart(true);
                    if (result != BGAPI.Result.OK)
                        return false;

                    break;
                }
                else
                {
                    result = m_parentSys.releaseCamera(ref tcam);
                    if (result != BGAPI.Result.OK)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 通过SN打开相机，打开相机之后自动开始采集图像
        /// </summary>
        /// <param name="serialno"></param>
        /// <returns></returns>
        public bool CreateCamBySN(string serialno)
        {
            if (m_parentSys == null)
                return false;

            int camno = 0;
            int result;
            m_imagebuffer = new IntPtr();
            BGAPI_FeatureState state = new BGAPI_FeatureState();
            BGAPIX_CameraInfo caminfo = new BGAPI.BGAPIX_CameraInfo();

            result = m_parentSys.open();
            if (result != BGAPI.Result.OK && result != BGAPI.Result.ALREADYDONE)
                return false;

            result = m_parentSys.countCameras(ref camno);
            if (result != BGAPI.Result.OK)
                return false;

            for (int i = 0; i < camno; i++)
            {
                BGAPI.Camera tcam = null;
                result = m_parentSys.createCamera(i, ref tcam);
                if (result != BGAPI.Result.OK)
                {
                    continue;
                }
                result = tcam.getDeviceInformation(ref state, ref caminfo);
                if (result != BGAPI.Result.OK)
                    return false;

                if (caminfo.serialNumber == serialno)
                {
                    m_strCamSN = caminfo.serialNumber;
                    m_strCamUserName = caminfo.label;
                    m_pCamera = tcam;

                    result = m_pCamera.open();
                    if (result != BGAPI.Result.OK)
                        return false;

                    //2018-07-16 解决Baumer相机采集的当前图像是上一次的图像
                    m_bNewImage = false;
                    m_pCamera.setTrigger(true);

                    m_pCamera.setTriggerSource(BGAPI_TriggerSource.BGAPI_TRIGGERSOURCE_SOFTWARE);

                    m_pCamera.setGVSHeartBeatTimeout(36000000);

                    m_pCamera.registerNotifyCallback(this, ImageCallBack);

                    result = BGAPI.EntryPoint.createImage(ref m_pImage);
                    if (result != BGAPI.Result.OK)
                        return false;

                    result = m_pCamera.setImage(ref m_pImage);
                    if (result != BGAPI.Result.OK)
                        return false;

                    //用软件触发的方式不能用
                    //result = m_pCamera.setImagePolling(true);
                    //if (result != BGAPI.Result.OK)
                    //    return false;

                    result = m_pCamera.setStart(true);
                    if (result != BGAPI.Result.OK)
                        return false;

                    break;
                }
                else
                {
                    result = m_parentSys.releaseCamera(ref tcam);
                    if (result != BGAPI.Result.OK)
                        return false;
                }
            }
            return true;
        }

        private int ImageCallBack(System.Object callBackOwner, ref Image image)
        {
            int ret = BGAPI.Result.OK;

            image.getSize(ref m_nWidth, ref m_nHeight);

            image.get(ref m_imagebuffer);

            ret = m_pCamera.setImage(ref image);

            m_bNewImage = true;

            return ret;
        }


        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            m_bNewImage = false;

            if (m_pCamera == null)
                return false;

            int result = m_pCamera.setStart(true);
            if (result != BGAPI.Result.OK)
                return false;

            return true;

        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            m_bNewImage = true;

            if (m_pCamera == null)
                return false;

            int result = m_pCamera.setStart(false);
            if (result != BGAPI.Result.OK)
                return false;

            return true;
        }

        /// <summary>
        /// 抓取图像
        /// </summary>
        /// <returns></returns>
        public bool Grab()
        {
            if (m_pCamera == null)
                return false;

            //int result = m_pCamera.getImage(ref m_pImage, 1000);
            //if (result != BGAPI.Result.OK)
            //    return false;

            //result = m_pImage.getSize(ref m_nWidth, ref m_nHeight);
            //if (result != BGAPI.Result.OK)
            //    return false;

            //result = m_pImage.get(ref m_imagebuffer);
            //if (result != BGAPI.Result.OK)
            //    return false;

            //m_pCamera.setImage(ref m_pImage);

            int result = m_pCamera.doTrigger();
            if (result != BGAPI.Result.OK)
                return false;

            while (!m_bNewImage)
            {
                Thread.Sleep(5);
            }

            m_bNewImage = false;

            return true;

        }

        public bool ReleaseCam()
        {
            m_bNewImage = false;
            int result = 0;
            if (m_parentSys != null)
            {
                if(m_pCamera != null)
                {
                    result = m_pCamera.setStart(false);
                    if (result != BGAPI.Result.OK)
                        return false;

                    result = m_parentSys.releaseCamera(ref m_pCamera);
                    if (result != BGAPI.Result.OK)
                        return false;
                    m_pCamera = null;
                }

                if (m_pImage != null)
                {
                    result = BGAPI.EntryPoint.releaseImage(ref m_pImage);
                    if (result != BGAPI.Result.OK)
                        return false;
                    m_pImage = null;
                }

            }
   
            return true;
        }
        public bool GetExposure(ref int nexposure)
        {

            if (m_pCamera == null)
                return false;
            int result = 0;
            BGAPI.BGAPI_FeatureState state = new BGAPI.BGAPI_FeatureState();
            BGAPI.BGAPIX_TypeRangeINT expo = new BGAPI.BGAPIX_TypeRangeINT();
            result = m_pCamera.getExposure(ref state, ref expo);
            if (result != BGAPI.Result.OK)
                return false;
            nexposure = expo.current;
            return true;
        }

        public bool SetExposure(int nexposure)
        {
            if (m_pCamera == null)
                return false;
            int result = 0;
            result = m_pCamera.setExposure(nexposure);
            if (result != BGAPI.Result.OK)
                return false;
            return true;
        }
        public bool GetSize(ref int w, ref int h)
        {
            int result = 0;
            if (m_pCamera == null)
                return false;

            BGAPI_FeatureState state = new BGAPI_FeatureState();
            BGAPIX_TypeROI roi = new BGAPIX_TypeROI();
            result = m_pCamera.getPartialScan(ref state, ref roi);
            if (result != BGAPI.Result.OK)
                return false;

            w = roi.curright - roi.curleft;
            h = roi.curbottom - roi.curtop;
            return true;

        }
        public string CamSN
        {
            get
            {
                return m_strCamSN;
            }
        }
        public string CamUserName
        {
            get
            {
                return m_strCamUserName;
            }
        }
    }
    public class CameraBaumer : CameraBase
    {
        private bool m_bIsOpen = false;
        private BaumerCam m_Baumer = new BaumerCam();
        public CameraBaumer(string strName) : base(strName)
        {

        }
        public override bool Close()
        {
            if (m_bIsOpen)
            {
                m_Baumer.ReleaseCam();
            }
            m_bIsOpen = false;

            DeInit();

            return true;
        }

        public override int Grab()
        {
            return Snap();
        }

        public override bool isOpen()
        {
            return m_bIsOpen;
        }

        public override bool Open()
        {
            if (m_bIsOpen)
            {
                return true;
            }

            m_bIsOpen = m_Baumer.CreateCamByUserName(this.Name);

            Thread.Sleep(100);

            return m_bIsOpen;
        }

        public override void SetGrabParam(string strParam, int nValue)
        {
            switch (strParam)
            {
                case "ExposureTimeRaw":
                case "ExposureTime":
                    m_Baumer.SetExposure(nValue);
                    break;

                default:
                    break;
            }
        }

        public override int Snap()
        {
            if(!m_bIsOpen)
            {
                Open();
            }

            if(m_bIsOpen)
            {
                try
                {
                    m_nCurrentIndex++;
                    m_image[m_nCurrentIndex % m_nBufferCount].Dispose();

                    m_Baumer.Grab();

                    HOperatorSet.GenImage1(out m_image[m_nCurrentIndex % m_nBufferCount], "byte",
                        m_Baumer.ImageWidth, m_Baumer.ImageHeight, m_Baumer.ImageData);

                }
                catch (HalconException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    WarningMgr.GetInstance().Info(e.Message);

                    return 0;
                }
                return 1;
            }

            return 0;
        }

        public override bool StopGrab()
        {
            return true;
        }
    }
}
