using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;
using AutoFrameUI;
using AutoFrameVision;
using AutoFrameDll;
using System.Threading;
using ToolEx;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace AutoFrame
{
    public partial class Form_Main : Form
    {
        Dictionary<RoundButton, Form> m_dicForm = new Dictionary<RoundButton, Form>();
        Form m_currentForm = null;
        RoundButton m_currentButton = null;

        const string NAME = "Secote Automation赛腾自动化科技";

        private PerformanceCounter m_CpuCounter;
        private PerformanceCounter m_RamCounter;

        /// <summary>
        /// 关闭应用程序事件
        /// </summary>
        //static public event EventHandler CloseProgrmEventHandler = null;

        public Form_Main()
        {
            InitializeComponent();
            try
            {
                //2020-03-23 Binggoo 加入CPU使用率显示
                string strProcessName = Process.GetCurrentProcess().ProcessName;
                m_CpuCounter = new PerformanceCounter("Process", "% Processor Time", strProcessName);
                m_RamCounter = new PerformanceCounter("Process", "Working Set - Private", strProcessName);

            }
            catch (Exception)
            {
                
            }
           }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (SystemMgr.GetInstance().GetParamBool("FullScreen"))
            {
                this.SetVisibleCore(false);
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.SetVisibleCore(true);
            }

            //关联页面和站位
            m_dicForm.Add(RoundButton_Auto, new Form_Auto());
            m_dicForm.Add(RoundButton_Manual, new Form_Manual());
            m_dicForm.Add(RoundButton_Vision, new Form_MachineDataConfig());
            m_dicForm.Add(RoundButton_Alarm, new Form_Alarm());
            m_dicForm.Add(RoundButton_Data, new Form_Data());
            m_dicForm.Add(RoundButton_Machine, new Form_Machine());
            m_dicForm.Add(RoundButton_File, new Form_File());
            m_dicForm.Add(RoundButton_Image, new Form_Image());
            m_dicForm.Add(RoundButton_Login, new Form_Login());
            //初始化页面属性
            foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
            {
                kp.Value.TopLevel = false;
                kp.Value.Parent = this.panel_main;
                kp.Value.Dock = DockStyle.Fill;
            }
            //显示主页面
            RoundButton_Auto.PerformClick();
            //修改标题栏
            this.Text = NAME + "－" + ProductMgr.GetInstance().DeviceName
                + "－" + ProductMgr.GetInstance().DeviceID;
            this.Text += " - 版本V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text += " - BLD:" + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
            //主界面添加报警信息委托,有报警时修改背景色提示
            WarningMgr.GetInstance().WarningEventHandler += new EventHandler(OnWarning);
            OnWarning(this, EventArgs.Empty);
            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;
            //添加站位状态变化响应函数操作
            StationMgr.GetInstance().StateChangedEvent += OnStationStateChanged;
            toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateAuto", "设备自动运行中");

            toolStripStatusLabel_SystemSpeed.Text = SystemMgr.GetInstance().SystemSpeed.ToString() + "%";
            try
            {
                toolStripStatusLabel_CPU.Text = "CPU: " + m_CpuCounter.NextValue().ToString("F1") + "%";

                double ram = m_RamCounter.NextValue();
                string level = "B";

                ByteConvert(ref ram, ref level);

                toolStripStatusLabel_Memery.Text = string.Format("RAM: {0:F1} {1}", ram, level);

            }
            catch (Exception)
            {
                
            }     
            

            OnProductInfoChanged();

            SystemMgr.GetInstance().StateChangedEvent += OnSystemStateChanged;
            ProductMgr.GetInstance().ProductInfoChangedEvent += OnProductInfoChanged;
            SystemMgr.GetInstance().SystemParamChangedEvent += OnSystemParamChanged;

            this.WindowState = FormWindowState.Maximized;

            button_LangSwitch.ImageIndex = LanguageMgr.GetInstance().LanguageID;

            LanguageMgr.GetInstance().ChangeLanguage(LanguageMgr.GetInstance().Language);

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

        }

        private void OnSystemParamChanged(string strParam, object oldValue, object newValue)
        {
            if (strParam == "SystemSpeed")
            {
                toolStripStatusLabel_SystemSpeed.Text = newValue.ToString() + "%";

                //2020-04-27 判断系统运行速度，超过50%，强制开启安全门
                if ((SystemMgr.GetInstance().GetParamBool("AutoSafeDoorMointor")) && (MotionMgr.GetInstance().GetSpeedPercent() >= 0.5))
                {
                    SystemMgr.GetInstance().SetParamBool("SafetyDoor", true);
                }
            }
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this.statusStrip1, ini);

                OnSystemStateChanged(SystemMgr.GetInstance().Mode);

                OnModeChanged();

                switch (StationMgr.GetInstance().CurrentState)
                {
                    case StationState.STATE_MANUAL:  //手动状态 
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateManual", "设备停止运行中");
                        break;
                    case StationState.STATE_AUTO:   //自动运行状态
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateAuto", "设备自动运行中");
                        break;
                    case StationState.STATE_READY:  //等待开始
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateReady", "设备准备就绪，请启动！");
                        break;
                    case StationState.STATE_EMG:         //急停状态
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateEmg", "设备异常急停，请检查！");
                        break;
                    case StationState.STATE_PAUSE:       //暂停状态
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StatePause", "设备暂停！");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this.statusStrip1, ini);
            }
        }

        private void OnProductInfoChanged()
        {
            //修改标题栏
            this.Text = NAME + "－" + ProductMgr.GetInstance().DeviceName
                + "－" + ProductMgr.GetInstance().DeviceID;
            this.Text += " - 版本V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text += " - BLD:" + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();

            toolStripStatusLabel_product_type.Text = ProductMgr.GetInstance().DeviceName;

            RoundButton_Machine.Text = ProductMgr.GetInstance().DeviceName;
        }

        /// <summary>
        /// 定时器,在状态栏上以秒为最小单位实时显示时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            toolStripStatusLabel_Time.Text = s;
            try
            {
                //2020-03-23 Binggoo 加入CPU和内存显示
                toolStripStatusLabel_CPU.Text = "CPU: " + m_CpuCounter.NextValue().ToString("F1") + "%";

                double ram = m_RamCounter.NextValue();
                string level = "B";

                ByteConvert(ref ram, ref level);

                toolStripStatusLabel_Memery.Text = string.Format("RAM: {0:F1} {1}", ram, level);

            }
            catch (Exception)
            {
                
            }
            
            SystemMgr.GetInstance().CheckSystemIdle();

            //2020-04-27 Binggoo 加入安全门打开时背景闪烁提示
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                this.BackColor = Color.FromArgb(250, 215, 214);
                foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
                {
                    kp.Value.BackColor = Color.FromArgb(250, 215, 214);
                }
            }
            else
            {
                if (IoMgr.GetInstance().IsSafeDoorOpen() && !SystemMgr.GetInstance().GetParamBool("SafetyDoor") && SystemMgr.GetInstance().GetParamBool("AutoSafeDoorMointor"))
                {
                    if (this.BackColor != Color.Yellow)
                    {
                        this.BackColor = Color.Yellow;
                    }
                    else
                    {
                        this.BackColor = Color.FromArgb(255, 255, 255);
                    }

                    foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
                    {
                        if (kp.Value.BackColor != Color.Yellow)
                        {
                            kp.Value.BackColor = Color.Yellow;
                        }
                        else
                        {
                            kp.Value.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
                else
                {
                    if (this.BackColor == Color.Yellow)
                    {
                        this.BackColor = Color.FromArgb(255, 255, 255);
                    }

                    foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
                    {
                        if (kp.Value.BackColor == Color.Yellow)
                        {
                            kp.Value.BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }

            }
        }

        private void OnSystemStateChanged(SystemMode Mode)
        {
            if (Mode == SystemMode.Normal_Run_Mode)
            {
                toolStripStatusLabel_system_mode.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "NormalRunMode", "正常运行模式");
            }
            else if (Mode == SystemMode.Dry_Run_Mode)
            {
                toolStripStatusLabel_system_mode.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "DryRunMode", "空跑运行模式");
            }
            else if (Mode == SystemMode.Calib_Run_Mode)
            {
                toolStripStatusLabel_system_mode.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "AutoCalibMode", "自动标定模式");
            }
            else if (Mode == SystemMode.Simulate_Run_Mode)
            {
                toolStripStatusLabel_system_mode.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "SimulateRunMode", "模拟运行模式");
            }
            else if (Mode == SystemMode.Other_Mode)
            {
                toolStripStatusLabel_system_mode.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "OtherMode", "GRR运行模式");
            }

        }
        private void OnStationStateChanged(StationState OldState, StationState NewState)
        {
            switch (NewState)
            {
                case StationState.STATE_MANUAL:  //手动状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        RoundButton_Start.ImageIndex = 11;
                        RoundButton_Pause.ImageIndex = 12;
                        RoundButton_Stop.ImageIndex = 14;
                        toolStripStatusLabel_current_state.ForeColor = System.Drawing.SystemColors.MenuHighlight;
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateManual", "设备停止运行中");
                        //亮红灯并蜂鸣
                        IoMgr.GetInstance().AlarmLight(LightState.黄灯开);
                        //IoMgr.GetInstance().AlarmLight(LightState.蜂鸣关);
                    });
                    break;
                case StationState.STATE_AUTO:   //自动运行状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        RoundButton_Start.ImageIndex = 10;
                        RoundButton_Pause.ImageIndex = 13;
                        RoundButton_Stop.ImageIndex = 15;
                        toolStripStatusLabel_current_state.ForeColor = Color.Green;
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateAuto", "设备自动运行中");
                        IoMgr.GetInstance().AlarmLight(LightState.绿灯开);

                        //2020-04-27 判断系统运行速度，超过50%，强制开启安全门
                        if (MotionMgr.GetInstance().GetSpeedPercent() >= 0.5 && SystemMgr.GetInstance().GetParamBool("AutoSafeDoorMointor"))
                        {
                            SystemMgr.GetInstance().SetParamBool("SafetyDoor", true);
                        }


                        //         IoMgr.GetInstance().AlarmLight(LightState.绿灯闪);
                        //         IoMgr.GetInstance().AlarmLight(LightState.蜂鸣关);
                    });
                    break;
                case StationState.STATE_READY:  //等待开始
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        toolStripStatusLabel_current_state.ForeColor = Color.Green;
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateReady", "设备准备就绪，请启动！");
                        IoMgr.GetInstance().AlarmLight(LightState.绿灯开);
                        //IoMgr.GetInstance().AlarmLight(LightState.蜂鸣关);
                    });
                    break;
                case StationState.STATE_EMG:         //急停状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        toolStripStatusLabel_current_state.ForeColor = Color.Red;
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StateEmg", "设备异常急停，请检查！");
                        //亮红灯并蜂鸣
                        IoMgr.GetInstance().AlarmLight(LightState.红灯开 | LightState.蜂鸣开);
                        //IoMgr.GetInstance().AlarmLight(LightState.蜂鸣开);
                    });
                    break;
                case StationState.STATE_PAUSE:       //暂停状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        RoundButton_Start.ImageIndex = 11;
                        RoundButton_Pause.ImageIndex = 12;
                        RoundButton_Stop.ImageIndex = 15;
                        toolStripStatusLabel_current_state.ForeColor = Color.DarkGreen;
                        toolStripStatusLabel_current_state.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "StatePause", "设备暂停！");
                        IoMgr.GetInstance().AlarmLight(LightState.绿灯闪);
                        //IoMgr.GetInstance().AlarmLight(LightState.蜂鸣关);
                    });
                    break;
                default:
                    break;
            }
            //  string strInfo = string.Format(OldState.ToString() + "change to " + NewState.ToString());

        }


        private void OnModeChanged()
        {
            string strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserOP", "操作员");
            switch (Security.GetUserMode())
            {
                case UserMode.Operator:
                    strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserOP", "操作员");
                    break;

                case UserMode.FAE:
                    strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserFAE", "FAE");
                    break;

                case UserMode.Adjustor:
                    strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserAdjustor", "调试员");
                    break;

                case UserMode.Engineer:
                    strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserEng", "工程师");
                    break;

                case UserMode.Administrator:
                    strUserName = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "UserAdmin", "管理员");
                    break;
            }

            this.BeginInvoke((MethodInvoker)delegate
            {
                this.toolStripStatusLabel_user.Text = strUserName;
            });

        }

        /// <summary>
        /// 点击自动界面按钮,显示自动界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Auto_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Auto);
        }

        /// <summary>
        /// 显示手动界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Manual_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Manual);
        }

        /// <summary>
        /// 显示相机界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Vision_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Vision);
        }

        /// <summary>
        /// 显示报警界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Alarm_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Alarm);
        }

        /// <summary>
        /// 显示数据界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Data_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Data);
        }

        /// <summary>
        /// 显示机台编号信息界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Machine_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Machine);
        }

        /// <summary>
        /// 开始自动流程,如果是暂停则恢复运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Start_Click(object sender, EventArgs e)
        {
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_0_1", "设备存在异常未处理，暂不能运行");
                string strCaption = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "异常提示", "异常提示");
                MessageBox.Show(strMsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (StationMgr.GetInstance().IsPause())
            {
                StationMgr.GetInstance().ResumeAllStation();
                RoundButton_Start.ImageIndex = 10;
                RoundButton_Pause.ImageIndex = 13;
                RoundButton_Stop.ImageIndex = 15;
            }
            else if (false == StationMgr.GetInstance().IsAutoRunning())
            {
                //2020-02-18 Binggoo 清除日志
                StationMgrEx.GetInstance().ClearAllLog();
                if (StationMgr.GetInstance().StartRun())
                {
                    RoundButton_Start.ImageIndex = 10;
                    RoundButton_Pause.ImageIndex = 13;
                    RoundButton_Stop.ImageIndex = 15;
                }
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Pause_Click(object sender, EventArgs e)
        {
            return;//禁用暂停
            if (StationMgr.GetInstance().AllowPause())
            {
                StationMgr.GetInstance().PauseAllStation();
                RoundButton_Start.ImageIndex = 11;
                RoundButton_Pause.ImageIndex = 12;
                RoundButton_Stop.ImageIndex = 15;
            }
        }

        /// <summary>
        /// 结束自动流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Stop_Click(object sender, EventArgs e)
        {
            StationMgr.GetInstance().StopRun();
            RoundButton_Start.ImageIndex = 11;
            RoundButton_Pause.ImageIndex = 12;
            RoundButton_Stop.ImageIndex = 14;

            StationMgr.GetInstance().StopManualRun();
        }

        /// <summary>
        /// 显示相关文件信息界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_File_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_File);
        }

        /// <summary>
        /// 显示图片显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Image_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Image);
        }

        /// <summary>
        /// 显示用户登录界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundButton_Login_Click(object sender, EventArgs e)
        {
            SwitchWnd(RoundButton_Login);
        }

        /// <summary>
        /// 关闭程序,清空报警类数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">附带数据的对象</param>
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (StationMgr.GetInstance().IsAutoRunning())
                StationMgr.GetInstance().StopRun();


            StationMgr.GetInstance().StopManualRun();

            WarningMgr.GetInstance().ClearAllWarning();

            foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
            {
                kp.Value.Close();
            }

        }

        /// <summary>
        /// 报警信息委托调用函数,改变整体对话框界面的颜色
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnWarning(object Sender, EventArgs e)
        {
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                this.BackColor = Color.FromArgb(250, 215, 214);
                foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
                {
                    if (kp.Value.InvokeRequired)
                    {
                        Action<Color> actionDelegate = (x) => { kp.Value.BackColor = x; };
                        kp.Value.BeginInvoke(actionDelegate, Color.FromArgb(250, 215, 214));
                    }
                    else
                        kp.Value.BackColor = Color.FromArgb(250, 215, 214);
                }
            }
            else
            {
                this.BackColor = Color.FromArgb(255, 255, 255);
                foreach (KeyValuePair<RoundButton, Form> kp in m_dicForm)
                {
                    if (kp.Value.InvokeRequired)
                    {
                        Action<Color> actionDelegate = (x) => { kp.Value.BackColor = x; };
                        kp.Value.BeginInvoke(actionDelegate, Color.FromArgb(255, 255, 255));
                    }
                    else
                        kp.Value.BackColor = Color.FromArgb(255, 255, 255);
                }
            }
        }

        /// <summary>
        /// 根据权限来登录点击的界面
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="btn"></param>
        /// <param name="bPopBox"></param>
        /// <returns></returns>
        public void SwitchWnd(RoundButton btn)
        {
            if (m_currentButton != btn)
            {

                if (m_currentButton != null)
                    m_currentButton.ImageIndex--;
                m_currentButton = btn;
                m_currentButton.ImageIndex++;

                if (m_currentForm != null)
                    m_currentForm.Hide();
                if (m_currentForm != m_dicForm[btn])
                {
                    m_currentForm = m_dicForm[btn];
                    m_currentForm.Show();
                }
            }
        }

        private void RoundButton_Machine_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Security.IsEngMode())
                {
                    Form_MachineMgr frm = new Form_MachineMgr();
                    frm.Show();
                }

            }
        }

        private void button_LangSwitch_Click(object sender, EventArgs e)
        {
            if (button_LangSwitch.ImageIndex == 0)
            {
                LanguageMgr.GetInstance().ChangeLanguage("en-US");

                button_LangSwitch.ImageIndex = 1;
            }
            else
            {
                LanguageMgr.GetInstance().ChangeLanguage("zh-CN");

                button_LangSwitch.ImageIndex = 0;
            }
        }

        private void button_LangSwitch_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Security.IsAdminMode())
                {
                    if (MessageBox.Show("To rewrite the language configuration files, do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        LanguageMgr.GetInstance().ChangeLanguage("zh-CN", true);

                        button_LangSwitch.ImageIndex = 0;

                        LanguageMgr.GetInstance().ChangeLanguage("zh-CN", false);
                    }
                }

            }
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                //应用程序要求关闭窗口
                case CloseReason.ApplicationExitCall:
                    //e.Cancel = false; //不拦截，响应操作
                    break;
                //自身窗口上的关闭按钮
                case CloseReason.FormOwnerClosing:
                //MDI窗体关闭事件
                case CloseReason.MdiFormClosing:
                //用户通过UI关闭窗口或者通过Alt+F4关闭窗口
                case CloseReason.UserClosing:

                    if (MessageBox.Show("The application will be closed,do you want to continue?", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        e.Cancel = true;//拦截，不响应操作
                    }
                    break;
                //不明原因的关闭
                case CloseReason.None:
                    break;
                //任务管理器关闭进程
                case CloseReason.TaskManagerClosing:
                    //e.Cancel = false;//不拦截，响应操作
                    break;

                //操作系统准备关机
                case CloseReason.WindowsShutDown:
                    //e.Cancel = false;//不拦截，响应操作
                    break;
                default:
                    break;
            }

            ProductMgr.GetInstance().Job.SaveData();
        }

        private void ByteConvert(ref double v, ref string level)
        {
            switch (level)
            {
                case "B":
                    if (v / 1024 > 1)
                    {
                        level = "KB";
                        v /= 1024;
                        ByteConvert(ref v, ref level);
                    }
                    break;

                case "KB":
                    if (v / 1024 > 1)
                    {
                        level = "MB";
                        v /= 1024;
                        ByteConvert(ref v, ref level);
                    }
                    break;

                case "MB":
                    if (v / 1024 > 1)
                    {
                        level = "GB";
                        v /= 1024;
                        ByteConvert(ref v, ref level);
                    }
                    break;
            }
        }
    }
}

