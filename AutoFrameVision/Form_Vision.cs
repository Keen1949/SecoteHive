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

namespace AutoFrameVision
{
    /// <summary>
    /// 自动显示的视觉界面
    /// </summary>
    public partial class Form_Vision : Form
    {

        private Form_Vision_config m_frmConfig;

        /// <summary>
        /// 构造函数,初始化系统中要用到的显示控件,关联视觉步骤的关系,以及日志显示控件
        /// </summary>
        public Form_Vision()
        {
            InitializeComponent();
            visionControl1.InitWindow();
            visionControl2.InitWindow();
            visionControl3.InitWindow();
            visionControl4.InitWindow();
            visionControl5.InitWindow();
            visionControl6.InitWindow();
            visionControl7.InitWindow();
            visionControl8.InitWindow();
            visionControl9.InitWindow();



            VisionMgr.GetInstance().BindWindow("T1", visionControl1);
            VisionMgr.GetInstance().BindWindow("T2", visionControl2);

            m_frmConfig = new Form_Vision_config();
            m_frmConfig.Owner = this;
            m_frmConfig.StartPosition = FormStartPosition.CenterScreen;

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
        /// 窗口加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Vision_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, VisionBase> vb in VisionMgr.GetInstance().m_dicVision)
            {
                comboBox_Step.Items.Add(vb.Key);
            }

            //2019-03-28 Binggoo 没有配置视觉步骤会报错
            if (comboBox_Step.Items.Count > 0)
            {
                comboBox_Step.SelectedIndex = 0;
            }       

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

            VisionMgr.GetInstance().SetLogListBox(listbox_log);
            VisionMgr.GetInstance().LogEvent += OnLogView;
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            if (Security.GetUserMode() < UserMode.Adjustor)
            {
                roundPanel_button.Enabled = false;

            }
            else
            {
                roundPanel_button.Enabled = true;

            }
        }
        public void OnLogView(Control ctrl, string strLog,LogLevel level = LogLevel.Info)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                ListBoxEx logListBox = ctrl as ListBoxEx;

                if (logListBox.Items.Count > 2000)
                {
                    logListBox.Clear();
                }

                Color color = logListBox.BackColor;

                switch (level)
                {
                    case LogLevel.Info:
                        color = logListBox.BackColor;
                        break;

                    case LogLevel.Warn:
                        color = Color.Yellow;
                        break;

                    case LogLevel.Error:
                        color = Color.Red;
                        break;

                }

                logListBox.Append(strLog, color, logListBox.ForeColor);

                logListBox.TopIndex = logListBox.Items.Count - (int)(logListBox.Height / logListBox.ItemHeight);

                if (SystemMgr.GetInstance().GetParamBool("SaveShowLogEnable"))
                {
                    WarningMgr.GetInstance().Info(strLog);
                }
            });
        }

        /// <summary>
        /// 清除全部日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Click(object sender, EventArgs e)
        {
            listbox_log.Items.Clear();
        }

        /// <summary>
        /// 显示手动调试窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_debug_Click(object sender, EventArgs e)
        {
                 Form_Vision_debug frm = new Form_Vision_debug();
                frm.ShowDialog(this);        
        }


        /// <summary>
        /// 删除指定项的日志显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delete_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection indices  = listbox_log.SelectedIndices;
            if (indices.Count > 0)
            {
                for (int n = indices.Count - 1; n >= 0; --n)
                {
                    listbox_log.Items.RemoveAt(indices[n]);
                }
            }
        }

        /// <summary>
        /// 调用标定对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cali_Click(object sender, EventArgs e)
        {
            Form_CaliNPoint cnp = new Form_CaliNPoint();
            cnp.ShowDialog(this);
        }

        /// <summary>
        /// 处理步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Step_Click(object sender, EventArgs e)
        {
            string strStepName = comboBox_Step.Text;

            VisionMgr.GetInstance().ProcessStep(strStepName);
        }

        private void button_Config_Click(object sender, EventArgs e)
        {
            m_frmConfig.Show();
        }

        private void comboBox_Step_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_frmConfig.StepName = comboBox_Step.Text;
        }
    }
}
