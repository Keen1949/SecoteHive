using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using AutoFrameDll;
using System.Windows.Forms;
using ToolEx;

namespace AutoFrame
{
    public class StationItem
    {
        public string m_strName = "";
        public StationBase m_StationBase;
        public ListBoxEx m_ListBox;
    }

    public class StationGroup
    {
        public int m_nGroupIndex = 0;
        public Dictionary<string, StationItem> m_dictStation = new Dictionary<string, StationItem>();

        public void Add(string strName,StationItem stationItem)
        {
            if(m_dictStation.ContainsKey(strName))
            {
                m_dictStation[strName] = stationItem;
            }
            else
            {
                m_dictStation.Add(strName, stationItem);
            }
        }
    }

    /// <summary>
    /// 信号种类
    /// </summary>
    public enum Signal
    {
        IO,
        RegBit,
        RegInt,
        RegDouble,
        RegStr,
    }

    /// <summary>
    /// 等待信号类
    /// </summary>
    public class WaitSignal
    {
        public Signal Signal;
        public TValue Name;
        public TValue Value;

        public WaitSignal(Signal sign,TValue name,TValue v)
        {
            Signal = sign;
            Name = name;
            Value = v;
        }
    }

    public class StationMgrEx : SingletonTemplate<StationMgrEx>
    {
        private Dictionary<int, StationGroup> m_dictStaionGroup = new Dictionary<int, StationGroup>();
        private List<TabControl> m_listTabControl = new List<TabControl>();
        private ListBoxEx m_listBoxMain;

        public delegate void NotifyHandler(string strLog, WaitSignal wait = null);
        public static event NotifyHandler NotifyEvent;

        public void AddStation(int group,Form frm, StationBase station, Form frm_manual = null)
        {
            StationItem stationItem = new StationItem();

            ListBoxEx listBox = new ListBoxEx();
            listBox.Dock = DockStyle.Fill;
            listBox.HorizontalScrollbar = true;
            listBox.IntegralHeight = false;

            stationItem.m_StationBase = station;
            stationItem.m_ListBox = listBox;
            stationItem.m_strName = station.Name;

            if (m_dictStaionGroup.ContainsKey(group))
            {
                m_dictStaionGroup[group].Add(station.Name, stationItem);
            }
            else
            {
                StationGroup stationGroup = new StationGroup();
                stationGroup.m_nGroupIndex = group;

                stationGroup.Add(station.Name, stationItem);

                m_dictStaionGroup.Add(group, stationGroup);
            }
            StationMgr.GetInstance().AddStation(frm, station, frm_manual);
        }

        public void SetLogListBox(TableLayoutPanel tableLayoutPanel, LogHandler handler)
        {
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            m_listTabControl.Clear();

            //把TabControl加入TableLayOutPanel中
            float percent = 100.0F / (m_dictStaionGroup.Count+1);

            tableLayoutPanel.ColumnCount = m_dictStaionGroup.Count+1;

            //总控站单独显示
            TabPage tabPageMain = new TabPage();
            tabPageMain.Name = "总控";
            tabPageMain.Text = "总控";

            Form_Notify frm = new Form_Notify();
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            frm.Parent = tabPageMain;
            frm.Visible = true;

            tabPageMain.Controls.Add(frm);

            TabControl tabContolMain = new TabControl();
            tabContolMain.Dock = DockStyle.Fill;

            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, percent));

            tableLayoutPanel.Controls.Add(tabContolMain, 0, 0);

            m_listTabControl.Add(tabContolMain);

            tabContolMain.Controls.Add(tabPageMain);

            StationMgr.GetInstance().SetLogListBox(frm.ListBoxCtrl);
            StationMgr.GetInstance().LogEvent += handler;

            m_listBoxMain = frm.ListBoxCtrl as ListBoxEx;

            //for (int i = 0; i < m_dictStaionGroup.Count;i++)
            int i = 1;
            foreach (var group in m_dictStaionGroup.Values)
            {
                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,percent));

                TabControl tabContol = new TabControl();
                tabContol.Dock = DockStyle.Fill;

                tableLayoutPanel.Controls.Add(tabContol, i, 0);

                m_listTabControl.Add(tabContol);

                //把ListBox加入到TabControl中
                foreach (var item in group.m_dictStation.Values)
                {
                    TabPage tabPage = new TabPage();
                    tabPage.Name = item.m_strName;
                    tabPage.Text = item.m_strName;

                    tabPage.Controls.Add(item.m_ListBox);

                    tabContol.Controls.Add(tabPage);

                    item.m_StationBase.SetLogListBox(item.m_ListBox);
                    item.m_StationBase.LogEvent += handler;   
                }

                i++;
            }

        }

        public void ClearAllLog()
        {
            foreach(var item in m_dictStaionGroup.Values)
            {
                foreach (var listbox in item.m_dictStation.Values)
                {
                    listbox.m_ListBox.Clear();
                }
            }

            if (m_listBoxMain != null && m_listBoxMain.IsHandleCreated)
            {
                m_listBoxMain.Clear();
            }
            
        }

        public static void Notify(string strLog,WaitSignal wait = null,StationEx station = null,int nTimeOut = 0)
        {
            if (NotifyEvent != null)
            {
                NotifyEvent(strLog,wait);
            }

            if (station != null && wait != null)
            {
                switch (wait.Signal)
                {
                    case Signal.IO:
                        station.WaitIo(wait.Name.S, wait.Value.B,nTimeOut);
                        break;

                    case Signal.RegBit:
                        station.WaitRegBit(wait.Name.I, wait.Value.B,nTimeOut);
                        break;

                    case Signal.RegInt:
                        station.WaitRegInt(wait.Name.I, wait.Value.I,nTimeOut);
                        break;
                }
            }
        }
    }
}
