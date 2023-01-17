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
using System.Diagnostics;
using AutoFrameDll;

namespace ToolEx
{
    public partial class Form_DataQuery : Form
    {
        private ComboBox m_comBoBox = new ComboBox();

        enum CUR_HEADS
        {
            名称,
            操作符,
            数值,
            逻辑运算,
        }

        enum SUM_HEADS
        {
            表1名称,
            表1项,
            表2名称,
            表2项,
            连接方式,
        }

        private readonly string[] OPERATORS = { "=", "!= ", ">", "<", ">=", "<=", "LIKE", "REGEXP" };

        private readonly string[] JOINS = { "INNER JOIN", "LEFT OUTER JOIN", "RIGHT OUTER JOIN", "UNION" };

        private Dictionary<string, string[]> m_dictTables = new Dictionary<string, string[]>();
        /// <summary>
        /// 数据库查询界面
        /// </summary>
        public Form_DataQuery()
        {
            InitializeComponent();

            InitDataBase();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);
            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }

        private void InitDataBase()
        {
            m_dictTables.Clear();

            SqlBase sql = new MySQL();

            if (!sql.Connect(DataMgr.GetInstance().Server,
                    DataMgr.GetInstance().Port,
                    DataMgr.GetInstance().UserID,
                    DataMgr.GetInstance().Password,
                    DataMgr.GetInstance().Database))
            {
                return;
            }

            //获取所有表
            string[] tables = sql.GetTables(DataMgr.GetInstance().Database);

            if (tables != null)
            {
                foreach (var item in tables)
                {
                    string[] cols = sql.GetTableColumns(DataMgr.GetInstance().Database, item);

                    if (cols != null && cols.Length > 0)
                    {
                        m_dictTables.Add(item, cols);
                    }
                }
            }
        }

