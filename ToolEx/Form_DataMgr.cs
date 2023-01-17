
using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ToolEx
{
    /// <summary>
    /// 数据管理界面
    /// </summary>
    public partial class Form_DataMgr : Form
    {
        private ComboBox comboBox_DataType = new ComboBox();
        private ComboBox comboBox_DataIndex = new ComboBox();
        private int m_nCurSelectTab = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Form_DataMgr()
        {
            InitializeComponent();

            InitComboBox();

            InitDataGridView();

            comboBox_Level.Items.Clear();

            foreach(UserMode item in Enum.GetValues(typeof(UserMode)))
            {  
                if (item != UserMode.Administrator)
                {
                    comboBox_Level.Items.Add(item.ToString());
                }
            }

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
            dataGridView_Data.Columns.Clear();
            dataGridView_Data.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Data.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            //string[] Heads = { "名称", "数据类型", "数据索引", "标准值", "上限/Fixture_ID", "下限/Head_ID", "补偿值", "单位" };
            foreach (string str in DataMgr.m_strDescribe)
            {
                int col = dataGridView_Data.Columns.Add(str, str);
                //dataGridView_Data.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells ;
                dataGridView_Data.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView_Data.Columns[0].Width = 200;
            dataGridView_Data.Columns[2].Width = 200 ;

            string[] heads = { "名称", "数据索引" };
            dataGridView_DataShow.Columns.Clear();
            dataGridView_DataShow.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_DataShow.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            foreach (string str in heads)
            {
                int col = dataGridView_DataShow.Columns.Add(str, str);
         
                dataGridView_DataShow.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridView_DataShow.Columns[col].Width = 200;
            }

            dataGridView_DataSave.Columns.Clear();
            dataGridView_DataSave.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_DataSave.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            foreach (string str in heads)
            {
                int col = dataGridView_DataSave.Columns.Add(str, str);

                dataGridView_DataSave.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridView_DataSave.Columns[col].Width = 200;
            }

            

        }

        private void InitComboBox()
        {

            //数据类型
            comboBox_DataType.DropDownStyle = ComboBoxStyle.DropDown;
           
            comboBox_DataType.Items.Clear();

            foreach (var item in Enum.GetValues(typeof (DataType)))
            {
                string strValue = String.Format("{0}", item.ToString());

                comboBox_DataType.Items.Add(strValue);
            }

            comboBox_DataType.Visible = false;

            comboBox_DataType.SelectedIndexChanged += SelectedIndexChanged;
            comboBox_DataType.LostFocus += SelectedIndexChanged;

            //数据索引
            comboBox_DataIndex.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox_DataIndex.Items.Clear();

            Type t = typeof(ProductData);
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            //字段
            foreach (FieldInfo field in fields)
            {
                string strValue = String.Format("{0}", field.Name);

                if (field.FieldType.IsArray)
                {
                    ProductData data = new ProductData();

                    Array list = (Array)data.GetValue(field.Name);

                    for(int i = 0; i < list.Length;i++)
                    {
                        strValue = String.Format("{0}[{1}]", field.Name, i);

                        comboBox_DataIndex.Items.Add(strValue);
                    }
                }
                else
                {
                    comboBox_DataIndex.Items.Add(strValue);
                }
            }

            //属性
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                string strValue = String.Format("{0}", pro.Name);

                if (pro.PropertyType.IsArray)
                {
                    ProductData data = new ProductData();

                    Array list = (Array)data.GetValue(pro.Name);

                    for (int i = 0; i < list.Length; i++)
                    {
                        strValue = String.Format("{0}[{1}]", pro.Name, i);

                        comboBox_DataIndex.Items.Add(strValue);
                    }
                }
                else
                {
                    comboBox_DataIndex.Items.Add(strValue);
                }
            }

            comboBox_DataIndex.Visible = false;

            comboBox_DataIndex.SelectedIndexChanged += SelectedIndexChanged;
            comboBox_DataIndex.LostFocus += SelectedIndexChanged;

            this.dataGridView_Data.Controls.Add(comboBox_DataType);
            this.dataGridView_Data.Controls.Add(comboBox_DataIndex);
        }


        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            DataGridView grid = grids[tabControl_Data.SelectedIndex];

            grid.CurrentCell.Value = combo.Text;   
            
        }

        private void Form_DataMgr_Load(object sender, EventArgs e)
        {
            comboBox_GroupName.DataSource = DataMgr.GetInstance().m_dictDataGroup.Keys.ToArray();

            DataMgr.GetInstance().UpdateDataGridFromParam(dataGridView_DataShow, dataGridView_DataSave);

            textBox_SavePath.Text = DataMgr.GetInstance().SavePath;

            checkBox_ShowSave.Checked = DataMgr.GetInstance().DataShowSaveEnable;

            checkBox_SaveData.Checked = DataMgr.GetInstance().DataSaveEnable;

            //2020-03-02 Binggoo 新增数据库保存
            textBox_ServerIp.Text = DataMgr.GetInstance().Server;
            textBox_Port.Text = DataMgr.GetInstance().Port.ToString();
            textBox_UserID.Text = DataMgr.GetInstance().UserID;
            textBox_Password.Text = DataMgr.GetInstance().Password;
            textBox_Database.Text = DataMgr.GetInstance().Database;
            textBox_TableName.Text = DataMgr.GetInstance().TableName;

            comboBox_SaveType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_SaveType.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(SaveType)))
            {
                comboBox_SaveType.Items.Add(item);
            }

            comboBox_SaveType.SelectedIndex = (int)DataMgr.GetInstance().SaveType;

            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;

            //2020-05-15 Binggoo 新增数据改变事件
            DataMgr.GetInstance().StandardValueChangedEvent += OnStandardValueChangedEvent;
            DataMgr.GetInstance().LimitLValueChangedEvent += OnLimitLValueChangedEvent;
            DataMgr.GetInstance().LimitUValueChangedEvent += OnLimitUValueChangedEvent;
            DataMgr.GetInstance().OffsetValueChangedEvent += OnOffsetValueChangedEvent;
        }

        private void OnOffsetValueChangedEvent(string strGroup, string strData, TValue oldValue, TValue newValue)
        {
            string strName = strGroup + "." + strData + ".Offset";
            ProductMgr.LogChanged(strName, oldValue.ToString(), newValue.ToString());
        }

        private void OnLimitUValueChangedEvent(string strGroup, string strData, TValue oldValue, TValue newValue)
        {
            string strName = strGroup + "." + strData + ".LimitU";
            ProductMgr.LogChanged(strName, oldValue.ToString(), newValue.ToString());
        }

        private void OnLimitLValueChangedEvent(string strGroup, string strData, TValue oldValue, TValue newValue)
        {
            string strName = strGroup + "." + strData + ".LimitL";
            ProductMgr.LogChanged(strName, oldValue.ToString(), newValue.ToString());
        }

        private void OnStandardValueChangedEvent(string strGroup, string strData, TValue oldValue, TValue newValue)
        {
            string strName = strGroup + "." + strData + ".Standard";
            ProductMgr.LogChanged(strName, oldValue.ToString(), newValue.ToString());
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {
            if (Security.IsOpMode())
            {
                comboBox_GroupName.DropDownStyle = ComboBoxStyle.DropDownList;
                textBox_Version.Enabled = false;
                checkBox_PDCA.Enabled = false;
                button_Apply.Enabled = false;
                button_Del.Enabled = false;
                button_DelGroup.Enabled = false;
                button_MoveDown.Enabled = false;
                button_MoveUp.Enabled = false;
                button_Save.Enabled = false;
                button_RecoverDefault.Enabled = false;
                button_Restore.Enabled = false;
                button_SaveDefault.Enabled = false;

                dataGridView_Data.ReadOnly = true;
                dataGridView_DataShow.ReadOnly = true;
                dataGridView_DataSave.ReadOnly = true;
                comboBox_DataType.Enabled = false;
                comboBox_DataIndex.Enabled = false;

                textBox_SavePath.Enabled = false;
                checkBox_ShowSave.Enabled = false;
                checkBox_SaveData.Enabled = false;

                comboBox_Level.Enabled = false;

                button_Browse.Enabled = false;

                //2020-03-02 Binggoo 加入数据库
                comboBox_SaveType.Enabled = false;
                textBox_ServerIp.Enabled = false;
                textBox_Port.Enabled = false;
                textBox_UserID.Enabled = false;
                textBox_Password.Enabled = false;
                textBox_Database.Enabled = false;
                textBox_TableName.Enabled = false;
            }
            else
            {
                if (Security.GetUserMode() >= UserMode.Engineer)
                {
                    comboBox_GroupName.DropDownStyle = ComboBoxStyle.DropDown;
                    button_DelGroup.Enabled = true;
                    button_Del.Enabled = true;
                    dataGridView_Data.Rows[dataGridView_Data.RowCount - 1].ReadOnly = false;
                    dataGridView_DataShow.Rows[dataGridView_DataShow.RowCount - 1].ReadOnly = false;
                    dataGridView_DataSave.Rows[dataGridView_DataSave.RowCount - 1].ReadOnly = false;

                    button_RecoverDefault.Enabled = true;
                    button_Restore.Enabled = true;
                    button_SaveDefault.Enabled = true;

                    textBox_SavePath.Enabled = true;
                    checkBox_ShowSave.Enabled = true;
                    checkBox_SaveData.Enabled = true;
                    checkBox_PDCA.Enabled = true;

                    comboBox_Level.Enabled = true;

                    button_Browse.Enabled = true;

                    //2020-03-02 Binggoo 加入数据库
                    comboBox_SaveType.Enabled = true;
                    textBox_ServerIp.Enabled = true;
                    textBox_Port.Enabled = true;
                    textBox_UserID.Enabled = true;
                    textBox_Password.Enabled = true;
                    textBox_Database.Enabled = true;
                    textBox_TableName.Enabled = true;

                    //2020-05-15 增加权限管理
                    dataGridView_Data.Columns[0].ReadOnly = false;
                    dataGridView_Data.Columns[7].ReadOnly = false;
                }
                else
                {
                    comboBox_GroupName.DropDownStyle = ComboBoxStyle.DropDownList;
                    button_DelGroup.Enabled = false;
                    button_Del.Enabled = false;
                    dataGridView_Data.Rows[dataGridView_Data.RowCount - 1].ReadOnly = true;
                    dataGridView_DataShow.Rows[dataGridView_DataShow.RowCount - 1].ReadOnly = true;
                    dataGridView_DataSave.Rows[dataGridView_DataSave.RowCount - 1].ReadOnly = true;

                    button_RecoverDefault.Enabled = false;
                    button_Restore.Enabled = false;
                    button_SaveDefault.Enabled = false;

                    textBox_SavePath.Enabled = false;
                    checkBox_ShowSave.Enabled = false;
                    checkBox_SaveData.Enabled = false;
                    checkBox_PDCA.Enabled = false;

                    comboBox_Level.Enabled = false;

                    button_Browse.Enabled = false;

                    //2020-03-02 Binggoo 加入数据库
                    comboBox_SaveType.Enabled = false;
                    textBox_ServerIp.Enabled = false;
                    textBox_Port.Enabled = false;
                    textBox_UserID.Enabled = false;
                    textBox_Password.Enabled = false;
                    textBox_Database.Enabled = false;
                    textBox_TableName.Enabled = false;

                    //2020-05-15 增加权限管理
                    dataGridView_Data.Columns[0].ReadOnly = true;
                    dataGridView_Data.Columns[7].ReadOnly = true;
                }

                button_Apply.Enabled = true;
                button_MoveDown.Enabled = true;
                button_MoveUp.Enabled = true;
                
                dataGridView_DataShow.ReadOnly = false;
                dataGridView_DataSave.ReadOnly = false;

                comboBox_DataType.Enabled = true;
                comboBox_DataIndex.Enabled = true;

                comboBox_Level_SelectedIndexChanged(null, null);

            }
        }

        private void comboBox_GroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strGroupName = comboBox_GroupName.Text;

            if (DataMgr.GetInstance().m_dictDataGroup.ContainsKey(strGroupName))
            {
                DataGroup group = DataMgr.GetInstance().m_dictDataGroup[strGroupName];

                textBox_Version.Text = group.m_strVersion;
                checkBox_PDCA.Checked = group.m_bIsPDCA;
                comboBox_Level.SelectedIndex = group.m_nLevel;
                checkBox_PDCA_CheckedChanged(null, null);
                DataMgr.GetInstance().UpdateGridFromParam(strGroupName, dataGridView_Data);

                dataGridView_Data.ReadOnly = ((int)Security.GetUserMode() < group.m_nLevel);

                if (Security.IsEngMode())
                {
                    dataGridView_Data.Rows[dataGridView_Data.RowCount - 1].ReadOnly = false;
                }
                else
                {
                    dataGridView_Data.Rows[dataGridView_Data.RowCount - 1].ReadOnly = true;
                }
            }

        }

        private void dataGridView_Data_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            DataGridView grid = grids[tabControl_Data.SelectedIndex];

            if (grid.CurrentCell == null)
            {
                comboBox_DataType.Visible = false;
                comboBox_DataIndex.Visible = false;
                return;
            }
            
            if (grid == dataGridView_Data)
            {
                if (dataGridView_Data.CurrentCell.ColumnIndex == 1)  //数据类型
                {
                    Rectangle rect = dataGridView_Data.GetCellDisplayRectangle(dataGridView_Data.CurrentCell.ColumnIndex, dataGridView_Data.CurrentCell.RowIndex, true);

                    comboBox_DataType.Top = rect.Top;
                    comboBox_DataType.Left = rect.Left;
                    comboBox_DataType.Width = rect.Width;
                    comboBox_DataType.Height = rect.Height;

                    if (dataGridView_Data.CurrentCell.Value != null)
                    {
                        comboBox_DataType.Text = dataGridView_Data.CurrentCell.Value.ToString();
                    }
                    else
                    {
                        comboBox_DataType.SelectedIndex = -1;
                    }

                    comboBox_DataType.Visible = true;
                    comboBox_DataIndex.Visible = false;
                }
                else if (dataGridView_Data.CurrentCell.ColumnIndex == 2)  //数据索引
                {
                    Rectangle rect = dataGridView_Data.GetCellDisplayRectangle(dataGridView_Data.CurrentCell.ColumnIndex, dataGridView_Data.CurrentCell.RowIndex, true);

                    comboBox_DataIndex.Top = rect.Top;
                    comboBox_DataIndex.Left = rect.Left;
                    comboBox_DataIndex.Width = rect.Width;
                    comboBox_DataIndex.Height = rect.Height;

                    if (dataGridView_Data.CurrentCell.Value != null)
                    {
                        comboBox_DataIndex.Text = dataGridView_Data.CurrentCell.Value.ToString();
                    }
                    else
                    {
                        comboBox_DataIndex.SelectedIndex = -1;
                    }

                    comboBox_DataIndex.Visible = true;
                    comboBox_DataType.Visible = false;
                }
                else
                {
                    comboBox_DataType.Visible = false;
                    comboBox_DataIndex.Visible = false;
                }
            }
            else
            {
                if (grid.CurrentCell.ColumnIndex == 1)  //数据索引
                {
                    Rectangle rect = grid.GetCellDisplayRectangle(grid.CurrentCell.ColumnIndex, grid.CurrentCell.RowIndex, true);

                    comboBox_DataIndex.Top = rect.Top;
                    comboBox_DataIndex.Left = rect.Left;
                    comboBox_DataIndex.Width = rect.Width;
                    comboBox_DataIndex.Height = rect.Height;

                    if (grid.CurrentCell.Value != null)
                    {
                        comboBox_DataIndex.Text = grid.CurrentCell.Value.ToString();
                    }
                    else
                    {
                        comboBox_DataIndex.SelectedIndex = -1;
                    }

                    comboBox_DataIndex.Visible = true;
                    comboBox_DataType.Visible = false;
                }
                else
                {
                    comboBox_DataType.Visible = false;
                    comboBox_DataIndex.Visible = false;
                }
            }
            
            
        }

        private void dataGridView_Data_Scroll(object sender, ScrollEventArgs e)
        {
            comboBox_DataType.Visible = false;
            comboBox_DataIndex.Visible = false;
        }

        private void dataGridView_Data_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            comboBox_DataType.Visible = false;
            comboBox_DataIndex.Visible = false;
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            string strMsg;

            strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_0_1", "是否应用参数?");

            if (DialogResult.Yes == MessageBox.Show(strMsg,"Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2))
            {
                DataMgr.GetInstance().UpdateParamFromGrid(comboBox_GroupName.Text, textBox_Version.Text, checkBox_PDCA.Checked, comboBox_Level.SelectedIndex, dataGridView_Data);
                DataMgr.GetInstance().UpdateParamFromDataGrid(dataGridView_DataShow, checkBox_ShowSave.Checked, dataGridView_DataSave, textBox_SavePath.Text, checkBox_SaveData.Checked);

                //2020-03-02 Binggoo 新增数据库保存
                DataMgr.GetInstance().Server = textBox_ServerIp.Text;
                DataMgr.GetInstance().Port = Convert.ToInt32(textBox_Port.Text);
                DataMgr.GetInstance().UserID = textBox_UserID.Text;
                DataMgr.GetInstance().Password = textBox_Password.Text;
                DataMgr.GetInstance().Database = textBox_Database.Text;
                DataMgr.GetInstance().TableName = textBox_TableName.Text;
                DataMgr.GetInstance().SaveType  = (SaveType)comboBox_SaveType.SelectedIndex;
            }
            
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            string strMsg;

            strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_0_2", "是否保存参数?");

            if (DialogResult.Yes != MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                return;
            }

            DataMgr.GetInstance().UpdateParamFromGrid(comboBox_GroupName.Text, textBox_Version.Text, checkBox_PDCA.Checked, comboBox_Level.SelectedIndex, dataGridView_Data);
            DataMgr.GetInstance().UpdateParamFromDataGrid(dataGridView_DataShow, checkBox_ShowSave.Checked, dataGridView_DataSave, textBox_SavePath.Text, checkBox_SaveData.Checked);

            //2020-03-02 Binggoo 新增数据库保存
            DataMgr.GetInstance().Server = textBox_ServerIp.Text;
            DataMgr.GetInstance().Port = Convert.ToInt32(textBox_Port.Text);
            DataMgr.GetInstance().UserID = textBox_UserID.Text;
            DataMgr.GetInstance().Password = textBox_Password.Text;
            DataMgr.GetInstance().Database = textBox_Database.Text;
            DataMgr.GetInstance().TableName = textBox_TableName.Text;
            DataMgr.GetInstance().SaveType = (SaveType)comboBox_SaveType.SelectedIndex;

            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";
            string strBackup = Application.StartupPath + "\\SystemDataBak.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            DataMgr.GetInstance().BackupXML(doc, strBackup);

            DataMgr.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save data configuration complete");
            }
            else
            {
                MessageBox.Show("保存数据配置完成");
            }

            //如果新增，需要更新界面
            if (comboBox_GroupName.Items.Count != DataMgr.GetInstance().m_dictDataGroup.Keys.Count)
            {
                comboBox_GroupName.DataSource = DataMgr.GetInstance().m_dictDataGroup.Keys.ToArray();

                comboBox_GroupName.SelectedIndex = comboBox_GroupName.Items.Count - 1;
            }
        }

        private void button_MoveUp_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            DataGridView grid = grids[tabControl_Data.SelectedIndex];

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
            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            DataGridView grid = grids[tabControl_Data.SelectedIndex];

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
                grid.CurrentCell = grid.Rows[nSelectRow+1].Cells[0];
            }
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            DataGridView grid = grids[tabControl_Data.SelectedIndex];

            if (grid.CurrentCell == null)
            {
                return;
            }

            int nSelectRow = grid.CurrentCell.RowIndex;
            if (nSelectRow < grid.Rows.Count-1)
            {
                grid.Rows.RemoveAt(grid.CurrentCell.RowIndex);
            }
            
        }

        private void button_DelGroup_Click(object sender, EventArgs e)
        {
            string strGroupName = comboBox_GroupName.Text;

            if (DataMgr.GetInstance().m_dictDataGroup.ContainsKey(strGroupName))
            {
                string str1 = "即将删除组{0},是否继续？";
                string str2 = "警告";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "The group {0} will be deleted. Do you want to continue? ";
                    str2 = "Warning";
                }

                if (DialogResult.Yes == MessageBox.Show(String.Format(str1,strGroupName),
                    str2,MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2))

                {
                    DataMgr.GetInstance().m_dictDataGroup.Remove(strGroupName);

                    comboBox_GroupName.DataSource = DataMgr.GetInstance().m_dictDataGroup.Keys.ToArray();

                    string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

                    XmlDocument doc = new XmlDocument();
                    doc.Load(cfg);

                    DataMgr.GetInstance().SaveCfgXML(doc);

                    doc.Save(cfg);


                }
                
            }
        }

        private void checkBox_PDCA_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_PDCA.Checked)
            {
                foreach(DataGridViewColumn col in dataGridView_Data.Columns)
                {
                    if (col.Name == "补偿值")
                    {
                        col.Visible = false;
                    }
                    else
                    {
                        col.Visible = true;
                    }
                    
                }
            }
            else
            {
                foreach (DataGridViewColumn col in dataGridView_Data.Columns)
                {
                    if (col.Name == "数据类型" || col.Name == "数据索引")
                    {
                        col.Visible = false;
                    }
                    else
                    {
                        col.Visible = true;
                    }
                    
                }
            }
        }

        private void tabControl_Data_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_DataType.Visible = false;
            comboBox_DataIndex.Visible = false;

            DataGridView[] grids = { dataGridView_Data, dataGridView_DataShow, dataGridView_DataSave };

            grids[m_nCurSelectTab].Controls.Remove(comboBox_DataIndex);
            grids[m_nCurSelectTab].Controls.Remove(comboBox_DataType);

            m_nCurSelectTab = tabControl_Data.SelectedIndex;

            grids[m_nCurSelectTab].Controls.Add(comboBox_DataIndex);
            grids[m_nCurSelectTab].Controls.Add(comboBox_DataType);


            if (m_nCurSelectTab == 0)
            {
                button_DelGroup.Visible = true;

                comboBox_Level_SelectedIndexChanged(null, null);
            }
            else
            {
                button_DelGroup.Visible = false;

                if (Security.GetUserMode() >= UserMode.Engineer)
                {
                    button_Apply.Enabled = true;
                    button_MoveDown.Enabled = true;
                    button_MoveUp.Enabled = true;
                    button_Save.Enabled = true;
                }
                
            }
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dlg.SelectedPath))
                {
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        MessageBox.Show(this, "Folder path cannot be empty", "Tips");
                    }
                    else
                    {
                        MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    }
                    
                    return;
                }

                textBox_SavePath.Text = dlg.SelectedPath;
            }

        }

        private void comboBox_Level_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strGroupName = comboBox_GroupName.Text;

            if (DataMgr.GetInstance().m_dictDataGroup.ContainsKey(strGroupName))
            {
                DataGroup group = DataMgr.GetInstance().m_dictDataGroup[strGroupName]; 

                if ((int)Security.GetUserMode() < group.m_nLevel)
                {
                    //无权限
                    textBox_Version.Enabled = false;

                    dataGridView_Data.ReadOnly = true;

                    if (tabControl_Data.SelectedIndex == 0)
                    {
                        button_Apply.Enabled = false; ;
                        button_MoveDown.Enabled = false;
                        button_MoveUp.Enabled = false;
                        button_Save.Enabled = false;
                    }
                }
                else
                {
                    //有权限
                    textBox_Version.Enabled = true;

                    dataGridView_Data.ReadOnly = false;

                    button_Apply.Enabled = true; ;
                    button_MoveDown.Enabled = true;
                    button_MoveUp.Enabled = true;
                    button_Save.Enabled = true;
                }
                    
                
            }
        }

        private void button_SaveDefault_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemDataDef.xml");

            if (File.Exists(strFile))
            {
                doc.Load(strFile);
            }
            else
            {
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(dec);
                //创建一个根节点（一级）
                XmlElement root = doc.CreateElement("SystemCfg");
                doc.AppendChild(root);
            }

            button_Apply_Click(null, null);

            DataMgr.GetInstance().SaveCfgXML(doc);

            doc.Save(strFile);

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save default data configuration complete");
            }
            else
            {
                MessageBox.Show("保存默认数据配置完成");
            }
            
        }

        private void button_RecoverDefault_Click(object sender, EventArgs e)
        {
            string str1 = "默认配置文件不存在";
            string str2 = "是否需要恢复成默认设置？";
            string str3 = "警告";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Default profile does not exist";
                str2 = "Do you want to revert to the default settings? ";
                str3 = "Warning";
            }

            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemDataDef.xml");

            if (!File.Exists(strFile))
            {
                MessageBox.Show(str1);
                return;
            }

            if (MessageBox.Show(str2,str3,MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(strFile);

                DataMgr.GetInstance().ReadCfgFromXml(doc);

                comboBox_GroupName.DataSource = DataMgr.GetInstance().m_dictDataGroup.Keys.ToArray();

                DataMgr.GetInstance().UpdateDataGridFromParam(dataGridView_DataShow, dataGridView_DataSave);

                textBox_SavePath.Text = DataMgr.GetInstance().SavePath;

                checkBox_ShowSave.Checked = DataMgr.GetInstance().DataShowSaveEnable;

                checkBox_SaveData.Checked = DataMgr.GetInstance().DataSaveEnable;

                //2020-03-02 Binggoo 新增数据库保存
                textBox_ServerIp.Text = DataMgr.GetInstance().Server;
                textBox_Port.Text = DataMgr.GetInstance().Port.ToString();
                textBox_UserID.Text = DataMgr.GetInstance().UserID;
                textBox_Password.Text = DataMgr.GetInstance().Password;
                textBox_Database.Text = DataMgr.GetInstance().Database;
                textBox_TableName.Text = DataMgr.GetInstance().TableName;

                comboBox_SaveType.SelectedIndex = (int)DataMgr.GetInstance().SaveType;

                //增加权限等级变更通知
                OnChangeMode();
            }
        }

        private void button_Restore_Click(object sender, EventArgs e)
        {
            string str1 = "上次配置文件不存在";
            string str2 = "是否需要还原成上次设置？";
            string str3 = "警告";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Last profile does not exist";
                str2 = "Do you want to revert to the last setting? ";
                str3 = "Warning";
            }

            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemDataBak.xml");

            if (!File.Exists(strFile))
            {
                MessageBox.Show(str1);
                return;
            }

            if (MessageBox.Show(str2, str3, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(strFile);

                DataMgr.GetInstance().ReadCfgFromXml(doc);

                comboBox_GroupName.DataSource = DataMgr.GetInstance().m_dictDataGroup.Keys.ToArray();

                DataMgr.GetInstance().UpdateDataGridFromParam(dataGridView_DataShow, dataGridView_DataSave);

                textBox_SavePath.Text = DataMgr.GetInstance().SavePath;

                checkBox_ShowSave.Checked = DataMgr.GetInstance().DataShowSaveEnable;

                checkBox_SaveData.Checked = DataMgr.GetInstance().DataSaveEnable;

                //2020-03-02 Binggoo 新增数据库保存
                textBox_ServerIp.Text = DataMgr.GetInstance().Server;
                textBox_Port.Text = DataMgr.GetInstance().Port.ToString();
                textBox_UserID.Text = DataMgr.GetInstance().UserID;
                textBox_Password.Text = DataMgr.GetInstance().Password;
                textBox_Database.Text = DataMgr.GetInstance().Database;
                textBox_TableName.Text = DataMgr.GetInstance().TableName;

                comboBox_SaveType.SelectedIndex = (int)DataMgr.GetInstance().SaveType;

                //增加权限等级变更通知
                OnChangeMode();

            }
        }

        private void comboBox_SaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSelectIndex = comboBox_SaveType.SelectedIndex;
            switch ((SaveType)nSelectIndex)
            {
                case SaveType.DB:
                    label_SavePath.Visible = false;
                    textBox_SavePath.Visible = false;
                    button_Browse.Visible = false;

                    label_Server.Visible = true;
                    textBox_ServerIp.Visible = true;

                    label_Port.Visible = true;
                    textBox_Port.Visible = true;

                    label_UserID.Visible = true;
                    textBox_UserID.Visible = true;

                    label_Password.Visible = true;
                    textBox_Password.Visible = true;

                    label_Database.Visible = true;
                    textBox_Database.Visible = true;

                    label_TableName.Visible = true;
                    textBox_TableName.Visible = true;

                    button_View.Visible = true;

                    dataGridView_DataSave.Top = label_Database.Bottom + 20;
                    break;

                default:
                    label_SavePath.Visible = true;
                    textBox_SavePath.Visible = true;
                    button_Browse.Visible = true;

                    label_Server.Visible = false;
                    textBox_ServerIp.Visible = false;

                    label_Port.Visible = false;
                    textBox_Port.Visible = false;

                    label_UserID.Visible = false;
                    textBox_UserID.Visible = false;

                    label_Password.Visible = false;
                    textBox_Password.Visible = false;

                    label_Database.Visible = false;
                    textBox_Database.Visible = false;

                    label_TableName.Visible = false;
                    textBox_TableName.Visible = false;

                    button_View.Visible = false;

                    dataGridView_DataSave.Top = label_SaveType.Bottom + 20;
                    break;
            }
        }

        private void button_View_Click(object sender, EventArgs e)
        {
            Form_DataQuery frm = new Form_DataQuery();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Show();
        }
    }
}
