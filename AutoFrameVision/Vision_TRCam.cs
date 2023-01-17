
//#define NO_EXPORT_APP_MAIN


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
    /// 视觉处理步骤样板类
    /// </summary>
    public class Vision_TRCam : VisionBase
    {
        HDevelopExport hde = new HDevelopExport();

        HTuple ModelId;
        HTuple ModelData;

        public bool OutLimit = false;
        public HTuple m_RowCenter = 0;
        public HTuple m_ColCenter = 0;
        public HTuple m_FixTool = 0;

        /// <summary>
        /// 构造函数,初始化配置
        /// </summary>
        /// <param name="strName"></param>
        public Vision_TRCam(string strName):base(strName)
        {

        }

        /// <summary>
        /// 初始化配置(模板数据)
        /// </summary>
        /// <returns></returns>
        public override bool InitConfig()
        {
            //HOperatorSet.GenEmptyObj(out xld);
            //hde.InitTemplete(out xld, "T1", out ModelId);
            //HOperatorSet.GetShapeModelContours(out xld, ModelId, 1);

           // hde.InitTemplete( m_strDir,out  ModelId, out ModelData);
            m_ExposureTime = 30000;
            return true;
        }

        /// <summary>
        /// 在显示控件变化时,用当前内容更新显示
        /// </summary>
        /// <param name="ctl"></param>
        public override void UpdateVisionControl(VisionControl ctl)
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
        /// 设定曝光值
        /// </summary>
        /// <param name="nExp"></param>
        public override void SetExposureTime(int nExp)
        {
            m_ExposureTime = nExp;
        }

        public override bool Process()
        {
            if (m_Camera != null)
            {
                if (m_visionControl != null)
                    m_visionControl.RegisterUpdateInterface(this);

                //m_Camera.SetGrabParam("ExposureTime", m_ExposureTime);
                if (Snap())
                {
                     return ProcessImage(m_visionControl);
                }
                else
                {
                    VisionMgr.GetInstance().ShowLog(Name + " process snap1 fail ! ");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 处理当前图像,显示在指定的控件上
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        public override bool ProcessImage( VisionControl vc)
        {
            
            if (vc != null)
            {
                HDevWindowStack.Push(vc.GetHalconWindow());
                vc.LockDisplay();        
                vc.DispImageFull(imgSrc);
            }
            try
            {
                HTuple data=0;
                if (this.Name == "T_RCamCreatModel")
                    hde.T_RCamCreatModel(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name== "T_RCamCreatROI")             
                    hde.T_RCamCreatROI(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_RCam")
                    hde.T_RCam(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);

                if (this.Name == "T_BandCreatModel_1")
                    hde.T_BandCreatModel_1(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_BandCreatROI_1")
                    hde.T_BandCreatROI_1(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Band_1")
                    hde.T_Band_1(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);

                if (this.Name == "T_Band_1CreatModel_Calib")
                    hde.T_Band_1CreatModel_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Band_1CreatROI_Calib")
                    hde.T_Band_1CreatROI_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Band_1_Calib")
                    hde.T_Band_1_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
              
                if (data[0] == 1)
                {

                    if (this.Name == "T_RCam")
                    {
                        if (data[1].D < SystemMgr.GetInstance().GetParamDouble("RCamPosXRangMin") ||
                            data[1].D > SystemMgr.GetInstance().GetParamDouble("RCamPosXRangMax") ||
                            data[2].D < SystemMgr.GetInstance().GetParamDouble("RCamPosYRangMin") ||
                            data[2].D > SystemMgr.GetInstance().GetParamDouble("RCamPosYRangMax") )
                 
                            OutLimit = true;
                        else
                            OutLimit = false;
                    }
                    if (this.Name == "T_Band_1")
                    {
                        if (data[1].D < SystemMgr.GetInstance().GetParamDouble("Band1PosXRangMin") ||
                            data[1].D > SystemMgr.GetInstance().GetParamDouble("Band1PosXRangMax") ||
                            data[2].D < SystemMgr.GetInstance().GetParamDouble("Band1PosYRangMin") ||
                            data[2].D > SystemMgr.GetInstance().GetParamDouble("Band1PosYRangMax"))

                            OutLimit = true;
                        else
                            OutLimit = false;
                    }

                    if (OutLimit)
                    {
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_X, 0, false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_Y, 0, false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, 0, false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_CruentAngle , 0, false);
                    }
                    else
                    {                  
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_X, data[1], false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_Y, data[2], false);
                        //SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, data[3], false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_CruentAngle, data[3], false);
                    }
                    //数据需要存储下来给T2_2用

                
                    return true;
                }
                else
                {

                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_X, 0, false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Cirl_Y, 0, false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, 0, false);

                    //处理失败时，必须将无效数值写入数据区，防止使用上一次的数据
                    //SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, data[0], false);
                    //SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_Y, data[0], false);             
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                //处理失败时，必须将无效数值写入数据区，防止使用上一次的数据
                //SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, VisionException, false);
                //SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_Y, VisionException, false);
                return false;
            }
            finally
            {
                if (vc != null)
                {
                    vc.UnlockDisplay();
                    HDevWindowStack.Pop();
                }
            }
        }
    }
}
