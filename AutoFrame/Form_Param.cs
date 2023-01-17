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
using System.IO;
using System.Runtime.InteropServices;
using ToolEx;


namespace AutoFrame
{
    public partial class Form_Param : Form
    {
        private ComboBox m_comboBoxLevel = new ComboBox();

        public Form_Param()
        {
            InitializeComponent();

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

        private void InitComboBox()
        {
            m_comboBoxLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            m_comboBoxLevel.Items.Clear();

            foreach (UserMode item in Enum.GetValues(typeof(UserMode)))
            {
                if (item != UserMode.Administrator)
                {
                    m_comboBoxLevel.Items.Add(string.Format("{0} - {1}",(int)item,item.ToString()));
                }
            }
            m_comboBoxLevel.Visible = false;
            m_comboBoxLevel.SelectedIndexChanged += OnSelectedIndexChanged;
            m_comboBoxLevel.LostFocus += OnSelectedIndexChanged;

            dataGridView_AllParam.Controls.Add(m_comboBoxLevel);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView_AllParam.CurrentCell.Value = m_comboBoxLevel.SelectedIndex;
        }

        private void Form_Param_Load(object sender, EventArgs e)
        {
            dataGridView_AllParam.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_AllParam.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            //SystemMgr.GetInstance().UpdateGridFromParam(dataGridView_AllParam); //从内存加载参数到界面表格
                                                                       //   dataGridView_AllParam.;
            try
            {
                treeviewUpdate();
            }
            catch{}

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

            SystemMgr.GetInstance().SystemParamChangedEvent += OnSystemParamChangedEvent;
        }

        private void OnSystemParamChangedEvent(string strParam, object oldValue, object newValue)
        {
            ProductMgr.LogChanged(strParam, oldValue.ToString(), newValue.ToString());
        }

        /// <summary>
        /// 更新树型控件
        /// </summary>
        private void treeviewUpdate()
        {
            treeView1.Nodes.Clear();
            string strPath = AppDomain.CurrentDomain.BaseDirectory;//获取当前路径
            TreeNode nodeParent = treeView1.Nodes.Add(strPath);
            GetSystemParamFiles(strPath, nodeParent);
            nodeParent.Expand();
            int n = treeView1.Nodes[0].GetNodeCount(false);
            string strName = Path.GetFileName(SystemMgr.GetInstance().m_strSystemParamName);
            for (int i = 0; i < n; i++)
            {
                if (treeView1.Nodes[0].Nodes[i].Text == /*"systemParam.xml"*/strName)
                {
                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i];
                }
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
                    dataGridView_AllParam.ReadOnly = false;
                    button_update.Enabled = true;
                    button_save.Enabled = true;
                    button_save_as.Enabled = true;
                    roundButtonSetParam.Enabled = true;

                    roundButton_SaveDefault.Enabled = true;
                    roundButton_Restore.Enabled = true;
                    roundButton_RecoverDefault.Enabled = true;

                    this.dataGridViewColumn_Level.ReadOnly = false;

                    break;

                case UserMode.Adjustor:
                case UserMode.FAE:
                    dataGridView_AllParam.ReadOnly = false;
                    button_update.Enabled = true;
                    button_save.Enabled = true;

                    button_save_as.Enabled = false;
                    roundButtonSetParam.Enabled = false;

                    roundButton_SaveDefault.Enabled = false;
                    roundButton_Restore.Enabled = false;
                    roundButton_RecoverDefault.Enabled = false;

                    this.dataGridViewColumn_Level.ReadOnly = true;

                    break;

                case UserMode.Operator:
                    dataGridView_AllParam.ReadOnly = true;
                    button_update.Enabled = false;
                    button_save.Enabled = false;
                    button_save_as.Enabled = false;
                    roundButtonSetParam.Enabled = false;

                    roundButton_SaveDefault.Enabled = false;
                    roundButton_Restore.Enabled = false;
                    roundButton_RecoverDefault.Enabled = false;

                    this.dataGridViewColumn_Level.ReadOnly = true;

                    break;
            }
        }

