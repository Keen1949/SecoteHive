using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace ToolEx
{
    /// <summary>
    /// 光源管理界面
    /// </summary>
    public partial class Form_LightMgr : Form
    {
        private ComboBox m_comboBox_LightType = new ComboBox();

        /// <summary>
        /// 光源管理
        /// </summary>
        public Form_LightMgr()
        {
            InitializeComponent();

            InitDataGridView();

            InitComboBox();

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

        private void Form_LightMgr_Load(object sender, EventArgs e)
        {
            LightMgr.GetInstance().UpdateGridFromParam(dataGridView1);

            LightMgr.GetInstance().UpdateGridFromParam(dataGridView2);

            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {
            if(Security.GetUserMode() >= UserMode.Engineer)
            {  
                if (!tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Add(tabPage2);
                }
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }    
            }

            switch (Security.GetUserMode())
            {
                case UserMode.Administrator:
                case UserMode.Engineer:
                    if (!tabControl1.TabPages.Contains(tabPage2))
                    {
                        tabControl1.TabPages.Add(tabPage2);
                    }
                    dataGridView1.ReadOnly = false;
                    button_Open.Enabled = true;
                    button_Close.Enabled = true;
                    trackBar_Light.Enabled = true;
                    break;

                case UserMode.Adjustor:
                    if (tabControl1.TabPages.Contains(tabPage2))
                    {
                        tabControl1.TabPages.Remove(tabPage2);
                    }
                    dataGridView1.ReadOnly = false;
                    button_Open.Enabled = true;
                    button_Close.Enabled = true;
                    trackBar_Light.Enabled = true;
                    break;

                case UserMode.FAE:
                case UserMode.Operator:
                    if (tabControl1.TabPages.Contains(tabPage2))
                    {
                        tabControl1.TabPages.Remove(tabPage2);
                    }
                    dataGridView1.ReadOnly = true;
                    button_Open.Enabled = false;
                    button_Close.Enabled = false;
                    trackBar_Light.Enabled = false;
                    break;
            }
        }

        private void InitDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            //去掉空白行
            dataGridView1.AllowUserToAddRows = false;

            int nCol;
            foreach (string head in LightMgr.HEADS)
            {
                nCol = dataGridView1.Columns.Add(head,head);
                dataGridView1.Columns[nCol].ReadOnly = true;
                dataGridView1.Columns[nCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[nCol].Width = 80;

                nCol = dataGridView2.Columns.Add(head, head);
                dataGridView2.Columns[nCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[nCol].Width = 80;
            }

            nCol= dataGridView1.Columns.Add("亮度", "亮度");
            dataGridView1.Columns[nCol].Width = 80;

            dataGridView1.Columns[0].Width = 120;
            dataGridView2.Columns[0].Width = 120;

        }

        private void InitComboBox()
        {
            m_comboBox_LightType.DropDownStyle = ComboBoxStyle.DropDown;


            m_comboBox_LightType.Items.Clear();

            Assembly assembly = Assembly.GetAssembly(typeof(LightBase));

            foreach (var t in assembly.GetTypes())
            {
                if(t.BaseType != null)
                {
                    if (t.BaseType.Name == typeof(LightBase).Name)
                    {
                        m_comboBox_LightType.Items.Add(t.Name);
                    }
                }
            }

            m_comboBox_LightType.Visible = false;

            m_comboBox_LightType.SelectedIndexChanged += SelectedIndexChanged;
            m_comboBox_LightType.LostFocus += SelectedIndexChanged;

            this.dataGridView2.Controls.Add(m_comboBox_LightType);
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.CurrentCell.Value = m_comboBox_LightType.Text;
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int nLight = trackBar_Light.Value;
                //获取行
                int nRow = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows[nRow].Cells[4].Value = nLight;

                if (dataGridView1.Rows[nRow].Cells[0].Value != null)
                {
                    LightBase light = LightMgr.GetInstance().GetLight(dataGridView1.Rows[nRow].Cells[0].Value.ToString());

                    if (light != null)
                    {
                        light.LightOn(nLight);
                    }
                }

            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                //获取行
                int nRow = dataGridView1.CurrentCell.RowIndex;

                if (dataGridView1.Rows[nRow].Cells[0].Value != null)
                {
                    LightBase light = LightMgr.GetInstance().GetLight(dataGridView1.Rows[nRow].Cells[0].Value.ToString());

                    if (light != null)
                    {
                        light.LightOff();
                    }
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            { 
                //获取行
                int nRow = dataGridView1.CurrentCell.RowIndex;

                if (dataGridView1.Rows[nRow].Cells[4].Value != null)
                {
                    string strValue = dataGridView1.Rows[nRow].Cells[4].Value.ToString();

                    if (strValue.Length == 0)
                    {
                        strValue = "255";
                        dataGridView1.Rows[nRow].Cells[4].Value = strValue;
                    }
                    int nLight = Convert.ToInt32(strValue);

                    trackBar_Light.Value = nLight;
                }
            }
        }

        private void trackBar_Light_Scroll(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int nLight = trackBar_Light.Value;
                //获取行
                int nRow = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows[nRow].Cells[4].Value = nLight;

                if (dataGridView1.Rows[nRow].Cells[0].Value != null)
                {
                    LightBase light = LightMgr.GetInstance().GetLight(dataGridView1.Rows[nRow].Cells[0].Value.ToString());

                    if (light != null)
                    {
                        light.LightOn(nLight);
                    }
                }
                
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            LightMgr.GetInstance().UpdateParamFromGrid(dataGridView2);

            LightMgr.GetInstance().UpdateGridFromParam(dataGridView1);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            LightMgr.GetInstance().UpdateParamFromGrid(dataGridView2);

            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            LightMgr.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save light configuration complete");
            }
            else
            {
                MessageBox.Show("保存光源配置完成");
            }
            
            LightMgr.GetInstance().UpdateGridFromParam(dataGridView1);
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            int nSelectRow = dataGridView2.CurrentCell.RowIndex;
            if (nSelectRow < dataGridView2.Rows.Count - 1)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentCell.RowIndex);
            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell == null)
            {
                m_comboBox_LightType.Visible = false;
                return;
            }

            if (dataGridView2.CurrentCell.ColumnIndex == 1)
            {
                Rectangle rect = dataGridView2.GetCellDisplayRectangle(dataGridView2.CurrentCell.ColumnIndex, 
                    dataGridView2.CurrentCell.RowIndex, true);

                m_comboBox_LightType.Top = rect.Top;
                m_comboBox_LightType.Left = rect.Left;
                m_comboBox_LightType.Width = rect.Width;
                m_comboBox_LightType.Height = rect.Height;

                if (dataGridView2.CurrentCell.Value != null)
                {
                    m_comboBox_LightType.Text = dataGridView2.CurrentCell.Value.ToString();
                }
                else
                {
                    m_comboBox_LightType.SelectedIndex = -1;
                }

                m_comboBox_LightType.Visible = true;
            }
            else
            {
                m_comboBox_LightType.Visible = false;
            }
        }

        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            m_comboBox_LightType.Visible = false;
        }

        private void dataGridView2_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_comboBox_LightType.Visible = false;
        }
    }
}
