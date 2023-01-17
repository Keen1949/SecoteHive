//2019-06-12 Binggoo 1. 手动回原点加入安全判断
//2019-06-20 Binggoo 1. 轴状态和轴运动的列数通过tablelayout的列数获取，防止添加列数忘记改数字而出问题。
//2019-07-08 Binggoo 1. 保存和更新点位时加入提示。
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
using ToolEx;
using System.Text.RegularExpressions;
using MotionIO;

namespace AutoFrame
{
    public partial class Form_sta_templete_ex : Form
    {
        private Label[,]label_state = new Label[4, 9];    //轴标签，轴的各个状态,报警、正限位、负限位、原点、急停、零位、到位、励磁
        private TextBox[] textBox_pos = new TextBox[4];   //各轴的当前位置
        private TextBox[] textBox_tar = new TextBox[4];   //各轴的移动距离
        private TextBox[] textBox_speed = new TextBox[4]; //各轴移动轴速度
        private Button[] btns_in;   //输入IO按钮数组
        private Button[] btns_out;  //输出IO按钮数组

        private int m_nCurPage = -1; //0 - XYZU  1 - ABCD
        private int m_nAxisStateHeight = 0; //显示轴状态控件的高度
        private int m_nAxisParamHeight = 0; //显示轴参数控件的高度

        public Form_sta_templete_ex()
        {
            InitializeComponent();

            InitialAnchor();
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);

                StationBase sta = ManaulTool.GetStation(this);
                if (sta != null)
                {
                    ManaulTool.updateIoText(btns_in, sta.io_in);   //显示输入IO名字
                    ManaulTool.updateIoText(btns_out, sta.io_out, false);  //显示输出IO名字
                }
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }


        private void InitialAnchor()
        {
            tabControl_manual.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            roundPanel_move.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            roundPanel_state.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            roundPanel_param.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            tabControl1.Anchor =  AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            panel_io_in.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            panel_io_out.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            panel_cylinder.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        }


        //界面初始化
        private void Form_sta_templete_Load(object sender, EventArgs e)
        {
            m_nAxisParamHeight = tableLayoutPanel_param.Height;
            m_nAxisStateHeight = tableLayoutPanel_state.Height;

            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                string strFile = System.Environment.CurrentDirectory + "\\res\\";
                strFile += sta.Name + ".png";

                FileInfo fi = new FileInfo(strFile);
                if (fi.Exists)
                    pictureBox1.Image = Image.FromFile(strFile);   //加载工站图片
                string[] strAxisName = { "x", "y", "z", "u" };
                for (int i = 0; i < label_state.GetLength(0); i++)     //一维遍历
                {
                    for (int j = 0; j < label_state.GetLength(1); j++)  //二维遍历
                    {
                        label_state[i, j] = (Label)tableLayoutPanel_state.Controls.Find("label_state_"
                                                    + strAxisName[i] + (j).ToString(), true).First();
                    }
                    label_state[i, 0].Text = sta.strAxisName[i];
                    tableLayoutPanel_param.Controls.Find("label_param_" + (i + 1).ToString(), true).First().Text = sta.strAxisName[i];

                    textBox_pos[i] = (TextBox)tableLayoutPanel_param.Controls.Find("textBox_pos_"
                                                    + (i + 1).ToString(), true).First();  //面板中搜索控件的属性(字符串)
                    textBox_tar[i] = (TextBox)tableLayoutPanel_param.Controls.Find("textBox_tar_"
                                                    + (i + 1).ToString(), true).First();
                    textBox_speed[i] = (TextBox)tableLayoutPanel_param.Controls.Find("textBox_speed_"
                                                    + (i + 1).ToString(), true).First();

                }

                SwitchToXYZU();

                BindIoButton();  //绑定IO按钮
                
                //隐藏点位中不需要的轴，并把名称改为自定义名称
                for(int i = 0; i < sta.AxisCount;i++)
                {
                    int nAxisNo = sta.GetAxisNo(i);

                    if (nAxisNo > 0)
                    {
                        dataGridView_point.Columns[i + 2].HeaderText = sta.GetAxisName(i);
                    }
                    else
                    {
                        dataGridView_point.Columns[i + 2].Visible = false;
                    }
                }

                ManaulTool.UpdateGrid(ManaulTool.GetStation(this), dataGridView_point);  //更新内存点位参数到工站表格
                ManaulTool.updateIoText(btns_in, sta.io_in);   //显示输入IO名字
                ManaulTool.updateIoText(btns_out, sta.io_out, false);  //显示输出IO名字

            }

