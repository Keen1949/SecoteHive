//2019-09-23 Binggoo 1. 加入通过AutoResetEvent来实现站位间同步
using CommonTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Concurrent;

namespace AutoFrame
{
    public struct RunModeInfo
    {
        public string m_strName;
        public string m_strStation;
        public string m_strMethod;
    }

    public enum RunMode
    {
        Calib,
        GRR
    }

    class SystemMgrEx : SingletonTemplate<SystemMgrEx>
    {
        public Dictionary<string, RunModeInfo> m_dictCalibs = new Dictionary<string, RunModeInfo>();
        public Dictionary<string, RunModeInfo> m_dictGrrs = new Dictionary<string, RunModeInfo>();

        public string CurrentCalib { get; set; }
        public string CurrentGrr { get; set; }

        private ConcurrentDictionary<int, AutoResetEvent> m_dictAutoResetEvent = new ConcurrentDictionary<int, AutoResetEvent>();

        public SystemMgrEx()
        {
            foreach (var item in Enum.GetValues(typeof(SysBitReg)))
            {
                AutoResetEvent evt = new AutoResetEvent(false);

                m_dictAutoResetEvent.TryAdd((int)item, evt);
            }
        }

        public void Release()
        {
            foreach (var item in m_dictAutoResetEvent.Values)
            {
                item.Set();
            }
        }


        /// <summary>
        /// 从xml文件中读取定义的信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictCalibs.Clear();
            m_dictGrrs.Clear();

            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "RunMode");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        XmlElement xe = (XmlElement)xn;

