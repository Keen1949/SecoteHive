using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonTool;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading.Tasks;
using AutoFrameDll;
using System.IO;
using ToolEx;

namespace AutoFrameVision
{
    /// <summary>
    /// 视觉管理类
    /// </summary>
    public class VisionMgr : SingletonTemplate<VisionMgr>
    {
        /// <summary>
        /// 视觉处理步骤的集合,以步骤名为key
        /// </summary>
        public Dictionary<string, VisionBase> m_dicVision = new Dictionary<string, VisionBase>();
        /// <summary>
        /// 相机采集类的集合,以相机名字为key
        /// </summary>
        public Dictionary<string, CameraBase> m_dicCamera = new Dictionary<string, CameraBase>();

        public string m_strConfigDir;

        public CaliTranslate m_CalibTrans = new CaliTranslate();  //标定


        public string ConfigDir
        {
            get { return m_strConfigDir; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public VisionMgr()
        {
            m_dicVision.Clear();
            m_dicCamera.Clear();

            m_strConfigDir = AppDomain.CurrentDomain.BaseDirectory + "VisionConfig\\" + ProductMgr.GetInstance().DeviceName + "\\";

            ProductMgr.GetInstance().ProductInfoChangedEvent += OnProductChanged;

            //HOperatorSet.SetSystem("temporary_mem_cache", "false");
        }

        /// <summary>
        /// 加入一个相机
        /// </summary>
        /// <param name="cb"></param>
        public void AddCamera(CameraBase cb)
        {
            m_dicCamera.Add(cb.Name, cb);
        }

        /// <summary>
        /// 加入一个处理步骤
        /// </summary>
        /// <param name="strCamName"></param>
        /// <param name="vb"></param>
        public void AddVisionStep(string strCamName, VisionBase vb)
        {
            CameraBase cb;
            if (m_dicCamera.TryGetValue(strCamName, out cb))
            {
                vb.BindCamera(cb);
                m_dicVision.Add(vb.Name, vb);
            }
            else
            {
                throw new Exception("系统指定的相机" + strCamName + "未找到");
            }
        }

        /// <summary>
        /// 保存各个相机的曝光值到ini
        /// </summary>
        public void WriteExposureTime(string strStep, int nExp)
        {
            string strFile = m_strConfigDir/*/* + strStep + "/"*/+ /*strStep+*/ "param.ini";
            //IniOperation.WriteValue(strFile, "ExposureTime", "nExp", nExp.ToString());
            IniOperation.WriteValue(strFile, strStep, "ExposureTime", nExp.ToString());

            //int nExp = 0;
            VisionBase vb;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                vb.SetExposureTime(nExp);
            }
            else
            {
                throw new Exception("系统指定的视觉处理步骤:" + strStep + "未找到");
            }

        }

        /// <summary>
        /// 读取各个相机的曝光值到ini
        /// </summary>
        public void ReadExposureTime(string strStep, out string strExp)
        {
            int nExp = 0;
            string strFile = m_strConfigDir /*+ strStep + "/" + strStep*/ + "param.ini";
            //IniOperation.GetStringValue(strFile, "ExposureTime", "nExp", nExp.ToString());
            //IniOperation.GetStringValue(strFile, strStep, "ExposureTime", nExp.ToString());
            string strTemp = IniOperation.GetStringValue(strFile, strStep, "ExposureTime", null);
            nExp = Convert.ToInt32(strTemp);

            VisionBase vb;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                if (nExp == 0)
                {
                    nExp = vb.m_ExposureTime;  //未读到将值设置成初始值
                    WriteExposureTime(strStep, nExp);
                }
                vb.SetExposureTime(nExp);
                strExp = nExp.ToString();
            }
            else
            {
                throw new Exception("系统指定的视觉处理步骤:" + strStep + "未找到");
            }

        }

        /// <summary>
        /// 析构时关闭已经打开的相机
        /// </summary>
        protected void Dispose()
        {
            foreach (KeyValuePair<string, CameraBase> cb in m_dicCamera)
            {
                if (cb.Value.isOpen())
                    cb.Value.Close();
            }
        }

        /// <summary>
        /// 获取指定名称的相机类
        /// </summary>
        /// <param name="strCamName"></param>
        /// <returns></returns>
        public CameraBase GetCam(string strCamName)
        {
            return m_dicCamera[strCamName];
        }

