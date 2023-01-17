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
using ToolEx;
using System.IO;

namespace AutoFrame
{
    public partial class Form_Password : Form
    {
        public Form_Password()
        {
            InitializeComponent();
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                this.Text = ini.GetString(this.GetType().Name, this.GetType().Name, "修改密码");
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
            }
            else
            {
                ini.WriteString(this.GetType().Name, this.GetType().Name, this.Text);
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }

        private void Form_Password_Load(object sender, EventArgs e)
        {
            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);
            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;
        }

        private void OnModeChanged()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            this.BeginInvoke((MethodInvoker)delegate
            {
                switch (Security.GetUserMode())
                {
                    case UserMode.Administrator:
                        comboBox1.Items.Clear();
                        foreach(UserMode item in Enum.GetValues(typeof(UserMode)))
                        {
                            if (item != UserMode.Operator)
                            {
                                comboBox1.Items.Add(item.ToString());
                            }
                        }
                        button_OK.Enabled = true;
                        break;

                    case UserMode.Engineer:
                        comboBox1.Items.Clear();
                        foreach (UserMode item in Enum.GetValues(typeof(UserMode)))
                        {
                            if (item != UserMode.Operator && item != UserMode.Administrator)
                            {
                                comboBox1.Items.Add(item.ToString());
                            }
                        }
                        button_OK.Enabled = true;
                        break;

                    default:
                        button_OK.Enabled = false;
                        comboBox1.Items.Clear();
                        break;


                }
            });
            
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strUser = comboBox1.Text;
            string strPassword = textBox_Password.Text;

            UserMode user;

            if (Enum.TryParse(strUser,out user))
            {
                if (Security.ChangePassword(user,strPassword))
                {
                    string strFormat = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_1_1", "修改用户{0}的密码成功！");
                    string strMsg = String.Format(strFormat, user.ToString());

                    WarningMgr.GetInstance().Info(strMsg);

                    MessageBox.Show(strMsg);
                }
                else
                {
                    string strFormat = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_1_2", "修改用户{0}的密码失败！");
                    string strMsg = String.Format(strFormat, user.ToString());
                    WarningMgr.GetInstance().Info(strMsg);

                    MessageBox.Show(strMsg);
                }
            }

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