        private void OnComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            DataGridView[] grids = { dataGridView_QueryTerm, dataGridView_SummaryQueryTerm };
            DataGridView grid = grids[tabControl_Main.SelectedIndex];
            if (grid.CurrentCell != null)
            {
                grid.CurrentCell.Value = combo.Text;
            }
        }

        private void Form_DataQuery_Load(object sender, EventArgs e)
        {
            //初始化DataGridView
            dataGridView_QueryTerm.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_QueryTerm.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            dataGridView_Current.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Current.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            dataGridView_SummaryQueryTerm.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_SummaryQueryTerm.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            dataGridView_Summary.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Summary.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            //初始化ComboBox
            m_comBoBox.DropDownStyle = ComboBoxStyle.DropDown;
            m_comBoBox.Visible = false;
            m_comBoBox.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
            m_comBoBox.LostFocus += OnComboBoxSelectedIndexChanged;

            dataGridView_QueryTerm.Controls.Add(m_comBoBox);

            comboBox_Count.Items.Add(10);
            comboBox_Count.Items.Add(20);
            comboBox_Count.Items.Add(30);
            comboBox_Count.Items.Add(50);
            comboBox_Count.Items.Add(100);
            comboBox_Count.Items.Add(200);
            comboBox_Count.Items.Add(500);
            comboBox_Count.Items.Add(1000);
            comboBox_Count.Items.Add(2000);
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                this.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, this.GetType().Name, this.Text);
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }

        private void dataGridView_QueryTerm_CurrentCellChanged(object sender, EventArgs e)
        {
            m_comBoBox.Visible = false;
            DataGridView grid = sender as DataGridView;
            if (grid.CurrentCell == null)
            {
                return;
            }

            int nColumnIndex = grid.CurrentCell.ColumnIndex;
            int nRowIndex = grid.CurrentCell.RowIndex;

            m_comBoBox.Items.Clear();
            m_comBoBox.Text = "";

            if (grid == dataGridView_QueryTerm)
            {
                switch (nColumnIndex)
                {
                    case (int)CUR_HEADS.名称:
                        {
                            m_comBoBox.Items.AddRange(DataMgr.GetInstance().m_dictDataSave.Keys.ToArray());

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;

                    case (int)CUR_HEADS.操作符:
                        {

                            m_comBoBox.Items.AddRange(OPERATORS);

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;

                    case (int)CUR_HEADS.逻辑运算:
                        {

                            m_comBoBox.Items.AddRange(new string[] { "AND", "OR" });

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;
                }
            }
            else if (grid == dataGridView_SummaryQueryTerm)
            {
                switch (nColumnIndex)
                {
                    case (int)SUM_HEADS.表1名称:
                    case (int)SUM_HEADS.表2名称:
                        {
                            m_comBoBox.Items.AddRange(m_dictTables.Keys.ToArray());

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;

                    case (int)SUM_HEADS.表1项:
                    case (int)SUM_HEADS.表2项:
                        {
                            if (grid.Rows[nRowIndex].Cells[nColumnIndex-1].Value == null)
                            {
                                return;
                            }

                            string strTableName = grid.Rows[nRowIndex].Cells[nColumnIndex-1].Value.ToString();
                            if (!m_dictTables.ContainsKey(strTableName))
                            {
                                return;
                            }

                            string[] array = m_dictTables[strTableName];

                            m_comBoBox.Items.AddRange(array);

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;

                    case (int)SUM_HEADS.连接方式:
                        {

                            m_comBoBox.Items.AddRange(JOINS);

                            Rectangle rect = grid.GetCellDisplayRectangle(nColumnIndex, nRowIndex, true);

                            m_comBoBox.Top = rect.Top;
                            m_comBoBox.Left = rect.Left;
                            m_comBoBox.Width = rect.Width;
                            m_comBoBox.Height = rect.Height;

                            if (grid.CurrentCell.Value != null)
                            {
                                m_comBoBox.Text = grid.CurrentCell.Value.ToString();
                            }
                            else
                            {
                                m_comBoBox.SelectedIndex = -1;
                            }

                            m_comBoBox.Visible = true;
                        }
                        break;
                }
            }
        }

        private void dataGridView_QueryTerm_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_comBoBox.Visible = false;
        }

        private void dataGridView_QueryTerm_Scroll(object sender, ScrollEventArgs e)
        {
            m_comBoBox.Visible = false;
        }

        private string Query(string strQuery, DataGridView grid)
        {
            SqlBase sql = new MySQL();

            if (!sql.Connect(DataMgr.GetInstance().Server,
                    DataMgr.GetInstance().Port,
                    DataMgr.GetInstance().UserID,
                    DataMgr.GetInstance().Password,
                    DataMgr.GetInstance().Database))
            {
                return "";
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DataTable dt = sql.ExecuteDataTable(strQuery);
            sw.Stop();

            grid.DataSource = dt;

            GroupBox[] groups = { groupBox_CurrentResult, groupBox_SummaryResult };

            if (dt != null)
            {
                return string.Format("Query Count : {0}  Used: {1} ms", dt.Rows.Count, sw.ElapsedMilliseconds);
            }
            else
            {
                return "";
            }

        }

        private void button_DataGridQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT * FROM {0}", DataMgr.GetInstance().TableName);

            bool bFirst = true;
            for (int i = 0; i < dataGridView_QueryTerm.Rows.Count - 1; i++)
            {
                if (dataGridView_QueryTerm.Rows[i].Cells[(int)CUR_HEADS.名称].Value == null
                    || dataGridView_QueryTerm.Rows[i].Cells[(int)CUR_HEADS.操作符].Value == null
                    || dataGridView_QueryTerm.Rows[i].Cells[(int)CUR_HEADS.数值].Value == null)
                {
                    continue;
                }

                if (bFirst)
                {
                    bFirst = false;
                    sb.Append(" WHERE ");
                }

                if (i < dataGridView_QueryTerm.Rows.Count - 2)
                {
                    sb.AppendFormat(@"{0} {1} '{2}' {3} "
                        , dataGridView_QueryTerm.Rows[i].Cells[0].Value
                        , dataGridView_QueryTerm.Rows[i].Cells[1].Value
                        , dataGridView_QueryTerm.Rows[i].Cells[2].Value
                        , dataGridView_QueryTerm.Rows[i].Cells[3].Value
                        );
                }
                else
                {
                    sb.AppendFormat(@"{0} {1} '{2}'"
                        , dataGridView_QueryTerm.Rows[i].Cells[0].Value
                        , dataGridView_QueryTerm.Rows[i].Cells[1].Value
                        , dataGridView_QueryTerm.Rows[i].Cells[2].Value
                        );
                }
            }

            sb.Append(";");

            if (sb.Length > 0)
            {
                textBox_QueryText.Text = sb.ToString();
            }

            groupBox_CurrentResult.Text = Query(textBox_QueryText.Text, dataGridView_Current);
        }

        private void button_TextBoxQuery_Click(object sender, EventArgs e)
        {
            groupBox_CurrentResult.Text = Query(textBox_QueryText.Text, dataGridView_Current);
        }

        private void button_RecentQuery_Click(object sender, EventArgs e)
        {
            string strSql = string.Format(@"SELECT * FROM {0} order by ID DESC limit {1};",
                DataMgr.GetInstance().TableName, comboBox_Count.Text);

            textBox_QueryText.Text = strSql;

            groupBox_CurrentResult.Text = Query(textBox_QueryText.Text, dataGridView_Current);
        }

        private void button_SummayQuery_Click(object sender, EventArgs e)
        {
            SqlBase sql = new MySQL();

            if (!sql.Connect(DataMgr.GetInstance().Server,
                    DataMgr.GetInstance().Port,
                    DataMgr.GetInstance().UserID,
                    DataMgr.GetInstance().Password,
                    DataMgr.GetInstance().Database))
            {
                return;
            }

            DataTable dt = sql.ExecuteDataTable(textBox_SummaryQuery.Text);

            dataGridView_Summary.DataSource = dt;

            if (dt != null)
            {
                groupBox_SummaryResult.Text = string.Format("Query Count : {0}", dt.Rows.Count);
            }
            else
            {
                groupBox_SummaryResult.Text = "";
            }
        }

        private void tabControl_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_comBoBox.Visible = false;

            int nSelectIndex = tabControl_Main.SelectedIndex;

            if (nSelectIndex == 0)
            {
                if (dataGridView_SummaryQueryTerm.Controls.Contains(m_comBoBox))
                {
                    dataGridView_SummaryQueryTerm.Controls.Remove(m_comBoBox);
                }

                if (!dataGridView_QueryTerm.Controls.Contains(m_comBoBox))
                {
                    dataGridView_QueryTerm.Controls.Add(m_comBoBox);
                }

                dataGridView_QueryTerm.ContextMenuStrip = contextMenuStrip1;
                dataGridView_SummaryQueryTerm.ContextMenuStrip = null;
            }
            else
            {
                if (dataGridView_QueryTerm.Controls.Contains(m_comBoBox))
                {
                    dataGridView_QueryTerm.Controls.Remove(m_comBoBox);
                }

                if (!dataGridView_SummaryQueryTerm.Controls.Contains(m_comBoBox))
                {
                    dataGridView_SummaryQueryTerm.Controls.Add(m_comBoBox);
                }

                dataGridView_QueryTerm.ContextMenuStrip = null;
                dataGridView_SummaryQueryTerm.ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void toolStripMenuItem_Add_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_QueryTerm, dataGridView_SummaryQueryTerm };
            DataGridView grid = grids[tabControl_Main.SelectedIndex];

            grid.Rows.Add();
        }

        private void toolStripMenuItem_Del_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_QueryTerm, dataGridView_SummaryQueryTerm };
            DataGridView grid = grids[tabControl_Main.SelectedIndex];
            if (grid.CurrentCell != null)
            {
                grid.Rows.RemoveAt(grid.CurrentCell.RowIndex);
            }
        }

        private void toolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_QueryTerm, dataGridView_SummaryQueryTerm };
            DataGridView grid = grids[tabControl_Main.SelectedIndex];

            grid.Rows.Clear();
        }

        private void button_DataGridSummaryQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT * FROM", DataMgr.GetInstance().TableName);

            bool bFirst = true;
            for (int i = 0; i < dataGridView_SummaryQueryTerm.Rows.Count; i++)
            {
                if (dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表1名称].Value == null
                    || dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表1项].Value == null
                    || dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表2名称].Value == null
                    || dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表2项].Value == null
                    || dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.连接方式].Value == null)
                {
                    continue;
                }

                if (bFirst)
                {
                    sb.AppendFormat(@" {0}"
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表1名称].Value
                    );

                    bFirst = false;
                }

                sb.AppendFormat(@" {0} {1} ON {2}.{3} = {4}.{5}"
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.连接方式].Value
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表2名称].Value
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表1名称].Value
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表1项].Value
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表2名称].Value
                    , dataGridView_SummaryQueryTerm.Rows[i].Cells[(int)SUM_HEADS.表2项].Value
                    );
            }

            sb.Append(";");

            if (sb.Length > 0)
            {
                textBox_SummaryQuery.Text = sb.ToString();
            }

            groupBox_SummaryResult.Text = Query(textBox_SummaryQuery.Text, dataGridView_Summary);
        }

        private void button_TextBoxSummaryQuery_Click(object sender, EventArgs e)
        {
            groupBox_SummaryResult.Text = Query(textBox_SummaryQuery.Text, dataGridView_Summary);
        }

        private void button_CurExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(dataGridView_Current);
        }

        private void button_SumExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(dataGridView_Summary);
        }

        private void ExportGridToExcel(DataGridView grid)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "CSV File(*.csv)|*.csv";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string path = dlg.FileName;

                    CsvOperation csv = new CsvOperation();
                    csv.FileName = path;

                    int row = 0, col = 0;
                    //写入标题
                    for (int k = 0; k < grid.Columns.Count; k++)
                    {
                        if (grid.Columns[k].Visible)//导出可见的标题
                        {
                            csv[row, col++] = grid.Columns[k].HeaderText.ToString();
                        }
                    }

                    row++;

                    for (int i = 0; i < grid.Rows.Count - 1; i++)
                    {
                        col = 0;
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < grid.Columns.Count; j++)
                        {
                            if (grid.Columns[j].Visible)//导出可见的单元格
                            {
                                csv[row, col++] = grid.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        row++;
                    }

                    csv.Save();

                    MessageBox.Show("Export success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


    }
}
