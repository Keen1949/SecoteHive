using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using AutoFrameDll;

namespace ToolEx
{
    /// <summary>
    /// 气缸管理界面
    /// </summary>
    public partial class Form_CylinderMgr : Form
    {
        private ComboBox comboBox_CylType = new ComboBox();
        private ComboBox comboBox_DO = new ComboBox();
        private ComboBox comboBox_DI = new ComboBox();

        private Dictionary<int, ComboBox> m_dictComboBox = new Dictionary<int, ComboBox>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Form_CylinderMgr()
        {
            InitializeComponent();

            InitDataGridView();

            InitComboBox();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language,true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

        }

        private void OnLanguageChangeEvent(string strLanguage,bool bChange)
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

        private void InitCylinderCtrl()
        {
            panel1.Controls.Clear();

            int n = 0;

            foreach (var cyl in CylinderMgr.GetInstance().m_dictCylinders.Values)
            {
                CylinderCtrl cylCtrl = new CylinderCtrl();
                cylCtrl.Name = "cylinder_" + cyl.m_strName;
                cylCtrl.CylinderObject = cyl;

                cylCtrl.IsCylinderSafeEvent += ManaulTool.IsCylinderSafe;

                //四舍五入法取整
                int cols = (int)Math.Round(panel1.Width * 1.0 / cylCtrl.Width);

                cylCtrl.Left = 1 + (n % cols) * cylCtrl.Width;
                cylCtrl.Top = 1 + (n / cols) * cylCtrl.Height;

                panel1.Controls.Add(cylCtrl);

                n++;
            }
        }

        private void InitComboBox()
        {
            comboBox_CylType.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox_DO.DropDownStyle = ComboBoxStyle.DropDown; 
            comboBox_DI.DropDownStyle = ComboBoxStyle.DropDown;


            comboBox_CylType.Items.Clear();
            comboBox_DO.Items.Clear();
            comboBox_DI.Items.Clear();

            foreach (var item in Enum.GetNames(typeof(CylType)))
            {

                comboBox_CylType.Items.Add(item);
            }

            foreach (var item in IoMgr.GetInstance().m_dicIn.Keys)
            {
                comboBox_DI.Items.Add(item);
            }

            foreach (var item in IoMgr.GetInstance().m_dicOut.Keys)
            {
                comboBox_DO.Items.Add(item);
            }

            comboBox_CylType.Visible = false;
            comboBox_DI.Visible = false;
            comboBox_DO.Visible = false;

            comboBox_CylType.SelectedIndexChanged += SelectedIndexChanged;
            comboBox_CylType.LostFocus += SelectedIndexChanged;

            comboBox_DI.SelectedIndexChanged += SelectedIndexChanged;
            comboBox_DI.LostFocus += SelectedIndexChanged;

            comboBox_DO.SelectedIndexChanged += SelectedIndexChanged;
            comboBox_DO.LostFocus += SelectedIndexChanged;

            this.dataGridView1.Controls.Add(comboBox_CylType);
            this.dataGridView1.Controls.Add(comboBox_DI);
            this.dataGridView1.Controls.Add(comboBox_DO);

            m_dictComboBox.Clear();

            m_dictComboBox.Add(2, comboBox_CylType);
            m_dictComboBox.Add(3, comboBox_DO);
            m_dictComboBox.Add(4, comboBox_DO);
            m_dictComboBox.Add(5, comboBox_DI);
            m_dictComboBox.Add(6, comboBox_DI);
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            dataGridView1.CurrentCell.Value = combo.Text;
        }

        void InitDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            int nCol;
            foreach (string head in CylinderMgr.HEADS)
            {
                nCol = dataGridView1.Columns.Add(head, head);
                dataGridView1.Columns[nCol].SortMode = DataGridViewColumnSortMode.NotSortable; 
            }
            dataGridView1.Columns[0].Width = 180;
            dataGridView1.Columns[1].Width = 180;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[4].Width = 180;
            dataGridView1.Columns[5].Width = 180;
            dataGridView1.Columns[6].Width = 180;
        }

        private void Form_CylinderMgr_Load(object sender, EventArgs e)
        {
            CylinderMgr.GetInstance().UpdateGridFromParam(dataGridView1);

            InitCylinderCtrl();

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

            if (Security.IsOpMode())
            {
                foreach(Control ctrl in panel1.Controls)
                {
                    ctrl.Enabled = false;
                }
            }
            else
            {
                foreach (Control ctrl in panel1.Controls)
                {
                    ctrl.Enabled = true;
                }
            }
        }

        private void dataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            comboBox_CylType.Visible = false;
            comboBox_DO.Visible = false;
            comboBox_DI.Visible = false;

            if (dataGridView1.CurrentCell == null)
            {
                return;
            }

            int ColumnIndex = dataGridView1.CurrentCell.ColumnIndex;

            if (m_dictComboBox.ContainsKey(ColumnIndex))
            {
                ComboBox combo = m_dictComboBox[ColumnIndex];

                Rectangle rect = dataGridView1.GetCellDisplayRectangle(ColumnIndex,
                    dataGridView1.CurrentCell.RowIndex, true);

                combo.Top = rect.Top;
                combo.Left = rect.Left;
                combo.Width = rect.Width;
                combo.Height = rect.Height;

                if (dataGridView1.CurrentCell.Value != null)
                {
                    combo.Text = dataGridView1.CurrentCell.Value.ToString();
                }
                else
                {
                    combo.SelectedIndex = -1;
                }

                combo.Visible = true;
            }
            else
            {
                comboBox_CylType.Visible = false;
                comboBox_DO.Visible = false;
                comboBox_DI.Visible = false;
            }
        }

        private void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            comboBox_CylType.Visible = false;
            comboBox_DO.Visible = false;
            comboBox_DI.Visible = false;
        }

        private void dataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            comboBox_CylType.Visible = false;
            comboBox_DO.Visible = false;
            comboBox_DI.Visible = false;
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            CylinderMgr.GetInstance().UpdateParamFromGrid(dataGridView1);

            InitCylinderCtrl();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            CylinderMgr.GetInstance().UpdateParamFromGrid(dataGridView1);

            InitCylinderCtrl();

            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            CylinderMgr.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save cylinder configuration complete");
            }
            else
            {
                MessageBox.Show("保存气缸配置完成");
            }
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            int nSelectRow = dataGridView1.CurrentCell.RowIndex;
            if (nSelectRow < dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
        }

        private void Form_CylinderMgr_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            try
            {
                int n = 0;
                foreach (Control cylCtrl in panel1.Controls)
                {
                    if (cylCtrl is CylinderCtrl)
                    {
                        //四舍五入法取整
                        int cols = (int)Math.Round(panel1.Width * 1.0 / cylCtrl.Width);

                        cylCtrl.Left = 1 + (n % cols) * cylCtrl.Width;
                        cylCtrl.Top = 1 + (n / cols) * cylCtrl.Height;

                        n++;
                    }

                }
            }
            catch (Exception)
            {
            }
            
        }
    }
}
