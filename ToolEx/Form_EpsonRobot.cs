using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communicate;
using System.Xml;
using CommonTool;

namespace ToolEx
{
    public partial class Form_EpsonRobot : Form
    {
        private Button[] btns_in;
        private Button[] btns_out;
        private string[] io_in;
        private string[] io_out;

        public Form_EpsonRobot()
        {
            InitializeComponent();

            InitDataGridView();
        }

        private void InitDataGridView()
        {

        }

        private void Form_EpsonRobot_Load(object sender, EventArgs e)
        {
            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {
            if (Security.IsOpMode())
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
            else
            {
                if (!tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Add(tabPage2);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
