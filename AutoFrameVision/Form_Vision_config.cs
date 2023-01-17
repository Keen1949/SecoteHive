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

namespace AutoFrameVision
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form_Vision_config : Form
    {
        private string m_strStepName = "";
         /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <returns></returns>
        public Form_Vision_config()
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

        public string StepName
        {
            get
            {
                return m_strStepName;
            }

            set
            {
                if (value != m_strStepName)
                {
                    m_strStepName = value;

                    InitUI();
                }
            }
        }

        private void InitUI()
        {
            try
            {
                groupBox_Step.Text = m_strStepName;

                VisionBase vb = VisionMgr.GetInstance().GetVisionBase(m_strStepName);

                if (vb != null)
                {
                    numericUpDown_ExposeTime.Value = vb.m_ExposureTime;
                    numericUpDown_Gain.Value = vb.m_GainRaw;
                    numericUpDown_DigitalShift.Value = vb.m_DigitalShift;
                    numericUpDown_U.Value = (decimal)vb.m_dbOffsetU;
                    numericUpDown_X.Value = (decimal)vb.m_dbOffsetX;
                    numericUpDown_Y.Value = (decimal)vb.m_dbOffsetY;

                    checkBox_Gain.Checked = vb.m_bGainEnable;

                    if (checkBox_Gain.Checked)
                    {
                        numericUpDown_Gain.Enabled = true;
                        numericUpDown_DigitalShift.Enabled = true;
                    }
                    else
                    {
                        numericUpDown_Gain.Enabled = false;
                        numericUpDown_DigitalShift.Enabled = false;
                    }
                }
            }
            catch (Exception exp)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(exp.Message, "Vision profile loading exception, please confirm whether the parameter configuration is correct or the UI control property is correct",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(exp.Message, "视觉配置文件加载异常，请确认参数配置是否正确，或UI控件属性是否正确",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        /// <summary>
        /// 初始化时根据视觉管理器配置添加各相机及步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Vision_config_Load(object sender, EventArgs e)
        {
            InitUI();

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;
        }


        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            if (Security.GetUserMode() < UserMode.Adjustor)
            {
                groupBox_Step.Enabled = false;
            }
            else
            {
                groupBox_Step.Enabled = true;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            VisionBase vb = VisionMgr.GetInstance().GetVisionBase(m_strStepName);
            vb.m_ExposureTime = (int)numericUpDown_ExposeTime.Value;
            vb.m_GainRaw = (int)numericUpDown_Gain.Value;
            vb.m_DigitalShift = (int)numericUpDown_DigitalShift.Value;
            vb.m_dbOffsetU = (double)numericUpDown_U.Value;
            vb.m_dbOffsetX = (double)numericUpDown_X.Value;
            vb.m_dbOffsetY = (double)numericUpDown_Y.Value;

            vb.m_bGainEnable = checkBox_Gain.Enabled;

            vb.SaveParam();
        }

        private void Form_Vision_config_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            e.Cancel = true;
        }

        private void checkBox_Gain_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Gain.Checked)
            {
                numericUpDown_Gain.Enabled = true;
                numericUpDown_DigitalShift.Enabled = true;
            }
            else
            {
                numericUpDown_Gain.Enabled = false;
                numericUpDown_DigitalShift.Enabled = false;
            }
        }
    }
}
