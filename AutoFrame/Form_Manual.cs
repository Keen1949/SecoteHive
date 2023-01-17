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
using CommonTool;
using Communicate;
using System.Drawing.Text;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Manual : Form
    {
        public Form_Manual()
        {
            InitializeComponent();
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
        /// 在tab控件中增加一个tabPage选项卡,并把一个窗体添加进选项卡
        /// </summary>
        /// <param name="frm">窗体对象(一个工站界面)</param>
        private void add_page(Form frm)
        {
            TabPage tabPage = new TabPage(); //创建一个TabControl 中的单个选项卡页。
            tabPage.Text = frm.Text;
            tabPage.Name = frm.Text;
            tabPage.BackColor = frm.BackColor;
            tabPage.Font = frm.Font;

            tabControl_manual.TabPages.Add(tabPage);   //添加tabPage选项卡到tab控件

            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            frm.Parent = tabPage;

            tabPage.Controls.Add(frm);  //tabPage选项卡添加一个窗体对象 
            frm.Visible = false;

        }
        private void Form_Manual_Load(object sender, EventArgs e)
        {
            tabControl_manual.TabPages.Clear();

            add_page(new FormServer());
            add_page(new Form_Param());

            //根据配置动态添加tabPage选项卡
            foreach (KeyValuePair<Form, StationBase> kvp in StationMgr.GetInstance().m_dicFormStation)
            {
                add_page(kvp.Key);
            }
            tabControl_manual.SelectedIndex = 1;

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }

        /// <summary>
        /// tabPage点击选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_manual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_manual.SelectedTab != null && tabControl_manual.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl_manual.SelectedTab.Controls)
                {
                    if (item is Form)
                    {
                        if (item.Visible == false)
                            item.Visible = true;
                    }
                }
            }
        }

        private void tabControl_manual_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(this.ForeColor);
            Rectangle rect = tabControl_manual.GetTabRect(e.Index);

            //       g.TextRenderingHint = TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(tabControl_manual.Controls[e.Index].Text, this.Font, brush, rect, sf);
        }
    }
}
