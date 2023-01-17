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
    /// 视觉处理步骤样板类2
    /// </summary>
    public class VisionTemplete2:VisionBase
    {
        HDevelopExport hde = new HDevelopExport();

        HTuple ModelId;
        HTuple ModelData;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strName"></param>
        public VisionTemplete2(string strName):base(strName)
        {
        }

        /// <summary>
        /// 初始化配置(模板数据)
        /// </summary>
        /// <returns></returns>
        public override bool InitConfig()
        {
            hde.InitTemplete(m_strDir, out ModelId, out ModelData);
            return true;
        }
        /// <summary>
        /// 更新显示控件
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

                    HObject model;

                    HOperatorSet.GetShapeModelContours(out model, ModelId, 1);
                    HOperatorSet.DispObj(model, ctl.GetHalconWindow());

                    hde.disp_message(ctl.GetHalconWindow(), "test", "window", 100, 100, "red", "true");
                    //    HOperatorSet.DispObj(ModelContour, ctl.GetHalconWindow());
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
    /// 处理当前图像并显示在指定控件上
    /// </summary>
    /// <param name="vc"></param>
    /// <returns></returns>
    public override bool ProcessImage(VisionControl vc)
        {
            if (vc != null)
            {
                HDevWindowStack.Push(vc.GetHalconWindow());
                vc.LockDisplay();
                vc.DispImageFull(imgSrc);

            }


            //         hde.RunHalcon(window);
            HTuple data;

            hde.T2(imgSrc, m_strDir, ModelId, ModelData, out data);
            Random rnd1 = new Random();
            double x = rnd1.Next(9000, 10000) / 10.0;
            double y = rnd1.Next(9000, 10000) / 10.0;
            double z = rnd1.Next(9000, 10000) / 10.0;

            SystemMgr.GetInstance().WriteRegDouble(3, x, false);
            SystemMgr.GetInstance().WriteRegDouble(4, y, false);
            SystemMgr.GetInstance().WriteRegDouble(5, z, true);


            if (vc != null)
            {
                vc.UnlockDisplay();
                HDevWindowStack.Pop();
            }

            if (data.Length > 1)

                return true;
            else
                return false;
        }
    }
}
