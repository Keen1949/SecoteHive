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
using MotionIO;
using CommonTool;
using AutoFrameVision;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_IO : Form
    {
        //TODO: need config from xml  .....
        private string[] system_in = {"1.1", "1.2", "1.3", "1.4", "1.5" };  //系统常用IO输入定义数组
        private string[] system_out = { "1.1", "1.2", "1.3", "1.4", "1.5" }; //系统常用IO输出定义数组

        private Button[] btns_in;   //IO输入点指示
        private Button[] btns_out;     //IO输出指示按钮
        private Button[] btns_in_system ; //系统常用IO输入指示
        private Button[] btns_out_system ;   //系统常用IO输出指示

        private int m_nCardIn = 0;  //
        private int m_nCardOut = 0;
        private int m_nIndexIn = 0;
        private int m_nIndexOut = 0;
        private long nDataIn = 0;  //当前IO输入缓冲
        private long nDataOut = 0; //当前IO输出缓冲

        public Form_IO()
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

                UpdateIoInText(m_nCardIn, m_nIndexIn);
                UpdateIoOutText(m_nCardOut, m_nIndexOut);

                ManaulTool.updateIoText(btns_in_system, system_in);
                ManaulTool.updateIoText(btns_out_system, system_out, false);
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }


        private void Form_IO_Load(object sender, EventArgs e)
        {
            int nIndex = 0;
            btns_in = new Button[groupBox_in.Controls.Count]; //IO输入点创建

            foreach (Control ctn in groupBox_in.Controls)
            {
                btns_in[nIndex++] = (Button)ctn;

                //修改字体大小,当IO描述太长显示不下时，改小字体
                ctn.Font = new Font("宋体", 9);
            }
            Array.Sort(btns_in, new ManaulTool.ControlSort()); //根据界面输入按钮坐标排序

            btns_out = new Button[groupBox_out.Controls.Count]; //IO输出点创建
            nIndex = 0;
            foreach (Control ctn in groupBox_out.Controls)
            {
                btns_out[nIndex] = (Button)ctn;
                btns_out[nIndex].Click += ManaulTool.Form_IO_Out_Click; //定义输出按钮点击事件
                nIndex++;

                //修改字体大小,当IO描述太长显示不下时，改小字体
                ctn.Font = new Font("宋体", 9);
            }
            Array.Sort(btns_out, new ManaulTool.ControlSort()); //根据界面输出按钮坐标排序

            btns_in_system = new Button[groupBox_system_in.Controls.Count]; //系统IO输入点创建
            nIndex = 0;
            foreach (Control ctn in groupBox_system_in.Controls)
            {
                btns_in_system[nIndex] = (Button)ctn;
                nIndex++;
            }
            Array.Sort(btns_in_system, new ManaulTool.ControlSort()); //根据界面系统IO输入点钮坐标排序

            btns_out_system = new Button[groupBox_system_out.Controls.Count]; //系统IO输出点创建
            nIndex = 0;
            foreach (Control ctn in groupBox_system_out.Controls)
            {
                btns_out_system[nIndex] = (Button)ctn;
                nIndex++;
            }
            Array.Sort(btns_out_system, new ManaulTool.ControlSort());  //根据界面系统IO输出点按钮坐标排序

            UpdateIoInText(m_nCardIn, m_nIndexIn);
            UpdateIoOutText(m_nCardOut, m_nIndexOut);
            UpdateSystemIoText();
            ManaulTool.updateIoText(btns_in_system, system_in);
            ManaulTool.updateIoText(btns_out_system, system_out, false);

            //监视TAB控件的页面切换和主界面的页面切换事件来确定是否需要扫描IO
            this.Parent.VisibleChanged += new System.EventHandler(this.Form_IO_VisibleChanged);
            this.Parent.Parent.Parent.VisibleChanged += new System.EventHandler(this.Form_IO_VisibleChanged);
            //增加权限等级变更通知
            OnChangeMode();
            Security.ModeChangedEvent += OnChangeMode;

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }


        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnChangeMode()
        {
            if (Security.IsOpMode())
            {
                groupBox_out.Enabled = false;
                roundPanel_mode.Enabled = false;
            }
            else
            {
                groupBox_out.Enabled = true;
                roundPanel_mode.Enabled = true;
            }
        }
        /// <summary>
        /// 显示IO输入点按钮名字
        /// </summary>
        /// <param name="nCardNo">即将跳转到的卡号索引</param>
        /// <param name="nIndex">IO点索引</param>
        void UpdateIoInText(int nCardNo, int nIndex)
        {
            if (IoMgr.GetInstance().CountCard > nCardNo) //要跳转到的卡号小于总IO卡数
            {
                IoCtrl Card = IoMgr.GetInstance().m_listCard.ElementAt(nCardNo);  //取出当前IO卡号对象
                for ( int i = 0; i < btns_in.Length; ++i)
                {
                    if(i + nIndex  < Card.m_strArrayIn.Length)
                    {
                        string strIoName = Card.m_strArrayIn[nIndex + i];
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            strIoName = IoMgr.GetInstance().GetIoInTranslate(strIoName);
                        }
                        btns_in[i].Text = string.Format("{0}.{1,2} {2}", (nCardNo + 1), ( i + nIndex + 1),strIoName); 
                        btns_in[i].Visible = true;
                    }
                    else
                    {
                        btns_in[i].Visible = false;
                    }
                }

                if (Card.m_strCardName == "Delta")
                {
                    groupBox_in.Text = string.Format("{0} DI: Card ID:{1},Node ID:{2},Port ID:{3}",
                        Card.m_strCardName, ((IoCtrl_Delta)Card).addrDI.nCardID, ((IoCtrl_Delta)Card).addrDI.nNodeID,((IoCtrl_Delta)Card).addrDI.nPortID);
                }
                else
                {
                    groupBox_in.Text = string.Format("{0} DI:  Card ID:{1},Port ID:{2}", Card.m_strCardName, m_nCardIn + 1, Card.CountIoIn);
                }
            }
        }
        
        /// <summary>
        /// 显示IO输出点按钮名字
        /// </summary>
        /// <param name="nCardNo">卡号索引</param>
        /// <param name="nIndex">IO点索引</param>
        void UpdateIoOutText(int nCardNo, int nIndex)
        {
            if (IoMgr.GetInstance().CountCard > nCardNo)
            {
                IoCtrl Card = IoMgr.GetInstance().m_listCard.ElementAt(nCardNo);
                for (int i = 0; i < btns_out.Length; ++i)
                {
                    
                    if (i + nIndex < Card.m_strArrayOut.Length)
                    {
                        string strIoName = Card.m_strArrayOut[i + nIndex];
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            strIoName = IoMgr.GetInstance().GetIoOutTranslate(strIoName);
                        }
                        btns_out[i].Text = string.Format("{0}.{1,2} {2}", (nCardNo + 1), (i + nIndex + 1), strIoName);
                        btns_out[i].Visible = true;
                    }
                    else
                    {
                        btns_out[i].Visible = false;
                    }
                }

                if (Card.m_strCardName == "Delta")
                {
                    groupBox_out.Text = string.Format("{0} DO: Card ID:{1},Node ID:{2},Port ID:{3}",
                        Card.m_strCardName, ((IoCtrl_Delta)Card).addrDO.nCardID, ((IoCtrl_Delta)Card).addrDO.nNodeID, ((IoCtrl_Delta)Card).addrDO.nPortID);
                }
                else
                {
                    groupBox_out.Text = string.Format("{0} DO: Card ID:{1},Port ID:{2}", Card.m_strCardName, m_nCardOut + 1, Card.CountIoOut);
                }
            }
        }
        
        /// <summary>
        /// 更新系统常用IO按钮名字
        /// </summary>
        void UpdateSystemIoText()
        {
            int nCount = IoMgr.GetInstance().m_listIoSystemIn.Count;  //系统IO输入的个数 
            for (int i = 0; i < btns_in_system.Length; ++i)
            {
                if (i < nCount)
                {
                    //system_in[i] = string.Format("{0}.{1,2}", (IoMgr.GetInstance().m_listIoSystemIn.ElementAt(i).nCard),
                    //    (IoMgr.GetInstance().m_listIoSystemIn.ElementAt(i).nBit));
                    system_in[i] = IoMgr.GetInstance().m_listIoSystemIn[i].strName;
                }
                else
                {
                    system_in[i] = "";
                    btns_in_system[i].Visible = false;
                }
            }

            nCount = IoMgr.GetInstance().m_listIoSystemOut.Count;  //系统IO输出的个数
            for (int i = 0; i < btns_out_system.Length; ++i)
            {
                if (i < nCount)
                {
                    //system_out[i] = string.Format("{0}.{1,2}", (IoMgr.GetInstance().m_listIoSystemOut.ElementAt(i).nCard),
                    //    (IoMgr.GetInstance().m_listIoSystemOut.ElementAt(i).nBit));
                    system_out[i] = IoMgr.GetInstance().m_listIoSystemOut[i].strName;
                    btns_out_system[i].Click += ManaulTool.Form_IO_Out_Click; //定义系统IO输出按钮点击事件
                }
                else
                {
                    system_out[i] = "";
                    btns_out_system[i].Visible = false;
                }
            }
        }
        
        /// <summary>
        /// IO输入点显示上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_in_prev_Click(object sender, EventArgs e)
        {
            if(m_nCardIn >= 0)
            {
                int n = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardIn).CountIoIn;
                if (m_nIndexIn - btns_in.Length > n-1)
                {
                    m_nIndexIn -= btns_in.Length;
                }
                else
                {
                    if (m_nCardIn == 0)
                        return;
                     --m_nCardIn;
                    m_nIndexIn = 0;
                }
                UpdateIoInText(m_nCardIn, m_nIndexIn );
            }
        }
        
        /// <summary>
        /// IO输入点显示下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_in_next_Click(object sender, EventArgs e)
        {
            if(m_nCardIn < IoMgr.GetInstance().CountCard)
            {
                int  n = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardIn).CountIoIn;
                if(m_nIndexIn + btns_in.Length > n - 1)
                {
                    if (m_nCardIn == IoMgr.GetInstance().CountCard - 1)
                        return;
                    ++m_nCardIn;
                    m_nIndexIn = 0;
                }
                else
                {
                    m_nIndexIn += btns_in.Length;
                }
      
                UpdateIoInText(m_nCardIn,m_nIndexIn);
            }
        }
        
        /// <summary>
        /// IO输出点显示上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_out_prev_Click(object sender, EventArgs e)
        {
            if (m_nCardOut >= 0)
            {
                int n = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardOut).CountIoOut;
                if (m_nIndexOut - btns_out.Length > n - 1)
                {
                    m_nIndexOut -= btns_out.Length;
                }
                else
                {
                    if (m_nCardOut == 0)
                        return;
                    --m_nCardOut;
                    m_nIndexOut = 0;
                }

                UpdateIoOutText(m_nCardOut, m_nIndexOut);
            }
        }
        
        /// <summary>
        /// IO输出点显示下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_out_next_Click(object sender, EventArgs e)
        {
            if (m_nCardOut < IoMgr.GetInstance().CountCard)
            {
                int n = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardOut).CountIoOut;
                if (m_nIndexOut + btns_out.Length > n - 1)
                {
                    if (m_nCardOut == IoMgr.GetInstance().CountCard - 1)
                        return;
                    ++m_nCardOut;
                    m_nIndexOut = 0;
                }
                else
                {
                    m_nIndexOut += btns_out.Length;
                }
                UpdateIoOutText(m_nCardOut, m_nIndexOut);
            }
        }
        
        /// <summary>
        /// TAB控件的页面切换和主界面的页面切换事件来确定开启或关闭定时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">附带数据的对象</param>
        private void Form_IO_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Parent.Visible == true)
            {
                timer1.Enabled = true;
       //         System.Diagnostics.Debug.WriteLine("enable true");
            }
            else
            {
                timer1.Enabled = false;
      //          System.Diagnostics.Debug.WriteLine("enable false");
            }
        }

        /// <summary>
        /// IO扫描定时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">附带数据的对象</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
           // long nDataIn = 0;
           int nData = 0;
            IoCtrl ioctrl = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardIn);
            ioctrl.ReadIOIn(ref nData);
   
            if(nDataIn != nData)
            {
                nDataIn = nData;
                for (int i = m_nIndexIn; i < ioctrl.m_strArrayIn.Length; ++i)
                {
                    int n = nData & (1 << i);
                    if (n != 0)
                        n = 1;
                    if (btns_in[i].ImageIndex  != n)
                    {
                        btns_in[i].ImageIndex = n;   //通过IO返回的0或1选择显示IO图片
                    }
                }
            }
            ioctrl = IoMgr.GetInstance().m_listCard.ElementAt(m_nCardOut);
            ioctrl.ReadIOOut(ref nData);
            if (nDataOut != nData)
            {
                nDataOut = nData;
                for (int i = m_nIndexOut; i < ioctrl.m_strArrayOut.Length; ++i)
                {
                    int n = nData & (1 << i);
                    if (n != 0)
                        n = 1;
                    if (btns_out[i].ImageIndex != n)
                    {
                        btns_out[i].ImageIndex = n;//通过IO返回的0或1选择显示IO图片
                    }
                }
            }
            for (int i = 0; i < system_in.Length; ++i)
            {
                /*
                string[] str = system_in[i].Split('.');
                if (str.Length == 2)
                {
                    int ncard = Convert.ToInt32(str[0]);
                    int nIndex = Convert.ToInt32(str[1]);

                    int status = 0;
                    if (ncard == m_nCardIn + 1)//是否为当前卡已经读入缓存
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().GetIoInState(ncard, nIndex));
                    }
                    else
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().ReadIoInBit(ncard, nIndex));
                    }
                    if (btns_in_system[i].ImageIndex != (int)status)
                    {
                        if (status != 0)
                            status = 1;
                        btns_in_system[i].ImageIndex = status;   //通过IO返回的0或1选择显示IO图片
                    }
                }
                */
                string strIoName = system_in[i];
                int ncard,nIndex;
                long num;
                if (IoMgr.GetInstance().m_dicIn.TryGetValue(strIoName, out num))
                {
                    ncard = (int)(num >> 8);
                    nIndex = (int)(num & 0xff);

                    int status = 0;
                    if (ncard == m_nCardIn + 1)//是否为当前卡已经读入缓存
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().GetIoInState(ncard, nIndex));
                    }
                    else
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().ReadIoInBit(ncard, nIndex));
                    }
                    if (btns_in_system[i].ImageIndex != (int)status)
                    {
                        if (status != 0)
                            status = 1;
                        btns_in_system[i].ImageIndex = status;   //通过IO返回的0或1选择显示IO图片
                    }
                }
            }
            for (int i = 0; i < system_out.Length; ++i)
            {
                /*
                string[] str = system_out[i].Split('.');
                if (str.Length == 2)
                {
                    int ncard = Convert.ToInt32(str[0]) ;
                    int nIndex = Convert.ToInt32(str[1]) ;
                    int status = 0;
                    if (ncard == m_nCardOut + 1)//是否为当前卡已经读入缓存
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().GetIoOutState(ncard, nIndex));
                    }
                    else
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().ReadIoOutBit(ncard, nIndex));
                    }
                    if (btns_out_system[i].ImageIndex != (int)status)
                    {
                        if (status != 0)
                            status = 1;
                        btns_out_system[i].ImageIndex = status;  //通过IO返回的0或1选择显示IO图片
                    }
                }
                */
                string strIoName = system_out[i];
                int ncard, nIndex;
                long num;
                if (IoMgr.GetInstance().m_dicOut.TryGetValue(strIoName, out num))
                {
                    ncard = (int)(num >> 8);
                    nIndex = (int)(num & 0xff);

                    int status = 0;
                    if (ncard == m_nCardOut + 1)//是否为当前卡已经读入缓存
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().GetIoOutState(ncard, nIndex));
                    }
                    else
                    {
                        status = Convert.ToInt32(IoMgr.GetInstance().ReadIoOutBit(ncard, nIndex));
                    }
                    if (btns_out_system[i].ImageIndex != (int)status)
                    {
                        if (status != 0)
                            status = 1;
                        btns_out_system[i].ImageIndex = status;  //通过IO返回的0或1选择显示IO图片
                    }
                }
            }
         }

        /// <summary>
        /// 机器人通讯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_robot_Click(object sender, EventArgs e)
        {
            //Form_Robot frm = new Form_Robot();
            ////frm.ShowDialog(this.Parent);
            //frm.Owner = this;
            //frm.SelectRobotCommunicate();
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.Show();

            //Form_RobotMgr frm = new Form_RobotMgr();
            //frm.Owner = this;
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.Show();

            Form_Vision_debug frm = new Form_Vision_debug();
            frm.Owner = this;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        /// <summary>
        /// 调试视觉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_vision_Click(object sender, EventArgs e)
        {
            //Form_Vision_debug frm =new  Form_Vision_debug();
            Form_LightMgr frm = new Form_LightMgr();
            frm.Owner = this;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        /// <summary>
        /// 外围设备,串口网口自由通讯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_eth_com_Click(object sender, EventArgs e)
        {
            Form_Robot frm = new Form_Robot();
            frm.Owner = this; 
            frm.SelectFreeCommunicate(system_in,system_out);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
            
        }

        private void button_station_Click(object sender, EventArgs e)
        {
            Form_debug frm = new Form_debug();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this.Parent.Parent);
        }

        private void button_OPC_Click(object sender, EventArgs e)
        {            
            FormServer frm = new FormServer();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this.Parent.Parent);
        }
    }
}
