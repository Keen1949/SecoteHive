using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoFrameDll;
using CommonTool;

namespace AutoFrame
{
    public partial class Form_Notify : Form
    {

        private List<WaitSignal> m_listWaits = new List<WaitSignal>();
        private object m_lock = new object();

        public Form_Notify()
        {
            InitializeComponent();

            StationMgrEx.NotifyEvent += OnNotifyEvent;

            StationMgr.GetInstance().StateChangedEvent += OnStationStateChanged; //委托中添加站位状态变化响应函数操作

            timer1.Interval = 20;
            timer1.Enabled = false;
        }

        /// <summary>
        /// 站位状态变化委托响应函数
        /// </summary>
        /// <param name="state">站位状态值</param>
        private void OnStationStateChanged(StationState OldState, StationState NewState)
        {
            switch (NewState)
            {
                case StationState.STATE_MANUAL:  //手动状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        timer1.Enabled = false;

                        Clear();
                    });
                    
                    break;
                case StationState.STATE_AUTO:   //自动运行状态
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        timer1.Enabled = true;
                    });
                    break;
                default:
                    break;
            }
        }


        private void OnNotifyEvent(string strLog, WaitSignal wait = null)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                lock(m_lock)
                {
                    int nItem = listBoxEx1.Append(strLog, Color.Yellow, listBoxEx1.ForeColor, true);

                    if (wait != null)
                    {
                        while (nItem > m_listWaits.Count)
                        {
                            m_listWaits.Add(null);
                        }

                        m_listWaits.Add(wait);
                    }
                }   

            });
        }

        public Control ListBoxCtrl
        {
            get
            {
                return listBoxEx1;
            }
        }

        private void Clear()
        {
            lock(m_lock)
            {
                for (int i = 0;i < m_listWaits.Count;i++)
                {
                    WaitSignal wait = m_listWaits[i];

                    if (wait != null && listBoxEx1.Count > i)
                    {
                        listBoxEx1.RemoveAt(i);
                        m_listWaits.RemoveAt(i);
                        i--;
                    }
                }
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            lock(m_lock)
            {
                for (int i = 0; i < m_listWaits.Count; i++)
                {
                    WaitSignal wait = m_listWaits[i];

                    if (wait == null)
                    {
                        continue;
                    }
                    bool bFlag = false;
                    switch (wait.Signal)
                    {
                        case Signal.IO:
                            if (IoMgr.GetInstance().ReadIoInBit(wait.Name.S) == wait.Value.B)
                            {
                                bFlag = true;
                            }
                            break;

                        case Signal.RegBit:
                            if (SystemMgr.GetInstance().GetRegBit(wait.Name.I) == wait.Value.B)
                            {
                                bFlag = true;
                            }
                            break;

                        case Signal.RegInt:
                            if (SystemMgr.GetInstance().GetRegInt(wait.Name.I) == wait.Value.I)
                            {
                                bFlag = true;
                            }
                            break;

                        case Signal.RegDouble:
                            if (SystemMgr.GetInstance().GetRegDouble(wait.Name.I) == wait.Value.D)
                            {
                                bFlag = true;
                            }
                            break;

                        case Signal.RegStr:
                            if (SystemMgr.GetInstance().GetRegString(wait.Name.I) == wait.Value.S)
                            {
                                bFlag = true;
                            }
                            break;
                    }

                    if (bFlag)
                    {
                        m_listWaits.RemoveAt(i);
                        listBoxEx1.RemoveAt(i);

                        i--;
                    }

                }
            }   
            timer1.Enabled = true;
        }
    }
}
