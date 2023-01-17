using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;
using System.Runtime.InteropServices;

namespace AutoFrameVision
{
    /// <summary>
    /// 控件刷新接口,由当前占用该控件的类来负责刷新
    /// </summary>
    public interface IVisionControlUpdate
    {
        /// <summary>
        /// 界面刷新函数
        /// </summary>
        /// <param name="ctl"></param>
        void UpdateVisionControl(VisionControl ctl);
    //    bool ChangeValid();
    }

    /// <summary>
    /// 图像处理显示控件
    /// </summary>
    public partial class VisionControl : PictureBox
    {
        HTuple m_windowHandle = null;       //图像显示控件的句柄
        Point ptMouse;
        object imgLock = new object();
        IVisionControlUpdate m_IVisionControlUpdate = null;
        HWindow m_hwindow = null;
        int m_nWidth = 2592;
        int m_nHeight = 1944;

        public bool m_MoveEnable = true;
        /// <summary>
        /// 构造函数
        /// </summary>
        public VisionControl()
        {
            InitializeComponent();
            this.BackColor = System.Drawing.Color.MidnightBlue;
        }

        /// <summary>
        /// 初始化halcon窗口,分辨率为2592 * 1944
        /// </summary>
        /// 
   //     [DllImport("user32.dll")]static extern IntPtr GetWindowDC(IntPtr hWnd);
        public void InitWindow()
        {
            try
            {
               
      //          HOperatorSet.NewExternWindow(this.Handle, 0, 0, this.Width, this.Height, out m_windowHandle);
      //          HOperatorSet.SetWindowDc(m_windowHandle, GetWindowDC(this.Handle));
                HOperatorSet.OpenWindow(0, 0, this.Width, this.Height, this.Handle, "", "", out m_windowHandle);
                HOperatorSet.SetWindowParam(m_windowHandle, "background_color", "#000040");
                HOperatorSet.ClearWindow(m_windowHandle);
                HOperatorSet.SetPart(m_windowHandle, 0, 0, m_nHeight, m_nWidth);
                m_hwindow = new HWindow(m_windowHandle);
                
            }
            catch(HalconException HDevExpDefaultException1)
            {
                System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
            }

        }

        /// <summary>
        /// 关闭halcon窗口
        /// </summary>
        public void DeinitWindow()
        {
            if(isOpen())
            {
                HOperatorSet.CloseWindow(m_windowHandle);
            }
        }


        /// <summary>
        /// 注册新的显示接口
        /// </summary>
        /// <param name="vsu"></param>
        public void RegisterUpdateInterface(IVisionControlUpdate vsu)
        {
            if(m_IVisionControlUpdate != vsu)
            {
                m_IVisionControlUpdate = vsu;
            }
        }
        /// <summary>
        /// 判断当前halcon窗口是否已经打开
        /// </summary>
        /// <returns></returns>
        private bool isOpen()
        {
            return m_windowHandle != null ;
        }

        /// <summary>
        /// 获取当前的halcon句柄
        /// </summary>
        /// <returns></returns>
        public HTuple GetHalconWindow()
        {
            return m_windowHandle;
        }

        /// <summary>
        /// 锁定控件显示,其它线程不得进入
        /// </summary>
        public void LockDisplay()
        {
            System.Threading.Monitor.Enter(imgLock);
        }
        /// <summary>
        /// 解锁控件显示,其它线程可操作
        /// </summary>
        public void UnlockDisplay()
        {
            if (System.Threading.Monitor.IsEntered(imgLock))
            {
                System.Threading.Monitor.Exit(imgLock);
            }
        }

        /// <summary>
        /// 全屏显示图像
        /// </summary>
        /// <param name="img"></param>
        public void DispImageFull(HObject img)
        {
            if (!isOpen())
            {
                InitWindow();
            }
            HTuple hv_Width = null, hv_Height = null;
            HOperatorSet.GetImageSize(img, out hv_Width, out hv_Height);

            if (hv_Width != m_nWidth || hv_Height != m_nHeight && hv_Width != null)
            {
                m_nWidth = hv_Width;
                m_nHeight = hv_Height;
                HOperatorSet.SetPart(m_windowHandle, 0, 0, m_nHeight, m_nWidth);
            }
            HOperatorSet.DispImage(img, m_windowHandle);
        }

