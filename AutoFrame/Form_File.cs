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
using Communicate;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_File : Form
    {
        private int m_nMinute = DateTime.Now.Minute;
        private int m_nOldMinute = DateTime.Now.Minute;
        private bool m_bRunThread = false;
        Thread m_thread = null;

        /// <summary>
        /// 互斥对象,在路径下文件更新过程中不接收其他消息
        /// </summary>
        private static readonly object syslock = new object();
        
        private volatile int m_nMutex = 0;
        public void LockMutex() { m_nMutex = 1; }
        public void ReleaseMutex() { m_nMutex = 0; }
        public bool IsRelease() { return m_nMutex == 0; }

        private int m_nMuxSend = 1, m_nMuxRead = 0;
        private void InitMuxSR()
        {
            m_nMuxSend = 1;
            m_nMuxRead = 0;
        }
        private bool IsEnableSend()
        {
            if (m_nMuxRead + 1 == m_nMuxSend)
            {
                m_nMuxSend++;
                return true;
            }
            return false;
        }
        private bool IsEpual()
        {
            return m_nMuxRead == m_nMuxSend;
        }

        private List<string[]> m_listData = new List<string[]>();

        public Form_File()
        {
            InitializeComponent();

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

        private void Form_File_Load(object sender, EventArgs e)
        {
            try
            {
                SystemMgr.GetInstance().MonitorImgFile(1, SystemMgr.GetInstance().GetLogPath(), "*.*",
                   OnCreated, OnDeleted, OnRenamed, OnChanged);
                OnFileRefresh();
            }
            catch{}
        }


        /// <summary>
        /// 遍历完一个节点下的所有文件(包括子节点)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tn"></param>
        private void expendtree(string path, TreeNode tn)
        {
            try
            {
                ///获取父节点目录的子目录
                string[] s1 = Directory.GetDirectories(path);
                ///子节点
                TreeNode subnode = new TreeNode();
                ///通过遍历给传进来的父节点添加子节点
                foreach (string j in s1)
                {
                    subnode = new TreeNode(Path.GetFileName(j), 0, 0);
                    tn.Nodes.Add(subnode);
                    ///对文件夹不断递归， 得到所有文件
                    expendtree(j, subnode);
                }               
            }
            catch { }
        }

        //树形控件选择点击事件
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                lock (syslock)
                {
                    if (IsRelease())
                    {
                        listBox1.Items.Clear();
                        string[] strFile = Directory.GetFiles(e.Node.FullPath, "*.*"); //获取被点击树形节点目录下所有文件名
                        foreach (string s in strFile)
                        {
                            listBox1.Items.Add(Path.GetFileName(s));  //列表框增加项目
                        }
                        if(listBox1.Items.Count > 0)
                        {
                            listBox1.SelectedIndex = 0;  //通知列表框更新
                        }
                    }
                }
            }
            catch{}
        }

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB; 

        //允许重绘pnl  
        //SendMessage(SelfInfo_pnlContact1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);  
        private void ResetDataGrid(string strPath)
        {
            dataGridView1.Rows.Clear();
            FileInfo fi = new FileInfo(strPath);
            if (!fi.Exists)
                return;
            StreamReader sr = new StreamReader(strPath, Encoding.Default);  //读取文件
            String strLine;
            int i = 1;
            //禁止pnl重绘  
            SendMessage(dataGridView1.Handle, WM_SETREDRAW, 0, IntPtr.Zero); 

            while ((strLine = sr.ReadLine()) != null)  //读取一行数据
            {
                if (strLine == string.Empty)
                    continue;
                string[] data = strLine.Split(',');

                if(data.Length > 4)
                {
                    StringBuilder sb = new StringBuilder(data[3]);
                    for (int k = 4; k < data.Length; ++k)
                    {
                        sb.Append(",");
                        sb.Append(data[k]);
                    }
                    dataGridView1.Rows.Add((i++).ToString(), data[0], data[1], data[2], sb.ToString());
                }
                else
                {
                    dataGridView1.Rows.Add((i++).ToString(), data[0], data[1], data[2], data[data.Length - 1]);
                }
            }
            //允许重绘pnl  
            SendMessage(dataGridView1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

            dataGridView1.Refresh();
            dataGridView1.CurrentCell = null;
            sr.Close();
        }

        //列表框选择点击事件
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strPath = treeView1.SelectedNode.FullPath + "\\" + listBox1.Text;
                //ResetDataGrid(strPath);
                lock (syslock)
                {
                    if (IsRelease())
                    {
                        LockMutex();
                        dataGridView1.Rows.Clear();
                        InitMuxSR();
                        //禁止pnl重绘  
                        SendMessage(dataGridView1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                        //创建更新数据表格线程
                        m_thread = new Thread(() => ThreadLoadData(strPath));
                        m_thread.Start();  
                    }
                }
            }
            catch{}
        }

        //选择列表框中当前选择项的下一个,当前项是末尾则自动回到首项
        private void button_next_Click(object sender, EventArgs e)
        {
            lock (syslock)
            {
                //if (m_thread.IsAlive)
                //    return;
                if (!IsRelease())
                {
                    WarningMgr.GetInstance().Info("button_next_Click and return");
                    return;
                }
                WarningMgr.GetInstance().Info("button_next_Click");
                if (listBox1.Items.Count > 0)
                {
                    int n = listBox1.SelectedIndex + 1;
                    if (n >= listBox1.Items.Count)
                    {
                        listBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        listBox1.SelectedIndex = n;
                    }
                }
            }
        }

        //选择列表框中当前选择项的上一个,当前项是首端则自动回到末尾项
        private void button_prev_Click(object sender, EventArgs e)
        {
            lock (syslock)
            {
                //if (m_thread.IsAlive)
                //    return;
                if (!IsRelease())
                {
                    WarningMgr.GetInstance().Info("button_prev_Click and return");
                    return;
                }
                WarningMgr.GetInstance().Info("button_prev_Click");
                if (listBox1.Items.Count > 0)
                {
                    int n = listBox1.SelectedIndex;
                    if (n == 0)
                    {
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                    else
                    {
                        listBox1.SelectedIndex = n - 1;
                    }
                }
            }
        }

        //todo:监视文件夹变更

        delegate void listBoxClearDelegate();
        delegate void treeviewDelegate();
        delegate void dfvRefreshDelegate(int nInit);
        delegate void dgvDelegate(List<String[]> listData, int nStart);

        private void ListBoxClear()
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new listBoxClearDelegate(ListBoxClear));
            }
            else
            {
                listBox1.Items.Clear();
            }
        }
        /// <summary>
        /// 更新树形控件
        /// </summary>
        private void UpdateTreeView()
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new treeviewDelegate(UpdateTreeView));
            }
            else
            {
                treeView1.Nodes.Clear();
                string strPath = SystemMgr.GetInstance().GetLogPath("");//获取Log保存路径   
                TreeNode nodeParent = treeView1.Nodes.Add(strPath);
                string[] folders = Directory.GetDirectories(strPath);  //获取路径下的所有文件夹名(子目录)
                foreach (string fod in folders)  //遍历文件
                {
                    TreeNode tnode = new TreeNode(Path.GetFileName(fod), 0, 0);
                    tnode.Name = fod;
                    nodeParent.Nodes.Add(tnode);
                    expendtree(fod, tnode);
                }
                nodeParent.Expand();
            }
        }

        /// <summary>
        /// 刷新表格控件
        /// </summary>
        private void  DgvRefresh(int nInit = 0)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.BeginInvoke(new dfvRefreshDelegate(DgvRefresh), new object[] { nInit });
            }
            else
            {
                if (nInit == 0)
                {
                    //允许重绘pnl  
                    SendMessage(dataGridView1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                    dataGridView1.Refresh();
                    dataGridView1.CurrentCell = null;
                }
                else
                {
                    dataGridView1.Rows.Clear();
                }
            }
        }




        /// <summary>
        /// 使用异步委托给表格添加数据
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="nStart"></param>
        /// <param name="nEnd"></param>
        private void SetDgvDataSource(List<String[]> listData, int nStart)
        {
           //禁止pnl重绘  
            SendMessage(dataGridView1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            for (int i = 0; i < 50; i++)
            {
                //string str = string.Format("nStart={0},nEnd={1},m_listData.Count={2}", nStart, nEnd, m_listData.Count);
                //WarningMgr.GetInstance().Info(str);
                int m = i + nStart ;
                if (m >= listData.Count)
                    break;
                try
                {
                    string[] data = listData.ElementAt(m);
                    if (data.Length > 4)
                    {
                        StringBuilder sb = new StringBuilder(data[3]);
                        for (int k = 4; k < data.Length; ++k)
                        {
                            sb.Append(",");
                            sb.Append(data[k]);
                        }
                        dataGridView1.Rows.Add((m+1).ToString(), data[0], data[1], data[2], sb.ToString());
                    }
                    else if (data.Length == 4)
                    {
                        dataGridView1.Rows.Add((m+1).ToString(), data[0], data[1], data[2], data[data.Length - 1]);
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            }
            SendMessage(dataGridView1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            dataGridView1.Refresh();
            dataGridView1.CurrentCell = null;

            m_nMuxRead++;
            //允许重绘pnl  
            //SendMessage(dataGridView1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }

        /// <summary>
        /// 目录下文件有变动,刷新
        /// </summary>
        private void OnFileRefresh()
        {
            lock (syslock)
            {
                if (IsRelease())
                {
                    LockMutex();
                    ListBoxClear();
                    DgvRefresh(1);
                    UpdateTreeView(); //此处界面控件更新都是异步的,有可能释放锁后下一个循环进入出现问题?
                    ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// 增加文件调用
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            OnFileRefresh();
        }

        /// <summary>
        /// 删除文件调用
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            OnFileRefresh();
        }

        /// <summary>
        /// 文件重新命名调用
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            OnFileRefresh();
        }

        /// <summary>
        /// 文件改变调用,大于十分钟调用一次
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            m_nMinute = DateTime.Now.Minute;
            if (m_nMinute > m_nOldMinute + 10 || (m_nMinute + 50 > m_nOldMinute && m_nOldMinute > m_nMinute))
            {
                m_nOldMinute = m_nMinute;
                OnFileRefresh();
            }
        }

        private void Form_File_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_thread != null && m_thread.IsAlive)
            {
                m_bRunThread = false;
                if (m_thread.Join(2000) == false)
                    m_thread.Abort();
                m_thread = null;
            }
        }

        /// <summary>
        /// 列表框点击刷新数据表格线程
        /// </summary>
        /// <param name="strPath"></param>
        private void ThreadLoadData(string strPath)
        {
            Thread.Sleep(50);
            m_listData.Clear();
            FileInfo fi = new FileInfo(strPath);
            if (!fi.Exists)
            {
                ReleaseMutex();
                return;
            }
            try
            {
                StreamReader sr = new StreamReader(strPath, Encoding.Default);  //读取文件
                String strLine;
                DateTime testStart = DateTime.Now;
                while ((strLine = sr.ReadLine()) != null)  //读取一行数据
                {
                    if (strLine == string.Empty)
                        continue;
                    string[] data = strLine.Split(',');

                    m_listData.Add(data);
                }
                sr.Close();

                int nCount = m_listData.Count;
                int nStart = 0;
                m_bRunThread = true;

                while (m_bRunThread)
                {
                    Thread.Sleep(20);
                    if (IsEnableSend())
                    {
                        dataGridView1.Invoke(new dgvDelegate(SetDgvDataSource), new object[] { m_listData, nStart });
                        nStart += 50;

                        if (nStart >= m_listData.Count)
                            m_bRunThread = false;

                    }
                }
            }

            catch { }
            finally
            {
                ReleaseMutex();
            }
        }
    }
}
