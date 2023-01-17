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
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace ToolEx
{
    /// <summary>
    /// 机器人管理界面
    /// </summary>
    public partial class Form_RobotMgr : Form
    {
        private ComboBox comboBox_ActiveLevelIoIn = new ComboBox();
        private ComboBox comboBox_EnableIoIn = new ComboBox();

        private ComboBox comboBox_ActiveLevelIoOut = new ComboBox();
        private ComboBox comboBox_EnableIoOut = new ComboBox();

        private Button[] m_btns_sysin;
        private Button[] m_btns_sysout;
        private string[] m_io_sysin;
        private string[] m_io_sysout;

        private Button[] m_btns_in;
        private Button[] m_btns_out;

        enum MotionMode
        {
            连续运动,
            相对运动,
            绝对运动,
        }

        private MotionMode m_motionMode = MotionMode.连续运动;


        /// <summary>
        /// 构造函数
        /// </summary>
        public Form_RobotMgr()
        {
            InitializeComponent();

            InitDataGridView();

            InitComboBox();

            InitCtrlAnchor();

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

        private void InitDataGridView()
        {
            string[] strHeadSysIoIn = { "功能描述", "点位名称", "有效电平", "启用" };
            dataGridView_sysIoIn.Columns.Clear();
            dataGridView_sysIoIn.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_sysIoIn.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadSysIoIn)
            {
                int col = dataGridView_sysIoIn.Columns.Add(str, str);
                dataGridView_sysIoIn.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_sysIoIn.Columns[1].Width = 200;

            string[] strHeadSysIoOut = { "功能描述", "点位名称", "有效电平","脉冲宽度", "启用" };
            dataGridView_sysIoOut.Columns.Clear();
            dataGridView_sysIoOut.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_sysIoOut.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadSysIoOut)
            {
                int col = dataGridView_sysIoOut.Columns.Add(str, str);
                dataGridView_sysIoOut.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_sysIoOut.Columns[1].Width = 200;


            string[] strHeadPoint = { "点序号", "点位名称" };
            dataGridView_IoIn.Columns.Clear();
            dataGridView_IoIn.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_IoIn.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadPoint)
            {
                int col = dataGridView_IoIn.Columns.Add(str, str);
                dataGridView_IoIn.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_IoIn.Columns[1].Width = 200;


            dataGridView_IoOut.Columns.Clear();
            dataGridView_IoOut.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_IoOut.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadPoint)
            {
                int col = dataGridView_IoOut.Columns.Add(str, str);
                dataGridView_IoOut.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_IoOut.Columns[1].Width = 200;


            dataGridView_Point.Columns.Clear();
            dataGridView_Point.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Point.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadPoint)
            {
                int col = dataGridView_Point.Columns.Add(str, str);
                dataGridView_Point.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_Point.Columns[1].Width = 200;

            string[] strHeadCmd = { "命令", "描述", "参数数量", "回复"};
            dataGridView_Cmd.Columns.Clear();
            dataGridView_Cmd.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Cmd.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in strHeadCmd)
            {
                int col = dataGridView_Cmd.Columns.Add(str, str);
                dataGridView_Cmd.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitComboBox()
        {
            comboBox_ActiveLevelIoIn.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox_ActiveLevelIoOut.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox_EnableIoIn.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox_EnableIoOut.DropDownStyle = ComboBoxStyle.DropDown;

            comboBox_ActiveLevelIoIn.Name = "comboBox_ActiveLevelIoIn";
            comboBox_ActiveLevelIoOut.Name = "comboBox_ActiveLevelIoOut";
            comboBox_EnableIoIn.Name = "comboBox_EnableIoIn";
            comboBox_EnableIoOut.Name = "comboBox_EnableIoOut";

            comboBox_ActiveLevelIoIn.Items.Clear();
            comboBox_ActiveLevelIoOut.Items.Clear();
            comboBox_EnableIoIn.Items.Clear();
            comboBox_EnableIoOut.Items.Clear();

            foreach (var item in Enum.GetValues(typeof(ActiveLevel)))
            {
                string strValue = String.Format("{0} - {1}", (int)item, item.ToString());
                comboBox_ActiveLevelIoIn.Items.Add(strValue);
                comboBox_ActiveLevelIoOut.Items.Add(strValue);
            }

            comboBox_EnableIoIn.Items.AddRange(new object[] { 0, 1 });
            comboBox_EnableIoOut.Items.AddRange(new object[] { 0, 1 });

            comboBox_ActiveLevelIoIn.Visible = false;
            comboBox_ActiveLevelIoOut.Visible = false;
            comboBox_EnableIoIn.Visible = false;
            comboBox_EnableIoOut.Visible = false;


            comboBox_ActiveLevelIoIn.SelectedIndexChanged += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_ActiveLevelIoOut.SelectedIndexChanged += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_EnableIoIn.SelectedIndexChanged += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_EnableIoOut.SelectedIndexChanged += ComboBox_ActiveLevel_SelectedIndexChanged;

            comboBox_ActiveLevelIoIn.LostFocus += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_ActiveLevelIoOut.LostFocus += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_EnableIoIn.LostFocus += ComboBox_ActiveLevel_SelectedIndexChanged;
            comboBox_EnableIoOut.LostFocus += ComboBox_ActiveLevel_SelectedIndexChanged;

            this.dataGridView_sysIoIn.Controls.Add(comboBox_ActiveLevelIoIn);
            this.dataGridView_sysIoIn.Controls.Add(comboBox_EnableIoIn);

            this.dataGridView_sysIoOut.Controls.Add(comboBox_ActiveLevelIoOut);
            this.dataGridView_sysIoOut.Controls.Add(comboBox_EnableIoOut);
        }

        private void ComboBox_ActiveLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Name)
            {
                case "comboBox_ActiveLevelIoIn":
                case "comboBox_EnableIoIn":
                    this.dataGridView_sysIoIn.CurrentCell.Value = comboBox.SelectedIndex;
                    break;

                case "comboBox_ActiveLevelIoOut":
                case "comboBox_EnableIoOut":
                    this.dataGridView_sysIoOut.CurrentCell.Value = comboBox.SelectedIndex;
                    break;
            }
        }

        private void InitCtrlAnchor()
        {
            tabControl1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            
        }

        private void Form_RobotMgr_Load(object sender, EventArgs e)
        {
            //初始化机器人名称下拉框
            comboBox_RobotName.Items.Clear();
            foreach (var item in RobotMgrEx.GetInstance().m_dictRobot.Keys)
            {
                comboBox_RobotName.Items.Add(item);
            }

            //初始化通信模式下拉框
            comboBox_Mode.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(CommMode)))
            {
                comboBox_Mode.Items.Add(item);
            }

            //初始化品牌
            comboBox_Vendor.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(RobotVendor)))
            {
                comboBox_Vendor.Items.Add(item);
            }

            if(comboBox_RobotName.Items.Count > 0)
            {
                comboBox_RobotName.SelectedIndex = 0;
            }

            //初始化机器人手动控制界面
            comboBox_Local.Items.Clear();
            comboBox_Local.Items.Add(0);
            comboBox_Local.Items.Add(1);
            comboBox_Local.Items.Add(2);
            comboBox_Local.Items.Add(3);
            comboBox_Local.SelectedIndex = 0;

            comboBox_Tool.Items.Clear();
            comboBox_Tool.Items.Add(0);
            comboBox_Tool.Items.Add(1);
            comboBox_Tool.Items.Add(2);
            comboBox_Tool.Items.Add(3);
            comboBox_Tool.SelectedIndex = 0;

            comboBox_Speed.Items.Clear();
            comboBox_Speed.Items.Add(100);
            comboBox_Speed.Items.Add(80);
            comboBox_Speed.Items.Add(50);
            comboBox_Speed.Items.Add(30);
            comboBox_Speed.Items.Add(10);
            comboBox_Speed.SelectedIndex = 0;

            comboBox_MoveMode.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(Robot.MoveMode)))
            {
                comboBox_MoveMode.Items.Add(item);
            }
            comboBox_MoveMode.SelectedIndex = 1;

            textBox_StepX.Text = "";
            textBox_StepY.Text = "";
            textBox_StepZ.Text = "";
            textBox_StepU.Text = "";
            textBox_StepV.Text = "";
            textBox_StepW.Text = "";

            radioButton_Continue.Checked = true;

            comboBox_Mode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Remote.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Manul.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Monitor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Vendor.DropDownStyle = ComboBoxStyle.DropDownList;

            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {
            if (Security.GetUserMode() >= UserMode.Engineer)
            {
                
                if (!tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                comboBox_Mode.Enabled = true;
                comboBox_Remote.Enabled = true;
                comboBox_Manul.Enabled = true;
                comboBox_Monitor.Enabled = true;

                comboBox_RobotName.DropDownStyle = ComboBoxStyle.DropDown;

            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                comboBox_Mode.Enabled = false;
                comboBox_Remote.Enabled = false;
                comboBox_Manul.Enabled = false;
                comboBox_Monitor.Enabled = false;

                comboBox_RobotName.DropDownStyle = ComboBoxStyle.DropDownList;
            }


            if (Security.GetUserMode() >= UserMode.Adjustor)
            {
                groupBox_Cmd.Enabled = true;
                groupBox_CommInfo.Enabled = true;
                groupBox_CurPos.Enabled = true;
                groupBox_ioOut.Enabled = true;
                groupBox_mgr.Enabled = true;
                groupBox_MotionCtrol.Enabled = true;
                groupBox_sysout.Enabled = true;
            }
            else
            {
                groupBox_Cmd.Enabled = false;
                groupBox_CommInfo.Enabled = false;
                groupBox_CurPos.Enabled = false;
                groupBox_ioOut.Enabled = false;
                groupBox_mgr.Enabled = false;
                groupBox_MotionCtrol.Enabled = false;
                groupBox_sysout.Enabled = false;
            }
        }

        private void comboBox_RobotName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                comboBox_Mode.SelectedIndex = (int)robot.Mode;

                comboBox_Remote.SelectedIndex = robot.Comm.m_nRemoteComm;
                comboBox_Manul.SelectedIndex = robot.Comm.m_nManulComm;
                comboBox_Monitor.SelectedIndex = robot.Comm.m_nMonitorComm;

                comboBox_Vendor.SelectedIndex = (int)robot.Vendor;

                UpdateIoButtons(robot);

                UpdateRobotInfo(robot);

                comboBox_Cmd.Items.Clear();

                foreach (var item in robot.RobotCmds.Keys)
                {
                    comboBox_Cmd.Items.Add(item);
                }

                if (comboBox_Cmd.Items.Count > 0)
                {
                    comboBox_Cmd.SelectedIndex = 0;
                }

                comboBox_Point.Items.Clear();
                foreach (var item in robot.RobotPoints.Values)
                {
                    comboBox_Point.Items.Add(String.Format("P{0} - {1}", item.m_nIndex, item.m_strName));
                }

                if (comboBox_Point.Items.Count > 0)
                {
                    comboBox_Point.SelectedIndex = 0;
                }
                

            }

            RobotMgrEx.GetInstance().UpdateGridFromParam(strRobotName, dataGridView_sysIoIn, dataGridView_sysIoOut,
                dataGridView_IoIn, dataGridView_IoOut,dataGridView_Point,dataGridView_Cmd);
        }

        private void UpdateRobotConfig()
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                UpdateIoButtons(robot);

                comboBox_Cmd.Items.Clear();

                foreach (var item in robot.RobotCmds.Keys)
                {
                    comboBox_Cmd.Items.Add(item);
                }

                if (comboBox_Cmd.Items.Count > 0)
                {
                    comboBox_Cmd.SelectedIndex = 0;
                }  

                comboBox_Point.Items.Clear();
                foreach (var item in robot.RobotPoints.Values)
                {
                    comboBox_Point.Items.Add(String.Format("P{0} - {1}", item.m_nIndex, item.m_strName));
                }

                if (comboBox_Point.Items.Count > 0)
                {
                    comboBox_Point.SelectedIndex = 0;
                }
                
            }
        }

        private void comboBox_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int mode = comboBox_Mode.SelectedIndex;

            int nCount = 0;
            if (mode == (int)CommMode.Tcp)
            {
                nCount = TcpMgr.GetInstance().Count;          
            }
            else if (mode == (int)CommMode.Com)
            {
                nCount = ComMgr.GetInstance().Count;
            }

            comboBox_Remote.Items.Clear();
            comboBox_Manul.Items.Clear();
            comboBox_Monitor.Items.Clear();

            for (int i = 0; i < nCount;i++)
            {
                comboBox_Remote.Items.Add(i);
                comboBox_Manul.Items.Add(i);
                comboBox_Monitor.Items.Add(i);
            }

            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                comboBox_Remote.SelectedIndex = robot.Comm.m_nRemoteComm;
                comboBox_Manul.SelectedIndex = robot.Comm.m_nManulComm;
                comboBox_Monitor.SelectedIndex = robot.Comm.m_nMonitorComm;
            }
            
        }

        private void dataGridView_IoIn_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView_sysIoIn.CurrentCell == null)
            {
                comboBox_ActiveLevelIoIn.Visible = false;
                comboBox_EnableIoIn.Visible = false;
                return;
            }

            if (dataGridView_sysIoIn.CurrentCell.ColumnIndex == 2)
            {
                comboBox_EnableIoIn.Visible = false;
                Rectangle rect = dataGridView_sysIoIn.GetCellDisplayRectangle(
                    dataGridView_sysIoIn.CurrentCell.ColumnIndex, 
                    dataGridView_sysIoIn.CurrentCell.RowIndex, true);

                comboBox_ActiveLevelIoIn.Top = rect.Top;
                comboBox_ActiveLevelIoIn.Left = rect.Left;
                comboBox_ActiveLevelIoIn.Width = rect.Width;
                comboBox_ActiveLevelIoIn.Height = rect.Height;

                if (dataGridView_sysIoIn.CurrentCell.Value != null)
                {
                    comboBox_ActiveLevelIoIn.Text = dataGridView_sysIoIn.CurrentCell.Value.ToString();
                }
                else
                {
                    comboBox_ActiveLevelIoIn.SelectedIndex = -1;
                }

                comboBox_ActiveLevelIoIn.Visible = true;
            }
            else if (dataGridView_sysIoIn.CurrentCell.ColumnIndex == 3)
            {
                comboBox_ActiveLevelIoIn.Visible = false;
                Rectangle rect = dataGridView_sysIoIn.GetCellDisplayRectangle(
                    dataGridView_sysIoIn.CurrentCell.ColumnIndex,
                    dataGridView_sysIoIn.CurrentCell.RowIndex, true);

                comboBox_EnableIoIn.Top = rect.Top;
                comboBox_EnableIoIn.Left = rect.Left;
                comboBox_EnableIoIn.Width = rect.Width;
                comboBox_EnableIoIn.Height = rect.Height;

                if (dataGridView_sysIoIn.CurrentCell.Value != null)
                {
                    comboBox_EnableIoIn.Text = dataGridView_sysIoIn.CurrentCell.Value.ToString();
                }
                else
                {
                    comboBox_EnableIoIn.SelectedIndex = -1;
                }

                comboBox_EnableIoIn.Visible = true;
            }
            else
            {
                comboBox_EnableIoIn.Visible = false;
                comboBox_ActiveLevelIoIn.Visible = false;
            }
        }

        private void dataGridView_IoOut_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView_sysIoOut.CurrentCell == null)
            {
                comboBox_ActiveLevelIoOut.Visible = false;
                comboBox_EnableIoOut.Visible = false;
                return;
            }

            if (dataGridView_sysIoOut.CurrentCell.ColumnIndex == 2)
            {
                comboBox_EnableIoOut.Visible = false; 

                Rectangle rect = dataGridView_sysIoOut.GetCellDisplayRectangle(
                    dataGridView_sysIoOut.CurrentCell.ColumnIndex,
                    dataGridView_sysIoOut.CurrentCell.RowIndex, true);

                comboBox_ActiveLevelIoOut.Top = rect.Top;
                comboBox_ActiveLevelIoOut.Left = rect.Left;
                comboBox_ActiveLevelIoOut.Width = rect.Width;
                comboBox_ActiveLevelIoOut.Height = rect.Height;

                if (dataGridView_sysIoOut.CurrentCell.Value != null)
                {
                    comboBox_ActiveLevelIoOut.Text = dataGridView_sysIoOut.CurrentCell.Value.ToString();
                }
                else
                {
                    comboBox_ActiveLevelIoOut.SelectedIndex = -1;
                }

                comboBox_ActiveLevelIoOut.Visible = true;
            }
            else if (dataGridView_sysIoOut.CurrentCell.ColumnIndex == 4)
            {
                comboBox_ActiveLevelIoOut.Visible = false;

                Rectangle rect = dataGridView_sysIoOut.GetCellDisplayRectangle(
                    dataGridView_sysIoOut.CurrentCell.ColumnIndex,
                    dataGridView_sysIoOut.CurrentCell.RowIndex, true);

                comboBox_EnableIoOut.Top = rect.Top;
                comboBox_EnableIoOut.Left = rect.Left;
                comboBox_EnableIoOut.Width = rect.Width;
                comboBox_EnableIoOut.Height = rect.Height;

                if (dataGridView_sysIoOut.CurrentCell.Value != null)
                {
                    comboBox_EnableIoOut.Text = dataGridView_sysIoOut.CurrentCell.Value.ToString();
                }
                else
                {
                    comboBox_EnableIoOut.SelectedIndex = -1;
                }

                comboBox_EnableIoOut.Visible = true;
            }
            else
            {
                comboBox_EnableIoOut.Visible = false;
                comboBox_ActiveLevelIoOut.Visible = false;
            }
        }

        private void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView dataGrid = (DataGridView)sender;

            switch (dataGrid.Name)
            {
                case "dataGridView_IoOut":
                    comboBox_EnableIoOut.Visible = false;
                    comboBox_ActiveLevelIoOut.Visible = false;
                    break;

                case "dataGridView_IoIn":
                    comboBox_ActiveLevelIoIn.Visible = false;
                    comboBox_EnableIoIn.Visible = false;
                    break;

            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            int vendor = comboBox_Vendor.SelectedIndex;

            RobotComm comm = new RobotComm();
            comm.m_comMode = (CommMode)comboBox_Mode.SelectedIndex;
            comm.m_nRemoteComm = comboBox_Remote.SelectedIndex;
            comm.m_nManulComm = comboBox_Manul.SelectedIndex;
            comm.m_nMonitorComm = comboBox_Monitor.SelectedIndex;

            RobotMgrEx.GetInstance().UpdateParamFromGrid(strRobotName,comm,vendor,
                dataGridView_sysIoIn, dataGridView_sysIoOut, dataGridView_IoIn, dataGridView_IoOut,
                dataGridView_Point,dataGridView_Cmd);

            UpdateRobotConfig();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            int vendor = comboBox_Vendor.SelectedIndex;

            RobotComm comm = new RobotComm();
            comm.m_comMode = (CommMode)comboBox_Mode.SelectedIndex;
            comm.m_nRemoteComm = comboBox_Remote.SelectedIndex;
            comm.m_nManulComm = comboBox_Manul.SelectedIndex;
            comm.m_nMonitorComm = comboBox_Monitor.SelectedIndex;

            RobotMgrEx.GetInstance().UpdateParamFromGrid(strRobotName, comm, vendor,
                dataGridView_sysIoIn, dataGridView_sysIoOut, dataGridView_IoIn, dataGridView_IoOut,
                dataGridView_Point, dataGridView_Cmd);

            UpdateRobotConfig();

            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            RobotMgrEx.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save robot configuration complete");
            }
            else
            {
                MessageBox.Show("保存机器人配置完成");
            }

            //如果新增，需要更新界面
            if (comboBox_RobotName.Items.Count != RobotMgrEx.GetInstance().m_dictRobot.Keys.Count)
            {
                //初始化机器人名称下拉框
                comboBox_RobotName.Items.Clear();
                foreach (var item in RobotMgrEx.GetInstance().m_dictRobot.Keys)
                {
                    comboBox_RobotName.Items.Add(item);
                }

                comboBox_RobotName.SelectedIndex = comboBox_RobotName.Items.Count - 1;
            }
        }

        private void button_DelRobot_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                string str1 = "即将删除机器人{0},是否继续？";
                string str2 = "警告";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Robot {0} will be deleted. Do you want to continue? ";
                    str2 = "Warning";
                }
                if (DialogResult.Yes == MessageBox.Show(String.Format(str1, strRobotName),
                    str2, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))

                {
                    RobotMgrEx.GetInstance().m_dictRobot.Remove(strRobotName);

                    //初始化机器人名称下拉框
                    comboBox_RobotName.Items.Clear();
                    foreach (var item in RobotMgrEx.GetInstance().m_dictRobot.Keys)
                    {
                        comboBox_RobotName.Items.Add(item);
                    }
                    if (comboBox_RobotName.Items.Count > 0)
                    {
                        comboBox_RobotName.SelectedIndex = 0;
                    }

                    string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

                    XmlDocument doc = new XmlDocument();
                    doc.Load(cfg);

                    RobotMgrEx.GetInstance().SaveCfgXML(doc);

                    doc.Save(cfg);

                }

            }
        }

        private void button_DelSelect_Click(object sender, EventArgs e)
        {
            string str1 = "即将删除选中项,是否继续？";
            string str2 = "警告";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "You are about to delete the selected items. Do you want to continue? ";
                str2 = "Warning";
            }
            if (DialogResult.No == MessageBox.Show(String.Format(str1),
                    str2, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                return;
            }

           int nSelectTab = tabControl2.SelectedIndex;

            DataGridView[] grids = { dataGridView_sysIoIn, dataGridView_sysIoOut, dataGridView_IoIn, dataGridView_IoOut, dataGridView_Point, dataGridView_Cmd };

            int nSelectRow = grids[nSelectTab].CurrentCell.RowIndex;
            if (nSelectRow < grids[nSelectTab].Rows.Count - 1)
            {
                grids[nSelectTab].Rows.RemoveAt(grids[nSelectTab].CurrentCell.RowIndex);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ManaulTool.updateIoState(m_btns_sysin, m_io_sysin);
            ManaulTool.updateIoState(m_btns_sysout, m_io_sysout, false);

            UpdateRobotState();
        }

        private void UpdateRobotInfo(Robot robot)
        {
            string str1 = "远程控制端口:";
            string str2 = "\r\n手动控制端口:";
            string str3 = "\r\n监控端口:";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Remote control port:";
                str2 = "\r\nManual control port:";
                str3 = "\r\nMonitor port:";
            }

            if (robot.Mode == CommMode.Tcp)
            {
                TcpLink t = (TcpLink)robot.LinkRemote;
                string strInfo = string.Format(str1 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n",
                    TcpMgr.m_strDescribe[0], t.m_nIndex.ToString(),
                    TcpMgr.m_strDescribe[1], t.m_strName,
                    TcpMgr.m_strDescribe[2], t.m_strIP,
                    TcpMgr.m_strDescribe[3], t.m_nPort.ToString(),
                    TcpMgr.m_strDescribe[4], t.m_nTime.ToString(),
                    TcpMgr.m_strDescribe[5], t.m_strLineFlag
                    );

                t = (TcpLink)robot.LinkManul;
                strInfo += string.Format(str2 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n",
                    TcpMgr.m_strDescribe[0], t.m_nIndex.ToString(),
                    TcpMgr.m_strDescribe[1], t.m_strName,
                    TcpMgr.m_strDescribe[2], t.m_strIP,
                    TcpMgr.m_strDescribe[3], t.m_nPort.ToString(),
                    TcpMgr.m_strDescribe[4], t.m_nTime.ToString(),
                    TcpMgr.m_strDescribe[5], t.m_strLineFlag
                    );

                t = (TcpLink)robot.LinkMonitor;
                strInfo += string.Format(str3 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n",
                    TcpMgr.m_strDescribe[0], t.m_nIndex.ToString(),
                    TcpMgr.m_strDescribe[1], t.m_strName,
                    TcpMgr.m_strDescribe[2], t.m_strIP,
                    TcpMgr.m_strDescribe[3], t.m_nPort.ToString(),
                    TcpMgr.m_strDescribe[4], t.m_nTime.ToString(),
                    TcpMgr.m_strDescribe[5], t.m_strLineFlag
                    );


                textBox_info.Text = strInfo;
            }
            else
            {
                ComLink t = (ComLink)robot.LinkRemote;
                string strInfo = string.Format(str1 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n{12}: {13}\r\n{14}: {15}\r\n{16}: {17}\r\n{18}: {19}\r\n",
                                ComMgr.m_strDescribe[0], t.m_nComNo,
                                ComMgr.m_strDescribe[1], t.m_strName,
                                ComMgr.m_strDescribe[2], t.m_nBaudRate,
                                ComMgr.m_strDescribe[3], t.m_nDataBit,
                                ComMgr.m_strDescribe[4], t.m_strPartiy,
                                ComMgr.m_strDescribe[5], t.m_strStopBit,
                                ComMgr.m_strDescribe[6], t.m_strFlowCtrl,
                                ComMgr.m_strDescribe[7], t.m_nTime,
                                ComMgr.m_strDescribe[8], t.m_nBufferSzie,
                                ComMgr.m_strDescribe[9], t.m_strLineFlag);

                t = (ComLink)robot.LinkManul;
                strInfo += string.Format(str2 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n{12}: {13}\r\n{14}: {15}\r\n{16}: {17}\r\n{18}: {19}\r\n",
                                ComMgr.m_strDescribe[0], t.m_nComNo,
                                ComMgr.m_strDescribe[1], t.m_strName,
                                ComMgr.m_strDescribe[2], t.m_nBaudRate,
                                ComMgr.m_strDescribe[3], t.m_nDataBit,
                                ComMgr.m_strDescribe[4], t.m_strPartiy,
                                ComMgr.m_strDescribe[5], t.m_strStopBit,
                                ComMgr.m_strDescribe[6], t.m_strFlowCtrl,
                                ComMgr.m_strDescribe[7], t.m_nTime,
                                ComMgr.m_strDescribe[8], t.m_nBufferSzie,
                                ComMgr.m_strDescribe[9], t.m_strLineFlag);

                t = (ComLink)robot.LinkMonitor;
                strInfo += string.Format(str3 + "\r\n{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n{12}: {13}\r\n{14}: {15}\r\n{16}: {17}\r\n{18}: {19}\r\n",
                                ComMgr.m_strDescribe[0], t.m_nComNo,
                                ComMgr.m_strDescribe[1], t.m_strName,
                                ComMgr.m_strDescribe[2], t.m_nBaudRate,
                                ComMgr.m_strDescribe[3], t.m_nDataBit,
                                ComMgr.m_strDescribe[4], t.m_strPartiy,
                                ComMgr.m_strDescribe[5], t.m_strStopBit,
                                ComMgr.m_strDescribe[6], t.m_strFlowCtrl,
                                ComMgr.m_strDescribe[7], t.m_nTime,
                                ComMgr.m_strDescribe[8], t.m_nBufferSzie,
                                ComMgr.m_strDescribe[9], t.m_strLineFlag);

                textBox_info.Text = strInfo;
            }


        }

        private void UpdateIoButtons(Robot robot)
        {
            timer1.Enabled = false;

            panel_sysIoIn.Controls.Clear();
            m_btns_sysin = new Button[robot.RobotSysIoIns.Count];
            m_io_sysin = new string[robot.RobotSysIoIns.Count];

            panel_sysIoOut.Controls.Clear();
            m_btns_sysout = new Button[robot.RobotSysIoOuts.Count];
            m_io_sysout = new string[robot.RobotSysIoOuts.Count];

            panel_IoIn.Controls.Clear();
            m_btns_in = new Button[robot.RobotIoIns.Count];

            panel_IoOut.Controls.Clear();
            m_btns_out = new Button[robot.RobotIoOuts.Count];

            //远程输入
            int n = 0;
            foreach (var item in robot.RobotSysIoIns.Values)
            {
                Button btn = new Button();
                btn.Name = item.m_strIoFuction;
                btn.Text = item.m_strIoName;
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.Left = 1;
                btn.Height = 48;
                btn.Top = n * btn.Height + 1;
                btn.Width = panel_sysIoIn.Width - 20;

                panel_sysIoIn.Controls.Add(btn);

                m_btns_sysin[n] = btn;
                m_io_sysin[n] = item.m_strIoName;
                n++;
            }

            //远程输出
            n = 0;
            foreach (var item in robot.RobotSysIoOuts.Values)
            {
                Button btn = new Button();
                btn.Name = item.m_strIoFuction;
                btn.Text = item.m_strIoName;
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += ManaulTool.Form_IO_Out_Click;

                btn.Left = 1;
                btn.Height = 48;
                btn.Top = n * btn.Height + 1;
                btn.Width = panel_sysIoOut.Width - 20;

                panel_sysIoOut.Controls.Add(btn);

                m_btns_sysout[n] = btn;
                m_io_sysout[n] = item.m_strIoName;
                n++;
            }

            ManaulTool.updateIoText(m_btns_sysin, m_io_sysin);
            ManaulTool.updateIoText(m_btns_sysout, m_io_sysout, false);

            //机器人输入
            n = 0;
            foreach (var item in robot.RobotIoIns.Values)
            {
                Button btn = new Button();
                btn.Name = item.m_strName;
                btn.Text = String.Format("{0}.{1}",item.m_nIndex,item.m_strName);
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.Left = 1;
                btn.Height = 48;
                btn.Top = n * btn.Height + 1;
                btn.Width = panel_IoIn.Width - 20;

                panel_IoIn.Controls.Add(btn);

                m_btns_in[n] = btn;
                n++;
            }

            //机器人输出
            n = 0;
            foreach (var item in robot.RobotIoOuts.Values)
            {
                Button btn = new Button();
                btn.Name = item.m_strName;
                btn.Text = String.Format("{0}.{1}", item.m_nIndex, item.m_strName);
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += ButtonOut_Click;

                btn.Left = 1;
                btn.Height = 48;
                btn.Top = n * btn.Height + 1;
                btn.Width = panel_IoOut.Width - 20;

                panel_IoOut.Controls.Add(btn);

                m_btns_out[n] = btn;
                n++;
            }

            timer1.Enabled = true;

        }

        private void button_open_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.InitRobot())
                {
                    button_open.BackColor = Color.LightGreen;

                    if (robot.Mode == CommMode.Tcp)
                    {
                        TcpLink t = (TcpLink)robot.LinkRemote;
                        t.BeginAsynReceive(OnDataReceivedEvent);
                    }
                    else
                    {
                        ComLink t = (ComLink)robot.LinkRemote;
                        t.BeginAsynReceive(OnDataReceivedEvent);
                    }
                }
                else
                {
                    button_open.BackColor = Color.Transparent;
                }
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                robot.DeInitRobot();

                if (robot.Mode == CommMode.Tcp)
                {
                    TcpLink t = (TcpLink)robot.LinkRemote;
                    t.EndAsynReceive();
                }
                else
                {
                    ComLink t = (ComLink)robot.LinkRemote;
                    t.EndAsynReceive();
                }

                button_open.BackColor = Color.Transparent;
            }
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);
                string strCmd = comboBox_Cmd.Text.Trim();

                string strParm = textBox_params.Text.Trim();

                if (strParm == "")
                {
                    strParm = "0";
                }

                //判断参数数量
                string[] strParams = strParm.Split(',');

                RobotCmd cmd;
                if (robot.RobotCmds.TryGetValue(strCmd,out cmd))
                {
                    int size = cmd.m_nLength;

                    if (strParams.Length < cmd.m_nLength)
                    {
                        size = strParams.Length;
                    }

                    for (int i = 0; i < size; i++)
                    {
                        strCmd += "," + strParams[i];
                    }

                    for (int i = size; i < cmd.m_nLength; i++)
                    {
                        strCmd += ",0";
                    }
                }
                else
                {
                    strCmd += "," + textBox_params.Text.Trim();
                }

                if (robot.IsRemoteConnected)
                {
                    robot.WriteLine(strCmd);
                }

            }


        }

        private void OnDataReceivedEvent(byte[] data, int length)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                textBox_Receive.Text += System.Text.Encoding.Default.GetString(data, 0, length);
            });
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            textBox_Receive.Text = "";
        }

        private void ButtonOut_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    Button btn = (Button)sender;

                    string strText = btn.Text;

                    Regex rg = new Regex(@"[0-9]+");  //正则表达式

                    Match m = rg.Match(strText);

                    if (m.Length == 0)
                        return;

                    int index = Convert.ToInt32(m.Value);

                    if (robot.GetDO(index))
                    {
                        robot.SetDO(index, false);
                    }
                    else
                    {
                        robot.SetDO(index, true);
                    }

                }
            }
        }

        private void UpdateRobotState()
        {
            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsMonitorConnected)
                {
                    //更新输入
                    foreach (Button btn in m_btns_in)
                    {
                        string strText = btn.Text;

                        Regex rg = new Regex(@"[0-9]+");  //正则表达式

                        Match m = rg.Match(strText);

                        if (m.Length == 0)
                            continue;

                        int index = Convert.ToInt32(m.Value);

                        bool bBit = robot.GetDI(index);

                        if (btn.ImageIndex != Convert.ToInt32(bBit))
                        {
                            btn.ImageIndex = Convert.ToInt32(bBit);
                        }
                    }

                    //更新输出
                    foreach (Button btn in m_btns_out)
                    {
                        string strText = btn.Text;

                        Regex rg = new Regex(@"[0-9]+");  //正则表达式

                        Match m = rg.Match(strText);

                        if (m.Length == 0)
                            continue;

                        int index = Convert.ToInt32(m.Value);

                        bool bBit = robot.GetDO(index);

                        if (btn.ImageIndex != Convert.ToInt32(bBit))
                        {
                            btn.ImageIndex = Convert.ToInt32(bBit);
                        }
                    }

                    //更新Motor Power Tool Speed
                    if (robot.Motor)
                    {
                        button_MotorOn.BackColor = Color.LightGreen;
                        button_MotorOff.BackColor = Color.Transparent;
                    }
                    else
                    {
                        button_MotorOn.BackColor = Color.Transparent;
                        button_MotorOff.BackColor = Color.LightGreen;
                    }

                    if (robot.Power)
                    {
                        button_PowerHigh.BackColor = Color.LightGreen;
                        button_PowerLow.BackColor = Color.Transparent;
                    }
                    else
                    {
                        button_PowerHigh.BackColor = Color.Transparent;
                        button_PowerLow.BackColor = Color.LightGreen;
                    }

                    //comboBox_Tool.Text = robot.Tool.ToString();

                    //comboBox_Speed.Text = robot.SpeedFactor.ToString();

                    //更新坐标
                    textBox_X.Text = robot.X.ToString("f3");
                    textBox_Y.Text = robot.Y.ToString("f3");
                    textBox_Z.Text = robot.Z.ToString("f3");
                    textBox_U.Text = robot.U.ToString("f3");
                    textBox_V.Text = robot.V.ToString("f3");
                    textBox_W.Text = robot.W.ToString("f3");
                }

            }
        }

        private void button_OpenMonitor_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.ConnectManul(true) && robot.ConnectMonitor(true))
                {
                    button_OpenMonitor.BackColor = Color.LightGreen;

                    robot.StartMonitor();

                    if (robot.Mode == CommMode.Tcp)
                    {
                       ((TcpLink)robot.LinkManul).BeginAsynReceive(OnDataReceivedEvent);
                    }
                    else
                    {
                        ((ComLink)robot.LinkManul).BeginAsynReceive(OnDataReceivedEvent);
                    }
                }
                else
                {
                    button_OpenMonitor.BackColor = Color.Transparent;

                    robot.StopMonitor();
                }
            }
        }

        private void button_CloseMonitor_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                robot.ConnectManul(false);
                robot.ConnectMonitor(false);

                robot.StopMonitor();

                if (robot.Mode == CommMode.Tcp)
                {
                    ((TcpLink)robot.LinkManul).EndAsynReceive();
                }
                else
                {
                    ((ComLink)robot.LinkManul).EndAsynReceive();
                }

                button_OpenMonitor.BackColor = Color.Transparent;
            }
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.SetLocal(Convert.ToInt32(comboBox_Local.Text));
                    robot.SetTool(Convert.ToInt32(comboBox_Tool.Text));
                    robot.SetSpeedFactor(Convert.ToInt32(comboBox_Speed.Text));
                }

            }
        }

        private void button_MotorOn_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.MotorOn(true);
                }

            }
        }

        private void button_MotorOff_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.MotorOn(false);
                }

            }
        }

        private void button_PowerLow_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.PowerHigh(false);
                }

            }
        }

        private void button_PowerHigh_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.PowerHigh(true);
                }

            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                if (robot.IsManulConnected)
                {
                    robot.Reset();
                }

            }
        }

        private void buttonMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_motionMode == MotionMode.连续运动)
            {
                string strRobotName = comboBox_RobotName.Text;

                if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
                {
                    Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                    Button btn = (Button)sender;

                    double x = 0, y = 0, z = 0, u = 0, v = 0, w = 0;

                    int step = 1;

                    switch (btn.Text)
                    {
                        case "X+":
                            x = step;
                            break;
                        case "X-":
                            x = -step;
                            break;
                        case "Y+":
                            y = step;
                            break;
                        case "Y-":
                            y = -step;
                            break;
                        case "Z+":
                            z = step;
                            break;
                        case "Z-":
                            z = -step;
                            break;
                        case "U+":
                            u = step;
                            break;
                        case "U-":
                            u = -step;
                            break;
                        case "V+":
                            v = step;
                            break;
                        case "V-":
                            v = -step;
                            break;
                        case "W+":
                            w = step;
                            break;
                        case "W-":
                            w = -step;
                            break;
                    }



                    robot.ContinueOn(x, y, z, u, v, w);


                }
                    
            }
        }

        private void buttonMove_MouseUp(object sender, MouseEventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);
                robot.ContinueOff();
            }
        }

        private void radioButton_Continue_CheckedChanged(object sender, EventArgs e)
        {
            m_motionMode = MotionMode.连续运动;
            textBox_StepX.Text = "";
            textBox_StepY.Text = "";
            textBox_StepZ.Text = "";
            textBox_StepU.Text = "";
            textBox_StepV.Text = "";
            textBox_StepW.Text = "";
        }

        private void radioButton_Relative_CheckedChanged(object sender, EventArgs e)
        {
            m_motionMode = MotionMode.相对运动;
            textBox_StepX.Text = "1";
            textBox_StepY.Text = "1";
            textBox_StepZ.Text = "1";
            textBox_StepU.Text = "1";
            textBox_StepV.Text = "1";
            textBox_StepW.Text = "1";
        }

        private void radioButton_Abs_CheckedChanged(object sender, EventArgs e)
        {
            m_motionMode = MotionMode.绝对运动;
            textBox_StepX.Text = "0";
            textBox_StepY.Text = "0";
            textBox_StepZ.Text = "0";
            textBox_StepU.Text = "0";
            textBox_StepV.Text = "0";
            textBox_StepW.Text = "0";
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            if (m_motionMode == MotionMode.连续运动)
            {
                return;
            }

            string strRobotName = comboBox_RobotName.Text;

            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                Button btn = (Button)sender;

                double x = 0, y = 0, z = 0, u = 0, v = 0, w = 0;

                string str1 = "机器人X方向即将绝对运动到{0},请确认。";
                string str2 = "机器人Y方向即将绝对运动到{0},请确认。";
                string str3 = "机器人Z方向即将绝对运动到{0},请确认。";
                string str4 = "机器人U方向即将绝对运动到{0},请确认。";
                string str5 = "机器人V方向即将绝对运动到{0},请确认。";
                string str6 = "机器人W方向即将绝对运动到{0},请确认。";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Robot X direction is about to move absolutely to {0}, please confirm. ";
                    str2 = "Robot Y direction is about to move absolutely to {0}, please confirm. ";
                    str3 = "Robot Z direction is about to move absolutely to {0}, please confirm. ";
                    str4 = "Robot U direction is about to move absolutely to {0}, please confirm. ";
                    str5 = "Robot V direction is about to move absolutely to {0}, please confirm. ";
                    str6 = "Robot W direction is about to move absolutely to {0}, please confirm. ";
                }

                string strMsg = "";

                switch (btn.Text)
                {
                    case "X+":
                    case "X-":
                        x = Convert.ToDouble(textBox_StepX.Text.Trim());
                        strMsg = String.Format(str1, x);
                        break;
                    case "Y+":
                    case "Y-":
                        y = Convert.ToDouble(textBox_StepY.Text.Trim());
                        strMsg = String.Format(str2, y);
                        break;
                    case "Z+":
                    case "Z-":
                        z = Convert.ToDouble(textBox_StepZ.Text.Trim());
                        strMsg = String.Format(str3, z);
                        break;
                    case "U+":
                    case "U-":
                        u = Convert.ToDouble(textBox_StepU.Text.Trim());
                        strMsg = String.Format(str4, u);
                        break;
                    case "V+":
                    case "V-":
                        v = Convert.ToDouble(textBox_StepV.Text.Trim());
                        strMsg = String.Format(str5, v);
                        break;
                    case "W+":
                    case "W-":
                        w = Convert.ToDouble(textBox_StepW.Text.Trim());
                        strMsg = String.Format(str6, w);
                        break;
                }

                if (m_motionMode == MotionMode.绝对运动)
                {
                    if (DialogResult.Cancel == MessageBox.Show(strMsg,"Tips",MessageBoxButtons.OKCancel,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1))
                    {
                        return;
                    }
                }
                else
                {
                    switch (btn.Text)
                    {
                        case "X-":
                            x *= -1;
                            break;
                       
                        case "Y-":
                            y *= -1;
                            break;
                        
                        case "Z-":
                            z *= -1;
                            break;
                       
                        case "U-":
                            u *= -1;
                            break;
                        case "V-":
                            v *= -1;
                            break;

                        case "W-":
                            w *= -1;
                            break;
                    }
                }

                robot.Move(m_motionMode == MotionMode.绝对运动, x, y, z, u, v, w);
            }
        }

        private void button_Move_Click(object sender, EventArgs e)
        {
            if (m_motionMode == MotionMode.连续运动)
            {
                return;
            }

            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                double x = 0, y = 0, z = 0, u = 0, v = 0, w = 0;
                x = Convert.ToDouble(textBox_StepX.Text.Trim());
                y = Convert.ToDouble(textBox_StepY.Text.Trim());
                z = Convert.ToDouble(textBox_StepZ.Text.Trim());
                u = Convert.ToDouble(textBox_StepU.Text.Trim());
                v = Convert.ToDouble(textBox_StepV.Text.Trim());
                w = Convert.ToDouble(textBox_StepW.Text.Trim());

                string str1 = "机器人即将运动,请确认。";
                string str2 = "提示";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "The robot is about to move, please confirm. ";
                    str2 = "Tips";
                }

                if (DialogResult.Cancel == MessageBox.Show(str1, str2, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                {
                    return;
                }

                robot.Move(m_motionMode == MotionMode.绝对运动, x, y, z, u, v, w);
            }


        }

        private void button_PointMove_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                string strText = comboBox_Point.Text;

                Regex rg = new Regex(@"[0-9]+");  //正则表达式

                Match m = rg.Match(strText);

                if (m.Length == 0)
                    return;

                int pos = Convert.ToInt32(m.Value);

                string str1 = "机器人即将运动,请确认。";
                string str2 = "提示";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "The robot is about to move, please confirm. ";
                    str2 = "Tips";
                }

                if (DialogResult.Cancel == MessageBox.Show(str1, str2, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                {
                    return;
                }

                Robot.MoveMode mode = (Robot.MoveMode)comboBox_MoveMode.SelectedIndex;
                switch (mode)
                {
                    case Robot.MoveMode.Go:
                        robot.Go(pos);
                        break;

                    case Robot.MoveMode.Jump:
                        robot.Jump(pos);
                        break;

                    case Robot.MoveMode.Move:
                        robot.Move(pos);
                        break;

                }

            }
        }

        private void button_Teach_Click(object sender, EventArgs e)
        {
            string strRobotName = comboBox_RobotName.Text;
            if (RobotMgrEx.GetInstance().m_dictRobot.ContainsKey(strRobotName))
            {
                Robot robot = RobotMgrEx.GetInstance().GetRobot(strRobotName);

                string strText = comboBox_Point.Text;

                Regex rg = new Regex(@"[0-9]+");  //正则表达式

                Match m = rg.Match(strText);

                if (m.Length == 0)
                    return;

                int pos = Convert.ToInt32(m.Value);

                string str1 = "即将示教点位,请确认。";
                string str2 = "提示";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "About to teach point position, please confirm. ";
                    str2 = "Tips";
                }

                if (DialogResult.Cancel == MessageBox.Show(str1, str2, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                {
                    return;
                }

                robot.TeachPoint(pos);
            }
        }
    }
}
