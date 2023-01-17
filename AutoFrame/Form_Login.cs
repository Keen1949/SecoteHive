using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoFrameDll;
using System.Xml;
using ToolEx;
using System.IO;


namespace AutoFrame
{

    public partial class Form_Login : Form
    {
        enum SpeedMode
        {
            Low,
            Mid,
            High,
        }

        private SpeedMode SpMode
        {
            get
            {
                IniHelper ini = new IniHelper();
                ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoFrame.ini");

                return (SpeedMode)ini.GetInt("Software", "SpeedMode", 2);
            }

            set
            {
                IniHelper ini = new IniHelper();
                ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoFrame.ini");

                ini.WriteInt("Software", "SpeedMode", (int)value);
            }
        }

        public Form_Login()
        {
            InitializeComponent();

            InitCoboBox();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

            switch (SpMode)
            {
                case SpeedMode.Low:
                    SystemMgr.GetInstance().SystemSpeed = 10;
                    radioButton_SpeedLow.Checked = true;
                    break;

                case SpeedMode.Mid:
                    SystemMgr.GetInstance().SystemSpeed = 50;
                    radioButton_SpeedMid.Checked = true;
                    break;

                case SpeedMode.High:
                    SystemMgr.GetInstance().SystemSpeed = 100;
                    radioButton_SpeedHigh.Checked = true;
                    break;

            }

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

        private void InitCoboBox()
        {
            comboBox_Name.Items.Clear();

            foreach (var item in Enum.GetNames(typeof(UserMode)))
            {
                comboBox_Name.Items.Add(item);
            }

            comboBox_Name.SelectedIndex = (int)Security.GetUserMode();
        }

        private void Form_Login_Load(object sender, EventArgs e)
        {
            WarningMgr.GetInstance().WarningEventHandler += new EventHandler(OnWarning);
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

            roundButton_Normal.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            roundButton_dry_run.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_calib.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_simulate.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_Grr.BaseColor = Color.FromArgb(220, 221, 224);

            OnWarning(this, EventArgs.Empty);
        }

        private void OnModeChanged()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                switch (Security.GetUserMode())
                {
                    case UserMode.Administrator:
                    case UserMode.Engineer:
                        roundButton_Engineering.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
                        roundButton_CPK.BaseColor = roundButton_production.BaseColor = Color.FromArgb(220, 221, 224);

                        roundButton_Normal.Visible = true;
                        roundButton_dry_run.Visible = false;
                        roundButton_calib.Visible = false;
                        roundButton_simulate.Visible = false;
                        roundButton_Grr.Visible = false;

                        roundButton_ChangePassword.Visible = true;

                        groupBox_SpeedMode.Enabled = true;

                        break;

                    case UserMode.Adjustor:
                        roundButton_CPK.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
                        roundButton_production.BaseColor = roundButton_Engineering.BaseColor = Color.FromArgb(220, 221, 224);
                        roundButton_Normal.Visible = true;
                        roundButton_dry_run.Visible = false;
                        roundButton_calib.Visible = false;
                        roundButton_simulate.Visible = false;
                        roundButton_Grr.Visible = false;

                        roundButton_ChangePassword.Visible = false;

                        groupBox_SpeedMode.Enabled = true;
                        break;

                    case UserMode.FAE:
                        roundButton_CPK.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
                        roundButton_production.BaseColor = roundButton_Engineering.BaseColor = Color.FromArgb(220, 221, 224);
                        roundButton_Normal.Visible = true;
                        roundButton_dry_run.Visible = false;
                        roundButton_calib.Visible = false;
                        roundButton_simulate.Visible = false;
                        roundButton_Grr.Visible = false;
                        roundButton_ChangePassword.Visible = false;

                        groupBox_SpeedMode.Enabled = false;
                        break;

                    default:
                        roundButton_production.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
                        roundButton_CPK.BaseColor = roundButton_Engineering.BaseColor = Color.FromArgb(220, 221, 224);

                        roundButton_Normal.Visible = false;
                        roundButton_dry_run.Visible = false;
                        roundButton_calib.Visible = false;
                        roundButton_simulate.Visible = false;
                        roundButton_Grr.Visible = false;
                        roundButton_ChangePassword.Visible = false;

                        groupBox_SpeedMode.Enabled = false;

                        break;


                }
            });

        }

        private void roundButton_login_Click(object sender, EventArgs e)
        {
            Form_Input frm = new Form_Input();
            frm.LimitLength = ProductMgr.GetInstance().WorkerLength;
            frm.StartPosition = FormStartPosition.CenterScreen;

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string strCode = textBox_Password.Text;

            int nSelectIndex = comboBox_Name.SelectedIndex;

            if (!Security.ChangeUserMode((UserMode)nSelectIndex, strCode))
            {
                Security.ChangeOpMode();
            }
            else
            {
                string strInput = frm.Input;

                ProductMgr.GetInstance().CurrentWorker = strInput;

                WarningMgr.GetInstance().Info("Current worker change to " + strInput);
            }

            textBox_Password.Text = "";
        }
        private void OnWarning(object Sender, EventArgs e)
        {
            string strX;
            string strY;
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                WARNING_DATA wd = WarningMgr.GetInstance().GetLastMsg();
                strX = "Alarm Time\n" + wd.tm.ToLongTimeString();
                strY = wd.strLevel + "\n" + wd.strCode;
            }
            else
            {
                strX = "No Alarm";
                strY = "Alarm Time";
            }
            if (roundButton_time.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { roundButton_time.Text = x.ToString(); };
                roundButton_time.BeginInvoke(actionDelegate, strX);
            }
            else
            {
                roundButton_time.Text = strX;
            }
            if (roundButton_alarm.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { roundButton_alarm.Text = x.ToString(); };
                roundButton_alarm.BeginInvoke(actionDelegate, strY);
            }
            else
            {
                roundButton_alarm.Text = strY;
            }
        }

        private void roundButton_auto_Click(object sender, EventArgs e)
        {
            if (StationMgr.GetInstance().IsAutoRunning() || StationMgr.GetInstance().IsPause())
            {
                return;
            }
            SystemMgr.GetInstance().ChangeMode(SystemMode.Normal_Run_Mode);
            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                sb.StationEnable = true;
            }
            roundButton_Normal.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            roundButton_dry_run.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_calib.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_simulate.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_Grr.BaseColor = Color.FromArgb(220, 221, 224);

        }

        private void roundButton_dry_run_Click(object sender, EventArgs e)
        {
            if (StationMgr.GetInstance().IsAutoRunning() || StationMgr.GetInstance().IsPause())
            {
                return;
            }
            SystemMgr.GetInstance().ChangeMode(SystemMode.Dry_Run_Mode);
            roundButton_Normal.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_dry_run.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            roundButton_calib.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_simulate.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_Grr.BaseColor = Color.FromArgb(220, 221, 224);
        }

        private void roundButton_calib_Click(object sender, EventArgs e)
        {
            Form_Select_Calib frm = new Form_Select_Calib();
            //   frm.Parent = this;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SystemMgr.GetInstance().ChangeMode(SystemMode.Calib_Run_Mode);
                roundButton_Normal.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_dry_run.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_calib.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
                roundButton_simulate.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_Grr.BaseColor = Color.FromArgb(220, 221, 224);
            }
        }

        private void roundButton_simulate_Click(object sender, EventArgs e)
        {
            SystemMgr.GetInstance().ChangeMode(SystemMode.Simulate_Run_Mode);
            roundButton_Normal.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_dry_run.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_calib.BaseColor = Color.FromArgb(220, 221, 224);
            roundButton_simulate.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            roundButton_Grr.BaseColor = Color.FromArgb(220, 221, 224);
        }

        private void roundButton_cover_Click(object sender, EventArgs e)
        {
            Form_Select_Grr frm = new Form_Select_Grr();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SystemMgr.GetInstance().ChangeMode(SystemMode.Other_Mode);
                roundButton_Normal.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_dry_run.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_calib.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_simulate.BaseColor = Color.FromArgb(220, 221, 224);
                roundButton_Grr.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            }

        }

        private void roundButton_ChangePassword_Click(object sender, EventArgs e)
        {
            Form_Password frm = new Form_Password();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Show();
        }

        private void radioButton_SpeedLow_CheckedChanged(object sender, EventArgs e)
        {
            SystemMgr.GetInstance().SystemSpeed = 10;
            SpMode = SpeedMode.Low;
        }

        private void radioButton_SpeedMid_CheckedChanged(object sender, EventArgs e)
        {
            SystemMgr.GetInstance().SystemSpeed = 50;
            SpMode = SpeedMode.Mid;
        }

        private void radioButton_SpeedHigh_CheckedChanged(object sender, EventArgs e)
        {
            SystemMgr.GetInstance().SystemSpeed = 100;
            SpMode = SpeedMode.High;
        }
    }
}
