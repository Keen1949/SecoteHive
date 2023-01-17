//2019-04-30 Binggoo 1. 界面保存数据超过2000条时，自动清空数据，防止数据过多，界面显示不出来。
//2019-06-12 Binggoo 1. 加入是否保存ShowLog参数
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
using ToolEx;
using System.Threading;

namespace AutoFrame
{
    public partial class Form_Auto : Form
    {
        private int m_okCount = 0;
        private int m_ngCount = 0;

        private DateTime m_tmCTBegin;
        private TimeSpan m_tsBestCT;
        private TimeSpan m_tsSoftware;
        private TimeSpan m_tsMachine;

        public Form_Auto()
        {
            InitializeComponent();

            InitCtrlAnchor();



            m_tsBestCT = TimeSpan.Zero;
            m_tsSoftware = TimeSpan.Zero;
            m_tsMachine = TimeSpan.Zero;

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

        private void InitCtrlAnchor()
        {
            //Anchor属性最好在程序里修改，这样便于修改界面，否则界面上的控件会随着窗口大小变化而变化，对添加控件产生困扰
            groupBox_Info.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox_WorkMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox_MachineState.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            //tableLayoutPanel_Log.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tabControl_Data.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

        }

        private void OnDataSaveChanged()
        {
            /*
            if (DataMgr.GetInstance().SaveType == SaveType.DB)
            {
                //删除表
                SqlBase sql = new MySQL();

                if (!sql.Connect(DataMgr.GetInstance().Server, 
                    DataMgr.GetInstance().Port, 
                    DataMgr.GetInstance().UserID, 
                    DataMgr.GetInstance().Password,
                    DataMgr.GetInstance().Database))
                {
                    return;
                }

                if (sql.IsTableExist(DataMgr.GetInstance().TableName))
                {
                    sql.DropTable(DataMgr.GetInstance().TableName);
                }
            }
            */
        }

        private void Form_Auto_Load(object sender, EventArgs e)
        {
            HiveMgr.GetInstance().StartUpload();
           
            //Robot_ABB.Init_All_Robot();
            SystemMgr.GetInstance().BitChangedEvent += OnSystemBitChanged;  //委托中添加系统位寄存器响应函数操作 
            SystemMgr.GetInstance().IntChangedEvent += OnSystemIntChanged;  //委托中添加系统整型寄存器响应函数操作 
            SystemMgr.GetInstance().DoubleChangedEvent += OnSystemDoubleChanged;  //委托中添加系统浮点型寄存器响应函数操作 

            StationMgr.GetInstance().StateChangedEvent += OnStationStateChanged; //委托中添加站位状态变化响应函数操作
            StationMgr.GetInstance().StopRun();

            DataMgr.GetInstance().DataSaveChangeEvent += OnDataSaveChanged;


            //关联站位对应的ListBox，站位中的ShowLog会显示在关联的ListBox中
            StationMgrEx.GetInstance().SetLogListBox(tableLayoutPanel_Log, OnLogView);

            if (SystemMgr.GetInstance().GetParamBool("AutoCycle"))
            {
                StationMgr.GetInstance().BAutoMode = true;  //设置半自动运行属性
                roundButton_auto.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);   //给自动按钮赋初始化颜色
                roundButton_manual.BaseColor = Color.FromArgb(220, 221, 224); //给手动操作按钮赋初始化颜色
            }
            else
            {
                StationMgr.GetInstance().BAutoMode = false;  //设置半自动运行属性
                roundButton_auto.BaseColor = Color.FromArgb(220, 221, 224);   //给自动按钮赋初始化颜色
                roundButton_manual.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff); //给手动操作按钮赋初始化颜色
            }

            WarningMgr.GetInstance().WarningEventHandler += OnWarning;//添加自动界面报警信息响应函数委托
            OnWarning(this, EventArgs.Empty);  //清除自动界面报警信息

            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;

            IoMgr.GetInstance().IoChangedEvent += OnIoChanged;

