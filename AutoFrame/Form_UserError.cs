//2019 Binggoo 1.加入权限管理和中英文切换
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
using ToolEx;

namespace AutoFrame
{
    public partial class Form_UserError : Form
    {
        private ComboBox m_comboxSystemErrType = new ComboBox();
        public Form_UserError()
        {
            InitializeComponent();

            InitDataGridView();

            InitComBox();
        }

        private void InitComBox()
        {
            //m_comboxSystemErrType.DropDownStyle = ComboBoxStyle.DropDownList;
            m_comboxSystemErrType.Items.Clear();

            foreach (string item in Enum.GetNames(typeof(CommonTool.ErrorType)))
            {
                m_comboxSystemErrType.Items.Add(item);
            }

            m_comboxSystemErrType.SelectedIndexChanged += OnSelectedIndexChanged;
            m_comboxSystemErrType.LostFocus += OnSelectedIndexChanged;

            m_comboxSystemErrType.Visible = false;

            dataGridView_UserErrList.Controls.Add(m_comboxSystemErrType);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string strErrType = m_comboxSystemErrType.Text;
            dataGridView_UserErrList.CurrentCell.Value = strErrType;

            int rowIndex = dataGridView_UserErrList.CurrentCell.RowIndex;

            CommonTool.ErrorType errType;
            if (Enum.TryParse(strErrType,out errType))
            {
                dataGridView_UserErrList.Rows[rowIndex].Cells[2].Value = (int)errType;
            }
            
        }

        private void InitDataGridView()
        {
            dataGridView_UserErrList.Columns.Clear();
            dataGridView_UserErrList.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_UserErrList.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            int col = 0;
            foreach (string head in ErrorCodeMgr.GetInstance().Heads)
            {
                col = dataGridView_UserErrList.Columns.Add(head, head);

                dataGridView_UserErrList.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_UserErrList.Columns[0].Width = 200;
            dataGridView_UserErrList.Columns[0].ReadOnly = true;
            dataGridView_UserErrList.Columns[2].ReadOnly = true;
        }

        private void Form_UserError_Load(object sender, EventArgs e)
        {
            ErrorCodeMgr.GetInstance().UpdateGridFromParam(dataGridView_UserErrList);

            this.dataGridView_UserErrList.CurrentCellChanged += new System.EventHandler(this.dataGridView_UserErrList_CurrentCellChanged);

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

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

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            switch (Security.GetUserMode())
            {
                case UserMode.Administrator:
                case UserMode.Engineer:
                    dataGridView_UserErrList.ReadOnly = false;

                    button_MoveUp.Enabled = true;
                    button_MoveDown.Enabled = true;
                    button_Remove.Enabled = true;
                    button_Apply.Enabled = true;
                    button_Save.Enabled = true;

                    break;

                default:
                    dataGridView_UserErrList.ReadOnly = true;

                    button_MoveUp.Enabled = false;
                    button_MoveDown.Enabled = false;
                    button_Remove.Enabled = false;
                    button_Apply.Enabled = false;
                    button_Save.Enabled = false;
                    break;
            }
        }

        private void button_MoveUp_Click(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_UserErrList;

            if (grid.CurrentCell == null)
            {
                return;
            }

            int nSelectRow = grid.CurrentCell.RowIndex;
            if (nSelectRow > 0 && nSelectRow < grid.Rows.Count - 1)
            {
                DataGridViewRow row = grid.Rows[nSelectRow];

                grid.Rows.RemoveAt(nSelectRow);
                grid.Rows.Insert(nSelectRow - 1, row);
                grid.CurrentCell = grid.Rows[nSelectRow - 1].Cells[0];
            }
        }

        private void button_MoveDown_Click(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_UserErrList;

            if (grid.CurrentCell == null)
            {
                return;
            }

            int nSelectRow = grid.CurrentCell.RowIndex;
            if (nSelectRow < grid.Rows.Count - 2)
            {
                DataGridViewRow row = grid.Rows[nSelectRow];

                grid.Rows.RemoveAt(nSelectRow);
                grid.Rows.Insert(nSelectRow + 1, row);
                grid.CurrentCell = grid.Rows[nSelectRow + 1].Cells[0];
            }
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_UserErrList;

            if (grid.CurrentCell == null)
            {
                return;
            }

            int nSelectRow = grid.CurrentCell.RowIndex;
            if (nSelectRow < grid.Rows.Count - 1)
            {
                grid.Rows.RemoveAt(grid.CurrentCell.RowIndex);
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            ErrorCodeMgr.GetInstance().UpdateParamFromGrid(dataGridView_UserErrList);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            ErrorCodeMgr.GetInstance().UpdateParamFromGrid(dataGridView_UserErrList);

            ErrorCodeMgr.GetInstance().SaveUserError();

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save data complete");
            }
            else
            {
                MessageBox.Show("保存数据完成");
            }
            
        }

        private void dataGridView_UserErrList_CurrentCellChanged(object sender, EventArgs e)
        {
            m_comboxSystemErrType.Visible = false;
            if (dataGridView_UserErrList.CurrentCell == null)
            {
                return;
            }

            int ColumnIndex = dataGridView_UserErrList.CurrentCell.ColumnIndex;
            int RowIndex = dataGridView_UserErrList.CurrentCell.RowIndex;

            if (ColumnIndex == 0)
            {

                Rectangle rect = dataGridView_UserErrList.GetCellDisplayRectangle(ColumnIndex,
                    RowIndex, true);

                m_comboxSystemErrType.Top = rect.Top;
                m_comboxSystemErrType.Left = rect.Left;
                m_comboxSystemErrType.Width = rect.Width;
                m_comboxSystemErrType.Height = rect.Height;

                if (dataGridView_UserErrList.CurrentCell.Value != null)
                {
                    m_comboxSystemErrType.Text = dataGridView_UserErrList.CurrentCell.Value.ToString();
                }
                else
                {
                    m_comboxSystemErrType.SelectedIndex = -1;
                }

                m_comboxSystemErrType.Visible = true;
            }
        }

        private void dataGridView_UserErrList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_comboxSystemErrType.Visible = false;
        }

        private void dataGridView_UserErrList_Scroll(object sender, ScrollEventArgs e)
        {
            m_comboxSystemErrType.Visible = false;
        }
    }
}
