
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
    public class Vision_TBucket : VisionBase
    {
        HDevelopExport hde = new HDevelopExport();

        HTuple ModelId;
        HTuple ModelData;
     

        public HTuple m_RowCenter = 0;
        public HTuple m_ColCenter = 0;
        public HTuple m_FixTool = 0;
        public bool OutLimit = false;
        /// <summary>
        /// 构造函数,初始化配置
        /// </summary>
        /// <param name="strName"></param>
        public Vision_TBucket(string strName):base(strName)
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
                
                if (this.Name == "T_BucketCreatModel")
                    hde.T_BucketCreatModel(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_BucketCreatROI")
                    hde.T_BucketCreatROI(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Bucket")
                    hde.T_Bucket(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);

                if (this.Name == "T_BucketCreatModel_Calib")
                    hde.T_BucketCreatModel_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_BucketCreatROI_Calib")
                    hde.T_BucketCreatROI_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Bucket_Calib")
                    hde.T_Bucket_Calib(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);

                if (this.Name == "T_BandCreatModel_2")
                    hde.T_BandCreatModel_2(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_BandCreatROI_2")
                    hde.T_BandCreatROI_2(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);
                if (this.Name == "T_Band_2")
                    hde.T_Band_2(imgSrc, m_strDir, ModelId, ModelData, out data, out m_FixTool);

                if (data[0] == 1)
                {
                
                    if (this.Name == "T_Bucket")
                    {
                        if (data[1].D < SystemMgr.GetInstance().GetParamDouble("BucketPosXRangMin")||
                            data[1].D > SystemMgr.GetInstance().GetParamDouble("BucketPosXRangMax")||
                            data[2].D < SystemMgr.GetInstance().GetParamDouble("BucketPosYRangMin")||
                            data[2].D > SystemMgr.GetInstance().GetParamDouble("BucketPosYRangMax")||
                            data[3].D < SystemMgr.GetInstance().GetParamDouble("BucketAngleRangMin") ||
                            data[3].D > SystemMgr.GetInstance().GetParamDouble("BucketAngleRangMax") )

                            OutLimit = true;
                        else
                            OutLimit = false;
                    }
                    if (this.Name == "T_Band_2")
                    {
                        if (data[1].D < SystemMgr.GetInstance().GetParamDouble("Band2PosXRangMin") ||
                            data[1].D > SystemMgr.GetInstance().GetParamDouble("Band2PosXRangMax") ||
                            data[2].D < SystemMgr.GetInstance().GetParamDouble("Band2PosYRangMin") ||
                            data[2].D > SystemMgr.GetInstance().GetParamDouble("Band2PosYRangMax") ||
                            data[3].D < SystemMgr.GetInstance().GetParamDouble("Band2AngleRangMin") ||
                            data[3].D > SystemMgr.GetInstance().GetParamDouble("Band2AngleRangMax"))

                            OutLimit = true;
                        else
                            OutLimit = false;
                    }

                    if (OutLimit)
                    {
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_X, 0, false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_Y, 0, false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_A, 0, false);
                   
                    }
                     else
                    {
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_X, data[1], false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_Y, data[2], false);
                        SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_A, data[3], false);

                    }   
                
                    
                    return true;
                }
                else
                {

                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_X, 0, false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_Y, 0, false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_A, 0, false);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_X, 0, false);
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_Y, 0, false);
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.Bucket_A, 0, false);
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
