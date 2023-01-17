
//#define NO_EXPORT_APP_MAIN


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using CommonTool;
using AutoFrameDll;
using System.Windows.Forms;
using System.IO;

namespace AutoFrameVision
{
    /// <summary>
    /// 视觉处理步骤样板类
    /// </summary>
    public class Vision_T1:VisionBase
    {
        HDevelopExport hde = new HDevelopExport();

        HTuple ModelId;
        HTuple ModelData;

        HTuple m_BaseData;

        CaliTranslate m_CalibTrans = new CaliTranslate();

        /// <summary>
        /// 构造函数,初始化配置
        /// </summary>
        /// <param name="strName"></param>
        public Vision_T1(string strName):base(strName)
        {

        }

        /// <summary>
        /// 初始化配置(模板数据)
        /// </summary>
        /// <returns></returns>
        public override bool InitConfig()
        {
            if(ModelId != null)
                HOperatorSet.ClearShapeModel(ModelId);
            hde.InitTemplete(null, m_strDir,out  ModelId, out ModelData);

            LoadParam();

            //根据具体项目需要决定是否保存处理模板的数据
            //假如我们所需要的位置和角度都是基于模板计算得来，那么就需要模板数据
            HObject ho_ImageModel;
            HOperatorSet.GenEmptyObj(out ho_ImageModel);
            ho_ImageModel.Dispose();
            HOperatorSet.ReadImage(out ho_ImageModel, m_strDir + "/Model.bmp");

            hde.T1(null,ho_ImageModel, m_strDir, ModelId, ModelData, out m_BaseData);
            if (m_BaseData[0].L != 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(this.Name + "Template image processing failed", "Configuration load error");
                }
                else
                {
                    MessageBox.Show(this.Name + "模板图像处理失败", "配置加载出错");
                }
            }

            return true;
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <returns></returns>
        public override bool LoadParam()
        {
            string strCalibFile = VisionMgr.GetInstance().ConfigDir + "\\CalibT1\\CalibT1.cal";

            if (File.Exists(strCalibFile))
            {
                m_CalibTrans.LoadCaliData(strCalibFile);
            }

            return base.LoadParam();
        }

        /// <summary>
        /// 设定曝光值
        /// </summary>
        /// <param name="nExp"></param>
        public override void SetExposureTime(int nExp)
        {
            m_ExposureTime = nExp;
            if (m_Camera != null)
            {
                //第一次拍照要求暗,
                m_Camera.SetGrabParam("ExposureTimeAbs", m_ExposureTime);
            }
            
        }

        /// <summary>
        /// 处理当前图像,显示在指定的控件上
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        public override bool ProcessImage( VisionControl vc,params object[] paramList)
        {
            if (vc != null)
            {
                 vc.LockDisplay();        
                HDevWindowStack.Push(vc.GetHalconWindow());
                vc.DispImageFull(imgSrc);
            }

            bool bRet = false;

            try
            {
                HTuple data=0;
                hde.T1(vc.GetHalconWindow(),imgSrc, m_strDir, ModelId, ModelData, out data);
                if (data[0] == 1)
                {
                    double robX, robY;
                    m_CalibTrans.Translate(data[1].D, data[2].D, out robX, out robY);

                    //数据需要存储下来给T2_2用
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, data[1], false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_Y, data[2], false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_A, data[5], false);

                    

                    
                    bRet = true;
                }
                else
                {
                    //处理失败时，必须将无效数值写入数据区，防止使用上一次的数据
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, data[0], false);
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_Y, data[0], false); 
                    SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_A, data[0], true);

                    bRet =  false;
                }
            }
            catch (Exception e)
            {
                WarningMgr.GetInstance().Info(e.Message);
                System.Diagnostics.Debug.WriteLine(e.ToString());
                //处理失败时，必须将无效数值写入数据区，防止使用上一次的数据
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_X, VisionException, false);
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_Y, VisionException, false);
                SystemMgr.GetInstance().WriteRegDouble((int)SysFloatReg.T1_A, VisionException, true);

                bRet = false;
            }
            finally
            {
                if (vc != null)
                {
                    HDevWindowStack.Pop();
                     vc.UnlockDisplay();
               }
            }

            return bRet;
        }
    }
}
