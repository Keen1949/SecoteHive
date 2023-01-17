using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFrame
{
    public partial class Form_Input : Form
    {
        /// <summary>
        /// 获取输入文本
        /// </summary>
        public string Input
        {
            get
            {
                return textBox_Input.Text;
            }
        }

        /// <summary>
        /// 设置输入长度
        /// </summary>
        public int LimitLength
        {
            get;
            set;
        }


        public Form_Input()
        {
            InitializeComponent();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

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

        private void Form_Input_Load(object sender, EventArgs e)
        {
            textBox_Input.Focus();

            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!textBox_Input.Focused)
            {
                textBox_Input.Focus();
            }
        }

        private void textBox_Input_TextChanged(object sender, EventArgs e)
        {
            string text = textBox_Input.Text.Trim();

            if (text.Length >= LimitLength)
            {
                this.DialogResult = DialogResult.OK;

                this.Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;

                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
