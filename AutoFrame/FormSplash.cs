using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFrame
{
    public partial class FormSplash : Form
    {
        private static FormSplash m_instance;

        private static object m_lock = new object();

        public static FormSplash Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new FormSplash();
                        }
                    }
                }
                        
                return m_instance;
            }
        }

        public FormSplash()
        {
            InitializeComponent();
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            
            int w = System.Windows.Forms.SystemInformation.WorkingArea.Width;
            int h = System.Windows.Forms.SystemInformation.WorkingArea.Height;
            this.Location = new Point((w - this.Width)/2, (h - this.Height)/2);

            label_Msg.Text = "";
            label_Percent.Text = "0%";
        }

        public void ShowMsg(string strMsg,int nPercent)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    label_Msg.Text = strMsg;
                    label_Percent.Text = string.Format("{0}%", nPercent);
                    progressBar1.Value = nPercent;

                    if (nPercent == 100)
                    {
                        Thread.Sleep(1000);
                        this.Close();
                        m_instance = null;
                    }
                });
            }
            else
            {
                label_Msg.Text = strMsg;
                label_Percent.Text = string.Format("{0}%", nPercent);
                progressBar1.Value = nPercent;

                if (nPercent == 100)
                {
                    Thread.Sleep(1000);
                    this.Close();
                    m_instance = null;
                }
            }
        }

        public static void UpdateUI(string strMsg, int nPercent)
        {
            if (m_instance == null)
            {
                Task.Run(delegate
                {
                    //Application.Run(FormSplash.Instance);
                    FormSplash.Instance.ShowDialog();
                });

                Thread.Sleep(1000);
            }

            if (m_instance != null)
            {
                FormSplash.Instance.ShowMsg(strMsg, nPercent);
            }
        }
    }
}