                        if (xn.Name == "Calib")
                        {
                            #region Calib
                            foreach (XmlNode item in xn.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                string strName = xeItem.GetAttribute("名称");
                                string strStation = xeItem.GetAttribute("站位");
                                string strMethod = xeItem.GetAttribute("方法");

                                if (string.IsNullOrEmpty(strName))
                                {
                                    continue;
                                }

                                RunModeInfo mode;
                                mode.m_strName = strName;
                                mode.m_strStation = strStation;
                                mode.m_strMethod = strMethod;

                                m_dictCalibs.Add(strName, mode);
                            }
                            #endregion
                        }
                        else if (xn.Name == "GRR")
                        {
                            #region GRR
                            foreach (XmlNode item in xn.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                string strName = xeItem.GetAttribute("名称");
                                string strStation = xeItem.GetAttribute("站位");
                                string strMethod = xeItem.GetAttribute("方法");

                                if (string.IsNullOrEmpty(strName))
                                {
                                    continue;
                                }

                                RunModeInfo mode;
                                mode.m_strName = strName;
                                mode.m_strStation = strStation;
                                mode.m_strMethod = strMethod;

                                m_dictGrrs.Add(strName, mode);
                            }
                            #endregion
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 更新内存参数到表格数据
        /// </summary>
        /// <param name="strGroupName">组名</param>
        /// <param name="grid">控件</param>
        public void UpdateGridFromParam(RunMode mode, DataGridView grid)
        {

            grid.Rows.Clear();

            switch (mode)
            {
                case RunMode.Calib:
                    #region Calib
                    {
                        foreach (var data in m_dictCalibs.Values)
                        {
                            int nRow = grid.Rows.Add();
                            int nCol = 0;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strStation;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strMethod;
                        }
                    }
                    #endregion
                    break;

                case RunMode.GRR:
                    #region GRR
                    {
                        foreach (var data in m_dictGrrs.Values)
                        {
                            int nRow = grid.Rows.Add();
                            int nCol = 0;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strStation;
                            grid.Rows[nRow].Cells[nCol++].Value = data.m_strMethod;
                        }
                    }
                    #endregion
                    break;
            }


        }

        /// <summary>
        /// 更新内存参数到表格数据
        /// </summary>
        /// <param name="strGroupName">组名</param>
        /// <param name="grid">控件</param>
        public void UpdateParamFromGrid(RunMode mode, DataGridView grid)
        {
            switch (mode)
            {
                case RunMode.Calib:
                    #region Calib
                    {
                        m_dictCalibs.Clear();
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            int nCol = 0;
                            RunModeInfo info;
                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strName = row.Cells[nCol].Value.ToString();
                                nCol++;
                            }
                            else
                            {
                                continue;
                            }

                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strStation = row.Cells[nCol].Value.ToString();
                                nCol++;
                            }
                            else
                            {
                                continue;
                            }

                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strMethod = row.Cells[nCol].Value.ToString();
                            }
                            else
                            {
                                info.m_strMethod = "";
                            }

                            nCol++;

                            m_dictCalibs.Add(info.m_strName, info);
                        }
                    }
                    #endregion
                    break;

                case RunMode.GRR:
                    #region GRR
                    {
                        m_dictGrrs.Clear();
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            int nCol = 0;
                            RunModeInfo info;
                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strName = row.Cells[nCol].Value.ToString();
                                nCol++;
                            }
                            else
                            {
                                continue;
                            }

                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strStation = row.Cells[nCol].Value.ToString();
                                nCol++;
                            }
                            else
                            {
                                continue;
                            }

                            if (row.Cells[nCol].Value != null)
                            {
                                info.m_strMethod = row.Cells[nCol].Value.ToString();
                            }
                            else
                            {
                                info.m_strMethod = "";
                            }

                            nCol++;

                            m_dictGrrs.Add(info.m_strName, info);
                        }
                    }
                    #endregion
                    break;
            }


        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");

            XmlNode root = xnl.SelectSingleNode("RunMode");
            if (root == null)
            {
                root = doc.CreateElement("RunMode");

                xnl.AppendChild(root);
            }

            root.RemoveAll();

            #region Calib
            XmlElement childCalib = doc.CreateElement("Calib");
            foreach (var calib in m_dictCalibs.Values)
            {
                XmlElement xe = doc.CreateElement("Item");

                xe.SetAttribute("名称", calib.m_strName);
                xe.SetAttribute("站位", calib.m_strStation);
                xe.SetAttribute("方法", calib.m_strMethod);

                childCalib.AppendChild(xe);
            }

            root.AppendChild(childCalib);
            #endregion

            #region GRR
            XmlElement childGrr = doc.CreateElement("GRR");
            foreach (var calib in m_dictGrrs.Values)
            {
                XmlElement xe = doc.CreateElement("Item");

                xe.SetAttribute("名称", calib.m_strName);
                xe.SetAttribute("站位", calib.m_strStation);
                xe.SetAttribute("方法", calib.m_strMethod);

                childGrr.AppendChild(xe);
            }

            root.AppendChild(childGrr);
            #endregion

        }

        public void SetEvent(int index)
        {
            AutoResetEvent evt;
            if (m_dictAutoResetEvent.TryGetValue(index,out evt))
            {
                evt.Set();
            }
            else
            {
                throw new Exception(string.Format("时间{0}不存在", index));
            }
        }

        public void ResetEvent(int index)
        {
            AutoResetEvent evt;
            if (m_dictAutoResetEvent.TryGetValue(index, out evt))
            {
                evt.Reset();
            }
            else
            {
                throw new Exception(string.Format("时间{0}不存在", index));
            }
        }

        public bool WaitEvent(int index,int timeoutS = -1)
        {
            AutoResetEvent evt;
            if (m_dictAutoResetEvent.TryGetValue(index, out evt))
            {
                if (timeoutS == -1)
                {
                    return evt.WaitOne();
                }
                else if (timeoutS == 0)
                {
                    return evt.WaitOne(SystemMgr.GetInstance().GetParamInt("RegTimeOut") * 1000);
                }
                else
                {
                    return evt.WaitOne(timeoutS * 1000);
                }
            }
            else
            {
                throw new Exception(string.Format("时间{0}不存在", index));
            }
        }
    }
}
