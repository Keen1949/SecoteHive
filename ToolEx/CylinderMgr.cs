using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using CommonTool;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace ToolEx
{
    /// <summary>
    /// 气缸类型
    /// </summary>
    public enum CylType
    {
        /// <summary>
        /// 单控
        /// </summary>
        Single,

        /// <summary>
        /// 双控
        /// </summary>
        Double,   
    }

    /// <summary>
    /// ShowLog委托
    /// </summary>
    /// <param name="strLog"></param>
    /// <param name="level"></param>
    public delegate void ShowLogHandler(string strLog, LogLevel level = LogLevel.Info);

    /// <summary>
    /// WaitIo委托
    /// </summary>
    /// <param name="strIoName">IO名称</param>
    /// <param name="bValid">有效值</param>
    /// <param name="nTimeOut">超时时间</param>
    /// <param name="bShowDialog">是否显示超时对话框</param>
    /// <param name="bPause">是否暂停</param>
    public delegate int WaitIoHandler(string strIoName, bool bValid, int nTimeOut = 0, bool bShowDialog = true, bool bPause = true);

    /// <summary>
    /// 气缸类
    /// </summary>
    public class Cylinder
    {
        /// <summary>
        /// 气缸类型
        /// </summary>
        public CylType m_CylType = CylType.Double;

        /// <summary>
        /// 气缸名称
        /// </summary>
        public string m_strName = "";

        /// <summary>
        /// 气缸输出IO
        /// </summary>
        public string[] m_strIoOuts = new string[2];

        /// <summary>
        /// 气缸输入IO
        /// </summary>
        public string[] m_strIoIns = new string[2];

        /// <summary>
        /// 气缸输入是否禁用
        /// </summary>
        public bool[] m_bEnableIns = new bool[2];

        /// <summary>
        /// 超时时间,单位s
        /// </summary>
        public int m_nTimeOutS = 10;

        /// <summary>
        /// 气缸伸出时间
        /// </summary>
        public int CylOutTimeMs { get; set; }

        /// <summary>
        /// 气缸缩回时间
        /// </summary>
        public int CylBackTimeMs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <param name="type">气缸类型</param>
        /// <param name="IoOut">输出IO</param>
        /// <param name="IoIn">输入IO</param>
        /// <param name="bIoIn">输入IO是否启用</param>
        /// <param name="nTimeout">超时时间ms</param>
        public Cylinder(string strName,CylType type,string[] IoOut,string[] IoIn,bool []bIoIn,int nTimeout)
        {
            m_strName = strName;
            m_CylType = type;

            m_strIoOuts = IoOut;
            m_strIoIns = IoIn;
            m_bEnableIns = bIoIn;

            m_nTimeOutS = nTimeout;
        }

        /// <summary>
        /// 气缸伸出状态
        /// </summary>
        public bool IsOut
        {
            get
            {
                return IoMgr.GetInstance().ReadIoOutBit(m_strIoOuts[0]);
            }
        }

        /// <summary>
        /// 气缸伸出
        /// </summary>
        /// <param name="ShowLog">委托</param>
        /// <param name="WaitIo">委托</param>
        /// <returns>0：未超时 1：超时</returns>
        public int CylOut(ShowLogHandler ShowLog, WaitIoHandler WaitIo)
        {
            int nRet = 1;
            string strLog = "";

            //显示记录
            if (ShowLog != null)
            {
                string str1 = "{0}:输出IO{1}=true";
                string str2 = "{0}:输出IO{1}=true,输出IO{2}=false";
                string strName = m_strName;
                string strIoOut0 = m_strIoOuts[0];
                string strIoOut1 = m_strIoOuts[1];
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "{0}: DO {1} = true";
                    str2 = "{0}: DO {1} = true, DO {2} = false";
                    strName = CylinderMgr.GetInstance().GetCylTranslate(strName);
                    strIoOut0 = IoMgr.GetInstance().GetIoOutTranslate(strIoOut0);
                    strIoOut1 = IoMgr.GetInstance().GetIoOutTranslate(strIoOut1);
                }

                if (m_CylType == CylType.Single)
                {
                    strLog = String.Format(str1,strName, strIoOut0);
                }
                else
                {
                    strLog = String.Format(str2,
                        strName, strIoOut0,strIoOut1);
                }

                ShowLog(strLog);
            }

            int startTick = Environment.TickCount;

            if (m_CylType == CylType.Single)
            {
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[0],true);
            }
            else
            {
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[0], true);
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[1], false);
            }

            // 如果等待
            if (WaitIo != null)
            {
                if (m_bEnableIns[0] && m_strIoIns[0].Length > 0)
                {
                    nRet = WaitIo(m_strIoIns[0], true, m_nTimeOutS);

                    CylOutTimeMs = Environment.TickCount - startTick;
                }

                if (m_bEnableIns[1] && m_strIoIns[1].Length > 0)
                {
                    int r = WaitIo(m_strIoIns[1], false, m_nTimeOutS);

                    CylOutTimeMs = Environment.TickCount - startTick;

                    if (nRet == 0)
                    {
                        nRet = r;
                    }
                }

            }

            return nRet;
        }

        /// <summary>
        /// 气缸缩回
        /// </summary>
        /// <param name="ShowLog">委托</param>
        /// <param name="WaitIo">委托</param>
        /// <returns>0：未超时 1：超时</returns>
        public int CylBack(ShowLogHandler ShowLog, WaitIoHandler WaitIo)
        {
            int nRet = 1;
            string strLog = "";

            //显示记录
            if (ShowLog != null)
            {
                string str1 = "{0}:输出IO{1}=false";
                string str2 = "{0}:输出IO{1}=false,输出IO{2}=true";
                string strName = m_strName;
                string strIoOut0 = m_strIoOuts[0];
                string strIoOut1 = m_strIoOuts[1];
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "{0}: DO {1} = false";
                    str2 = "{0}: DO {1} = false, DO {2} = true";
                    strName = CylinderMgr.GetInstance().GetCylTranslate(strName);
                    strIoOut0 = IoMgr.GetInstance().GetIoOutTranslate(strIoOut0);
                    strIoOut1 = IoMgr.GetInstance().GetIoOutTranslate(strIoOut1);
                }

                if (m_CylType == CylType.Single)
                {
                    strLog = String.Format(str1, strName, strIoOut0);
                }
                else
                {
                    strLog = String.Format(str2,
                        strName, strIoOut0, strIoOut1);
                }

                ShowLog(strLog);       
            }

            int startTick = Environment.TickCount;

            if (m_CylType == CylType.Single)
            {
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[0], false);
            }
            else
            {
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[0], false);
                IoMgr.GetInstance().WriteIoBit(m_strIoOuts[1], true);
            }

            // 如果等待
            if (WaitIo != null)
            {
                if (m_bEnableIns[0] && m_strIoIns[0].Length > 0)
                {
                    nRet = WaitIo(m_strIoIns[0], false, m_nTimeOutS);

                    CylBackTimeMs = Environment.TickCount - startTick;

                }

                if (m_bEnableIns[1] && m_strIoIns[1].Length > 0)
                {
                    int r = WaitIo(m_strIoIns[1], true, m_nTimeOutS);

                    CylBackTimeMs = Environment.TickCount - startTick;

                    if (nRet == 0)
                    {
                        nRet = r;
                    }
                }

            }

            return nRet;
        }

        //209-03-25 Binggoo 加入等待方法
        /// <summary>
        /// 等待气缸伸出
        /// </summary>
        /// <param name="ShowLog">委托</param>
        /// <param name="WaitIo">委托</param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitOut(ShowLogHandler ShowLog, WaitIoHandler WaitIo)
        {
            int nRet = 1;
            string strLog = "";
            //显示记录
            if (ShowLog != null)
            {
                string str1 = "等待{0}伸出";
                string strName = m_strName;
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Wait for {0} to out";
                    strName = CylinderMgr.GetInstance().GetCylTranslate(strName);
                }
                strLog = String.Format(str1, m_strName);

                ShowLog(strLog);
            }

            int startTick = Environment.TickCount;

            // 如果等待
            if (WaitIo != null)
            {
                if (m_bEnableIns[0] && m_strIoIns[0].Length > 0)
                {
                    nRet = WaitIo(m_strIoIns[0], true, m_nTimeOutS);

                    CylOutTimeMs = Environment.TickCount - startTick;
                }

                if (m_bEnableIns[1] && m_strIoIns[1].Length > 0)
                {
                    int r = WaitIo(m_strIoIns[1], false, m_nTimeOutS);

                    CylOutTimeMs = Environment.TickCount - startTick;

                    if (nRet == 0)
                    {
                        nRet = r;
                    }
                }

            }

            return nRet;
        }

        /// <summary>
        /// 等待气缸缩回
        /// </summary>
        /// <param name="ShowLog">委托</param>
        /// <param name="WaitIo">委托</param>
        /// <returns>0：未超时 1：超时</returns>
        public int WaitBack(ShowLogHandler ShowLog, WaitIoHandler WaitIo)
        {
            int nRet = 1;
            string strLog = "";

            //显示记录
            if (ShowLog != null)
            {
                string str1 = "等待{0}缩回";
                string strName = m_strName;
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Wait for {0} to back";
                    strName = CylinderMgr.GetInstance().GetCylTranslate(strName);
                }

                strLog = String.Format(str1, strName);

                ShowLog(strLog);
            }

            int startTick = Environment.TickCount;

            // 如果等待
            if (WaitIo != null)
            {
                if (m_bEnableIns[0] && m_strIoIns[0].Length > 0)
                {
                    nRet = WaitIo(m_strIoIns[0], false, m_nTimeOutS);

                    CylBackTimeMs = Environment.TickCount - startTick;

                }

                if (m_bEnableIns[1] && m_strIoIns[1].Length > 0)
                {
                    int r = WaitIo(m_strIoIns[1], true, m_nTimeOutS);

                    CylBackTimeMs = Environment.TickCount - startTick;

                    if (nRet == 0)
                    {
                        nRet = r;
                    }
                }

            }

            return nRet;
        }
    }
        
    /// <summary>
    /// 气缸管理类
    /// </summary>
    public class CylinderMgr : SingletonTemplate<CylinderMgr>
    {
        /// <summary>
        /// 气缸数据
        /// </summary>
        public Dictionary<string, Cylinder> m_dictCylinders = new Dictionary<string, Cylinder>();

        private Dictionary<string, string> m_dictCylTranslate = new Dictionary<string, string>();

        /// <summary>
        /// 表头
        /// </summary>
        public static readonly string[] HEADS = { "名称","翻译", "类型", "伸出输出", "缩回输出",
            "伸出输入", "缩回输入","伸出启用","缩回启用","超时时间"};

        /// <summary>
        /// 获取气缸对象
        /// </summary>
        /// <param name="strName">气缸名称</param>
        /// <returns></returns>
        public Cylinder GetCyLinder(string strName)
        {
            Cylinder cyl;

            if (!m_dictCylinders.TryGetValue(strName,out cyl))
            {
                string text = string.Format("不存在的气缸名称 {0}， 请确认配置是否正确", strName);

                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    text = string.Format("The cylinder name {0} does not exist, please confirm whether the configuration is correct", strName);
                }

                MessageBox.Show(text);
            }

            return cyl;
        }

        /// <summary>
        /// 获取气缸翻译
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetCylTranslate(string strName)
        {
            string strTranslate = "";
            if (m_dictCylTranslate.TryGetValue(strName,out strTranslate))
            {
                return strTranslate;
            }

            return strName;
        }

        /// <summary>
        /// 从xml文件中读取定义的Data信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictCylinders.Clear();
            m_dictCylTranslate.Clear();
            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Cylinder");            

            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        try
                        {
                            int nCol = 0;
                            //名称
                            string strName = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //2020-02-14 Binggoo 增加气缸翻译
                            string strTranslate = xe.GetAttribute(HEADS[nCol++]).Trim();
                            if (string.IsNullOrEmpty(strTranslate))
                            {
                                strTranslate = strName;
                            }

                            //类型
                            string strType = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //伸出输出
                            string strOutO = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //缩回输出
                            string strBackO = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //伸出输入
                            string strOutI = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //缩回输入
                            string strBackI = xe.GetAttribute(HEADS[nCol++]).Trim();

                            bool bEnableOutI = Convert.ToInt32(xe.GetAttribute(HEADS[nCol++]).Trim()) == 1;

                            bool bEnableBackI = Convert.ToInt32(xe.GetAttribute(HEADS[nCol++]).Trim()) == 1;

                            //2019-03-25，Binggoo 加入超时时间
                            int nTimeOut = 10;

                            if (!int.TryParse(xe.GetAttribute(HEADS[nCol++]).Trim(), out nTimeOut))
                            {
                                nTimeOut = 10;
                            }

                            CylType type = (CylType)Enum.Parse(typeof(CylType), strType);

                            Cylinder cyl = new Cylinder(strName, type, new string[] { strOutO, strBackO },
                                new string[] { strOutI, strBackI }, new bool[] { bEnableOutI, bEnableBackI },nTimeOut);

                            m_dictCylinders.Add(strName,cyl);
                            m_dictCylTranslate.Add(strName, strTranslate);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 跟新内存参数到表格数据
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();

            foreach (var data in m_dictCylinders.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;

                //2020-02-14 Binggoo 增加气缸翻译
                grid.Rows[nRow].Cells[nCol++].Value = m_dictCylTranslate[data.m_strName];

                grid.Rows[nRow].Cells[nCol++].Value = data.m_CylType.ToString();
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoOuts[0];
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoOuts[1];

                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoIns[0];
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoIns[1];

                grid.Rows[nRow].Cells[nCol++].Value = data.m_bEnableIns[0] ? 1: 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_bEnableIns[1] ? 1 : 0;

                //2019-03-25,Binggoo 加入超时时间
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nTimeOutS.ToString();
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            m_dictCylinders.Clear();
            m_dictCylTranslate.Clear();
            foreach (DataGridViewRow row in grid.Rows)
            {
                int nCol = 0;

                try
                {
                    //名称
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strName = row.Cells[nCol++].Value.ToString();

                    //2020-02-14 Binggoo 增加气缸翻译
                    string strTranslate = strName;
                    if (row.Cells[nCol].Value != null)
                    {
                        strTranslate = row.Cells[nCol].Value.ToString();
                    }
                    nCol++;

                    //类型
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strType = row.Cells[nCol++].Value.ToString();

                    //伸出输出
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    string strOutO = row.Cells[nCol++].Value.ToString();

                    //缩回输出
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    string strBackO = row.Cells[nCol++].Value.ToString();

                    //伸出输入
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    string strOutI = row.Cells[nCol++].Value.ToString();

                    //缩回输入
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    string strBackI = row.Cells[nCol++].Value.ToString();

                    //伸出输入启用
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "1";
                    }
                    bool bEnableOutI = Convert.ToInt32(row.Cells[nCol++].Value.ToString()) == 1;

                    //缩回输入启用
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "1";
                    }
                    bool bEnableBackI = Convert.ToInt32(row.Cells[nCol++].Value.ToString()) == 1;

                    //2019-03-25 Binggoo 增加超时时间
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "10";
                    }
                    int nTimeOut = 10;
                    if (!int.TryParse(row.Cells[nCol++].Value.ToString(), out nTimeOut))
                    {
                        nTimeOut = 10;
                    }

                    CylType type = (CylType)Enum.Parse(typeof(CylType), strType);

                    Cylinder cyl = new Cylinder(strName, type, new string[] { strOutO, strBackO },
                        new string[] { strOutI, strBackI }, new bool[] { bEnableOutI, bEnableBackI },nTimeOut);

                    m_dictCylinders.Add(strName, cyl);
                    m_dictCylTranslate.Add(strName, strTranslate);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");

            XmlNode root = xnl.SelectSingleNode("Cylinder");
            if (root == null)
            {
                root = doc.CreateElement("Cylinder");

                xnl.AppendChild(root);
            }

            root.RemoveAll();

            foreach (var data in m_dictCylinders.Values)
            {
                XmlElement xe = doc.CreateElement("Cylinder");
                int nItem = 0;
                xe.SetAttribute(HEADS[nItem++], data.m_strName);

                //2020-02-14 Binggoo 增加气缸翻译
                xe.SetAttribute(HEADS[nItem++], m_dictCylTranslate[data.m_strName]);

                xe.SetAttribute(HEADS[nItem++], data.m_CylType.ToString());
                xe.SetAttribute(HEADS[nItem++], data.m_strIoOuts[0]);
                xe.SetAttribute(HEADS[nItem++], data.m_strIoOuts[1]);
                xe.SetAttribute(HEADS[nItem++], data.m_strIoIns[0]);
                xe.SetAttribute(HEADS[nItem++], data.m_strIoIns[1]);
                xe.SetAttribute(HEADS[nItem++], data.m_bEnableIns[0] ? "1" : "0");
                xe.SetAttribute(HEADS[nItem++], data.m_bEnableIns[1] ? "1" : "0");
                //209-03-25 Binggoo 增加超时时间
                xe.SetAttribute(HEADS[nItem++], data.m_nTimeOutS.ToString());
                root.AppendChild(xe);
            }
        }
    }
}
