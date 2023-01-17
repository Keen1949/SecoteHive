//2019-05-22 Binggoo 1.完善自动化测试，加入开始测试事件，用于更新选中行
//2019-05-28 Binggoo 1.完善自动化测试，加入限制时间和错误信息。
//                   2.支持添加和插入多条测试项，当添加/插入重复项时提醒是否继续添加。

using AutoFrameDll;
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
using System.Xml;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_AutoTest : Form
    {
        private TestType m_testType;

        public Form_AutoTest()
        {
            InitializeComponent();
        }

        private void InitCylinder()
        {
            dataGridView_Cylinder.Columns.Clear();
            dataGridView_Cylinder.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Cylinder.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            string[] Heads = { "气缸名称", "操作"};
            foreach (string str in Heads)
            {
                int col = dataGridView_Cylinder.Columns.Add(str, str);
                dataGridView_Cylinder.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_Cylinder.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_Cylinder.Columns[1].Width = 100;

            foreach(var item in CylinderMgr.GetInstance().m_dictCylinders.Keys)
            {
                int row = dataGridView_Cylinder.Rows.Add();

                dataGridView_Cylinder.Rows[row].Cells[0].Value = item;
                dataGridView_Cylinder.Rows[row].Cells[1].Value = "Out";

                row = dataGridView_Cylinder.Rows.Add();
                dataGridView_Cylinder.Rows[row].Cells[0].Value = item;
                dataGridView_Cylinder.Rows[row].Cells[1].Value = "Back";
            }
        }

        private void InitDI()
        {
            dataGridView_DI.Columns.Clear();
            dataGridView_DI.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_DI.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            string[] Heads = { "DI名称", "等待值" };
            foreach (string str in Heads)
            {
                int col = dataGridView_DI.Columns.Add(str, str);
                dataGridView_DI.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_DI.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_DI.Columns[1].Width = 100;

            foreach (var item in IoMgr.GetInstance().m_dicIn.Keys)
            {
                //判断气缸中是否存在
                bool bExist = false;
                foreach (var cyl in CylinderMgr.GetInstance().m_dictCylinders.Values)
                {
                    if (cyl.m_strIoIns[0] == item || cyl.m_strIoIns[1] == item)
                    {
                        bExist = true;
                        break;
                    }
                }

                if (bExist)
                {
                    continue;
                }

                int row = dataGridView_DI.Rows.Add();

                dataGridView_DI.Rows[row].Cells[0].Value = item;
                dataGridView_DI.Rows[row].Cells[1].Value = "On";

                row = dataGridView_DI.Rows.Add();
                dataGridView_DI.Rows[row].Cells[0].Value = item;
                dataGridView_DI.Rows[row].Cells[1].Value = "Off";
            }
        }

        private void InitDO()
        {
            dataGridView_DO.Columns.Clear();
            dataGridView_DO.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_DO.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            string[] Heads = { "DO名称", "输出值" };
            foreach (string str in Heads)
            {
                int col = dataGridView_DO.Columns.Add(str, str);
                dataGridView_DO.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_DO.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_DO.Columns[1].Width = 100;

            foreach (var item in IoMgr.GetInstance().m_dicOut.Keys)
            {
                //判断气缸中是否存在
                bool bExist = false;
                foreach (var cyl in CylinderMgr.GetInstance().m_dictCylinders.Values)
                {
                    if (cyl.m_strIoOuts[0] == item || cyl.m_strIoOuts[1] == item)
                    {
                        bExist = true;
                        break;
                    }
                }

                if (bExist)
                {
                    continue;
                }

                int row = dataGridView_DO.Rows.Add();

                dataGridView_DO.Rows[row].Cells[0].Value = item;
                dataGridView_DO.Rows[row].Cells[1].Value = "On";

                row = dataGridView_DO.Rows.Add();
                dataGridView_DO.Rows[row].Cells[0].Value = item;
                dataGridView_DO.Rows[row].Cells[1].Value = "Off";
            }
        }

        private void InitAxisMove()
        {
            dataGridView_AxisMove.Columns.Clear();
            dataGridView_AxisMove.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_AxisMove.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            string[] Heads = { "卡类型","轴号","操作", "描述", };
            foreach (string str in Heads)
            {
                int col = dataGridView_AxisMove.Columns.Add(str, str);
                dataGridView_AxisMove.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView_AxisMove.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (var item in MotionMgr.GetInstance().m_listCard)
            {
                for (int i = item.GetMinAxisNo(); i <= item.GetMaxAxisNo();i++)
                {
                    string strAxisName,strDesc;
                    StationBase sta = StationMgr.GetInstance().GetStation(i,out strAxisName);
                    if (sta != null)
                    {
                        strDesc = sta.Name + strAxisName;
                    }
                    else
                    {
                        strDesc = string.Format("{0} axis no assignment", i);
                    }

                    int row = dataGridView_AxisMove.Rows.Add();
                    dataGridView_AxisMove.Rows[row].Cells[0].Value = item.GetCardName();
                    dataGridView_AxisMove.Rows[row].Cells[1].Value = i;
                    dataGridView_AxisMove.Rows[row].Cells[2].Value = AxisMotion.On;
                    dataGridView_AxisMove.Rows[row].Cells[3].Value = strDesc;

                    row = dataGridView_AxisMove.Rows.Add();
                    dataGridView_AxisMove.Rows[row].Cells[0].Value = item.GetCardName();
                    dataGridView_AxisMove.Rows[row].Cells[1].Value = i;
                    dataGridView_AxisMove.Rows[row].Cells[2].Value = AxisMotion.Off;
                    dataGridView_AxisMove.Rows[row].Cells[3].Value = strDesc;

                    row = dataGridView_AxisMove.Rows.Add();
                    dataGridView_AxisMove.Rows[row].Cells[0].Value = item.GetCardName();
                    dataGridView_AxisMove.Rows[row].Cells[1].Value = i;
                    dataGridView_AxisMove.Rows[row].Cells[2].Value = AxisMotion.PEL;
                    dataGridView_AxisMove.Rows[row].Cells[3].Value = strDesc;

                    row = dataGridView_AxisMove.Rows.Add();
                    dataGridView_AxisMove.Rows[row].Cells[0].Value = item.GetCardName();
                    dataGridView_AxisMove.Rows[row].Cells[1].Value = i;
                    dataGridView_AxisMove.Rows[row].Cells[2].Value = AxisMotion.MEL;
                    dataGridView_AxisMove.Rows[row].Cells[3].Value = strDesc;

                    row = dataGridView_AxisMove.Rows.Add();
                    dataGridView_AxisMove.Rows[row].Cells[0].Value = item.GetCardName();
                    dataGridView_AxisMove.Rows[row].Cells[1].Value = i;
                    dataGridView_AxisMove.Rows[row].Cells[2].Value = AxisMotion.Home;
                    dataGridView_AxisMove.Rows[row].Cells[3].Value = strDesc;
                }
            }
        }

        private void InitTestItem()
        {
            dataGridView_TestItem.Columns.Clear();
            dataGridView_TestItem.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_TestItem.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            foreach (string str in AutoTestMgr.HEADS)
            {
                int col = dataGridView_TestItem.Columns.Add(str, str);
                dataGridView_TestItem.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_TestItem.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                if (col != 4)
                {
                    dataGridView_TestItem.Columns[col].ReadOnly = true;
                }
                else
                {
                    dataGridView_TestItem.Columns[col].ReadOnly = false;
                }
            }
        }

        private void Form_AutoTest_Load(object sender, EventArgs e)
        {
            AutoTestMgr.GetInstance().SetLogListBox(listBox_Log, OnLogEvent);
            AutoTestMgr.GetInstance().OnTestItemChanged += OnTestItemChanged;
            AutoTestMgr.GetInstance().OnTestFinished += OnTestFinished;
            AutoTestMgr.GetInstance().OnTestItemStart += OnTestItemStart;

            InitCylinder();

            InitDI();

            InitDO();

            InitAxisMove();

            InitTestItem();

            tabControl1.SelectedIndex = 0;

            m_testType = TestType.Cylinder;

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;
        }

        private void OnTestItemStart(int index, TestItem item)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                dataGridView_TestItem.CurrentCell = dataGridView_TestItem.Rows[index].Cells[0];

            });
        }

        private void OnTestFinished()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                //生成报表
                //生成测试报告
                string strFileName = string.Format("AutoTest_{0}.html", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string strFilePath = Path.Combine(SystemMgr.GetInstance().GetReportPath(), strFileName);

                AutoTestMgr.GetInstance().GenerateHtmlReport(strFilePath);

                System.Diagnostics.Process.Start(strFilePath);
            });
        }

        private void OnTestItemChanged(int index, TestItem item)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                dataGridView_TestItem.Rows[index].Cells[5].Value = item.Result ? "PASS":"FAIL";
                dataGridView_TestItem.Rows[index].Cells[6].Value = item.UsedTimeS.ToString("F3");
                dataGridView_TestItem.Rows[index].Cells[7].Value = item.Score.ToString("F2");
                dataGridView_TestItem.Rows[index].Cells[8].Value = item.Message;
            });
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

        private void OnChangeMode()
        {
            if (Security.GetUserMode() >= UserMode.Engineer)
            {
                groupBox_TestItem.Enabled = true;
                //groupBox_Operation.Enabled = true;
                foreach (Control ctrl in groupBox_Operation.Controls)
                {
                    ctrl.Enabled = true;
                }

                dataGridView_TestItem.Columns[4].ReadOnly = false;
            }
            else
            {
                groupBox_TestItem.Enabled = false;
                //groupBox_Operation.Enabled = false;
                foreach (Control ctrl in groupBox_Operation.Controls)
                {
                    if (ctrl.Name != button_ViewLastReport.Name)
                    {
                        ctrl.Enabled = false;
                    }
                }

                dataGridView_TestItem.Columns[4].ReadOnly = true;
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_Cylinder, dataGridView_DI, dataGridView_DO, dataGridView_AxisMove };

            DataGridView grid = grids[tabControl1.SelectedIndex];

            if (grid.SelectedCells.Count == 0)
            {
                return;
            }

            for(int i = grid.SelectedCells.Count-1;i >= 0; i--)
            {
                DataGridViewRow row = grid.SelectedCells[i].OwningRow;

                int nFromIndex = 0, nToIndex = 0;
                if (grid == dataGridView_AxisMove)
                {
                    nFromIndex = 1;
                }

                bool bExist = IsExist(m_testType.ToString(),
                    row.Cells[nFromIndex].Value.ToString(),
                    row.Cells[nFromIndex + 1].Value.ToString());

                if (bExist)
                {
                    string strMessage = string.Format("{0}:{1} - {2},已经添加，请确认是否需要继续添加?",
                        m_testType.ToString(), row.Cells[nFromIndex].Value.ToString(), row.Cells[nFromIndex + 1].Value.ToString());
                    if (DialogResult.No == MessageBox.Show(strMessage,"Warning",
                        MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2))
                    {
                        continue;
                    }
                }

                int testRow = dataGridView_TestItem.Rows.Add();

                dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = m_testType;
                dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[nFromIndex].Value.ToString();
                

                if (grid == dataGridView_AxisMove)
                {
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[3].Value.ToString();
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[2].Value.ToString();
                    
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 3 * 60 ;
                }
                else
                {
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[0].Value.ToString();
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[1].Value.ToString();

                    if (grid == dataGridView_Cylinder)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 10;
                    }
                    else if (grid == dataGridView_DI)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 3 * 60;
                    }
                    else if(grid == dataGridView_DO)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 60;
                    }
                }

                dataGridView_TestItem.CurrentCell = dataGridView_TestItem.Rows[testRow].Cells[0];
            }

        }

        private void button_Insert_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_Cylinder, dataGridView_DI, dataGridView_DO, dataGridView_AxisMove };

            DataGridView grid = grids[tabControl1.SelectedIndex];

            if (grid.SelectedCells.Count == 0) 
            {
                return;
            }

            int testRow = 0;
            for (int i = grid.SelectedCells.Count - 1; i >= 0;i--)
            {
                DataGridViewRow row = grid.SelectedCells[i].OwningRow;

                int nFromIndex = 0, nToIndex = 0;
                if (grid == dataGridView_AxisMove)
                {
                    nFromIndex = 1;
                }

                bool bExist = IsExist(m_testType.ToString(),
                    row.Cells[nFromIndex].Value.ToString(),
                    row.Cells[nFromIndex + 1].Value.ToString());

                if (bExist)
                {
                    string strMessage = string.Format("{0}:{1} - {2},已经添加，请确认是否需要继续添加?",
                        m_testType.ToString(), row.Cells[nFromIndex].Value.ToString(), row.Cells[nFromIndex + 1].Value.ToString());
                    if (DialogResult.No == MessageBox.Show(strMessage, "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                    {
                        continue;
                    }
                }

                if (dataGridView_TestItem.CurrentRow == null)
                {
                    testRow = dataGridView_TestItem.Rows.Add();
                }
                else
                {
                    dataGridView_TestItem.Rows.Insert(dataGridView_TestItem.CurrentRow.Index, 1);

                    testRow = dataGridView_TestItem.CurrentRow.Index - 1;
                }

                dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = m_testType;

                dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[nFromIndex++].Value.ToString();

                if (grid == dataGridView_AxisMove)
                {
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[3].Value.ToString();
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[2].Value.ToString();
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 3 * 60;
                }
                else
                {
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[0].Value.ToString();
                    dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = row.Cells[1].Value.ToString();

                    if (grid == dataGridView_Cylinder)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 10;
                    }
                    else if (grid == dataGridView_DI)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 3 * 60;
                    }
                    else if (grid == dataGridView_DO)
                    {
                        dataGridView_TestItem.Rows[testRow].Cells[nToIndex++].Value = 60;
                    }
                }

            }
           
            dataGridView_TestItem.CurrentCell = dataGridView_TestItem.Rows[testRow].Cells[0];
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {

            if (dataGridView_TestItem.SelectedCells.Count == 0)
            {
                return;
            }
            for (int i = dataGridView_TestItem.SelectedCells.Count-1; i >= 0;i--)
            {
                DataGridViewRow row = dataGridView_TestItem.SelectedCells[i].OwningRow;
                dataGridView_TestItem.Rows.Remove(row);
            }
            
        }

        private void button_MoveUp_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_TestItem.CurrentRow;

            if (row == null)
            {
                return;
            }

            int index = row.Index;

            if (index > 0)
            {
                dataGridView_TestItem.Rows.RemoveAt(index);
                dataGridView_TestItem.Rows.Insert(index - 1, row);

                dataGridView_TestItem.CurrentCell = dataGridView_TestItem.Rows[index - 1].Cells[0];
            }

            
        }

        private void button_MoveDown_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_TestItem.CurrentRow;

            if (row == null)
            {
                return;
            }

            int index = row.Index;

            if (index < dataGridView_TestItem.Rows.Count - 1)
            {
                dataGridView_TestItem.Rows.RemoveAt(index);
                dataGridView_TestItem.Rows.Insert(index + 1, row);

                dataGridView_TestItem.CurrentCell = dataGridView_TestItem.Rows[index + 1].Cells[0];
            }
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            dataGridView_TestItem.Rows.Clear();
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML File|*.xml";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string strFile = dlg.FileName;

                XmlDocument doc = new XmlDocument();
                doc.Load(strFile);

                AutoTestMgr.GetInstance().ReadCfgFromXml(doc);

                AutoTestMgr.GetInstance().UpdateGridFromParam(dataGridView_TestItem);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {

            AutoTestMgr.GetInstance().UpdateParamFromGrid(dataGridView_TestItem);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML File|*.xml";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string strFile = dlg.FileName;

                XmlDocument doc = new XmlDocument();

                AutoTestMgr.GetInstance().SaveCfgXML(doc);

                doc.Save(strFile);
            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            listBox_Log.Clear();

            foreach(DataGridViewRow row in dataGridView_TestItem.Rows)
            {
                row.Cells[5].Value = "";
                row.Cells[6].Value = "";
                row.Cells[7].Value = "";
                row.Cells[8].Value = "";
            }

            AutoTestMgr.GetInstance().ResetTestItems();

            AutoTestMgr.GetInstance().UpdateParamFromGrid(dataGridView_TestItem);

            AutoTestMgr.GetInstance().Start();
        }

        private void OnLogEvent(Control ctrl, string strLog, LogLevel level = LogLevel.Info)
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

                strLog = DateTime.Now.ToString("yyyyMMdd HH:mm:ss  - ") + strLog;

                logListBox.Append(strLog, color, logListBox.ForeColor);

                logListBox.TopIndex = logListBox.Items.Count - (int)(logListBox.Height / logListBox.ItemHeight);

            });
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            AutoTestMgr.GetInstance().Stop();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_testType = (TestType)tabControl1.SelectedIndex;
        }

        private void button_ViewLastReport_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(SystemMgr.GetInstance().GetReportPath(),"*.html");

            if (files != null && files.Length > 0)
            {
                DateTime recent = DateTime.MinValue;
                string strLastFile = "";
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);

                    if (fi.LastWriteTime > recent)
                    {
                        recent = fi.LastWriteTime;
                        strLastFile = file;
                    }
                }

                System.Diagnostics.Process.Start(strLastFile);
            }
        }

        private bool IsExist(string strType,string strName,string strOpertion)
        {
            foreach (DataGridViewRow row in dataGridView_TestItem.Rows)
            {
                if (strType == row.Cells[0].Value.ToString()
                    && strName == row.Cells[1].Value.ToString()
                    && strOpertion == row.Cells[3].Value.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        private void button_Continue_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_TestItem.Rows)
            {
                if(row.Cells[5].Value != null && row.Cells[5].Value.ToString() != "PASS")
                {
                    row.Cells[5].Value = "";
                    row.Cells[6].Value = "";
                    row.Cells[7].Value = "";
                    row.Cells[8].Value = "";
                }
            }

            AutoTestMgr.GetInstance().UpdateParamFromGrid(dataGridView_TestItem);

            AutoTestMgr.GetInstance().Start();
        }
    }
}
