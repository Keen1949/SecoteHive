//2018-10-09 Binggoo 1.加入中英文切换
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using Communicate;
using CommonTool;
using ToolEx;
using System.Xml;

namespace AutoFrame
{
    public partial class FormServer : Form
    {
        private Dictionary<AsyncSocketTCPServer, TreeNode> m_dictServerNode = new Dictionary<AsyncSocketTCPServer, TreeNode>();

        private ComboBox m_combox = new ComboBox();

        private AsyncSocketState m_currentState = null;

        public FormServer()
        {
            InitializeComponent();

            InitDataGridView();

            InitComboBox();
        }

        private void InitComboBox()
        {
            m_combox.DropDownStyle = ComboBoxStyle.DropDown;


            m_combox.Items.Clear();

            m_combox.Items.Add(true);
            m_combox.Items.Add(false);

            m_combox.Visible = false;

            m_combox.SelectedIndexChanged += SelectedIndexChanged;
            m_combox.LostFocus += SelectedIndexChanged;

            this.dataGridView1.Controls.Add(m_combox);
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = m_combox.Text;
        }

        void InitDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            int nCol;
            foreach (string head in TcpServerMgr.m_strDescribe)
            {
                nCol = dataGridView1.Columns.Add(head, head);
                dataGridView1.Columns[nCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                //dataGridView1.Columns[nCol].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridView1.Columns[1].Width = 300;
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            m_dictServerNode.Clear();

            int nItem = 1;
            foreach(var item in TcpServerMgr.GetInstance().m_listServers)
            {
                TreeNode node = new TreeNode();
                node.Text = string.Format("{0} - {1}:{2}", nItem, item.Address.ToString(), item.Port);
                node.Tag = item;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;

                item.DataReceived += OnDataReceived;

                m_dictServerNode.Add(item, node);

                treeView1.Nodes.Add(node);
            }

            TcpServerMgr.GetInstance().UpdateGridFromParam(this.dataGridView1);

            OnClientChanged(null,null);
            TcpServerMgr.GetInstance().ClientConnected += OnClientChanged;
            TcpServerMgr.GetInstance().ClientDisconnected += OnClientChanged;

            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language);
            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange = true)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
                this.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, this.Name, this.Text);
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
                LanguageMgr.GetInstance().WriteString(this.GetType().Namespace, this.GetType().Name, this.Name, this.Text);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Security.ModeChangedEvent -= OnChangeMode;
            TcpServerMgr.GetInstance().ClientDisconnected -= OnClientChanged;
            TcpServerMgr.GetInstance().ClientDisconnected -= OnClientChanged;

