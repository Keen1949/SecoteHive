//2019-07-08 Binggoo 1. 解决日志信息中有换行符，导致读文件出错问题
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
using CommonTool;
using AutoFrameDll;
using System.Threading;
using System.Runtime.InteropServices;
using AutoFrameVision;
using Communicate;
//using HalconDotNet;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Alarm : Form
    {
        private double[] m_dDayDowntime = new double[31];
        Dictionary<string, int> m_dicWarn = new Dictionary<string, int>();
        public delegate void WarningDelegate(WarningEventData wed);
        private int m_nMinute = DateTime.Now.Minute;
        private int m_nOldMinute = DateTime.Now.Minute;
        Thread m_thread = null;
        private bool m_bRunThread = false;
        /// <summary>
        /// 互斥对象,在路径下文件更新过程中不接收其他消息
        /// </summary>
        private static readonly object syslock = new object();

        private volatile int m_nMutex = 0;
        void LockMutex() { m_nMutex = 1; }
        void Release() { m_nMutex = 0; }
        bool IsRelease() { return m_nMutex == 0; }

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

        List<string[]> m_listData = new List<string[]>();

        public Form_Alarm()
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

        private void Form_Alarm_Load(object sender, EventArgs e)
        {
            //添加关闭窗口处理事件
            //Form_Main.CloseProgrmEventHandler += new EventHandler(CloseFrm);
            //添加报警委托函数
            WarningMgr.GetInstance().WarningEventHandler += new EventHandler(OnWarning);
            //添加当前报警类信息到当前表格
            for (int k = 0; k < WarningMgr.GetInstance().Count; ++k)
            {
                WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(k);
                dataGridView_current.Rows.Add(wd.tm.ToShortDateString(), wd.tm.ToLongTimeString(),
                                            wd.strCode, wd.strCategory, wd.strMsg, "0");
                SetRowColor(dataGridView_current.Rows[dataGridView_current.Rows.Count - 1], wd.strLevel);
            }
            //复位当前信息表和历史信息表大小
            ResetLayout();
            timer_flash.Enabled = WarningMgr.GetInstance().HasErrorMsg();

            //监控ErrorLog中文件的增加,删除,重命名,改变
            SystemMgr.GetInstance().MonitorImgFile(0, SystemMgr.GetInstance().GetLogPath("\\ErrorLog"), "*.csv",
                OnCreated, OnDelete, OnRenamed, OnChanged);
            //UpdateGrid();
            ////todo 加线程处理
            //LoadErrorLog(SystemMgr.GetInstance().GetLogPath("\\ErrorLog")); //加载指定路径下的ErrorLog文件
            //chart_downtime.Series.First().Points.DataBindY(m_dDayDowntime);   
            //chart_warn.Series.First().Points.DataBindXY(m_dicWarn.Keys, m_dicWarn.Values);
            dataGridView_current.CurrentCell = null;
            //dataGridView_history.CurrentCell = null;
            OnFileRefresh();
        }

        /// <summary>
        /// 设置表格行的颜色,区分错误和警告的颜色
        /// </summary>
        /// <param name="row">表格的行对象</param>
        /// <param name="str">错误或者警告字符串,用来区分表格颜色</param>
        private void SetRowColor(DataGridViewRow row, string str)
        {
            if (str == "ERROR")
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 73, 0);
            else if (str == "WARN")
                row.DefaultCellStyle.BackColor = Color.FromArgb(254, 246, 76);
        }

        /// <summary>
        /// 复位当前信息表和历史信息表大小
        /// </summary>
        private void ResetLayout()
        {
            int n = dataGridView_current.Rows.Count;
            int nHeight = dataGridView_current.ColumnHeadersHeight + (n > 3 ? 3 : n) * dataGridView_current.RowTemplate.Height;
            dataGridView_current.Height = nHeight;
            label1.Top = dataGridView_current.Bottom + 1;
            dataGridView_history.Top = label1.Bottom + 1;
            dataGridView_history.Height = roundButton1.Top - dataGridView_history.Top - 5;
        }

        /// <summary>
        /// 报警信息异步委托调用函数
        /// </summary>
        /// <param name="wed">报警信息</param>
        void ProcessWarning(WarningEventData wed)
        {
            if (wed.bAdd) //增加一条异常信息-----todo
            {
                if (WarningMgr.GetInstance().HasErrorMsg())
                {
                    WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(wed.nIndex);        

                    dataGridView_current.Rows.Add(wd.tm.ToShortDateString(), wd.tm.ToLongTimeString(), wd.strCode, wd.strCategory, wd.strMsg, "0");
                    SetRowColor(dataGridView_current.Rows[dataGridView_current.Rows.Count - 1], wd.strLevel);
                    ResetLayout();
                    timer_flash.Enabled = true;
                }
            }
            else
            {
                if (wed.nIndex == -1)  //清除所有异常信息
                {
                    dataGridView_current.Rows.Clear();
                }
                else
                {
                    dataGridView_current.Rows.RemoveAt(wed.nIndex);
                }
                ResetLayout();
                timer_flash.Enabled = dataGridView_current.Rows.Count > 0;
            }
        }

        /// <summary>
        /// 报警信息委托调用函数
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnWarning(object Sender, EventArgs e)
        {
            WarningEventData wed = (WarningEventData)e;
            WarningDelegate da = new WarningDelegate(ProcessWarning);
            this.BeginInvoke(da, wed); // 异步调用委托,调用后立即返回并立即执行下面的语句
        }

        /// <summary>
        /// 加载指定路径下的ErrorLog文件显示到表格
        /// </summary>
        /// <param name="strPath">文件的路径</param>
        private void LoadErrorLog(string strPath)
        {
            dataGridView_history.Rows.Clear();
            m_dicWarn.Clear();
            m_dDayDowntime.Initialize();
            try
            {
                //得到路径下所有文件名
                string[] strFile = Directory.GetFiles(strPath, "*.csv");
                foreach (string s in strFile)
                {
                    //得到不包含后缀类型的的文件名
                    string strTmp = Path.GetFileNameWithoutExtension(s);
                    string strDate = strTmp.Substring(strTmp.Length - 8, 8);
                    //得到年月日期
                    DateTime dt = DateTime.ParseExact(strDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    TimeSpan ts = DateTime.Now - dt;
                    if (ts.Days < 31)
                    {
                        FileInfo fi = new FileInfo(s);
                        if (!fi.Exists)
                            continue;
                        StreamReader sr = new StreamReader(s, Encoding.Default);
                        String line;
                        //从文件中读取一行数据
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == string.Empty)
                                break;
                            string[] data = line.Split(',');

                            TimeSpan tm = new TimeSpan();

                            bool bException = false;
                            string strTempLine = "";
                            do
                            {
                                try
                                {
                                    strTempLine += line.Trim();

                                    data = strTempLine.Split(',');

                                    tm = TimeSpan.ParseExact(data[data.Length - 1], "g", System.Globalization.CultureInfo.CurrentCulture);

                                    bException = false;
                                }
                                catch (Exception)
                                {
                                    //无效格式，再多读一行
                                    bException = true;
                                }

                            } while (bException && (line = sr.ReadLine()) != null);

                            if (data.Length > 7)
                            {
                                StringBuilder sb = new StringBuilder(data[5]);
                                for (int k = 6; k < data.Length - 1; ++k)
                                {
                                    sb.Append(",");
                                    sb.Append(data[k]);
                                }

                                dataGridView_history.Rows.Add(data[0], data[1], data[3], data[4], sb.ToString(), data[data.Length - 1]);
                            }
                            else
                            {
                                dataGridView_history.Rows.Add(data[0], data[1], data[3], data[4], data[5], data[data.Length - 1]);
                            }
                            SetRowColor(dataGridView_history.Rows[dataGridView_history.Rows.Count - 1], data[2]);

                            m_dDayDowntime[ts.Days] += tm.TotalMinutes;
                            //统计报警信息分类的个数
                            if (m_dicWarn.ContainsKey(data[3]))
                            {
                                m_dicWarn[data[3]] += 1;
                            }
                            else
                                m_dicWarn.Add(data[3], 1);
                        }
                        sr.Close();
                    }
                }
                //DataTable table;
                //table = new DataTable();
            }
            catch (Exception e)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(e.Message, "Alarm file read failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(e.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        /// <summary>
        /// 定时器,定时显示当前报警信息所持续的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_flash_Tick(object sender, EventArgs e)
        {
            int i = 0;
            for (int k = 0; k < WarningMgr.GetInstance().Count; ++k)
            {
                WARNING_DATA wd = WarningMgr.GetInstance().GetWarning(k);
                if (i < dataGridView_current.Rows.Count)
                {
                    TimeSpan ts = DateTime.Now - wd.tm;
                    dataGridView_current.Rows[i].Cells[5].Value = ts.ToString(@"hh\:mm\:ss");
                    ++i;
                }
            }
        }

        /// <summary>
        /// 响应右键消息菜单,清除全部警告类信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_CleanAll_Click(object sender, EventArgs e)
        {
            WarningMgr.GetInstance().ClearAllWarning();
        }

        /// <summary>
        /// 响应右键消息菜单,清除当前行警告类信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Clean_Click(object sender, EventArgs e)
        {
            if (dataGridView_current.CurrentRow != null)
            {
                WarningMgr.GetInstance().ClearWarning(dataGridView_current.CurrentRow.Index);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_current_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (dataGridView_current.CurrentRow != null)
                    dataGridView_current.CurrentRow.Selected = false;
                dataGridView_current.Rows[e.RowIndex].Selected = true;
                dataGridView_current.CurrentCell = dataGridView_current.Rows[e.RowIndex].Cells[e.ColumnIndex];
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);  //鼠标右键后,显示菜单项
            }
        }
        //监视文件夹变动, 刷新表格

        /// <summary>
        /// 更新界面数据
        /// </summary>
        private void UpdateGrid()
        {
            LoadErrorLog(SystemMgr.GetInstance().GetLogPath("\\ErrorLog")); //加载指定路径下的ErrorLog文件
            chart_downtime.Series.First().Points.DataBindY(m_dDayDowntime);
            chart_warn.Series.First().Points.DataBindXY(m_dicWarn.Keys, m_dicWarn.Values);
            Release();
        }


        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;
        delegate void dgvDelegate(List<String[]> m_listData, int nStart);
        delegate void dgvClearDelegate();
        delegate void chartDownTimeDelegate();
        delegate void chartWarnDelegate();
        delegate void dgvRefreshDelegate();

        /// <summary>
        /// 表格增加项
        /// </summary>
        /// <param name="m_listData"></param>
        /// <param name="nStart"></param>
        /// <param name="nEnd"></param>
        private void DgvDataAdd(List<String[]> listData, int nStart)
        {
            //禁止pnl重绘  
            SendMessage(dataGridView_history.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
           
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    //string str = string.Format("nStart={0},nEnd={1},m_listData.Count={2}", nStart, nEnd, m_listData.Count);
                    //WarningMgr.GetInstance().Info(str);
                    int m = i + nStart;
                    if (m >= listData.Count)
                        break;

                    string[] data = listData.ElementAt(m);
                    if (data != null && data.Length > 7)
                    {
                        StringBuilder sb = new StringBuilder(data[5]);
                        for (int k = 6; k < data.Length - 1; ++k)
                        {
                            sb.Append(",");
                            sb.Append(data[k]);  
                        }
                            
                        dataGridView_history.Rows.Add(data[0], data[1], data[3], data[4], sb.ToString(), data[data.Length - 1]);
                    }
                    else if (data != null)
                    {
                        dataGridView_history.Rows.Add(data[0], data[1], data[3], data[4], data[5], data[data.Length - 1]);
                    }
                    SetRowColor(dataGridView_history.Rows[dataGridView_history.Rows.Count - 1], data[2]);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
       
            SendMessage(dataGridView_history.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            dataGridView_history.Refresh();
            dataGridView_history.CurrentCell = null;
            m_nMuxRead++;
            //允许重绘pnl  
            //SendMessage(dataGridView_history.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            //dataGridView_history.Refresh();
        }

        private void SetDgvDataSource(List<String[]> listData, int nStart)
        {
              DgvDataAdd(listData, nStart);
        }

        /// <summary>
        /// 初始化表格以及相关参数
        /// </summary>
        private void ClearDgv()
        {
            if (dataGridView_history.InvokeRequired)
            {
                dataGridView_history.Invoke(new dgvClearDelegate(ClearDgv));
            }
            else
            {
                InitMuxSR();
                dataGridView_history.Rows.Clear();
                m_dicWarn.Clear();
                m_dDayDowntime.Initialize();
                //禁止pnl重绘  
                SendMessage(dataGridView_history.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            }
        }
        private void ChartDownTime()
        {
            if (chart_downtime.InvokeRequired)
            {
                chart_downtime.BeginInvoke(new chartDownTimeDelegate(ChartDownTime));
            }
            else
            {
                chart_downtime.Series.First().Points.DataBindY(m_dDayDowntime);
            }
        }
        private void ChartWarn()
        {
            if (chart_warn.InvokeRequired)
            {
                chart_warn.BeginInvoke(new chartWarnDelegate(ChartWarn));
            }
            else
            {
                chart_warn.Series.First().Points.DataBindXY(m_dicWarn.Keys, m_dicWarn.Values);
            }
        }
        private void DgvRefresh()
        {
            if (dataGridView_history.InvokeRequired)
            {
                dataGridView_history.BeginInvoke(new dgvRefreshDelegate(DgvRefresh));
            }
            else
            {
                dataGridView_history.CurrentCell = null;
                //允许重绘pnl  
                SendMessage(dataGridView_history.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                dataGridView_history.Refresh();
            }
        }

        /// <summary>
        /// 监控的目录下文件有变动,刷新
        /// </summary>
        private void OnFileRefresh()
        {
            lock(syslock)
            {
                if (IsRelease())
                {
                    LockMutex();
                    ClearDgv();
                    m_thread = new Thread(new ThreadStart(ThreadLoadData));
                    m_thread.Start();
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
        private void OnDelete(object source, FileSystemEventArgs e)
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

        private void Form_Alarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_thread != null && m_thread.IsAlive)
            {
                m_bRunThread = false;
                if (m_thread.Join(2000) == false)
                {
                    m_thread.Abort();
                }
                m_thread = null;
            }
        }

        /// <summary>
        /// 更新Error数据，线程调用函数
        /// </summary>
        private void ThreadLoadData()
        {
            Thread.Sleep(50);
            m_listData.Clear();
            string strPath = SystemMgr.GetInstance().GetLogPath("\\ErrorLog");
            try
            {
                //得到路径下所有文件名
                string[] strFile = Directory.GetFiles(strPath, "*.csv");
                DateTime testStart = DateTime.Now;
                foreach (string s in strFile)
                {
                    Thread.Sleep(5);
                    //得到不包含后缀类型的的文件名
                    string strTmp = Path.GetFileNameWithoutExtension(s);
                    string strDate = strTmp.Substring(strTmp.Length - 8, 8);
                    int nDate = strDate.Length;
                    bool bContinue = false;
                    for (int i = 0; i < nDate; i++)
                    {
                        if (strDate[i] > '9' || strDate[i] < '0')
                        {
                            bContinue = true;
                            break;
                        }
                    }
                    if (bContinue)
                        continue;
                    //得到年月日期
                    DateTime dt = DateTime.ParseExact(strDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    TimeSpan ts = DateTime.Now - dt;
                    if (ts.Days < 31)
                    {
                        FileInfo fi = new FileInfo(s);
                        if (!fi.Exists)
                            continue;
                        StreamReader sr = new StreamReader(s, Encoding.Default);
                        String line;
                        //从文件中读取一行数据
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == string.Empty)
                                break;
                            string[] data = line.Split(',');

                            TimeSpan tm = new TimeSpan();

                            bool bException = false;
                            string strTempLine = "";
                            do
                            {
                                try
                                {
                                    strTempLine += line.Trim();

                                    data = strTempLine.Split(',');

                                    tm = TimeSpan.ParseExact(data[data.Length - 1], "g", System.Globalization.CultureInfo.CurrentCulture);

                                    bException = false;
                                }
                                catch (Exception)
                                {
                                    //无效格式，再多读一行
                                    bException = true;
                                }

                            } while (bException && (line = sr.ReadLine()) != null);
                            
                     
                            if (ts.Days >= m_dDayDowntime.Length || ts.Days < 0)
                                continue;

                            m_dDayDowntime[ts.Days] += tm.TotalMinutes;

                            m_listData.Add(data);
                            //统计报警信息分类的个数
                            if (m_dicWarn.ContainsKey(data[3]))
                            {
                                m_dicWarn[data[3]] += 1;
                            }
                            else
                                m_dicWarn.Add(data[3], 1);
                        }
                        sr.Close();
                    }
                }

                int nCount = m_listData.Count;
                int nStart = 0;
                m_bRunThread = true;
                bool bThread = false;
                while (m_bRunThread)
                {
                    Thread.Sleep(5);
                    if (IsEnableSend())
                    {
                        if (!bThread)
                        {
                            dataGridView_history.Invoke(new dgvDelegate(SetDgvDataSource), new object[] { m_listData, nStart });
                            nStart += 50;

                            if (nStart >= m_listData.Count)
                                bThread = true;
                            
                            
                        }
                        else if (bThread)
                        {
                            ChartDownTime();
                            ChartWarn();
                            DgvRefresh();
                            m_bRunThread = false;
                            break;
                        }
                    }
                    if (IsEpual())
                    {
                        WarningMgr.GetInstance().Info("Form_Alarm thread has BUG");
                        MessageBox.Show("Form_Alarm thread has BUG");
                        m_bRunThread = false;
                    }
                }
            }
            catch (Exception e)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(e.Message, "Alarm file read failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show(e.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            finally
            {
                Release();
            }
        }

     }
}