        public VisionBase GetVisionBase(string strStepName)
        {
            return m_dicVision[strStepName];
        }

        /// <summary>
        /// 绑定处理步骤和显示控件
        /// </summary>
        /// <param name="strStep"></param>
        /// <param name="vc"></param>
        public void BindWindow(string strStep, VisionControl vc)
        {
            VisionBase vb = null;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                vb.BindWindow(vc);
            }
        }
        /// <summary>
        /// 保存当前处理步骤的源图像
        /// </summary>
        /// <param name="strStep"></param>
        /// <param name="image"></param>
        /// <param name="bResult"></param>
        public void SaveSrcImage(string strStep, HObject image, bool bResult)
        {
            if (image != null)
            {
                int nSave = SystemMgr.GetInstance().GetParamInt("ImageSaveParam");
                if (nSave > 0)
                {
                    if (nSave == 2 || (nSave == 1 && bResult == false))
                    {
                        string strDir = SystemMgr.GetInstance().GetImagePath("\\RawImage\\" +
                            DateTime.Now.ToString("yyyyMMdd") + "\\" + strStep + (bResult ? "_OK" : "_NG"));

                        try
                        {
                            if (image != null && image.IsInitialized() && image.Key != IntPtr.Zero)
                            {
                                HOperatorSet.WriteImage(image, "png", 0, strDir + "\\" + DateTime.Now.ToString("HH_mm_ss_ffff"));
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// 保存当前处理步骤的窗口图像
        /// </summary>
        /// <param name="strStep"></param>
        /// <param name="image"></param>
        /// <param name="bResult"></param>
        public void SaveWindowImage(string strStep, HObject image, bool bResult)
        {
            if (image != null)
            {
                int nSave = SystemMgr.GetInstance().GetParamInt("WindowSaveParam");
                if (nSave > 0)
                {
                    if (nSave == 2 || (nSave == 1 && bResult == false))
                    {
                        string strDir = SystemMgr.GetInstance().GetImagePath("\\WindowImage\\" +
                            DateTime.Now.ToString("yyyyMMdd") + "\\" + strStep + (bResult ? "_OK" : "_NG"));
                        try
                        {
                            HOperatorSet.WriteImage(image, "png", 0, strDir + "\\" + DateTime.Now.ToString("HH_mm_ss_ffff"));
                        }
                        catch { }
                    }
                }
            }
        }


        /// <summary>
        /// 手动调试用,指定相机拍照
        /// </summary>
        /// <param name="strCamera"></param>
        /// <returns></returns>
        public HObject CameraSnap(string strCamera)
        {
            CameraBase cb = null;
            if (m_dicCamera.TryGetValue(strCamera, out cb))
            {
                if (cb.isOpen() == false)
                    cb.Open();
                if (cb.Snap() != 0)
                {
                    ShowLog(strCamera + " snap ok! ");
                    //     SystemMgr.GetInstance().GetImagePath(strCamera)
                    return cb.GetImage();
                }
            }
            ShowLog(strCamera + " snap fail!");
            return null;
        }

        /// <summary>
        ///处理指定图像,显示在指定窗口中,手动调试用 
        /// </summary>
        /// <param name="strStep"></param>
        /// <param name="image"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public bool ProcessImage(string strStep, HObject image, VisionControl ctl)
        {
            VisionBase vb = null;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                vb.SetSrcImage(image);
                if (vb.Process(ctl))
                {
                    ShowLog(strStep + " image ok! ");
                    return true;
                }
            }
            ShowLog(strStep + " image fail! ");
            return false;
        }


        /// <summary>
        ///指定步骤开始采集图像,用于采集和处理分开时先采集图像 
        /// </summary>
        /// <param name="strStep"></param>
        /// <returns></returns>
        public bool SnapImage(string strStep)
        {
            VisionBase vb = null;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                if (vb.Snap())
                {
                    ShowLog(strStep + " snap ok! ");
                    return true;
                }
            }
            ShowLog(strStep + " snap fail! ");
            return false;
        }


        /// <summary>
        ///处理相机缓存中的图像,用于采集和处理分开时后处理图像 
        /// </summary>
        /// <param name="strStep"></param>
        /// <returns></returns>
        public bool ProcessImage(string strStep)
        {
            VisionBase vb = null;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                bool bResult = vb.Process();

                //Action<object> action = (object obj) =>
                //{
                SaveSrcImage(strStep, vb.GetSrcImage(), bResult);
                SaveWindowImage(strStep, vb.GetWindowImage(), bResult);
                //};
                //Task t1 = new Task(action, "");
                //t1.Start();

                ShowLog(strStep + (bResult ? " process ok! " : "  fail! "));
                return bResult;
            }
            ShowLog(strStep + " process undefine! ");
            return false;
         

        }

        /// <summary>
        ///  步骤处理,采集图像和图像处理一次完成
        /// </summary>
        /// <param name="strStep">步骤名称</param>
        /// <returns>处理成功或失败</returns>
        public bool ProcessStep(string strStep, int nFlag = 0)
        {
            VisionBase vb = null;
            if (m_dicVision.TryGetValue(strStep, out vb))
            {
                bool bResult = vb.Process();

                //Action<object> action = (object obj) =>
                //{
                SaveSrcImage(strStep, vb.GetSrcImage(), bResult);
                SaveWindowImage(strStep, vb.GetWindowImage(), bResult);
                //};
                //Task t1 = new Task(action, "");
                //t1.Start();
                ShowLog(strStep + (bResult ? " success! " : " fail! "));
                return bResult;
            }
            ShowLog(strStep + " step undefine! ");
            return false;
        }

        public void OnProductChanged()
        {
            m_strConfigDir = AppDomain.CurrentDomain.BaseDirectory + "VisionConfig\\" + ProductMgr.GetInstance().DeviceName + "\\";
            DirectoryInfo di = new DirectoryInfo(m_strConfigDir);
            if (di.Exists)
            {
                foreach (KeyValuePair<string, VisionBase> vb in m_dicVision)
                {
                    vb.Value.ChangeConfigDir(m_strConfigDir);
                   
                }
            }
            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(string.Format("The vision configuration directory for the current product '{0}' cannot be found. Please confirm the configuration", ProductMgr.GetInstance().DeviceName),
                    "Prompt for missing configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format("当前产品{0}的视觉配置目录找不到,请确认配置", ProductMgr.GetInstance().DeviceName),
                    "配置缺失提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
          
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public void LoadParam()
        {
            foreach (var vision in m_dicVision.Values)
            {
                vision.LoadParam();
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        public void SaveParam()
        {
            foreach (var vision in m_dicVision.Values)
            {
                vision.SaveParam();
            }
        }



        /// <summary>
        /// 测试线程,后台测试处理步骤
        /// </summary>
        public override void ThreadMonitor()
        {
            //base.ThreadMonitor();

            CameraFile cf = new CameraFile(@"E:\software_rd\AutoFrame\AutoFrameVision\halcon");
            cf.Open();
            while (m_bRunThread && cf.Grab() != 0)
            {
                //        ProcessImage("T1", cf.GetImage(), m_dicVision["T1"].m_visionControl);
                ProcessImage("T2", cf.GetImage(), m_dicVision["T1"].m_visionControl);

                System.Threading.Thread.Sleep(15);
            }

            cf.StopGrab();
            cf.Close();

        }

        /// <summary>
        /// 测试线程2,后台测试处理步骤,确认交互无异常
        /// </summary>
        public void ThreadMonitor2()
        {
            //base.ThreadMonitor();

            CameraFile cf = new CameraFile(@"E:\software_rd\AutoFrame\AutoFrameVision\halcon");
            cf.Open();
            while (m_bRunThread && cf.Grab() != 0)
            {
                ProcessImage("T1", cf.GetImage(), m_dicVision["T2"].m_visionControl);
                //    ProcessImage("T2", cf.GetImage(), m_dicVision["T2"].m_visionControl);
                System.Threading.Thread.Sleep(20);
            }

            cf.StopGrab();
            cf.Close();

        }

        Thread td = null;
        /// <summary>
        /// 开始测试线程
        /// </summary>
        public void StartTest()
        {
            this.StartMonitor();//开启第一处理线程
            if (td == null)
            {
                td = new Thread(ThreadMonitor2);//开启第二处理线程
                td.Start();
            }
        }

        /// <summary>
        /// 结束测试线程
        /// </summary>
        public void EndTest()
        {
            this.StopMonitor();
            if (td != null)
                td.Abort();
        }
    }
}