        /// <summary>
        /// 得到指定路径下系统文件，添加到指定当前路径下的根节点，空节点不添加
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="tn"></param>
        public void GetSystemParamFiles(string strPath, TreeNode tn)
        {
            DirectoryInfo di = new DirectoryInfo(strPath);
            FileInfo[] fi = di.GetFiles("*systemParam*.xml");
            try
            {
                foreach (FileInfo tmpfi in fi)
                {
                    string fileName = tmpfi.Name;
                    fileName = fileName.Substring(fileName.LastIndexOf("\\")+1);
                    TreeNode tnode = new TreeNode(Path.GetFileName(fileName), 0, 0);
                    tn.Nodes.Add(tnode);
                }

                //遍历当前文件夹下所有子文件夹
                List<string> folders = new List<string>(Directory.GetDirectories(strPath));
                folders.ForEach(c =>
                {
                    string childDir = Path.Combine(new string[] { strPath, Path.GetFileName(c) });
                    TreeNode tnode = new TreeNode(Path.GetFileName(Path.GetFileName(c)), 0, 0);
                    tn.Nodes.Add(tnode);
                    GetSystemParamFiles(childDir,tnode);//递归
                    if (tnode.Nodes.Count == 0)
                    {
                        tn.Nodes.Remove(tnode);
                    }
                });
            }
            catch{}
        }

        /// <summary>
        /// 更新参数到内存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_Click(object sender, EventArgs e)
        {
            string str1 = "确定更新当前参数到内存？";
            string str2 = "更新参数";
            string str3 = "参数更新成功";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Are you sure to update the current parameters to memory?";
                str2 = "Update parameters";
                str3 = "Parameter updated successfully";
            }
            if (DialogResult.OK == MessageBox.Show(str1, str2, MessageBoxButtons.OKCancel))
            {
                SystemMgr.GetInstance().UpdateParamFromGrid(dataGridView_AllParam);
                SystemMgr.GetInstance().UpdateParamFromTemp();
                MessageBox.Show(str3);
            }
            //this.Hide();
        }