            //监视TAB控件的页面切换和主界面的页面切换事件来确定是否需要扫描轴的状态
            this.Parent.VisibleChanged += new System.EventHandler(this.Form_sta_templete_VisibleChanged);
            this.Parent.Parent.Parent.VisibleChanged += new System.EventHandler(this.Form_sta_templete_VisibleChanged);

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

            Form frm = sta.m_Form_Manual;
            if (frm != null)
            {

                TabPage tabPage = new TabPage(); //创建一个TabControl 中的单个选项卡页。
                tabPage.Text = "手动操作";
                tabPage.Name = frm.Text;
                tabPage.BackColor = frm.BackColor;

                tabControl_manual.TabPages.Add(tabPage);   //添加tabPage选项卡到tab控件

                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.Parent = tabPage;
                frm.Visible = true;


                tabPage.Controls.Add(frm);  //tabPage选项卡添加一个窗体对象 
               
            }

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
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
                    roundPanel_move.Enabled = true;
                    roundPanel_param.Enabled = true;
                    panel_io_out.Enabled = true;
                    roundPanel_point.Enabled = true;

                    panel_cylinder.Enabled = true;

                    button_save.Enabled = true;

                    break;

                case UserMode.Adjustor:
                    roundPanel_move.Enabled = true;
                    roundPanel_param.Enabled = true;
                    panel_io_out.Enabled = true;
                    roundPanel_point.Enabled = true;
                    panel_cylinder.Enabled = true;

                    button_save.Enabled = true;

                    break;

                case UserMode.FAE:
                    roundPanel_move.Enabled = true;
                    roundPanel_param.Enabled = true;
                    panel_io_out.Enabled = true;
                    roundPanel_point.Enabled = false;

                    panel_cylinder.Enabled = true;
                    break;

                default:
                    roundPanel_move.Enabled = false;
                    roundPanel_param.Enabled = false;
                    panel_io_out.Enabled = false;
                    roundPanel_point.Enabled = false;

