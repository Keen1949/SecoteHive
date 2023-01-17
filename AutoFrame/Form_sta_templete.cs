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

namespace AutoFrame
{
    public partial class Form_sta_templete : Form
    {
        private Label[,]label_state = new Label[4, 9];    //轴标签，轴的各个状态,报警、正限位、负限位、原点、急停、零位、到位、励磁
        private TextBox[] textBox_pos = new TextBox[4];   //各轴的当前位置
        private TextBox[] textBox_tar = new TextBox[4];   //各轴的移动距离
        private TextBox[] textBox_speed = new TextBox[4]; //各轴移动轴速度
        private Button[] btns_in;   //输入IO按钮数组
        private Button[] btns_out;  //输出IO按钮数组

        public Form_sta_templete()
        {
            InitializeComponent();
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

        //界面初始化
        private void Form_sta_templete_Load(object sender, EventArgs e)
        {
             StationBase sta = ManaulTool.GetStation(this);

            if (sta != null)
            {
                string strFile = System.Environment.CurrentDirectory + "\\res\\";
                strFile += sta.Name + ".png";

                FileInfo fi = new FileInfo(strFile);
                if (fi.Exists)
           //     Image img = Image.FromFile(strFile);
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
                BindIoButton();  //绑定IO按钮
                HideUnuseControl(sta); //隐藏工站不用的控件

                ManaulTool.UpdateGrid(ManaulTool.GetStation(this), dataGridView_point);  //更新内存点位参数到工站表格
                ManaulTool.updateIoText(btns_in, sta.io_in);   //显示输入IO名字
                ManaulTool.updateIoText(btns_out, sta.io_out, false);  //显示输出IO名字
                if(sta.io_in.Length > 6 && sta.io_in.Length < sta.io_out.Length)  //通过IO输入点和输出点的个数比较来分配空间
                {
                    int n = roundPanel_out.Width;
                 //   roundPanel_out.Location.Offset(roundPanel_out.Width / 2, 0);
                    roundPanel_out.Width = roundPanel_in.Width;
                    roundPanel_in.Width = n;
                    roundPanel_out.Left = roundPanel_in.Right + 1;

                     n = label_out.Width;
                   
                    label_out.Width = label_in.Width;
                    label_in.Width = n;
                    label_out.Left = label_in.Right + 1;

                }
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
                    roundPanel_out.Enabled = true;
                    roundPanel_point.Enabled = true;

                    button_save.Enabled = true;

                    break;

                case UserMode.Adjustor:
                    roundPanel_move.Enabled = true;
                    roundPanel_param.Enabled = true;
                    roundPanel_out.Enabled = true;
                    roundPanel_point.Enabled = true;

                    button_save.Enabled = false;

                    break;

                case UserMode.FAE:
                    roundPanel_move.Enabled = true;
                    roundPanel_param.Enabled = true;
                    roundPanel_out.Enabled = true;
                    roundPanel_point.Enabled = false;
                    break;

                default:
                    roundPanel_move.Enabled = false;
                    roundPanel_param.Enabled = false;
                    roundPanel_out.Enabled = false;
                    roundPanel_point.Enabled = false;
                    break;
            }

        }


        /// <summary>
        /// 创建IO输入输出按钮并声明按钮点击事件
        /// </summary>
        private void BindIoButton()
        {
            int nIndex = 0;
            btns_in = new Button[roundPanel_in.Controls.Count];

            foreach (Control ctn in roundPanel_in.Controls)
            {
                btns_in[nIndex++] = (Button)ctn;
            }
            Array.Sort(btns_in, new ManaulTool.ControlSort());    //根据界面输入按钮坐标排序

            btns_out = new Button[roundPanel_out.Controls.Count];
            nIndex = 0;
            foreach (Control ctn in roundPanel_out.Controls)
            {
                btns_out[nIndex] = (Button)ctn;
                btns_out[nIndex].Click += ManaulTool.Form_IO_Out_Click;  //定义输出按钮点击事件
                nIndex++;
            }
            Array.Sort(btns_out, new ManaulTool.ControlSort()); //根据界面输出按钮坐标排序
        }

        /// <summary>
        /// 隐藏工站不用的控件
        /// </summary>
        /// <param name="sta">工站对象</param>
        private void HideUnuseControl(StationBase sta)
        {
             for (int i = 0; i < 4; ++i)
            {
                if (sta.GetAxisNo(i) == 0)
                {
                    for (int m = 0; m < 9; ++m)
                    {
                        tableLayoutPanel_state.GetControlFromPosition(m, i).Visible = false;//隐藏相应轴的标签以及轴状态控件
                    }
                    for (int m = 0; m < 7; ++m)
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
            }

        }

        private void radioButton_abs_CheckedChanged(object sender, EventArgs e)
        {
            label_target.Text = "目标位置";
        }
        private void radioButton_rel_CheckedChanged(object sender, EventArgs e)
        {
            label_target.Text = "移动距离";
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
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1;
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
                Button btn = (Button)sender;
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1;
                int nAxisNo = sta.GetAxisNo(nIndex);
                if (nAxisNo > 0)
                {
                    if (radioButton_pos.Checked)
                        MotionMgr.GetInstance().Home(nAxisNo, 0);
                    else
                    {
                        MotionMgr.GetInstance().Home(nAxisNo, 1);
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
                int nIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1)) - 1;
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
                int n = sta.m_nAxisArray.Length;
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
            ManaulTool.singleMove(dataGridView_point, ManaulTool.GetStation(this), textBox_speed);
        }

        /// <summary>
        /// 选中点位全轴移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_all_Click(object sender, EventArgs e)
        {
            ManaulTool.allMove(dataGridView_point, ManaulTool.GetStation(this), textBox_speed);
        }

        /// <summary>
        /// 保存当前工站所有点位到文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            ManaulTool.SavePoint(dataGridView_point, ManaulTool.GetStation(this));
        }

        /// <summary>
        /// 更新选中的表格单元轴位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_axis_Click(object sender, EventArgs e)
        {
            ManaulTool.updateAxisPos(dataGridView_point, ManaulTool.GetStation(this));
        }

        /// <summary>
        /// 更新选中的表格点位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_point_Click(object sender, EventArgs e)
        {
            ManaulTool.updatePointPos(dataGridView_point, ManaulTool.GetStation(this));
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
                int nAxisNo = sta.GetAxisNo(nIndex);
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
                ManaulTool.UpdateMotionState(this, textBox_pos, label_state);
                ManaulTool.updateIoState(btns_in, sta.io_in, true);
                ManaulTool.updateIoState(btns_out, sta.io_out, false);
            }
        //    System.Diagnostics.Debug.WriteLine("Time tick \r\n");
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string str1 = "当前站位已经被禁用，是否开启站位";
            string str2 = "提示";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "The current station has been disabled. Do you want to enable it?";
                str2 = "Tips";
            }

            StationBase sta = ManaulTool.GetStation(this);
            if(sta.StationEnable == false)
            {
                if (MessageBox.Show(str1, str2, MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel)
                    return;
                else
                    sta.StationEnable = true;
            }
            StationMgr.GetInstance().TestRun(sta);
        }

    
    }
}
