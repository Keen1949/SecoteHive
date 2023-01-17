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
using System.Reflection;
using System.Xml;
using System.IO;
using ToolEx;


namespace AutoFrame
{
    public partial class Form_Select_Calib : Form
    {
        private List<RadioButton> m_listRadioButton = new List<RadioButton>();

        private ComboBox m_comboBoxStation = new ComboBox();

        private ComboBox m_comboBoxMethod = new ComboBox();

        public Form_Select_Calib()
        {
            InitializeComponent();

            InitComboBox();

            InitDataGridView();

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


        private void InitComboBox()
        {
            m_comboBoxStation.DropDownStyle = ComboBoxStyle.DropDownList;
            m_comboBoxStation.Sorted = true;
            m_comboBoxStation.SelectedIndexChanged += SelectedIndexChanged;
            m_comboBoxStation.LostFocus += SelectedIndexChanged;

            m_comboBoxStation.Items.Clear();
            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                m_comboBoxStation.Items.Add(sb.Name);
            }

            m_comboBoxMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            m_comboBoxMethod.Sorted = true;
            m_comboBoxMethod.SelectedIndexChanged += SelectedIndexChanged;
            m_comboBoxMethod.LostFocus += SelectedIndexChanged;

            dataGridView_Calib.Controls.Add(m_comboBoxStation);
            dataGridView_Calib.Controls.Add(m_comboBoxMethod);

            m_comboBoxStation.Visible = false;
            m_comboBoxMethod.Visible = false;
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            dataGridView_Calib.CurrentCell.Value = combo.Text;
        }

        private void InitDataGridView()
        {
            dataGridView_Calib.Columns.Clear();

            dataGridView_Calib.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Calib.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            string[] Heads = { "名称", "站位", "方法" };

            foreach(string head in Heads)
            {
                int col = dataGridView_Calib.Columns.Add(head,head);
                dataGridView_Calib.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Calib.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void Form_Select_Calib_Load(object sender, EventArgs e)
        {
            SystemMgrEx.GetInstance().UpdateGridFromParam(RunMode.Calib, dataGridView_Calib);
            UpdateRadioButtons();

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
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }
                
            }
        }

        private void UpdateComboBoxMethod(string strStation)
        {
            m_comboBoxMethod.Items.Clear();

            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                if (sb.Name == strStation)
                {
                    Type t = sb.GetType();

                    MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

                    foreach (MethodInfo method in methods)
                    {
                        m_comboBoxMethod.Items.Add(method.Name);
                    }

                    break;
                }
            }
        }

        private void UpdateRadioButtons()
        {
            panel_Calib.Controls.Clear();
            m_listRadioButton.Clear();

            int btnWidth = 200;
            int btnHeight = 36;

            int cols = panel_Calib.Width / btnWidth;
            if (cols < 1)
            {
                cols = 1;
            }

            //动态创建按钮
            int nIndex = 0;
            foreach(var item in SystemMgrEx.GetInstance().m_dictCalibs.Values)
            {
                RadioButton radio = new RadioButton();
                radio.Name = "radiobutton" + nIndex;
                radio.Text = item.m_strName;
                radio.Left = 1 + (nIndex % cols) * btnWidth;
                radio.Top = 1 + (nIndex / cols) * btnHeight;
                radio.Height = btnHeight;
                radio.Width = btnWidth;

                if (SystemMgrEx.GetInstance().CurrentCalib == item.m_strName)
                {
                    radio.Checked = true;
                }
                else
                {
                    radio.Checked = false;
                }
                
                panel_Calib.Controls.Add(radio);

                m_listRadioButton.Add(radio);

                nIndex++;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (var radio in m_listRadioButton)
            {
                if (radio.Checked)
                {       
                    RunModeInfo info;

                    if (SystemMgrEx.GetInstance().m_dictCalibs.TryGetValue(radio.Text,out info))
                    {
                        SystemMgrEx.GetInstance().CurrentCalib = radio.Text;

                        foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
                        {
                            sb.StationEnable = false;
                        }

                        StationMgr.GetInstance().GetStation(info.m_strStation).StationEnable = true;
                    }
                    
                    else
                    {
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            MessageBox.Show("This calibration item does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else
                        {
                            MessageBox.Show("不存在此标定项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }   

                }
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_Calib_CurrentCellChanged(object sender, EventArgs e)
        {
            m_comboBoxStation.Visible = false;
            m_comboBoxMethod.Visible = false;

            if (dataGridView_Calib.CurrentCell == null)
            {
                return;
            }

            int rowIndex = dataGridView_Calib.CurrentCell.RowIndex;
            int colIndex = dataGridView_Calib.CurrentCell.ColumnIndex;

            if (colIndex == 1)
            {
                Rectangle rect = dataGridView_Calib.GetCellDisplayRectangle(colIndex, rowIndex, true);

                m_comboBoxStation.Top = rect.Top;
                m_comboBoxStation.Left = rect.Left;
                m_comboBoxStation.Width = rect.Width;
                m_comboBoxStation.Height = rect.Height;

                if (dataGridView_Calib.CurrentCell.Value != null)
                {
                    m_comboBoxStation.Text = dataGridView_Calib.CurrentCell.Value.ToString();
                }
                else
                {
                    m_comboBoxStation.SelectedIndex = -1;
                }

                m_comboBoxStation.Visible = true;
            }
            else if(colIndex == 2)
            {
                if (dataGridView_Calib.Rows[rowIndex].Cells[1].Value == null)
                {
                    return;
                }

                UpdateComboBoxMethod(dataGridView_Calib.Rows[rowIndex].Cells[1].Value.ToString());

                Rectangle rect = dataGridView_Calib.GetCellDisplayRectangle(colIndex, rowIndex, true);

                m_comboBoxMethod.Top = rect.Top;
                m_comboBoxMethod.Left = rect.Left;
                m_comboBoxMethod.Width = rect.Width;
                m_comboBoxMethod.Height = rect.Height;

                if (dataGridView_Calib.CurrentCell.Value != null)
                {
                    m_comboBoxMethod.Text = dataGridView_Calib.CurrentCell.Value.ToString();
                }
                else
                {
                    m_comboBoxMethod.SelectedIndex = -1;
                }

                m_comboBoxMethod.Visible = true;
            }
        }

        private void dataGridView_Calib_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_comboBoxStation.Visible = false;
            m_comboBoxMethod.Visible = false;
        }

        private void dataGridView_Calib_Scroll(object sender, ScrollEventArgs e)
        {
            m_comboBoxStation.Visible = false;
            m_comboBoxMethod.Visible = false;
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            int nSelectRow = dataGridView_Calib.CurrentCell.RowIndex;

            if (nSelectRow < dataGridView_Calib.Rows.Count - 1)
            {
                dataGridView_Calib.Rows.RemoveAt(nSelectRow);
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            SystemMgrEx.GetInstance().UpdateParamFromGrid(RunMode.Calib, dataGridView_Calib);

            UpdateRadioButtons();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SystemMgrEx.GetInstance().UpdateParamFromGrid(RunMode.Calib, dataGridView_Calib);

            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            SystemMgrEx.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save configuration complete");

            }
            else
            {
                MessageBox.Show("保存配置完成");

            }
        }
    }
}