                    panel_cylinder.Enabled = false;
                    break;
            }
        }
    

        /// <summary>
        /// 创建IO输入输出按钮并声明按钮点击事件
        /// </summary>
        private void BindIoButton()
        {
            StationBase sta = ManaulTool.GetStation(this);

            //输入
            panel_io_in.Controls.Clear();
            btns_in = new Button[sta.io_in.Length];

            int btnWidth = 250;
            int btnHeight = 36;

            int cols = panel_io_in.Width / btnWidth;
            if (cols < 1)
            {
                cols = 1;
            }
            //动态创建按钮
            for (int i = 0; i < sta.io_in.Length;i++)
            {
                Button btn = new Button();

                btn.Name = "button_"+sta.io_in[i];
                btn.Text = sta.io_in[i];
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.Left = 1 + (i%cols) * btnWidth;
                btn.Top = 1 + (i / cols) * btnHeight;
                btn.Height = btnHeight;
                btn.Width = btnWidth;          

                panel_io_in.Controls.Add(btn);

                btns_in[i] = btn;
            }

            //输出
            panel_io_out.Controls.Clear();
            btns_out = new Button[sta.io_out.Length];

            cols = panel_io_out.Width / btnWidth;
            if (cols < 1)
            {
                cols = 1;
            }
            //动态创建按钮
            for (int i = 0; i < sta.io_out.Length; i++)
            {
                Button btn = new Button();

                btn.Name = "button_" + sta.io_out[i];
                btn.Text = sta.io_out[i];
                btn.ImageList = imageList1;
                btn.ImageIndex = 0;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += ManaulTool.Form_IO_Out_Click;

                btn.Left = 1 + (i % cols) * btnWidth;
                btn.Top = 1 + (i / cols) * btnHeight;
                btn.Height = btnHeight;
                btn.Width = btnWidth;

                panel_io_out.Controls.Add(btn);

                btns_out[i] = btn;
            }

            //气缸
            Type type = sta.GetType();
            if (type.BaseType != null)
            {
                if (type.BaseType.Name == typeof(StationEx).Name)
                {
                    StationEx staex = sta as StationEx;

                    panel_cylinder.Controls.Clear();

                    for (int i = 0; i < staex.m_cylinders.Length;i++)
                    {
                        CylinderCtrl cylCtrl = new CylinderCtrl();

                        cylCtrl.Name = "cylinder_" + staex.m_cylinders[i];

                        cols = panel_cylinder.Width / cylCtrl.Width;

                        if (cols < 1)
                        {
                            cols = 1;
                            cylCtrl.Width = panel_cylinder.Width - 50;
                        }

                        
                        cylCtrl.CylinderObject = CylinderMgr.GetInstance().GetCyLinder(staex.m_cylinders[i]);                     

                        cylCtrl.IsCylinderSafeEvent += ManaulTool.IsCylinderSafe;

                        cylCtrl.Left = 1 + (i % cols) * cylCtrl.Width;
                        cylCtrl.Top = 1 + (i / cols) * cylCtrl.Height;

                        panel_cylinder.Controls.Add(cylCtrl);
                    }
                }
            }  

        }

        /// <summary>
        /// 隐藏工站不用的控件
        /// </summary>
        /// <param name="sta">工站对象</param>
        private void HideUnuseControl(StationBase sta)
        {
            tableLayoutPanel_state.Height = m_nAxisStateHeight;
            tableLayoutPanel_param.Height = m_nAxisParamHeight;

             for (int i = 0; i < 4; ++i)
            {
                int nStartIndex = m_nCurPage * 4 + i;
                if (sta.GetAxisNo(nStartIndex) == 0)
                {
                    for (int m = 0; m < tableLayoutPanel_state.ColumnCount; ++m)
                    {
                        tableLayoutPanel_state.GetControlFromPosition(m, i).Visible = false;//隐藏相应轴的标签以及轴状态控件
                    }

                    for (int m = 0; m < tableLayoutPanel_param.ColumnCount; ++m)
                    {
                        tableLayoutPanel_param.GetControlFromPosition(m, i).Visible = false;//隐藏相应轴的标签以及轴参数等控件
                    }

                    tableLayoutPanel_state.Height -= (int)tableLayoutPanel_state.RowStyles[i].Height;
                    tableLayoutPanel_state.RowStyles[i].Height = 0;

                    tableLayoutPanel_param.Height -= (int)tableLayoutPanel_param.RowStyles[i].Height;
                    tableLayoutPanel_param.RowStyles[i].Height = 0;

                    tableLayoutPanel_move.Controls.Find("button_positive_" + (i + 1).ToString(), true).First().Visible = false;
                    tableLayoutPanel_move.Controls.Find("button_negtive_" + (i + 1).ToString(), true).First().Visible = false;
          //          tableLayoutPanel_move.GetControlFromPosition(position[i,0,0], position[i,0,1]).Visible = false;
          //            tableLayoutPanel_move.GetControlFromPosition(position[i,1,0], position[i,1,1]).Visible = false;
                }
                else
                {
                    for (int m = 0; m < tableLayoutPanel_state.ColumnCount; ++m)
                    {
                        tableLayoutPanel_state.GetControlFromPosition(m, i).Visible = true;//隐藏相应轴的标签以及轴状态控件
                    }

                    for (int m = 0; m < tableLayoutPanel_param.ColumnCount; ++m)
                    {
                        tableLayoutPanel_param.GetControlFromPosition(m, i).Visible = true;//隐藏相应轴的标签以及轴参数等控件
                    }

                    tableLayoutPanel_state.RowStyles[i].Height = m_nAxisStateHeight / 4;

                   
                    tableLayoutPanel_param.RowStyles[i].Height = m_nAxisParamHeight / 4;

                    tableLayoutPanel_move.Controls.Find("button_positive_" + (i + 1).ToString(), true).First().Visible = true;
                    tableLayoutPanel_move.Controls.Find("button_negtive_" + (i + 1).ToString(), true).First().Visible = true;
                }
            }

        }

        private void radioButton_abs_CheckedChanged(object sender, EventArgs e)
        {
            //label_target.Text = "目标位置";
            label_target.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "目标位置", "目标位置");
        }
        private void radioButton_rel_CheckedChanged(object sender, EventArgs e)
        {
            //label_target.Text = "移动距离";
            label_target.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "移动距离", "移动距离");
        }

        /// <summary>
        /// 轴上伺服
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_svo_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if(sta != null)
            {
                Button btn = (Button)sender;
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1 + m_nCurPage * 4;

                int nAxisNo = sta.GetAxisNo(nIndex);
                if (nAxisNo > 0)
                {
                    if (MotionMgr.GetInstance().GetServoState(nAxisNo))
                    {
                        MotionMgr.GetInstance().ServoOff(nAxisNo);
                        btn.Text = "svo on";
                    }
                    else
					{
                        MotionMgr.GetInstance().ServoOn(nAxisNo);
                        btn.Text = "svo off";
				    }
                }
            }
       }

        /// <summary>
        /// 轴回原点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_org_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                string str1 = "危险操作";
                string str2 = "回原点模式未配置";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Dangerous operation";
                    str2 = "Return to home mode not configured";
                }

                Button btn = (Button)sender;
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1 + m_nCurPage * 4; 
                int nAxisNo = sta.GetAxisNo(nIndex);
                if (nAxisNo > 0)
                {
                    if (!ManulSafeMgr.GetInstance().IsManulMotionSafe(nAxisNo))
                    {
                        MessageBox.Show(str1);

                        return;
                    }

                    AxisCfg cfg;
                    if (MotionMgr.GetInstance().GetAxisCfg(nAxisNo, out cfg))
                    {
                        //MotionMgr.GetInstance().Home(nAxisNo, cfg.nHomeMode);
                        MotionMgr.GetInstance().Home(nAxisNo);
                    }
                    else
                    {
                        MessageBox.Show(str2);
                    }
                }
            }
        }

        /// <summary>
        /// 轴位置清零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_zero_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                Button btn = (Button)sender;
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1 + m_nCurPage * 4;
                int nAxisNo = sta.GetAxisNo(nIndex);
                if (nAxisNo > 0)
                {
                    MotionMgr.GetInstance().SetPosZero(nAxisNo);
                }
            }
         }

        /// <summary>
        /// 相应工站上的所有轴紧急停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stop_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                int n = sta.AxisCount;
                for (int i = 0; i < n; ++i)
                {
                    int nAxisNo = sta.GetAxisNo(i);
                    if (nAxisNo > 0)
                       MotionMgr.GetInstance().StopEmg(nAxisNo);
                }

                sta.StopManualRun();
            }
        }

        /// <summary>
        /// 选中点位单轴移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_single_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message5", "是否单轴移动到此点?");
            if (DialogResult.Yes == MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                ManaulTool.singleMove(dataGridView_point, ManaulTool.GetStation(this), textBox_speed);
            } 
        }

        /// <summary>
        /// 选中点位全轴移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_all_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message6", "是否全轴移动到此点?");
            if (DialogResult.Yes == MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                ManaulTool.allMove(dataGridView_point, ManaulTool.GetStation(this), textBox_speed);
            }
        }

        /// <summary>
        /// 保存当前工站所有点位到文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message2", "是否保存所有点位?");
            if (DialogResult.Yes == MessageBox.Show(strMsg,"Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning))
            {
                ManaulTool.SavePoint(dataGridView_point, ManaulTool.GetStation(this));
            }
        }

        /// <summary>
        /// 更新选中的表格单元轴位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_axis_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message3", "是否更新此轴点位?");
            if (DialogResult.Yes == MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                ManaulTool.updateAxisPos(dataGridView_point, ManaulTool.GetStation(this));
            } 
        }

        /// <summary>
        /// 更新选中的表格点位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_point_Click(object sender, EventArgs e)
        {
            string strMsg = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message4", "是否更新此点点位?");
            if (DialogResult.Yes == MessageBox.Show(strMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                ManaulTool.updatePointPos(dataGridView_point, ManaulTool.GetStation(this));
            }
            
        }

        /// <summary>
        /// 轴移动,可选择绝对、相对、Jog运动模式
        /// </summary>
        /// <param name="nIndex">轴号</param>
        /// <param name="bPositive">方向 true正  false负</param>
        private void AxisMove(int nIndex, bool bPositive )
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                int nStartIndex = nIndex + m_nCurPage * 4;
                int nAxisNo = sta.GetAxisNo(nStartIndex);
                if (nAxisNo > 0)
                {
                    if (radioButton_abs.Checked)
                    {
                        ManaulTool.absMove(nAxisNo, textBox_tar[nIndex], textBox_speed[nIndex], sta.bPositiveMove[nIndex] ? bPositive : !bPositive);
                    }
                    if (radioButton_rel.Checked)
                    {
                        ManaulTool.relMove(nAxisNo, textBox_tar[nIndex], textBox_speed[nIndex], sta.bPositiveMove[nIndex]? bPositive : !bPositive);
                    }
                    if (radioButton_jog.Checked)
                    {
                        ManaulTool.jogMove(nAxisNo, textBox_tar[nIndex], textBox_speed[nIndex], sta.bPositiveMove[nIndex] ? bPositive : !bPositive);
                    }
                }
            }        
        }

        /// <summary>
        /// 轴负向移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_negtive_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1;
            AxisMove(nIndex, false);
        }

        /// <summary>
        /// 轴正向移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_postive_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1;
            AxisMove(nIndex, true);
        }

        /// <summary>
        /// TAB控件的页面切换和主界面的页面切换事件来确定开启或关闭定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_sta_templete_VisibleChanged(object sender, EventArgs e)
        {
            // if (this.Parent.Visible == true)
            if (this.Visible == true)//              todo
            {
                timer1.Enabled = true;
                ManaulTool.UpdateGrid(ManaulTool.GetStation(this), dataGridView_point);  //更新内存点位参数到工站表格
            }
            else
            {
                timer1.Enabled = false;
            }
        }

        /// <summary>
        /// 轴状态、IO输入输出扫描定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                ManaulTool.UpdateMotionState(this, textBox_pos, label_state,m_nCurPage);
                ManaulTool.updateIoState(btns_in, sta.io_in, true);
                ManaulTool.updateIoState(btns_out, sta.io_out, false);
            }
        //    System.Diagnostics.Debug.WriteLine("Time tick \r\n");
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if(sta.StationEnable == false)
            {
                string strMessage = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Message1", "当前站位已经被禁用，是否开启站位");
                string strTips = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "提示", "提示");
                if (MessageBox.Show(strMessage, strTips, MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel)
                    return;
                else
                    sta.StationEnable = true;
            }
            StationMgr.GetInstance().TestRun(sta);
        }

        private void button_clear_error_Click(object sender, EventArgs e)
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta != null)
            {
                Button btn = (Button)sender;
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1 + m_nCurPage * 4;
                int nAxisNo = sta.GetAxisNo(nIndex);
                if (nAxisNo > 0)
                {
                    MotionMgr.GetInstance().ClearError(nAxisNo);
                }
            }
        }

        private void button_LastPage_Click(object sender, EventArgs e)
        {
            if (m_nCurPage != 0)
            {
                SwitchToXYZU();
            }
        }

        private void button_NextPage_Click(object sender, EventArgs e)
        {
            if (m_nCurPage != 1)
            {
                SwitchToABCD();
            }

            
        }

        private void SwitchToXYZU()
        {
            if (m_nCurPage != 0)
            {
                m_nCurPage = 0;
                button_LastPage.BackColor = Color.Green;
                button_NextPage.BackColor = Color.LightGray;

                UpdateUI();
            }
        }

        private void SwitchToABCD()
        {
            if (m_nCurPage != 1)
            {
                m_nCurPage = 1;

                button_NextPage.BackColor = Color.Green;
                button_LastPage.BackColor = Color.LightGray;

                UpdateUI();
            }

            
        }

        private void UpdateUI()
        {
            StationBase sta = ManaulTool.GetStation(this);
            if (sta == null)
            {
                return;
            }

            //调整方向按键的正负方向
            Button[] btnsPositive = { button_positive_1, button_positive_2, button_positive_3, button_positive_4 };
            Button[] btnsNegative = { button_negtive_1, button_negtive_2, button_negtive_3, button_negtive_4 };

            Label[] labelStateAxisName = { label_state_x0, label_state_y0, label_state_z0, label_state_u0 };
            Label[] labelParamAxisName = { label_param_1, label_param_2, label_param_3, label_param_4 };
            Button[] btnsServoOn = { button_svo_1, button_svo_2, button_svo_3, button_svo_4 };

            for (int i = 0; i < 4; i++)
            {
                int nStartIndex = m_nCurPage * 4 + i;

                labelStateAxisName[i].Text = sta.strAxisName[nStartIndex];
                labelParamAxisName[i].Text = sta.strAxisName[nStartIndex];

                int nAxisNo = sta.GetAxisNo(nStartIndex);
                if (nAxisNo > 0)
                {
                    if (MotionMgr.GetInstance().GetServoState(nAxisNo))
                    {
                        btnsServoOn[i].Text = "OFF";
                    }
                    else
                    {
                        btnsServoOn[i].Text = "ON";
                    }
                }

                if (sta.bPositiveMove[nStartIndex])
                {
                    btnsPositive[i].Text = sta.strAxisName[nStartIndex] + "+";
                    btnsNegative[i].Text = sta.strAxisName[nStartIndex] + "-";
                }
                else
                {
                    btnsPositive[i].Text = sta.strAxisName[nStartIndex] + "-";
                    btnsNegative[i].Text = sta.strAxisName[nStartIndex] + "+";
                }
            }




            HideUnuseControl(sta); //隐藏工站不用的控件
        }
    }


}
