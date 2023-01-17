using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CommonTool;
using ToolEx;
using System.Threading;

using HalconDotNet;

using OperationIni;

namespace AutoFrameVision
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form_Vision_debug : Form, IVisionControlUpdate
    {
        HObject m_image = null;      //当前图像缓存，只用于文件读取
        bool m_bRunThread = false;   //线程是否在运行中
        Thread m_thread = null;     //连续采集的线程       
        CameraBase m_camera = null; //当前选择的相机采集实例
        bool m_bPause = false;      //是否在暂停中
        bool m_bAutoTest = false;   //是否开启自动测试
        bool m_bBatch = false;      //是否在批量测试过程中

        

        private HTuple hv_Rows = new HTuple(), hv_Cols = new HTuple();
        private HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
        private HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
        private HTuple hv_Direct,  hv_ModelId ;
        //
        HObject ho_spoke, ho_circle, ho_TemplateImage , ho_ModelContours,ho_model_Roi=null;
        HTuple hv_RowCenter, hv_ColCenter, hv_Radius;
        private string[] polarity_items = { "所有", "黑到白", "白到黑" };
        private string[] pointSelect_items = { "所有点", "第一点", "最后点" };
        private string[] ID_items = { "Circle","Line"};
        private string m_pram_path = VisionMgr.GetInstance().ConfigDir;
        private string strAllType;
        private AutoResetEvent m_hEvent = new AutoResetEvent(false);
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <returns></returns>
        public Form_Vision_debug()
        {
            InitializeComponent();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language,true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }

        /// <summary>
        /// 初始化时根据视觉管理器配置添加各相机及步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Vision_debug_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, CameraBase> cb in VisionMgr.GetInstance().m_dicCamera)
            {
                comboBox_cam.Items.Add(cb.Key);
            }
            comboBox_cam.SelectedIndex = 0;

            foreach (KeyValuePair<string, VisionBase> vb in VisionMgr.GetInstance().m_dicVision)
            {
                comboBox_step.Items.Add(vb.Key);
            }
            comboBox_step.SelectedIndex = 0;

            HOperatorSet.GenEmptyObj(out m_image);
            //        textBox_dir.Text = SystemMgr.GetInstance().GetImagePath();
            this.BeginInvoke((MethodInvoker)delegate
            {
                visionControl1.InitWindow();
            });

            //strAllType = ReadIniString("pram_all", "all_step：", "", m_pram_path + comboBox_step.Text +@"\pram_path.ini");
            
            //ID_items = strAllType.Split(',');
            //comboBox1.Items.AddRange(ID_items);
            //C_Polarity.Items.AddRange(polarity_items);
            //C_PointSelect.Items.AddRange(pointSelect_items);
            //comboBox1.Text = ReadIniString("pram_path", "参数路径：", "", m_pram_path + comboBox_step.Text +@"\pram_path.ini");
            //Read_SetParm(comboBox1.Text);//读圆参数

            textBox1.Text = trackBar1.Value.ToString();
            textBox2.Text = trackBar2.Value.ToString();
            textBox3.Text = trackBar3.Value.ToString();
            comboBox2.SelectedIndex = 0;
            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;
            toolStripMenuItem1.Click += new EventHandler(this. button_Add_Create_Click);
            toolStripMenuItem2.Click += new EventHandler(Delete_Click);
        }


        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            if (Security.GetUserMode() >= UserMode.Engineer)
            {
                groupBox_cam.Enabled = true;
                groupBox_func.Enabled = true;
                button_cali.Enabled = true;
            }
            else
            {
                groupBox_cam.Enabled = false;
                groupBox_func.Enabled = false;
                button_cali.Enabled = false;
            }
        }
    



        /// <summary>
        /// 在视觉显示控件要求更新时,用当前内容更新它
        /// </summary>
        /// <param name="ctl"></param>
        public void UpdateVisionControl(VisionControl ctl)
        {
            HTuple num = 0;
            ctl.LockDisplay();
            try
            {
                if (m_image != null && m_image.IsInitialized() && m_image.Key != IntPtr.Zero)
                {
                    HOperatorSet.DispImage(m_image, ctl.GetHalconWindow());

                }

            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Diagnostics.Debug.WriteLine(HDevExpDefaultException1.ToString());

            }
            catch (AccessViolationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());

            }
            finally
            {
                ctl.UnlockDisplay();

            }
        }


        /// <summary>
        /// 捕捉一张图像并显示
        /// </summary>  
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_snap_Click(object sender, EventArgs e)
        {
            if (m_thread != null)
            {
                StopThread();
            }
            HObject img = VisionMgr.GetInstance().CameraSnap(comboBox_cam.Text);
            if (img != null)
            {
                m_image = img;
                if(m_bAutoTest)
                {
                    TestImage();
                }
                else
                {
                    visionControl1.RegisterUpdateInterface(this);
                    //Action<object> action = (object obj) =>
                    //{
                        visionControl1.DispImageFull(m_image);
                    //};
                    //Task t1 = new Task(action, "");
                    //t1.Start();
                    //t1.Wait();
                }
            }
        }

        /// <summary>
        /// 异步采集一幅图像并显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_grab_Click(object sender, EventArgs e)
        {
            StopThread();

            m_camera = VisionMgr.GetInstance().GetCam(comboBox_cam.Text);
            if (m_camera != null)
            {              
                button_grab.Enabled = false;
                button_batch_test.Enabled = false;
                button_stop_grab.Enabled = true;
                StartThread();
            }
        }

        /// <summary>
        /// 异步采集线程及批量处理线程
        /// </summary>
        void ThreadGrab()
        {
            if (m_camera != null && m_camera.Open())
            {
                while (m_bRunThread)
                {
                    if(m_bPause == false)
                    {
                        visionControl1.LockDisplay();
                        int n = m_camera.Grab();
                        visionControl1.UnlockDisplay();
                        if (n != 0)
                        {
                          
                            m_image = m_camera.GetImage();
                         
                            if (m_bAutoTest || m_bBatch)
                            {
                                TestImage();
                            }
                            else
                            {
                                visionControl1.RegisterUpdateInterface(this);
                                UpdateVisionControl(visionControl1);

                            }

                            if (n == -1)//采集完成,自动停止
                            {
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    this.button_batch_stop.PerformClick();
                                });
                                break;
                            }                              
                       //     visionControl1.DispImageFull(m_image);
                        } 
                                               
                    }
                    if(m_bBatch )
                    {
                        int n = 100;
                        this.Invoke((MethodInvoker)delegate
                        {
                            n = Convert.ToInt32(textBox_span.Text);
                        });

                        if (n==0)
                        {
                            m_hEvent.WaitOne();
                     
                        }
                        else
                        {
                            Thread.Sleep(n);
                        }
                        
                    }
                    else
                    {
                        Thread.Sleep(20);
                    }                    
                }
            }
            if (m_camera != null)
            {
                m_camera.StopGrab();
                m_camera = null;
            }
            return;
        }
        /// <summary>
        /// 开始异步采集或批量处理
        /// </summary>
        void StartThread()
        {
            if (m_thread == null)
            {
                m_hEvent.Reset();
                m_thread = new Thread(ThreadGrab);
                m_bRunThread = true;
                m_thread.Start();
            }
            if (m_thread.ThreadState != ThreadState.Running)
            {               
                m_thread.Start();
            }
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        void StopThread()
        {
            button_stop_grab.Enabled = false;
            button_batch_stop.Enabled = false;
            button_batch_pause.Enabled = false;

            button_batch_test.Enabled = true;
            button_grab.Enabled = true;
            m_bBatch = false;
            m_hEvent.Set();

            if (m_thread != null)
            {
                m_bRunThread = false;
                if (m_thread.Join(5000) == false)
                    m_thread.Abort();
                
                m_thread = null;
            }
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stop_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        /// <summary>
        /// 打开图像文件并显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_open_Click(object sender, EventArgs e)
        {          
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StopThread();
                if(m_image!=null)
                    m_image.Dispose();//释放图片内存
                HOperatorSet.ReadImage(out m_image, openFileDialog1.FileName);
                if (m_bAutoTest)
                {
                    TestImage();
                }
                else
                {
                    visionControl1.RegisterUpdateInterface(this);
                    visionControl1.DispImageFull(m_image);  
                }
            }
        }

        /// <summary>
        /// 当选择的相机改变时自动停止采集线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_cam_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopThread();            
        }

        /// <summary>
        /// 窗口关闭时停止线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Vision_debug_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
            //m_image.Dispose();
        }

        /// <summary>
        /// 保存当前图像至image_tmp目录下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            string strDir = SystemMgr.GetInstance().GetImagePath("\\image_temp");
            string strFile = strDir + "\\" + comboBox_cam.Text + DateTime.Now.ToString(" yyyyMMdd_HH_mm_ss");
            HOperatorSet.WriteImage(m_image, "bmp", 0, strFile);
        }

        /// <summary>
        /// 用当前图像和当前选中的图像步骤测试图像算法并显示
        /// </summary>
        void TestImage()
        {
            if (m_image != null)
            {
             //   try
                {
             //       GC.Collect();
                    HTuple num = 0;
                    HOperatorSet.CountObj(m_image, out num);
             
                    if (num > 0)//&& m_image.IsInitialized() && m_image.Key != IntPtr.Zero)
                    {
                        string strStep = string.Empty;
                        this.Invoke((MethodInvoker)delegate
                        {
                            strStep = comboBox_step.Text;
                        });

                        VisionMgr.GetInstance().ProcessImage(strStep, m_image, visionControl1);
                    }
                }
             //   catch { }
            }
        }

        /// <summary>
        /// 测试当前图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_test_Click(object sender, EventArgs e)
        {
            StopThread();
            TestImage();
        }
        /// <summary>
        /// 重新选择批量处理图像的目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_path_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_dir.Text = folderBrowserDialog1.SelectedPath;
                textBox_dir.SelectionStart = textBox_dir.TextLength;
            }
        }
        /// <summary>
        /// 利用后台线程开始批量测试图像算法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_test_batch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox_dir.Text) == false && Directory.Exists(textBox_dir.Text))
            {
                StopThread();
                m_camera = new CameraFile(textBox_dir.Text);
                if (m_camera != null)
                {
                    button_batch_stop.Enabled = true;
                    button_batch_pause.Enabled = true;
                    button_grab.Enabled = false;
                    button_batch_test.Enabled = false;

                    m_bBatch = true;
                    StartThread();
                }
            }
        }

        /// <summary>
        /// 暂停批量测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_batch_pause_Click(object sender, EventArgs e)
        {
            if(button_batch_pause.Text == "暂停测试")
            {
                m_bPause = true;
                button_batch_pause.Text = "继续测试";
            }
            else
            {
                m_bPause = false;
                button_batch_pause.Text = "暂停测试";
            }
        }

        /// <summary>
        /// 停止批量测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_batch_stop_Click(object sender, EventArgs e)
        {
            StopThread();       
        }

        /// <summary>
        /// 切换自动测试模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_auto_Click(object sender, EventArgs e)
        {
            m_bAutoTest = !m_bAutoTest;
            checkBox_auto.Checked = m_bAutoTest;

        }

        /// <summary>
        /// 显示标定对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_light_Click(object sender, EventArgs e)
        {
            Form_CaliNPoint cnp = new Form_CaliNPoint();
            cnp.ShowDialog(this);
        }

        private void Save_Pram_bt_Click(object sender, EventArgs e)
        {
            string str1 = "参数保存失败!";
            string str2 = "参数保存成功!";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Parameter save failed!";
                str2 = "parameter saved successfully!";
            }

            bool b_txt = (C_Rule.text != "" & C_RuleHeiht.text != "" & C_RuleWith.text != "" & C_Smooth.text != "" & C_Threshold.text != "" & C_Polarity.Text != "" & C_PointSelect.Text != "");
            if (b_txt != true)
            {
                MessageBox.Show(str1);
                return;
            }

            Write_SetParm(comboBox1.Text);//写参数
            MessageBox.Show(str2);
        }
        private void Read_SetParm(string path)
        {
            try
            {
                string pram_path = m_pram_path + comboBox_step.Text;
                if (Directory.Exists(pram_path + "\\" + path))
                {
                    C_Rule.text = ReadIniString("Parm", "卡尺数量：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_RuleHeiht.text = ReadIniString("Parm", "卡尺长度：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_RuleWith.text = ReadIniString("Parm", "卡尺宽度：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_Threshold.text = ReadIniString("Parm", "边缘阀值：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_Smooth.text = ReadIniString("Parm", "平滑系数：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_Polarity.Text = ReadIniString("Parm", "极性：", "", pram_path + "\\" + path + @"\Parm.ini");
                    C_PointSelect.Text = ReadIniString("Parm", "点选择：", "", pram_path + "\\" + path + @"\Parm.ini");
                }
                HOperatorSet.ReadTuple(pram_path + "\\" + path + @"\hv_Rows.tup", out hv_Rows);
                HOperatorSet.ReadTuple(pram_path + "\\" + path + @"\hv_Cols.tup", out hv_Cols);
                if(comboBox1.Text.Substring(0,4)=="Circ")
                    HOperatorSet.ReadTuple(pram_path + "\\" + path + @"\hv_Direct.tup", out hv_Direct);
                DataGridView();//数据刷新
            }
            catch (HalconException)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Parameter load failed！", "Warning！");
                }
                else
                {
                    MessageBox.Show("参数加载失败！", "警告！");
                }
            }
        }
        private string ReadIniString(string section, string key, string noText, string iniFilePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            InitFile.ReadString(section, key, noText, temp, 1024, iniFilePath);
            return temp.ToString();
        }
        private void Write_SetParm(string path)
        {
            try
            {
                string pram_path = m_pram_path  + comboBox_step.Text;
                if (!Directory.Exists(pram_path + "\\" + path))
                {
                    Directory.CreateDirectory(pram_path + "\\" + path);//创建参数文件夹
                }
                WriteIniString("Parm", "卡尺数量：", C_Rule.text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "卡尺长度：", C_RuleHeiht.text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "卡尺宽度：", C_RuleWith.text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "边缘阀值：", C_Threshold.text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "平滑系数：", C_Smooth.text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "极性：", C_Polarity.Text, pram_path + "\\" + path + @"\Parm.ini");
                WriteIniString("Parm", "点选择：", C_PointSelect.Text, pram_path + "\\" + path + @"\Parm.ini");

                HOperatorSet.WriteTuple(hv_Rows, pram_path + "\\" + path + @"\hv_Rows.tup");
                HOperatorSet.WriteTuple(hv_Cols, pram_path + "\\" + path + @"\hv_Cols.tup");
                if(comboBox1.Text.Substring(0, 4) == "Circ")
                    HOperatorSet.WriteTuple(hv_Direct, pram_path + "\\" + path + @"\hv_Direct.tup");
                WriteIniString("pram_path", "参数路径：", comboBox1.Text, pram_path + "\\" + "pram_path.ini");
            }
            catch (HalconException)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Parameter save failed！", "Warning！");
                }
                else
                {
                    MessageBox.Show("参数保存失败！", "警告！");
                }
            }
        }

        private void WriteIniString(string section, string key, string val, string iniFilePath)
        {
            InitFile.WriteString(section, key, val, iniFilePath);
        }

        private void Fit_circle_bt_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Substring(0, 4) == "Circ")
                Fit_circle();
            if (comboBox1.Text.Substring(0, 4) == "Line")
                Fit_Line();
        }
        private void Fit_Line()
        {
            try
            {         
            string strPram;
            HOperatorSet_Ex.Read_SetParm(m_pram_path + comboBox_step.Text, comboBox1.Text, out strPram, out hv_Rows, out hv_Cols, out hv_Direct);
            HOperatorSet_Ex.Fit_Line(m_image, visionControl1.GetHalconWindow(), strPram, hv_Rows, hv_Cols, 
                     out hv_RowBegin, out hv_ColBegin,out hv_RowEnd, out hv_ColEnd);
                dataGridView2.Columns.Clear();
                dataGridView2.Columns.Add("", "Row");
                dataGridView2.Columns.Add("", "Col");
                dataGridView2.Rows.Add(1);//加载row
                ;//row表头ID
                dataGridView2.Rows[0].HeaderCell.Value = "点1";//row表头
                dataGridView2.Rows[1].HeaderCell.Value = "点2";//row表头
                ;//表格赋值
                string str_RowCenter, str_ColCenter;
                str_RowCenter = hv_RowBegin.TupleString("0.3f");//表格赋值col(0)
                str_ColCenter = hv_ColBegin.TupleString("0.3f"); ;//表格赋值col(2)

                dataGridView2.Rows[0].Cells[0].Value = str_RowCenter; ;//表格赋值col(0)
                dataGridView2.Rows[0].Cells[1].Value = str_ColCenter; ;//表格赋值col(1)

                str_RowCenter = hv_RowEnd.TupleString("0.3f");//表格赋值col(0)
                str_ColCenter = hv_ColEnd.TupleString("0.3f"); ;//表格赋值col(2)

                dataGridView2.Rows[1].Cells[0].Value = str_RowCenter; ;//表格赋值col(0)
                dataGridView2.Rows[1].Cells[1].Value = str_ColCenter; ;//表格赋值col(1)
            }
            catch(Exception)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Search failed, please adjust the parameter value!", "Warning！");
                }
                else
                {
                    MessageBox.Show("查找失败，请调整参数值！", "警告！");
                }
            }
        }
        private void Fit_circle()
        {
            try
            { 
                string strPram;
                HOperatorSet_Ex.Read_SetParm(m_pram_path + comboBox_step.Text, comboBox1.Text, out strPram, out hv_Rows, out hv_Cols, out hv_Direct);
                HOperatorSet_Ex.Fit_Circle(m_image, visionControl1.GetHalconWindow(),  strPram,  hv_Rows,  hv_Cols,  hv_Direct,
                        out hv_RowCenter, out hv_ColCenter, out hv_Radius);
                dataGridView2.Columns.Clear();
                dataGridView2.Columns.Add("", "RowCenter");
                dataGridView2.Columns.Add("", "ColCenter");
                dataGridView2.Columns.Add("", "Radius");
                dataGridView2.Rows.Add(1);//加载row
                //row表头ID
                dataGridView2.Rows[0].HeaderCell.Value = "ID" + (0);//row表头
                //表格赋值
                string str_RowCenter, str_ColCenter, str_Radius;
                str_RowCenter = hv_RowCenter.TupleString("0.3f");//表格赋值col(0)
                str_ColCenter = hv_ColCenter.TupleString("0.3f"); ;//表格赋值col(2)
                str_Radius = hv_Radius.TupleString("0.3f"); ;//表格赋值col(2)
                dataGridView2.Rows[0].Cells[0].Value = str_RowCenter; ;//表格赋值col(0)
                dataGridView2.Rows[0].Cells[1].Value = str_ColCenter; ;//表格赋值col(1)
                dataGridView2.Rows[0].Cells[2].Value = str_Radius; ;//表格赋值col(2)
            }
            catch(Exception)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Search failed, please adjust the parameter value!", "Warning！");
                }
                else
                {
                    MessageBox.Show("查找失败，请调整参数值！", "警告！");
                }
            }
        }


        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Read_SetParm(comboBox1.Text);//读圆参数
        }

        private void button_Modle_Create_Click(object sender, EventArgs e)
        {
            string str1 = "是否确定从新制作模板？";
            string str2 = "1、画1个区域，击右键确认";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Are you sure you want to make a new template? ";
                str2 = "1. Draw an area and right-click to confirm";
            }

            if (ho_model_Roi ==null)
            { 
                 if (DialogResult.No == MessageBox.Show(str1, "", MessageBoxButtons.YesNo))
                     return;
            }
            HTuple row, col, phi, length1, length2;
            
            HObject Rectangle2, Rectangle2xld;
            visionControl1.m_MoveEnable = false;
            HOperatorSet.SetColor(visionControl1.GetHalconWindow(), "red");
            HOperatorSet_Ex.disp_message(visionControl1.GetHalconWindow(), str2, "window", 12, 12, "red", "false");
            Button btn = (Button)sender;
            if (btn.Text == "画矩形" || btn.Text == "Rectangle")
            { 
                HOperatorSet.DrawRectangle2(visionControl1.GetHalconWindow(), out row, out col, out phi, out length1, out length2);
                HOperatorSet.GenRectangle2(out Rectangle2, row, col, phi, length1, length2);
                HOperatorSet.GenRectangle2ContourXld(out Rectangle2xld, row, col, phi, length1, length2);
                HOperatorSet.DispObj(Rectangle2xld, visionControl1.GetHalconWindow());
            }
            else
            {
                HOperatorSet.DrawCircle(visionControl1.GetHalconWindow(), out row, out col, out phi);
                HOperatorSet.GenCircle(out Rectangle2, row, col, phi);
                HOperatorSet.GenCircleContourXld(out Rectangle2xld, row, col, phi, (new HTuple(-0)).TupleRad(), (new HTuple(360)).TupleRad(), "positive",1);
                HOperatorSet.DispObj(Rectangle2xld, visionControl1.GetHalconWindow());
            }
            if(0==comboBox2.SelectedIndex && ho_model_Roi !=null)
                HOperatorSet.Union2(ho_model_Roi, Rectangle2, out ho_model_Roi);
            if(1 == comboBox2.SelectedIndex && ho_model_Roi != null)
                HOperatorSet.Intersection(ho_model_Roi, Rectangle2, out ho_model_Roi);
            if(2 == comboBox2.SelectedIndex && ho_model_Roi != null)
                HOperatorSet.Difference(ho_model_Roi, Rectangle2, out ho_model_Roi);
            if (ho_model_Roi == null)
                ho_model_Roi = Rectangle2;
            HOperatorSet.ReduceDomain(m_image, ho_model_Roi, out ho_TemplateImage);
            visionControl1.m_MoveEnable = true;

            HOperatorSet.CreateScaledShapeModel(
               ho_TemplateImage,
               "auto",
               (new HTuple(0)).TupleRad(),
               (new HTuple(360)).TupleRad(),
               "auto",
               0.95,
               1.05,
               "auto",
               (new HTuple("none")).TupleConcat("no_pregeneration"),
               "use_polarity",
               ((new HTuple(trackBar1.Value)).TupleConcat(trackBar2.Value)).TupleConcat(trackBar3.Value),
               "auto",
               out hv_ModelId);

            HOperatorSet_Ex.get_shape_model_contour_ref(ho_TemplateImage, out ho_ModelContours, hv_ModelId);
            HOperatorSet.DispObj(m_image, visionControl1.GetHalconWindow());
            HOperatorSet.SetColor(visionControl1.GetHalconWindow(), "green");
            HOperatorSet.DispObj(ho_ModelContours, visionControl1.GetHalconWindow());
            HOperatorSet.SetDraw(visionControl1.GetHalconWindow(), "margin");
            HOperatorSet.DispObj(ho_model_Roi, visionControl1.GetHalconWindow());
            //HObject modelimage, modelRios;

            //HOperatorSet.InspectShapeModel(ho_TemplateImage, out modelimage, out modelRios, 1, 30);

            //HOperatorSet.DispObj(modelRios, visionControl1.GetHalconWindow());

        }

        private void button_setExposure_Click(object sender, EventArgs e)
        {
            int index = comboBox_step.SelectedIndex;
            if (textBox_exposure.Text == "")
                return;
            int nExp = Convert.ToInt32(textBox_exposure.Text);
            string strStep = comboBox_step.Text;

            VisionMgr.GetInstance().WriteExposureTime(strStep, nExp);
            VisionMgr.GetInstance().GetCam(comboBox_cam.Text).SetGrabParam("ExposureTimeAbs", nExp);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string str1 = "是否确定删除当前步骤？";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Are you sure you want to delete the current step? ";
            }

            if (DialogResult.No == MessageBox.Show(str1, "", MessageBoxButtons.YesNo))
                return;
            string returnStr = comboBox1.Text;
            comboBox1.Items.Remove(returnStr);
            comboBox1.SelectedIndex = 0;
            comboBox1.Text = comboBox1.Items[0].ToString();
            strAllType = "";
            foreach (string s in comboBox1.Items)
            {
                strAllType += s+",";
            }
            strAllType = strAllType.Substring(0, strAllType.Length - 1);
            WriteIniString("pram_all", "all_step：", strAllType, m_pram_path + comboBox_step.Text + @"\pram_path.ini");
            WriteIniString("pram_path", "参数路径：", comboBox1.Text, m_pram_path + comboBox_step.Text + @"\pram_path.ini");
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Space)
            {
                int nInternal = 0;
                int.TryParse(textBox_span.Text, out nInternal);
                if (m_bBatch && nInternal == 0)
                {
                    m_hEvent.Set();

                    return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        private void Form_Vision_debug_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                int nInternal = 0;
                int.TryParse(textBox_span.Text, out nInternal);
                if (m_bBatch && nInternal == 0)
                {
                    m_hEvent.Set();
                }
            }
            
        }

        private void button_Add_Create_Click(object sender, EventArgs e)
        {
            string str1 = "提示输入Line或Circle开头的步骤名";
            string str2 = "标题";
            string str3 = "步骤名重复！请重新设置！！";
            string str4 = "请输入Line或Circle开头的步骤名！";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Prompt for step name beginning with line or circle";
                str2 = "Title";
                str3 = "Duplicate step name! Please reset!! ";
                str4 = "Please enter the step name starting with line or circle! ";
            }

            string returnStr = Microsoft.VisualBasic.Interaction.InputBox(str1, str2, 
                comboBox1.Text+"_"+ comboBox1.Items.Count.ToString(), 300, 300);
            foreach(string s in comboBox1.Items)
            {
                if (returnStr.Equals(s))
                {
                    MessageBox.Show(str3);
                    return;
                }
                  
            }
                
            if (returnStr.Substring(0, 4).Equals("Line") || returnStr.Substring(0, 4).Equals("Circ"))
            {                
                comboBox1.Items.Add(returnStr);
                comboBox1.Text = returnStr;
                strAllType += ",";
                strAllType += returnStr;
                WriteIniString("pram_all", "all_step：", strAllType, m_pram_path + comboBox_step.Text + @"\pram_path.ini");
             }
            else
            {
                MessageBox.Show(str4);
            }
        }

        private void textBox_exposure_KeyPress(object sender, KeyPressEventArgs e)
        {
           if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar2.Value)
                trackBar2.Value = trackBar1.Value;
            textBox1.Text = trackBar1.Value.ToString();
            textBox2.Text = trackBar2.Value.ToString();
            textBox3.Text = trackBar3.Value.ToString();
            HOperatorSet.CreateScaledShapeModel(
               ho_TemplateImage,
               "auto",
               (new HTuple(0)).TupleRad(),
               (new HTuple(360)).TupleRad(),
               "auto",
               0.95,
               1.05,
               "auto",
               (new HTuple("none")).TupleConcat("no_pregeneration"),
               "use_polarity",
               ((new HTuple(trackBar1.Value)).TupleConcat(trackBar2.Value)).TupleConcat(trackBar3.Value),
               "auto",
               out hv_ModelId);

            HOperatorSet_Ex.get_shape_model_contour_ref(ho_TemplateImage, out ho_ModelContours, hv_ModelId);
            HOperatorSet.DispObj(m_image, visionControl1.GetHalconWindow());
            HOperatorSet.DispObj(ho_ModelContours, visionControl1.GetHalconWindow());
        }

        private void button_Model_Save_Click(object sender, EventArgs e)
        {
            HOperatorSet.WriteShapeModel(hv_ModelId, m_pram_path + comboBox_step.Text + @"\Model.shm");
            HOperatorSet.WriteImage(m_image, "bmp", 0, m_pram_path + comboBox_step.Text + @"\Model");
            ho_model_Roi = null;
            VisionBase vb = null;
            if (VisionMgr.GetInstance().m_dicVision.TryGetValue(comboBox_step.Text, out vb))
            {
                if (vb != null)
                    vb.InitConfig();
            }
        }

        private void comboBox_step_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strStep = comboBox_step.Text;
            string strExp = "";
            VisionMgr.GetInstance().ReadExposureTime(strStep, out strExp);
            textBox_exposure.Text = strExp;

            VisionBase vb = VisionMgr.GetInstance().GetVisionBase(strStep);

            if (vb != null)
            {
                Type type = vb.GetType();
                if (type.Name == typeof(Vision_Std).Name)
                {
                    strAllType = ReadIniString("pram_all", "all_step：", "", m_pram_path + comboBox_step.Text + @"\pram_path.ini");

                    ID_items = strAllType.Split(',');
                    comboBox1.Items.AddRange(ID_items);
                    C_Polarity.Items.AddRange(polarity_items);
                    C_PointSelect.Items.AddRange(pointSelect_items);
                    comboBox1.Text = ReadIniString("pram_path", "参数路径：", "", m_pram_path + comboBox_step.Text + @"\pram_path.ini");
                    Read_SetParm(comboBox1.Text);//读圆参数
                }
            }
        }

        private void DataGridView()
        {
            string str_Rows, str_Cols;
            dataGridView1.Columns.Clear();//
            dataGridView1.Columns.Add("", "Rows");//
            dataGridView1.Columns.Add("", "Cols");//

            for (int i = 0; i < hv_Rows.Length; i++)
            {
                dataGridView1.Rows.Add(1);//加载row
                ;//row表头ID
                dataGridView1.Rows[i].HeaderCell.Value = "ID" + (i);//row表头
                ;//表格赋值
                str_Rows = hv_Rows.TupleSelect(i).TupleString("0.3f");//表格赋值col(0)
                str_Cols = hv_Cols.TupleSelect(i).TupleString("0.3f"); ;//表格赋值col(1)
                dataGridView1.Rows[i].Cells[0].Value = str_Rows; ;//表格赋值col(0)
                dataGridView1.Rows[i].Cells[1].Value = str_Cols; ;//表格赋值col(1)
            }
        }
        /// <summary>
        /// 设置ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw_circle_bt_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text.Substring(0, 4) == "Circ")
                Draw_circle();
            if (comboBox1.Text.Substring(0, 4) == "Line")
                Draw_line();
            if (comboBox1.Text == "Rectangle2")
                Draw_Rectangle2();
        }
        private void Draw_line()
        {
            string str1 = "设定1参数加载失败！";
            string str2 = "警告！";
            string str3 = "请加载图片！";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Setting 1 parameter loading failed!";
                str2 = "Warning! ";
                str3 = "Please load the picture! ";
            }

            bool b_txt = (C_Rule.text != "" && C_RuleHeiht.text != "" &&
            C_RuleWith.text != "" && C_Smooth.text != "" &&
            C_Threshold.text != "" && C_Polarity.Text != "" && C_PointSelect.Text != "");
            if (b_txt != true)
            {
                MessageBox.Show(str1, str2);
                return;
            }
            if (m_image != null)
            {
                if (ho_spoke != null)
                {
                    ho_spoke.Dispose();//释放内存
                    ho_spoke = null;
                }
                if (ho_circle != null)
                {
                    ho_circle.Dispose();//释放内存
                    ho_circle = null;
                }
                visionControl1.m_MoveEnable = false;
                HObject ho_Regions;
                HOperatorSet.SetColor(visionControl1.GetHalconWindow(), "red");
                //HSystem.SetSystem("flush_graphic", "true"); //显示 一定要加

                HTuple hv_Row1, hv_Row2, hv_Col1, hv_Col2;
                HOperatorSet_Ex.draw_rake(out ho_Regions, visionControl1.GetHalconWindow(),//hv_WindowHandle,
                    int.Parse(C_Rule.text),
                    int.Parse(C_RuleHeiht.text),
                    int.Parse(C_RuleWith.text),
                    out hv_Row1, out hv_Col1,
                    out hv_Row2, out hv_Col2);
                HOperatorSet.SetColored(visionControl1.GetHalconWindow(), 3);
                HOperatorSet.DispObj(ho_Regions, visionControl1.GetHalconWindow());
                //HSystem.SetSystem("flush_graphic", "false"); //显示 一定要加
                hv_Rows = new HTuple();
                hv_Cols = new HTuple();
                hv_Rows[0] = hv_Row1;
                hv_Rows = hv_Rows.TupleConcat(hv_Row2);
                hv_Cols[0] = hv_Col1;
                hv_Cols = hv_Cols.TupleConcat(hv_Col2);
                DataGridView();//数据刷新
                visionControl1.m_MoveEnable = true;
            }
            else
            {
                MessageBox.Show(str3, str2);
            }
        }
        private void Draw_Rectangle2()
        {

        }
        /// <summary>
        /// 画圆
        /// </summary>
        private void Draw_circle()
        {
            string str1 = "设定1参数加载失败！";
            string str2 = "警告！";
            string str3 = "请加载图片！";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Setting 1 parameter loading failed!";
                str2 = "Warning! ";
                str3 = "Please load the picture! ";
            }

            bool b_txt = (C_Rule.text != "" && C_RuleHeiht.text != "" &&
            C_RuleWith.text != "" && C_Smooth.text != "" &&
            C_Threshold.text != "" && C_Polarity.Text != "" && C_PointSelect.Text != "");
            if (b_txt != true)
            {
                MessageBox.Show(str1, str2);
                return;
            }
            if (m_image != null)
            {
                if (ho_spoke != null)
                {
                    ho_spoke.Dispose();//释放内存
                    ho_spoke = null;
                }
                if (ho_circle != null)
                {
                    ho_circle.Dispose();//释放内存
                    ho_circle = null;
                }
                visionControl1.m_MoveEnable = false;
                HObject ho_Regions;
                HOperatorSet.SetColor(visionControl1.GetHalconWindow(),"red");
                HOperatorSet_Ex.draw_spoke(m_image, out ho_Regions, visionControl1.GetHalconWindow(),
                    int.Parse(C_Rule.text),
                    int.Parse(C_RuleHeiht.text),
                    int.Parse(C_RuleWith.text),
                    out hv_Rows, out hv_Cols,
                    out hv_Direct);         
                HOperatorSet.SetColored(visionControl1.GetHalconWindow(), 3);
                HOperatorSet.DispObj(ho_Regions, visionControl1.GetHalconWindow());
                DataGridView();//数据刷新
                visionControl1.m_MoveEnable = true;
            }
            else
            {
                MessageBox.Show(str3, str2);
            }
        }
    }
}