            ProductMgr.GetInstance().SendProductDataEvent += OnSendProductDataEvent;


            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }



        private void OnSendProductDataEvent(ProductData data)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    #region 更新界面
                    if (data.m_bResult)
                    {
                        m_okCount++;
                    }
                    else
                    {
                        m_ngCount++;
                    }


                    int nDataCol = 0;

                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_strHSGCode;
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_strJigCode;
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_nGlueIndex.ToString() ;
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_dtBeginTime.ToString("yyyyMMdd HH:mm:ss");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_dtEndTime.ToString("yyyyMMdd HH:mm:ss");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.GlueTimeS.ToString("f3");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.ExposureGlueTimeS.ToString("f3");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_dbRecheckX.ToString("f3");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_dbRecheckY.ToString("f3");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_dbRecheckU.ToString("f2");
                    //dataGridView_Result.Rows[nDataRow].Cells[nDataCol++].Value = data.m_bResult ? "OK":"NG";


                    #endregion


                    ProductMgr.GetInstance().SaveData(data);

                    /* 2020-2-10 保存文件和上传移到站位去坐，便于管控
                    #region 保存文件
                    if (DataMgr.GetInstance().DataSaveEnable && !string.IsNullOrEmpty(DataMgr.GetInstance().SavePath))
                    {
                        Task.Run(delegate
                        {
                            string strSavePath = DataMgr.GetInstance().SavePath;
                            string fileName = String.Format("{0}_{1}.csv", ProductMgr.GetInstance().DeviceName, DateTime.Now.ToString("yyyyMMdd"));

                            //记录文件
                            CsvOperation csv = new CsvOperation(strSavePath, fileName);

                            int row = 0, col = 0;
                            if (!File.Exists(csv.FileName))
                            {
                                foreach (var item in DataMgr.GetInstance().m_dictDataSave.Keys)
                                {
                                    csv[0, col++] = item;
                                }

                                row = 1;
                            }

                            col = 0;

                            foreach (var item in DataMgr.GetInstance().m_dictDataSave)
                            {
                                Type t = data.GetType(item.Value);
                                if (t == typeof(DateTime))
                                {
                                    csv[row, col++] = ((DateTime)data.GetValue(item.Value)).ToString("yyyyMMdd HH:mm:ss");
                                }
                                else if (t == typeof(double))
                                {
                                    csv[row, col++] = ((double)data.GetValue(item.Value)).ToString("f3");
                                }
                                else if (t == typeof(bool))
                                {
                                    csv[row, col++] = ((bool)data.GetValue(item.Value)) ? "OK" : "NG";
                                }
                                else
                                {
                                    csv[row, col++] = data.GetValue(item.Value).ToString();
                                }
                            }

                            csv.Save();
                        });
                    }

                    #endregion

                    #region 上传PDCA
                    DataGroup pdca;
                    if (DataMgr.GetInstance().m_dictDataGroup.TryGetValue("PDCA数据", out pdca)
                        && SystemMgr.GetInstance().GetParamBool("PDCAEnable"))
                    {
                        string strPDCA = ProductMgr.GetInstance().GetPDCAString(pdca, data.m_strBarCode, data);

                        //此处上传PDCA
                        if (m_tcpPDCA != null)
                        {
                            if (m_tcpPDCA.IsOpen())
                            {
                                Task.Run(delegate
                                {
                                    if (m_tcpPDCA.WriteString(strPDCA))
                                    {
                                        //此处采用异步读取结果，也可同步读取，但会卡时间

                                    }
                                });

                            }
                        }



                        strPDCA = strPDCA.Replace("\n", "\r\n");

                        textBox_Send.Text = strPDCA;
                    }
                    #endregion
                    */
                }
                catch (Exception ex)
                {
                    WarningMgr.GetInstance().Error(ex.Message);

                    MessageBox.Show(ex.ToString());
                }


            });
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {

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
                    label_sta_manual.ImageIndex = 1;
                    label_sta_pause.ImageIndex = 0;
                    label_sta_auto.ImageIndex = 0;
                    label_sta_ready.ImageIndex = 0;
                    label_sta_emg.ImageIndex = 0;

                    break;
                case StationState.STATE_AUTO:   //自动运行状态
                    label_sta_auto.ImageIndex = 1;
                    label_sta_manual.ImageIndex = 0;
                    label_sta_pause.ImageIndex = 0;
                    label_sta_ready.ImageIndex = 0;
                    label_sta_emg.ImageIndex = 0;


                    break;
                case StationState.STATE_READY:  //等待开始
                    label_sta_ready.ImageIndex = 1;
                    break;
                case StationState.STATE_EMG:         //急停状态
                    label_sta_emg.ImageIndex = 2;
                    label_sta_pause.ImageIndex = 0;
                    label_sta_ready.ImageIndex = 0;

                    break;
                case StationState.STATE_PAUSE:       //暂停状态
                    label_sta_pause.ImageIndex = 1;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 响应IO变化事件
        /// </summary>
        /// <param name="nCard"></param>
        private void OnIoChanged(int nCard)
        {
            if (nCard == 1)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {

                });
            }


        }

        /// <summary>
        /// 报警信息委托调用函数
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnWarning(object Sender, EventArgs e)
        {
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                if (label_warning.BackColor == Color.FromKnownColor(KnownColor.Control))
                {
                    label_warning.BackColor = Color.Red;
                }
                if (label_warning.InvokeRequired)  //c#中禁止跨线程直接访问控件，InvokeRequired是为了解决这个问题而产生的,用一个异步执行委托
                {
                    Action<string> actionDelegate = (x) => { this.label_warning.Text = x.ToString(); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    this.label_warning.BeginInvoke(actionDelegate, WarningMgr.GetInstance().GetLastMsg().strMsg);

                }
                else
                {
                    label_warning.Text = WarningMgr.GetInstance().GetLastMsg().strMsg;
                }

                object test = WarningMgr.GetInstance().GetLastMsg();


            }
            else
            {
                label_warning.BackColor = Color.FromKnownColor(KnownColor.Control);
                if (label_warning.InvokeRequired) //c#中禁止跨线程直接访问控件，InvokeRequired是为了解决这个问题而产生的,用一个异步执行委托
                {
                    Action<string> actionDelegate = (x) => { this.label_warning.Text = x.ToString(); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    this.label_warning.BeginInvoke(actionDelegate, string.Empty);

                }
                else
                    label_warning.Text = string.Empty;
            }
        }

        /// <summary>
        /// 系统位寄存器变化委托响应函数
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        protected void OnSystemBitChanged(int nIndex, bool bBit)
        {
            SysBitReg sbr = (SysBitReg)nIndex;
            switch (sbr)
            {
            }
        }


        //定义一个关联进度条时间刷新的委托
        public delegate void CrossDelegate(int nStep);

        /// <summary>
        /// 系统整型寄存器变化委托响应函数
        /// </summary>
        /// <param name="nIndex">寄存器索引</param>
        /// <param name="nData">寄存器值</param>
        protected void OnSystemIntChanged(int nIndex, int nData)
        {
            switch (nIndex)
            {
                //case (int)SysIntReg.Int_Process_Step:
                //    // ProcessStep(nData);
                //    CrossDelegate da = new CrossDelegate(ProcessStep);
                //    this.BeginInvoke(da, nData); // 异步调用委托,调用后立即返回并立即执行下面的语句
                //    break;
            }
        }

        public delegate void CrossDelegateDouble(int nIndex);
        void ProcessDoubleChange(int nIndex)
        {

        }

        /// <summary>
        /// 系统浮点型寄存器变化委托响应函数
        /// </summary>
        /// <param name="nIndex">寄存器索引</param>
        /// <param name="fData">寄存器值</param>
        protected void OnSystemDoubleChanged(int nIndex, double fData)
        {
            CrossDelegateDouble dl = new CrossDelegateDouble(ProcessDoubleChange);
            this.BeginInvoke(dl, nIndex);
        }

        /// <summary>
        /// 清除界面数据记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clean_Click(object sender, EventArgs e)
        {
            StationMgrEx.GetInstance().ClearAllLog();
            m_tsBestCT = TimeSpan.Zero;
        }

        /// <summary>
        /// 清除界面最后一条报警信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_warning_clean_Click(object sender, EventArgs e)
        {
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                WarningMgr.GetInstance().ClearWarning(WarningMgr.GetInstance().Count - 1);
            }
            else
            {
                label_warning.Text = "";
            }
        }
        TcpLink m_hiveplc;
        public static bool b_send = true;

        /// <summary>
        /// 计时,定时1000ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (StationMgr.GetInstance().IsAutoRunning()&&!StationMgr.GetInstance().IsPause()&& !SystemMgr.GetInstance().GetRegBit((int)SysBitReg.bit_PLC蜂鸣响))
            {
                try
                {
                    if (m_hiveplc == null)
                    {
                        m_hiveplc = TcpMgr.GetInstance().GetTcpLink(2);
                        m_hiveplc.Open();
                    }

                    if (b_send)
                    {
                        m_hiveplc.WriteLine("WRS DM1150.L 1 1");
                        labelhive.ImageIndex = 1;
                        b_send = false;
                    }
                    else
                    {
                        m_hiveplc.WriteLine("WRS DM1150.L 1 0");
                        labelhive.ImageIndex = 0;
                        b_send = true;
                    }
                }
                catch
                {
                    SystemMgr.GetInstance().ShowLog("与plc连接中断",LogLevel.Error);
                    m_hiveplc.Close();
                    Thread.Sleep(2000);
                    m_hiveplc = TcpMgr.GetInstance().GetTcpLink(2);
                    m_hiveplc.Open();
                }
                   
            }

            string strTimeFormat = "{0:00} {1:00}:{2:00}:{3:00}";

            m_tsSoftware += new TimeSpan(0, 0, 1);
            label_time_soft_total.Text = string.Format(strTimeFormat, m_tsSoftware.Days,
                                    m_tsSoftware.Hours, m_tsSoftware.Minutes, m_tsSoftware.Seconds);
            if (StationMgr.GetInstance().IsAutoRunning())
            {
                m_tsMachine += new TimeSpan(0, 0, 1);
                label_time_machine_total.Text = string.Format(strTimeFormat, m_tsMachine.Days,
                                    m_tsMachine.Hours, m_tsMachine.Minutes, m_tsMachine.Seconds);


            }
            if (WarningMgr.GetInstance().HasErrorMsg())
            {
                if (label_warning.BackColor == Color.Red)
                {
                    label_warning.BackColor = Color.FromKnownColor(KnownColor.Control);
                }
                else
                {
                    label_warning.BackColor = Color.Red;
                }
            }
            else if (IoMgr.GetInstance().IsSafeDoorOpen() && !SystemMgr.GetInstance().GetParamBool("SafetyDoor") && SystemMgr.GetInstance().GetParamBool("AutoSafeDoorMointor"))
            {
                string strMsg = "安全门被打开，存在安全隐患";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    strMsg = "The door has been opened, and there is potential safety hazard.";
                }

                label_warning.Text = strMsg;
            }
            else
            {
                label_warning.Text = "";
            }


            if (SystemMgr.GetInstance().GetRegBit((int)SysBitReg.bit_Hive连接失败))
            {
                this.roundButton_HiveNow.Text = "Hive断连";

                if (this.roundButton_HiveNow.BaseColor != Color.Red)
                {
                    this.roundButton_HiveNow.BaseColor = Color.Red;
                }
                else
                {
                    this.roundButton_HiveNow.BaseColor = Color.Yellow;
                }

            }
            else
            {
                this.roundButton_HiveNow.Text = "Hive正常";
                if (this.roundButton_HiveNow.BaseColor != Color.FromArgb(174, 218, 151))
                {
                    this.roundButton_HiveNow.BaseColor = Color.FromArgb(174, 218, 151);
                }

            }
            switch (HiveMgr.Hive_MachineState)
            {
                case 1://(int)MachineState.Running
                    button_HIVE.BackColor = Color.Green;
                    break;
                case 2://(int)MachineState.Idle:
                    button_HIVE.BackColor = Color.Yellow;
                    break;
                case 3://(int)MachineState.PlannedDown:
                    button_HIVE.BackColor = Color.Pink;
                    break;
                case 4://(int)MachineState.Engineering:
                    button_HIVE.BackColor = Color.Orange;
                    break;
                case 5://(int)MachineState.UnplannedDown:
                    button_HIVE.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
            label_sta_Hive.ImageIndex = SystemMgr.GetInstance().GetParamBool("EnableHive") == true ? 1 : 0;
            label_PDCA.ImageIndex = SystemMgr.GetInstance().GetParamBool("PDCAEnable") == true ? 1 : 0;

        }

        /// <summary>
        /// 双击报警框,打开界面报警信息 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_warning_DoubleClick(object sender, EventArgs e)
        {
            if (label_warning.Text != string.Empty)
            {
                Form_Warning fw = new Form_Warning();
                fw.ShowDialog(this);
            }
        }

        /// <summary>
        /// 自动循环模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundButton_auto_Click(object sender, EventArgs e)
        {
            StationMgr.GetInstance().BAutoMode = true;
            roundButton_auto.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            roundButton_manual.BaseColor = Color.FromArgb(220, 221, 224);
        }

        /// <summary>
        /// 切换单步作业模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundButton_manual_Click(object sender, EventArgs e)
        {
            //StationMgr.GetInstance().BAutoMode = false;
            //roundButton_auto.BaseColor = Color.FromArgb(220, 221, 224);
            //roundButton_manual.BaseColor = Color.FromArgb(0xb3, 0xca, 0xff);
            Form_Hive frm = new Form_Hive();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this.Parent.Parent);
        }


        /// <summary>
        /// 在列表框中显示字符串
        /// </summary>
        /// <param name="strLog"></param>
        /// <param name="level"></param>
        public void OnLogView(Control ctrl, string strLog, LogLevel level = LogLevel.Info)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    ListBoxEx logListBox = ctrl as ListBoxEx;

                    if (logListBox.Items.Count > 2000)
                    {
                        logListBox.Clear();
                    }

                    Color color = logListBox.BackColor;

                    switch (level)
                    {
                        case LogLevel.Info:
                            color = logListBox.BackColor;
                            break;

                        case LogLevel.Warn:
                            color = Color.Yellow;
                            break;

                        case LogLevel.Error:
                            color = Color.Red;
                            break;

                    }

                    logListBox.Append(strLog, color, logListBox.ForeColor);

                    logListBox.TopIndex = logListBox.Items.Count - (int)(logListBox.Height / logListBox.ItemHeight);

                    if (SystemMgr.GetInstance().GetParamBool("SaveShowLogEnable"))
                    {
                        WarningMgr.GetInstance().Info(strLog);
                    }
                }
                catch (Exception ex)
                {
                    WarningMgr.GetInstance().Error(ex.Message);

                    MessageBox.Show(ex.ToString());
                }

            });
        }


        private void Form_Auto_FormClosed(object sender, FormClosedEventArgs e)
        {
            Robot_ABB.Robot_Stop((int)controller_name.Load);
            ProductMgr.GetInstance().SoftwareTime += (int)m_tsSoftware.TotalSeconds;
            ProductMgr.GetInstance().MachineTime += (int)m_tsMachine.TotalSeconds;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            WarningMgr.GetInstance().Error("板卡故障测试");
            WarningMgr.GetInstance().Error(ErrorType.Err_Vision, "textObject", "板卡故障测试");
            WarningMgr.GetInstance().Error("TE12345678", "Err_Test", "textObject", "板卡故障测试");
        }
    }
}