        /// <summary>
        /// 鼠标滚动时缩放图片大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (isOpen() && m_MoveEnable)
            {
                HTuple row, col, button;
                HTuple row0, col0, row1, col1;
                bool bUpdate = false;

                LockDisplay();
                try
                {

                    HOperatorSet.GetMposition(m_windowHandle, out row, out col, out button);
                    //Action<object> action = (object obj) =>
                    //{
                        HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);

                        HTuple width = col1 - col0;
                        HTuple height = row1 - row0;

                        //col = ((double)e.X / this.Width) * Width;
                        //row = ((double)e.X / this.Width) * Width;

                        float k = (float)width / m_nWidth;
                        if ((k < 50 && e.Delta < 0) || (k > 0.02 && e.Delta > 0))
                        {
                            HTuple Zoom;
                            if (e.Delta > 0)
                            {
                                Zoom = 1.3;
                            }
                            else
                            {
                                Zoom = 1 / 1.3;
                            }

                            HTuple r1 = (row0 + ((1 - (1.0 / Zoom)) * (row - row0)));
                            HTuple c1 = (col0 + ((1 - (1.0 / Zoom)) * (col - col0)));
                            HTuple r2 = r1 + (height / Zoom);
                            HTuple c2 = c1 + (width / Zoom);

                            HOperatorSet.SetPart(m_windowHandle, r1, c1, r2, c2);
                            //if(e.Delta < 0)
                            HOperatorSet.ClearWindow(m_windowHandle);
                            bUpdate = true;
                        }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
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
                    UnlockDisplay();
                }
                if (bUpdate && m_IVisionControlUpdate != null)
                {
                    //Action<object> action = (object obj) =>
                    //{
                        m_IVisionControlUpdate.UpdateVisionControl(this);
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
        }
        /// <summary>
        /// 鼠标按下时切换光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Focused == false)
                this.Focus();
            if(e.Button == MouseButtons.Left)
            {
                if(m_MoveEnable)
                    this.Cursor = Cursors.Hand;
                ptMouse.X = e.X;
                ptMouse.Y = e.Y;
            }
        }

        /// <summary>
        /// 平移图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isOpen())
            {
                if (this.Cursor == Cursors.Hand)
                {
                    //Action<object> action = (object obj) =>
                    //{
                        int x = e.X - ptMouse.X;
                        int y = e.Y - ptMouse.Y;

                        if (Math.Abs(x) > 2 || Math.Abs(y) > 2)
                        {
                            ptMouse.X = e.X;
                            ptMouse.Y = e.Y;
                            HTuple row0, col0, row1, col1;
                            bool bUpdate = false;
                            LockDisplay();
                            try
                            {
                                HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);
                                int zoom = (row1 - row0) / this.Height;
                                x *= zoom;
                                y *= zoom;

                                HOperatorSet.SetPart(m_windowHandle, row0 - y, col0 - x, row1 - y, col1 - x);
                                HOperatorSet.ClearWindow(m_windowHandle);
                                bUpdate = true;
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
                                UnlockDisplay();
                            }

                            if (bUpdate && m_IVisionControlUpdate != null)
                            {
                                m_IVisionControlUpdate.UpdateVisionControl(this);
                            }
                        }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
        }

        /// <summary>
        /// 鼠标右键按下时返回全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (isOpen())
                {
                    //Action<object> action = (object obj) =>
                    //{
                        bool bUpdate = false;
                        HTuple row0, col0, row1, col1;
                        LockDisplay();
                        try
                        {
                            HOperatorSet.GetPart(m_windowHandle, out row0, out col0, out row1, out col1);

                            if (row0 != 0 || col0 != 0 || col1 - col0 != m_nWidth || row1 - row0 != m_nHeight)
                            {
                                HOperatorSet.SetPart(m_windowHandle, 0, 0, m_nHeight, m_nWidth);
                                bUpdate = true;

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
                            UnlockDisplay();
                        }

                        if (bUpdate && m_IVisionControlUpdate != null)
                        {
                            m_IVisionControlUpdate.UpdateVisionControl(this);

                        }
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (this.Cursor == Cursors.Hand)
                    this.Cursor = Cursors.Arrow;
            }
        }

        private void VisionControl_MouseEnter(object sender, EventArgs e)
        {
            if (this.Focused == false)
                this.Focus();
            
        }

        private void VisionControl_MouseLeave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 控件缩放时自动调整halcon窗口的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionControl_SizeChanged(object sender, EventArgs e)
        {
            if (m_windowHandle != null)
            {             
                //Action<object> action = (object obj) =>
                //{
                    bool bUpdate = false;
                    LockDisplay();
                    try
                    {
                        HOperatorSet.SetWindowExtents(m_windowHandle, this.ClientRectangle.Y, this.ClientRectangle.X, this.ClientRectangle.Width, this.ClientRectangle.Height);
                        bUpdate = true;
                    }
                    catch (HalconException HDevExpDefaultException1)
                    {
                        System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());
                    }
                    catch (Exception exp)
                    {
                        System.Diagnostics.Debug.WriteLine(exp.ToString());
                    }
                    finally { UnlockDisplay(); }
                    if (bUpdate && m_IVisionControlUpdate != null)
                    {
                        m_IVisionControlUpdate.UpdateVisionControl(this);
                    }
                //};
                //Task t1 = new Task(action, "");
                //t1.Start();
                //t1.Wait();
            }
        }
    }
}
