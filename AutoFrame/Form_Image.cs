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
using System.Threading;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Image : Form
    {
        /// <summary>
        /// 互斥对象,在路径下文件更新过程中不接收其他消息
        /// </summary>
        private static readonly object syslock = new object();
        public string m_sdPath = "";

        private volatile int m_nMutex = 0;
        void LockMutex() { m_nMutex = 1; }
        void Release() { m_nMutex = 0; }
        bool IsRelease() { return m_nMutex == 0; }

        public Form_Image()
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

        private void Form_Image_Load(object sender, EventArgs e)
        {
            try
            {
                //treeView1.Nodes.Clear();
                //string strPath = SystemMgr.GetInstance().GetImagePath();

                //string[] folders = Directory.GetDirectories(strPath);
                //TreeNode nodeParent = treeView1.Nodes.Add(strPath);
                //foreach (string fod in folders)
                //{
                //    TreeNode tnode = new TreeNode(Path.GetFileName(fod), 0, 0);
                //    tnode.Name = fod;
                //    nodeParent.Nodes.Add(tnode);
                //    expendtree(fod, tnode);
                //}
                //nodeParent.Expand();

                SystemMgr.GetInstance().MonitorImgFile(2, SystemMgr.GetInstance().GetImagePath(), "*.*",
                   OnCreated, OnDeleted, OnRenamed, OnChanged);
                ListBoxClear();
                PictureBox1Clear();
                UpdateTreeView();
            }
            catch {}            
        }

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
                    subnode.Name = j;
                    tn.Nodes.Add(subnode);
                    ///对文件夹不断递归， 得到所有文件
                    expendtree(j, subnode);
                }
            }
            catch { }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                lock (syslock)
                {
                    if (IsRelease())
                    {
                        pictureBox1.Image = null;
                        listBox1.Items.Clear();
                        //点击根节点更新整个树
                        m_sdPath = e.Node.FullPath;
                        if (m_sdPath == SystemMgr.GetInstance().GetImagePath())
                        {
                            UpdateTreeView();
                        }
                        string[] strFile = Directory.GetFiles(e.Node.FullPath, "*.*");
                        foreach(string s in strFile)
                        {
                            listBox1.Items.Add(Path.GetFileName(s));
                        }
                        if (listBox1.Items.Count > 0)
                        {
                            listBox1.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch{}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strPath = treeView1.SelectedNode.FullPath + "\\" + listBox1.Text;
                lock (syslock)
                {
                    if (IsRelease())
                    {
                        LockMutex();
                        FileInfo fi = new FileInfo(strPath);
                        if (!fi.Exists)
                            return;
                        else
                            pictureBox1.Image = Image.FromFile(strPath);
                        Release();
                    }
                }
            }
            catch{}
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            lock (syslock)
            {
                if (!IsRelease())
                    return;
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

        private void button_prev_Click(object sender, EventArgs e)
        {
            lock (syslock)
            {
                if (!IsRelease())
                    return;
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

        delegate void listBoxFlushDelegate(string fileDir);
        delegate void listBoxClearDelegate();
        delegate void listBoxAddDelegate(string fileName); 
        delegate void listBoxDeleteDelegate(string fileName);
        delegate void pictureBox1ClearDelegate();
        delegate void treeviewDelegate();
        delegate void treeviewAddNodeDelegate(string parentPath, string fullPath); 
        delegate void treeviewDeleteNodeDelegate(string parentPath, string fullPath);

        /// <summary>
        /// 刷新列表框
        /// </summary>
        private void ListBoxFlush(string fileDir)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new listBoxFlushDelegate(ListBoxFlush),new object[] {fileDir});
            }
            else
            {
                listBox1.Items.Clear();
                string[] strFile = Directory.GetFiles(fileDir, "*.*");
                foreach (string s in strFile)
                {
                    listBox1.Items.Add(Path.GetFileName(s));
                }
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 清空列表框
        /// </summary>
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
        /// 列表框增加项目
        /// </summary>
        private void ListBoxAdd(string fileName)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new listBoxAddDelegate(ListBoxAdd),new object[] { fileName });
            }
            else
            {
                listBox1.Items.Add(fileName);
            }
        }

        /// <summary>
        /// 列表框删除项目
        /// </summary>
        private void ListBoxDelete(string fileName)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new listBoxDeleteDelegate(ListBoxDelete), new object[] { fileName });
            }
            else
            {
                listBox1.Items.Remove(fileName);
            }
        }

        /// <summary>
        /// 清空图像区
        /// </summary>
        private void PictureBox1Clear()
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.BeginInvoke(new pictureBox1ClearDelegate(PictureBox1Clear));
            }
            else
            {
                pictureBox1.Image = null;
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
                string strPath = SystemMgr.GetInstance().GetImagePath();

                string[] folders = Directory.GetDirectories(strPath);
                TreeNode nodeParent = treeView1.Nodes.Add(strPath);
                foreach (string fod in folders)
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
        /// 树形控件添加节点,文件夹节点
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="fullName"></param>
        private void TreeViewAddNode(string parentPath, string fullPath)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new treeviewAddNodeDelegate(TreeViewAddNode),new object[] { parentPath, fullPath });
            }
            else
            {
                if (parentPath == SystemMgr.GetInstance().GetImagePath()) //根节点下一级节点
                {
                    TreeNode tnode = new TreeNode(Path.GetFileName(fullPath), 0, 0);
                    tnode.Name = fullPath;
                    treeView1.Nodes[0].Nodes.Add(tnode);
                    return;
                }
                TreeNode[] td  = treeView1.Nodes.Find(parentPath, true);
                string diName = Path.GetFileName(fullPath);
                string pPath = Path.GetDirectoryName(fullPath);
                for (int i=0; i<td.Length; i++)
                {
                    if (td[i].FullPath == pPath)
                    {
                        TreeNode tnode = new TreeNode(Path.GetFileName(fullPath), 0, 0);
                        tnode.Name = fullPath;
                        td[i].Nodes.Add(tnode);
                        //td[i].Nodes.Add(diName);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 树形控件删除节点,文件夹节点
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="fullName"></param>
        private void TreeViewDeleteNode(string parentPath, string fullPath)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new treeviewDeleteNodeDelegate(TreeViewDeleteNode), new object[] { parentPath, fullPath });
            }
            else
            {
                if (parentPath == SystemMgr.GetInstance().GetImagePath()) //根节点下一级节点
                {
                    if (treeView1.Nodes[0].Nodes.ContainsKey(fullPath))
                    {
                        treeView1.Nodes[0].Nodes.RemoveByKey(fullPath);
                    }
                    return;
                }
                TreeNode[] td = treeView1.Nodes.Find(parentPath, true);
                string pPath = Path.GetDirectoryName(fullPath);
                for (int i = 0; i < td.Length; i++)
                {
                    if (td[i].FullPath == pPath)
                    {
                        if (td[i].Nodes.ContainsKey(fullPath))
                        {
                            td[i].Nodes.RemoveByKey(fullPath);
                        }
                        break;
                    }
                }
            }
        }
   
        private void OnFileRefresh(object source, FileSystemEventArgs e)
        {
            lock (syslock)
            {
                if (IsRelease())
                {
                    LockMutex();
                    string fullPath = e.FullPath;
                    DirectoryInfo di = new DirectoryInfo(fullPath);
                    string dir = Path.GetDirectoryName(fullPath);
                    if (di.Exists) //如果是目录
                    {
                        if (e.ChangeType == WatcherChangeTypes.Created)
                        {
                            TreeViewAddNode(dir, fullPath);
                            Release();
                            return;
                        }
                    }

                    string dirName = Path.GetDirectoryName(fullPath);
                    string fileName = Path.GetFileName(fullPath);
                    if (e.ChangeType == WatcherChangeTypes.Created)
                    {
                        //判断是否是当前节点,是的话则在list框中增加，其他情况不用管
                        if (dirName == m_sdPath)
                        {
                            ListBoxAdd(fileName);
                        }
                    }
                    if (e.ChangeType == WatcherChangeTypes.Deleted)
                    {
                        //判断是否是当前节点，是的话则在list框中减少项目，其他情况不用管
                        if (dirName == m_sdPath)
                        {
                            ListBoxDelete(fileName);
                        }
                        //此处为删除目录
                        TreeViewDeleteNode(dir, fullPath);
                    }
                    if (e.ChangeType == WatcherChangeTypes.Renamed)
                    {
                        //判断是否是当前节点，是的话清空list,并更新，其他情况不用管
                        if (dirName == m_sdPath)
                        {
                            ListBoxFlush(dirName);
                        }
                    }
                    Release();
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
            OnFileRefresh(source,e);
        }

        /// <summary>
        /// 删除文件调用
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            OnFileRefresh(source, e);
        }

        /// <summary>
        /// 文件重新命名调用
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            OnFileRefresh(source, e);
        }

        /// <summary>
        /// 文件改变调用,大于十分钟调用一次
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //m_nMinute = DateTime.Now.Minute;
            //if (m_nMinute > m_nOldMinute + 10 || (m_nMinute + 50 > m_nOldMinute && m_nOldMinute > m_nMinute))
            //{
            //    m_nOldMinute = m_nMinute;
            //    OnFileRefresh(source,e);
            //}
        }
    }
}
