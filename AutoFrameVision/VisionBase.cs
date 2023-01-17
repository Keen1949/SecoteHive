//2018-12-1 Binggoo 1. 加入当Process图像采集失败时报错，防止在使用时未加返回值判断而出错。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;
using AutoFrameDll;
using ToolEx;
using CommonTool;

namespace AutoFrameVision
{
    /// <summary>
    /// 视觉处理步骤基类
    /// </summary>
    public abstract  class  VisionBase:IVisionControlUpdate
    {
        public int m_ExposureTime;//曝光值
        public int m_GainRaw = 0;     //增益
        public int m_DigitalShift = 0; //增益倍数
        public bool m_bGainEnable = false; //是否启用增益
        public double m_dbOffsetU = 0.0;   //角度补偿
        public double m_dbOffsetX = 0; //X方向补偿
        public double m_dbOffsetY = 0; //Y方向补偿

        public const int VisionException = -990;
        /// <summary>
        /// 视觉配置目录
        /// </summary>
        public string m_strDir = string.Empty;
        /// <summary>
        /// 步骤名称
        /// </summary>
        string _strName;
        /// <summary>
        /// 当前处理的图像缓存
        /// </summary>
        public HObject imgSrc = null;       
        /// <summary>
        /// 当前图像处理的窗口缓存
        /// </summary>
        public HObject imgWindow = null;
        
        /// <summary>
        /// 图像处理的返回结果,为数组显示
        /// </summary>
        public HTuple tupleResult = null;
        /// <summary>
        /// 自动流程时当前处理步骤绑定的视觉显示控件
        /// </summary>
        public VisionControl m_visionControl = null;
        /// <summary>
        /// 自动流程时当前处理步骤绑定的相机
        /// </summary>
        public CameraBase m_Camera = null;
        /// <summary>
        /// 当前处理步骤的名称
        /// </summary>
        public string Name
        {
            get { return _strName; }     
        }
        /// <summary>
        /// 以名称构造
        /// </summary>
        /// <param name="strName"></param>
        public VisionBase(string strName)
        {
            _strName = strName;
            ChangeConfigDir(VisionMgr.GetInstance().ConfigDir);
        }