        /// <summary>
        /// 保存参数到xml文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            string str1 = "是否保存文件:";
            string str2 = "保存文件";
            string str3 = "参数已更新且文件保存成功,文件名为:";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Do you want to save file:";
                str2 = "Save file";
                str3 = "The parameter has been updated and the file has been saved successfully. The file name is:";
            }

            string strFileName = SystemMgr.GetInstance().m_strSystemParamName;
            string str = str1 + strFileName;
            if (DialogResult.OK == MessageBox.Show(str, str2, MessageBoxButtons.OKCancel))
            {
                string strBackUpFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemParamBak.xml");

                File.Copy(strFileName, strBackUpFile, true);

                SystemMgr.GetInstance().UpdateParamFromGrid(dataGridView_AllParam);
                SystemMgr.GetInstance().UpdateParamFromTemp();

                int index = SystemMgr.GetInstance().m_strFileDescribe[3].IndexOf(":");
                string strfdn = SystemMgr.GetInstance().m_strFileDescribe[3].Substring(index+1);
                index = SystemMgr.GetInstance().m_strFileDescribe[4].IndexOf(":");
                string strfds = SystemMgr.GetInstance().m_strFileDescribe[4].Substring(index+1);
                if (SystemMgr.GetInstance().SaveParamFile(strFileName, strfdn, strfds, true))
                    MessageBox.Show(str3+ strFileName);
            }
            //this.Hide();
        }

        /// <summary>
        /// 更新参数并输出
        /// </summary>
        /// <param name="strOut">输出对象</param>
        /// <param name="obj">输入对象</param>
        private void GetCellParam(out string strOut, object obj)
        {
            if (obj == null)
                strOut = string.Empty;
            else
                strOut = obj.ToString().Trim();
        }

        /// <summary>
        /// 表格单元内容改变触发事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">附带数据的对象</param>
        private void dataGridView_param_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                string strValue, strMin, strMax;
                GetCellParam(out strValue, dataGridView_AllParam.Rows[e.RowIndex].Cells[0].Value);
                GetCellParam(out strMin, dataGridView_AllParam.Rows[e.RowIndex].Cells[3].Value);
                GetCellParam(out strMax, dataGridView_AllParam.Rows[e.RowIndex].Cells[4].Value);

                if (strMin != string.Empty && strMax != string.Empty && strValue != strMax)
                {
                    double value = 0;
                    try
                    {
                        value = Convert.ToDouble(strValue);
                    }
                    catch
                    {
                        dataGridView_AllParam.Rows[e.RowIndex].Cells[0].Value =
                            SystemMgr.GetInstance().m_DicParam.ElementAt(e.RowIndex).Value.m_strValue;//恢复原值
                        return;
                    }
                    double min = Convert.ToDouble(strMin);
                    double max = Convert.ToDouble(strMax);
                    if (value > max || value < min)
                    {
                        string str1 = "参数超过限制值";
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            str1 = "Parameter exceeds limit";
                        }
                        MessageBox.Show(str1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView_AllParam.Rows[e.RowIndex].Cells[0].Value =
                            SystemMgr.GetInstance().m_DicParam.ElementAt(e.RowIndex).Value.m_strValue;//恢复原值
                    }
                }
            }
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_as_Click(object sender, EventArgs e)
        {
            Form_ParmSaveAs frm = new Form_ParmSaveAs();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Init();
            frm.ShowDialog();
            if (DialogResult.OK == frm.DialogResult)
            {
                string strDir="", modifier="", fileDescribe="";
                frm.GetParam(ref strDir, ref modifier, ref fileDescribe);
                //保存
                SystemMgr.GetInstance().UpdateParamFromGrid(dataGridView_AllParam);
                SystemMgr.GetInstance().SaveParamFile(strDir,modifier,fileDescribe,true);

                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Parameter file saved successfully");
                }
                else
                {
                    MessageBox.Show("参数文件保存成功");
                }
                
                treeviewUpdate();
            }
            else if (DialogResult.Cancel == frm.DialogResult)
            {
                //取消
            }
        }

        /// <summary>
        /// 树形控件被选中项触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strFile = treeView1.SelectedNode.FullPath;
            FileInfo fileInfo = new FileInfo(strFile);
            if  (fileInfo.Exists)
            {
                SystemMgr.GetInstance().m_strSystemParamName = strFile;          
                SystemMgr.GetInstance().LoadParamFileToGrid(strFile,dataGridView_AllParam);
                int nCount = SystemMgr.GetInstance().m_strFileDescribe.Length;
                listView1.Items.Clear();
                for (int i=1; i<nCount; i++)
                {
                    listView1.Items.Add(SystemMgr.GetInstance().m_strFileDescribe[i]);
                }
            }
        }

        /// <summary>
        /// 选中当前文件名作为下次启动的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetParam_Click(object sender, EventArgs e)
        {
            string str1 = "请选择当前目录下文件!";
            string str2 = "是否设置当前文件: ";
            string str3 = "作为参数配置文件 ?";
            string str4 = "选择文件";
            string str5 = "参数文件保存成功";
            string str6 = "参数文件名长度不够";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Please select a file in the current directory!";
                str2 = "Set current file or not: ";
                str3 = "as a parameter profile?";
                str4 = "Select file";
                str5 = "Parameter file saved successfully";
                str6 = "Insufficient length of parameter file name";
            }

            if (treeView1.SelectedNode != null)
            {
                string strFullPath = treeView1.SelectedNode.FullPath;
                string strPath = Path.GetDirectoryName(strFullPath);
                string strFileName = Path.GetFileName(strFullPath);
                string sDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                if (strPath != sDir)
                {
                    MessageBox.Show(str1);
                    return;
                }
                if (strFileName.Length >= 11)
                {
                    string str = str2 + strFileName + str3;
                    if (DialogResult.OK == MessageBox.Show(str, str4, MessageBoxButtons.OKCancel))
                    {
                        SystemMgr.GetInstance().m_strSystemParamName = strFileName;
                        //ConfigMgr.GetInstance().SaveCfgFile("SystemCfg.xml");
                        SystemMgr.GetInstance().AppendSystemParamName("SystemCfg.xml");
                        MessageBox.Show(str5);
                        treeviewUpdate();
                    }
                }
                else
                {
                    MessageBox.Show(str6);
                    return;
                }
            }
        }

        private void roundButton_SaveDefault_Click(object sender, EventArgs e)
        {
            string str1 = "系统默认配置参数";
            string str2 = "保存默认参数成功";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "System default configuration parameters";
                str2 = "Save default parameters succeeded";
            }
            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemParamDef.xml");
            //保存
            SystemMgr.GetInstance().UpdateParamFromGrid(dataGridView_AllParam);
            SystemMgr.GetInstance().SaveParamFile(strFile, "Admin", str1, true);
            MessageBox.Show(str2);
            treeviewUpdate();
        }

        private void roundButton_Restore_Click(object sender, EventArgs e)
        {
            string str1 = "是否还原上一次的参数设置？";
            string str2 = "还原上一次参数完成";
            string str3 = "不存在上一次参数设置文件";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Do you want to restore the last parameter settings? ";
                str2 = "Restore last parameter completed";
                str3 = "There is no previous parameter setting file";
            }

            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemParamBak.xml");

            if(File.Exists(strFile))
            {
                if (DialogResult.OK == MessageBox.Show(str1, "Warning", MessageBoxButtons.OKCancel))
                {
                    //SystemMgr.GetInstance().m_strSystemParamName = "systemParamBak.xml";

                    SystemMgr.GetInstance().LoadParamFileToGrid(strFile, dataGridView_AllParam);

                    MessageBox.Show(str2);
                    //treeviewUpdate();
                }
            }
            else
            {
                MessageBox.Show(str3);
            }

            
        }

        private void roundButton_RecoverDefault_Click(object sender, EventArgs e)
        {
            string str1 = "是否恢复默认参数设置？";
            string str2 = "恢复默认参数完成";
            string str3 = "不存在默认参数设置文件";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Do you want to restore the default parameter settings? ";
                str2 = "Restore default parameters complete";
                str3 = "There is no default parameter setting file";
            }

            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "systemParamDef.xml");
            if (File.Exists(strFile))
            {
                if (DialogResult.OK == MessageBox.Show(str1, "Warning", MessageBoxButtons.OKCancel))
                {
                    //SystemMgr.GetInstance().m_strSystemParamName = "systemParamDef.xml";

                    //SystemMgr.GetInstance().UpdateGridFromParam(dataGridView_AllParam); //从内存加载参数到界面表格
                    SystemMgr.GetInstance().LoadParamFileToGrid(strFile, dataGridView_AllParam);

                    MessageBox.Show(str2);
                    //treeviewUpdate();
                }
            }
            else
            {
                MessageBox.Show(str3);
            }
            
        }

        private void Form_Param_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                SystemMgr.GetInstance().UpdateGridFromParam(dataGridView_AllParam);
            }
        }

        private void dataGridView_AllParam_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_comboBoxLevel.Visible = false;
        }

        private void dataGridView_AllParam_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;

            m_comboBoxLevel.Visible = false;

            if (grid == null || grid.CurrentCell == null)
            {
                return;
            }

            int rowIndex = grid.CurrentCell.RowIndex;
            int colIndex = grid.CurrentCell.ColumnIndex;

            if (grid == dataGridView_AllParam)
            {
                if (colIndex == 5)
                {
                    //只有工程师以上的权限才能修改
                    if (Security.IsAdminMode() || Security.IsEngMode())
                    {
                        Rectangle rect = grid.GetCellDisplayRectangle(colIndex, rowIndex, true);

                        m_comboBoxLevel.Top = rect.Top;
                        m_comboBoxLevel.Left = rect.Left;
                        m_comboBoxLevel.Width = rect.Width;
                        m_comboBoxLevel.Height = rect.Height;

                        if (grid.CurrentCell.Value != null)
                        {
                            m_comboBoxLevel.SelectedIndex = Convert.ToInt32(grid.CurrentCell.Value);
                        }
                        else
                        {
                            m_comboBoxLevel.SelectedIndex = -1;
                        }

                        m_comboBoxLevel.Visible = true;
                    }
                }
                else if (colIndex == 0)
                {
                    //获取权限
                    int level = Convert.ToInt32(grid.Rows[rowIndex].Cells[5].Value);

                    if ((int)Security.GetUserMode() >= level)
                    {
                        grid.CurrentCell.ReadOnly = false;
                    }
                    else
                    {
                        grid.CurrentCell.ReadOnly = true;
                    }
                }
            }
        }

        private void dataGridView_AllParam_Scroll(object sender, ScrollEventArgs e)
        {
            m_comboBoxLevel.Visible = false;
        }
    }
}