            foreach (var item in TcpServerMgr.GetInstance().m_listServers)
            {
                item.DataReceived -= OnDataReceived;
            }
        }

        private void OnChangeMode()
        {
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    if (Security.GetUserMode() >= UserMode.Engineer)
                    {
                        checkBox_Loop.Enabled = true;
                        checkBox_Receive.Enabled = true;
                        checkBox_Send.Enabled = true;

                        button_Disconect.Enabled = true;
                        button_Send.Enabled = true;
                        button_StartServer.Enabled = true;
                        button_Clear_Send.Enabled = true;
                        button_Clear_Receive.Enabled = true;

                        textBox_Internal.ReadOnly = false;
                        textBox_Send.ReadOnly = false;

                        if (!tabControl1.TabPages.Contains(tabPage2))
                        {
                            tabControl1.TabPages.Add(tabPage2);
                        }
                    }
                    else
                    {
                        checkBox_Loop.Enabled = false;
                        checkBox_Receive.Enabled = false;
                        checkBox_Send.Enabled = false;

                        button_Disconect.Enabled = false;
                        button_Send.Enabled = false;
                        button_StartServer.Enabled = false;
                        button_Clear_Send.Enabled = false;
                        button_Clear_Receive.Enabled = false;

                        textBox_Internal.ReadOnly = true;
                        textBox_Send.ReadOnly = true;

                        if (tabControl1.TabPages.Contains(tabPage2))
                        {
                            tabControl1.TabPages.Remove(tabPage2);
                        }
                    }
                });
            }
        }

        private void OnDataReceived(object sender, AsyncSocketEventArgs e)
        {
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    AsyncSocketTCPServer server = (AsyncSocketTCPServer)sender;

                    string strData = "";
                    if (m_currentState != e.m_state)
                    {
                        if (textBox_Receive.Text.Length != 0)
                        {
                            strData += "\r\n";
                        }
                        strData += "[Receive from " + e.m_state.ClientSocket.RemoteEndPoint.ToString() +"]";

                        m_currentState = e.m_state;
                    }

                    if (checkBox_Receive.Checked)
                    {
                        strData += HelpTool.ByteToASCIIString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
                    }
                    else
                    {
                        strData += server.Encoding.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);
                    }

                    textBox_Receive.AppendText(strData);

                });
            }
        }

        private void OnClientChanged(object sender,AsyncSocketEventArgs e)
        {
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        node.Nodes.Clear();
                    }

                    foreach (var item in TcpServerMgr.GetInstance().m_dictClients)
                    {
                        TreeNode root;

                        if (m_dictServerNode.TryGetValue(item.Value,out root))
                        {
                            TreeNode child = new TreeNode();
                            child.Text = item.Key.ClientSocket.RemoteEndPoint.ToString();
                            child.Tag = item.Key;
                            child.ImageIndex = 1;
                            child.SelectedImageIndex = 1;

                            root.Nodes.Add(child);
                        }
                    }

                    treeView1.ExpandAll();
                });
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AsyncSocketTCPServer server = null;
            switch (e.Node.Level)
            {
                case 0:
                    server = e.Node.Tag as AsyncSocketTCPServer;

                    break;

                case 1:
                    server = e.Node.Parent.Tag as AsyncSocketTCPServer;
                    break;
            }

            string strStart = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "开始服务", "开始服务");
            string strStop = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "停止服务", "停止服务");

            button_StartServer.Text = server.IsRunning ? strStop : strStart;
        }

        private void button_StartServer_Click(object sender, EventArgs e)
        {
            AsyncSocketTCPServer server = null;
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 0)
                {
                    server = treeView1.SelectedNode.Tag as AsyncSocketTCPServer;
                }
                else
                {
                    server = treeView1.SelectedNode.Parent.Tag as AsyncSocketTCPServer;
                }

                if (server.IsRunning)
                {
                    server.Stop();
                }
                else
                {
                    server.Start();
                }

                string strStart = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "开始服务", "开始服务");
                string strStop = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "停止服务", "停止服务");

                button_StartServer.Text = server.IsRunning ? strStop : strStart;
            }
        }

        private void button_Disconect_Click(object sender, EventArgs e)
        {
            AsyncSocketTCPServer server = null;
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 0)
                {
                    server = treeView1.SelectedNode.Tag as AsyncSocketTCPServer;

                    server.CloseAllClient();
                }
                else
                {
                    server = treeView1.SelectedNode.Parent.Tag as AsyncSocketTCPServer;

                    AsyncSocketState state = treeView1.SelectedNode.Tag as AsyncSocketState;

                    server.Close(state);
                }
            }
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (checkBox_Loop.Checked)
            {
                timer1.Interval = Convert.ToInt32(textBox_Internal.Text.Trim());
                timer1.Enabled = true;
            }

            SendData();
        }

        private void SendData()
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    AsyncSocketTCPServer server = treeView1.SelectedNode.Parent.Tag as AsyncSocketTCPServer;

                    AsyncSocketState state = treeView1.SelectedNode.Tag as AsyncSocketState;

                    if (checkBox_Send.Checked)
                    {
                        byte[] buffer = HelpTool.ASCIIStringToByte(textBox_Send.Text, " ");

                        server.Send(state, buffer);
                    }
                    else
                    {
                        server.Send(state, textBox_Send.Text);
                    }
                }
            }
        }

        private void checkBox_Loop_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox_Loop.Checked)
            {
                SendData();
            }
            else
            {
                timer1.Enabled = false;
            }
        }

        private void checkBox_Send_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Send.Checked)
            {
                byte[] buffer = System.Text.Encoding.Default.GetBytes(textBox_Send.Text);

                textBox_Send.Text = HelpTool.ByteToASCIIString(buffer, 0, buffer.Length);
            }
            else
            {
                byte[] buffer = HelpTool.ASCIIStringToByte(textBox_Send.Text, " ");

                textBox_Send.Text = System.Text.Encoding.Default.GetString(buffer);
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                m_combox.Visible = false;
                return;
            }

            if (dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex,
                    dataGridView1.CurrentCell.RowIndex, true);

                m_combox.Top = rect.Top;
                m_combox.Left = rect.Left;
                m_combox.Width = rect.Width;
                m_combox.Height = rect.Height;

                if (dataGridView1.CurrentCell.Value != null)
                {
                    m_combox.Text = dataGridView1.CurrentCell.Value.ToString();
                }
                else
                {
                    m_combox.SelectedIndex = -1;
                }

                m_combox.Visible = true;
            }
            else
            {
                m_combox.Visible = false;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            m_combox.Visible = false;
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            m_combox.Visible = false;
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            int nSelectRow = dataGridView1.CurrentCell.RowIndex;
            if (nSelectRow < dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_0_1", "即将应用参数，是否继续？");
            string strCaption = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "警告", "警告");

            if (DialogResult.Yes == MessageBox.Show(strMsg,strCaption,MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2))
            {
                TcpServerMgr.GetInstance().UpdateParamFromGrid(dataGridView1);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message_0_2", "即将保存参数，是否继续？");
            string strCaption = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "警告", "警告");

            if (DialogResult.Yes == MessageBox.Show(strMsg, strCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                TcpServerMgr.GetInstance().UpdateParamFromGrid(dataGridView1);

                string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);

                TcpServerMgr.GetInstance().SaveCfgXML(doc);

                doc.Save(cfg);
            }
        }

        private void button_Clear_Receive_Click(object sender, EventArgs e)
        {
            textBox_Receive.Text = "";
        }

        private void button_Clear_Send_Click(object sender, EventArgs e)
        {
            textBox_Send.Text = "";
        }
    }
}