        public void ChangeConfigDir(string strConfigDir)
        {
            m_strDir = strConfigDir + Name;
            try
            {
                InitConfig();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(),
                    Name + "视觉配置错误,对应产品型号" + ProductMgr.GetInstance().DeviceName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
           
        }

        /// <summary>
        /// 显示控件刷新
        /// </summary>
        /// <param name="ctl"></param>
        public virtual void UpdateVisionControl(VisionControl ctl)
        {
            ctl.LockDisplay();
            try
            {
                if (imgSrc != null && imgSrc.IsInitialized() && imgSrc.Key != IntPtr.Zero)
                {

                    HTuple num = 0;
                    HOperatorSet.CountObj(imgSrc, out num);
                    if (num > 0)//&& m_image.IsInitialized() && m_image.Key != IntPtr.Zero)
                    {
                        HOperatorSet.DispImage(imgSrc, ctl.GetHalconWindow());
                    }
                }
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
            }
            finally
            {
                ctl.UnlockDisplay();
            }
        }
        /// <summary>
        /// 初始化配置(模板)
        /// </summary>
        /// <returns></returns>
        public abstract bool InitConfig();  
        /// <summary>
        /// 处理当前缓存的图像并显示在指定视觉控件上
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public abstract bool ProcessImage(VisionControl ctl,params object[] paramList);
        /// <summary>
        /// 绑定一个视觉控件,自动流程时显示处理结果
        /// </summary>
        /// <param name="vc"></param>
        public void BindWindow(VisionControl vc)
        {
            m_visionControl = vc;
        }

        /// <summary>
        /// 绑定一个相机,自动流程时不需要再指定相机采集
        /// </summary>
        /// <param name="cb"></param>
        public void BindCamera(CameraBase cb)
        {
            m_Camera = cb;
        }

        /// <summary>
        /// 设置曝光值
        /// </summary>
        /// <param name="nExp"></param>
        public virtual void SetExposureTime(int nExp)
        {

        }

   

        /// <summary>
        /// 使用当前绑定的相机进行采集
        /// </summary>
        /// <returns></returns>
        public bool Snap()
        {
            if (m_Camera != null)
            {
                if (m_visionControl != null)
                {
                    m_visionControl.LockDisplay();
                }               
                if (m_Camera.Snap() != 0)
                {
                    //if (m_visionControl != null)  //控件图像区还未初始化时先初始化
                    //{
                    //    //todo
                    //    m_visionControl.DispImageFull(m_Camera.GetImage());
                    //}
                    imgSrc = m_Camera.GetImage();
                    if (m_visionControl != null)
                    {
                        m_visionControl.UnlockDisplay();
                    }                  
                    return true;
                }
            }
            if (m_visionControl != null)
            {
                m_visionControl.UnlockDisplay();
            }          
            return false;
        }

        /// <summary>
        /// 处理图像并显示在指定视觉控件上
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public bool Process(VisionControl ctl,params object[] paramList)
        {           
            ctl.RegisterUpdateInterface(this);
            return ProcessImage(ctl,paramList);
        }
        /// <summary>
        /// 处理图像并显示在绑定的视觉控件上
        /// </summary>
        /// <returns></returns>
        public virtual bool Process(params object[] paramList)
        {
            if (m_Camera != null)
            {
                if (m_visionControl != null)
                    m_visionControl.RegisterUpdateInterface(this);

                m_Camera.SetGrabParam("ExposureTimeAbs", m_ExposureTime);

                //设置增益
                if (m_bGainEnable)
                {
                    m_Camera.SetGrabParam("GainRaw", m_GainRaw);
                    m_Camera.SetGrabParam("DigitalShift", m_DigitalShift);
                }

                if (Snap())
                {

                    return ProcessImage(m_visionControl, paramList);
                }
                else
                {
                    VisionMgr.GetInstance().ShowLog(Name + " process snap fail ! ");
                    //WarningMgr.GetInstance().Error(Name + " process snap fail ! ");
                    WarningMgr.GetInstance().Error(ErrorType.Err_Vision_Snap,Name,
                        Name + " process snap fail ! ");

                    return false;
                }
            }
            VisionMgr.GetInstance().ShowLog(Name + " process camera null ! ");
            //WarningMgr.GetInstance().Error(Name + " process camera null ! ");
            WarningMgr.GetInstance().Error(ErrorType.Err_Vision_Process,Name,
                Name + " process camera null ! ");

            return false;
        }

        /// <summary>
        /// 获取当前的缓存图像
        /// </summary>
        /// <returns></returns>
        public HObject GetSrcImage()
        {
            return imgSrc;
        }
        /// <summary>
        /// 设置当前要处理的缓存图像
        /// </summary>
        /// <param name="img"></param>
        public void SetSrcImage(HObject img)
        {
            imgSrc = img;
        }
        /// <summary>
        /// 获取当前处理结果的窗口图像
        /// </summary>
        /// <returns></returns>
        public HObject GetWindowImage()
        {
            if (imgWindow == null)
            {
                HOperatorSet.GenEmptyObj(out imgWindow);
            }
            if (m_visionControl != null)
            {
                imgWindow.Dispose();
                HOperatorSet.DumpWindowImage(out imgWindow, m_visionControl.GetHalconWindow());
                return imgWindow;
            }
            return null;
        }
        public virtual bool LoadParam()
        {
            string strFile = VisionMgr.GetInstance().ConfigDir + "param.ini";

            string strTemp = IniOperation.GetStringValue(strFile, this.Name, "ExposureTime", "1000");
            m_ExposureTime = Convert.ToInt32(strTemp);

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "GainRaw", "10");
            m_GainRaw = Convert.ToInt32(strTemp);

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "DigitalShift", "1");
            m_DigitalShift = Convert.ToInt32(strTemp);

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "GainEnable", "0");
            m_bGainEnable = strTemp == "1";

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "OffsetU", "0");
            m_dbOffsetU = Convert.ToDouble(strTemp);

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "OffsetX", "0");
            m_dbOffsetX = Convert.ToDouble(strTemp);

            strTemp = IniOperation.GetStringValue(strFile, this.Name, "OffsetY", "0");
            m_dbOffsetY = Convert.ToDouble(strTemp);

            return true;
        }
        public virtual bool SaveParam()
        {
            string strFile = VisionMgr.GetInstance().ConfigDir + "param.ini";

            IniOperation.WriteValue(strFile, this.Name, "ExposureTime", m_ExposureTime.ToString());

            IniOperation.WriteValue(strFile, this.Name, "GainRaw", m_GainRaw.ToString());

            IniOperation.WriteValue(strFile, this.Name, "DigitalShift", m_DigitalShift.ToString());

            IniOperation.WriteValue(strFile, this.Name, "GainEnable", m_bGainEnable ? "1" : "0");

            IniOperation.WriteValue(strFile, this.Name, "OffsetU", m_dbOffsetU.ToString());

            IniOperation.WriteValue(strFile, this.Name, "OffsetX", m_dbOffsetX.ToString());

            IniOperation.WriteValue(strFile, this.Name, "OffsetY", m_dbOffsetY.ToString());

            return true;
        }
    }
    
}
