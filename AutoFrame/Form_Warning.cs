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
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Warning : Form
    {
        public Form_Warning()
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form_warning_Load(object sender, EventArgs e)
        {
            WarningMgr.GetInstance().WarningEventHandler += new EventHandler(OnWarning);//添加报警委托函数

            
            for(int i = 0; i<WarningMgr.GetInstance().Count; ++i)//添加当前报警类信息到表格
            {
                WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(i);
                dataGridView_current.Rows.Add(wd.tm.ToShortDateString(), wd.tm.ToLongTimeString(), wd.strCode, wd.strCategory, wd.strMsg, "0");
                SetRowColor(dataGridView_current.Rows[dataGridView_current.Rows.Count - 1], wd.strLevel);
            }

            timer_flash.Enabled = WarningMgr.GetInstance().HasErrorMsg();

            dataGridView_current.CurrentCell = null;
        }

        /// <summary>
        /// 设置表格行的颜色,区分错误和警告的颜色
        /// </summary>
        /// <param name="row">表格的行对象</param>
        /// <param name="str">错误或者警告字符串,用来区分表格颜色</param>
        private void SetRowColor(DataGridViewRow row, string str)
        {
            if (str == "error")
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 73, 0);
            else if (str == "warn")
                row.DefaultCellStyle.BackColor = Color.FromArgb(254, 246, 76);
        }

        /// <summary>
        /// 报警信息委托调用函数
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnWarning(object Sender, EventArgs e)
        {
            WarningEventData wed = (WarningEventData)e;
            if (wed.bAdd) //增加一条异常信息
            {
                if (WarningMgr.GetInstance().HasErrorMsg())
                {
                    WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(wed.nIndex);

                    dataGridView_current.Rows.Add(wd.tm.ToShortDateString(), wd.tm.ToLongTimeString(), wd.strCode, wd.strCategory, wd.strMsg, "0");
                    SetRowColor(dataGridView_current.Rows[dataGridView_current.Rows.Count - 1], wd.strLevel);
                    timer_flash.Enabled = true;
                }
            }
            else
            {
                if (wed.nIndex == -1)  //清除所有异常信息
                {
                    dataGridView_current.Rows.Clear();
                }
                else
                {
                    dataGridView_current.Rows.RemoveAt(wed.nIndex);
                }
              
                timer_flash.Enabled = dataGridView_current.Rows.Count > 0;
            }
        }

        /// <summary>
        /// 定时器,定时显示当前报警信息所持续的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_flash_Tick(object sender, EventArgs e)
        {
            int i = 0;
            for (int k = 0; k < WarningMgr.GetInstance().Count; ++k)
            {
                WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(k);
                if (i < dataGridView_current.Rows.Count)
                {
                    TimeSpan ts = DateTime.Now - wd.tm;
                    dataGridView_current.Rows[i].Cells[5].Value = ts.ToString(@"hh\:mm\:ss");
                    ++i;
                }
            }
        }

        /// <summary>
        /// 清除所有报警类信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundButton1_Click(object sender, EventArgs e)
        {
            WarningMgr.GetInstance().ClearAllWarning();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
