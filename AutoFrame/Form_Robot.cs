using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Communicate;
using AutoFrameDll;
using CommonTool;
using System.IO.Ports;
using System.Net.Sockets;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    ///todo: need test com and tcp.
    public partial class Form_Robot : Form
    {
        private Button[] btns_in;
        private Button[] btns_out;
        public string[] io_in = { "1.1", "1.2", "1.3", "1.4", "1.5" };
        public string[] io_out = { "1.1", "1.2", "1.3", "1.4", "1.5" };
        public string[] cmd_list = { "start", "cmd_one", "cmd_second", "cmd_third", "stop" };

        int m_nIndex = 0;   //指定当前机器人要使用的通讯连接索引号,表示在包括全部时的索引
        bool bIsTcp = true; //指定当前通讯端口是网口还是串口
        string[] m_strCmd = { }; //机器人通讯下命令数组
        public enum CommiSts
        {
            RbtCommi = 0,  //机器人通讯
            FreeCommi = 1    //自由通讯
        }
        CommiSts m_comtSts = new CommiSts(); //切换通讯模式

        Thread m_thread = null;
        private Object thisLock = new Object();

        public Form_Robot()
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

        private void BindIoButton()
        {
            int nIndex = 0;
            btns_in = new Button[groupBox_in.Controls.Count];

            foreach (Control ctn in groupBox_in.Controls)
            {
                btns_in[nIndex++] = (Button)ctn;
            }
            Array.Sort(btns_in, new ManaulTool.ControlSort());

            btns_out = new Button[groupBox_out.Controls.Count];
            nIndex = 0;
            foreach (Control ctn in groupBox_out.Controls)
            {
                btns_out[nIndex] = (Button)ctn;
                btns_out[nIndex].Click += ManaulTool.Form_IO_Out_Click;
                nIndex++;
            }
            Array.Sort(btns_out, new ManaulTool.ControlSort());
        }
        private void Form_Robot_Load(object sender, EventArgs e)
        {
            //BindIoButton();
            //ManaulTool.updateIoText(btns_in, io_in);
            //ManaulTool.updateIoText(btns_out, io_out, false);
            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;
        }


        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            if (Security.GetUserMode() >= UserMode.Engineer)
            {
                groupBox_out.Enabled = true;
                button_send.Enabled = true;
                button_open.Enabled = true;
                button_close.Enabled = true;
            }
            else
            {
                groupBox_out.Enabled = false;
                button_send.Enabled = false;
                button_open.Enabled = false;
                button_close.Enabled = false;
            }
        }
    

        private void timer1_Tick(object sender, EventArgs e)
        {
            ManaulTool.updateIoState(btns_in, io_in);
        }

        void SendCmd(string strCmd)
        {
            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    if (t.IsOpen())
                    {
                        lock (thisLock)
                        {
                            t.WriteLine(strCmd);
                        }
                    }
                }
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    if (t.IsOpen())
                        lock (thisLock)
                        {
                            t.WriteLine(strCmd);
                        }
                }
            }
        }
        void SendData(Byte[] sendBytes)
        {
            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    if (t.IsOpen())
                    {
                        lock (thisLock)
                        {
                            t.WriteData(sendBytes, sendBytes.Length);
                        }
                    }
                }
                
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    if (t.IsOpen())
                    {
                        lock (thisLock)
                        {
                            t.WriteData(sendBytes, sendBytes.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 任意字符串转换为十六进制
        /// </summary>
        /// <param name="strCmd">待转换的字符串</param>
        /// <returns></returns>
        string convert_hex(string strCmd)  
        {
            char[] values = strCmd.ToCharArray();
            string hexOutput = string.Empty;
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                hexOutput += String.Format("{0:X}", value);
            }
            return hexOutput;
            //string[] strSplit = strCmd.Split(' ');
            //string strReturn = string.Empty;
            //foreach(string strPart in strSplit)  
            //{
            //    int k = Convert.ToInt32(strPart,16);
            //    strReturn += k.ToString("X2");
            //}
            //return strReturn;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_send_Click(object sender, EventArgs e)
        {
            if (tabControl_send.SelectedIndex == 0)
            { //命令发送
                SendCmd(listBox_cmd.Text);
            }
            else
            {
                if (checkBox_hex.Checked)
                {
                    SendData(ToByte(textBox_cmd.Text));
                }
                else
                {
                    SendData(System.Text.Encoding.Default.GetBytes(textBox_cmd.Text.ToCharArray()));
                    //SendCmd(textBox_cmd.Text);
                }
            }
        }

        private string ToHexString(string strData, bool bSplit = true)
        {
            string strText = "";
            //文本转换为16进制ASCII
            byte[] data = System.Text.Encoding.Default.GetBytes(strData);

            foreach (byte b in data)
            {
                strText += b.ToString("X2");

                if (bSplit)
                {
                    strText += " ";
                }
            }

            return strText;
        }

        private byte[] ToByte(string strData,int fromBase = 16)
        {
            //16进制ASCII转换为字符串
            string strByte = "";
            List<byte> list = new List<byte>();
            for (int i = 0; i < strData.Length; i++)
            {
                strByte += strData[i];
                if (strByte.Length == 2 || strData[i] == ' ')
                {
                    try
                    {
                        byte by = Convert.ToByte(strByte.Trim(), fromBase);
                        list.Add(by);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        strByte = "";
                    }


                }

            }

            return list.ToArray();
        }

        private void checkBox_hex_CheckedChanged(object sender, EventArgs e)
        {
            //textBox_cmd.Text = "";
            string strText = "";
            if (checkBox_hex.Checked)
            {
                //文本转换为16进制ASCII
                strText = ToHexString(textBox_cmd.Text);
            }
            else
            {
                //16进制ASCII转换为字符串
                strText = System.Text.Encoding.Default.GetString(ToByte(textBox_cmd.Text));
            }

            textBox_cmd.Text = strText;
        }

        private void textBox_cmd_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = " 0123456789ABCDEF".IndexOf(char.ToUpper(e.KeyChar)) < 0;
        }

        /// <summary>
        /// 打开网/串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_open_Click(object sender, EventArgs e)
        {
            bool bOpen = false;
            if (bIsTcp) //网口通讯
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    lock (thisLock)
                    {
                        bOpen = t.Open();
                    }
                }
            }
            else  //串口通讯
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    lock (thisLock)
                    {
                        bOpen = t.Open();
                    }
                }
            }

            if (bOpen)
            {
                button_open.Text = "已打开";
                comboBox_EthOrCom.Enabled = false;
                StartCommunicate();
            }
        }

        /// <summary>
        /// 关闭网/串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    StopCommunicate();
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    lock (thisLock)
                    {
                        t.Close();
                    }
                }
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    StopCommunicate();
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    lock (thisLock)
                    {
                        t.Close();
                    }
                }
            }

            button_open.Text = "打开";
            comboBox_EthOrCom.Enabled = true;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Robot_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopCommunicate();

            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    //StopCommunicate();
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    lock (thisLock)
                    {
                        t.Close();
                    }
                }
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    //StopCommunicate();
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    lock (thisLock)
                    {
                        t.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 开始监视线程
        /// </summary>
        public void StartCommunicate()
        {
            /*
            if (m_thread == null)
                m_thread = new Thread(ThreadCommunicate);
            if (m_thread.ThreadState != ThreadState.Running)
            {
                m_thread.Start();
            }
            */
            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    t.BeginAsynReceive(OnDataReceive);
                }
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    t.BeginAsynReceive(OnDataReceive);
                }
            }
        }

        /// <summary>
        /// 结束监视线程
        /// </summary>
        public void StopCommunicate()
        {
            /*
            if (m_thread != null)
            {
                if (m_thread.Join(2000) == false)
                    m_thread.Abort();
                m_thread = null;
            }
            */
            if (bIsTcp)
            {
                if (m_nIndex < TcpMgr.GetInstance().Count)
                {
                    TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                    t.EndAsynReceive();
                }
            }
            else
            {
                if (m_nIndex < ComMgr.GetInstance().Count)
                {
                    ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                    t.EndAsynReceive();
                }
            }
        }

        /// <summary>
        /// 通讯线程,待完善
        /// </summary>
        private void ThreadCommunicate()
        {//todo:需要做异步互斥
            while (true)
            {
                Thread.Sleep(1000);

                if (bIsTcp)
                {
                    if (m_nIndex < TcpMgr.GetInstance().Count)
                    {
                        TcpLink t = TcpMgr.GetInstance().GetTcpLink(m_nIndex);
                        if (t.IsOpen())
                        {
                            string strData;
                            lock (thisLock)
                            {
                                t.ReadLine(out strData);
                            }
                            if (strData.Length > 0)
                            {
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    listBox_receive.Items.Add(strData);
                                });
                              
                             
                            }
                        }
                    }
                }
                else
                {
                    if (m_nIndex < ComMgr.GetInstance().Count)
                    {
                        ComLink t = ComMgr.GetInstance().GetComLink(m_nIndex);
                        if (t.IsOpen())
                        {
                            string strData;
                            lock (thisLock)
                            {
                                t.ReadLine(out strData);
                            }
                            if (strData.Length > 0)
                            {
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    listBox_receive.Items.Add(strData);
                                });


                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空文本发送区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_write_clear_Click(object sender, EventArgs e)
        {
            textBox_cmd.Text = "";
        }

        /// <summary>
        /// 清空接收区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_read_clear_Click(object sender, EventArgs e)
        {
            listBox_receive.Items.Clear();
        }

        /// <summary>
        /// 组合框索引变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_cmd.Items.Clear();
            m_nIndex = comboBox_EthOrCom.SelectedIndex;
            int nEth = 0, nCom = 0, nIndex = 0, nTemp = -1;
            int nLinkMode = -1;
            nEth = TcpMgr.GetInstance().Count;
            nCom = ComMgr.GetInstance().Count;
            if (CommiSts.FreeCommi == m_comtSts) //自由通讯
            {
                nIndex = m_nIndex;
                nTemp = nIndex;
            }
            else if (CommiSts.RbtCommi == m_comtSts)  //机器人通讯
            {
                if (RobotMgr.GetInstance().m_listRobot.Count > m_nIndex)
                {
                    //通过组合框下拉的索引号,找到机器人的通讯方式以及索引号
                    nLinkMode = RobotMgr.GetInstance().m_listRobot[m_nIndex].nLinkMode;
                    string strRbtName = RobotMgr.GetInstance().m_listRobot[m_nIndex].strName;
                    //List<string> strListCmd = RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listStrCmd;
                    List<string> strListCmd = RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listStrCmd;
                    for (int i = 0; i < strListCmd.Count(); ++i)
                    {
                        listBox_cmd.Items.Add(strListCmd[i]);
                    }
                    nIndex = RobotMgr.GetInstance().m_listRobot[m_nIndex].nPort - 1;
                    nTemp = nIndex;
                    //绑定IO
                    //int nIoIn = RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemIn.Count;
                    int nIoIn = RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemIn.Count;
                    for (int i = 1; i <= 5; i++)
                    {
                        if (i <= nIoIn)
                        {
                            //io_in[i-1] = string.Format("{0}.{1}",
                            //    RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemIn[i-1].nCard,
                            //    RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemIn[i-1].nBit);
                            //io_in[i - 1] = string.Format("{0}.{1}",
                            //    RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemIn[i - 1].nCard,
                            //    RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemIn[i - 1].nBit);
                            io_in[i - 1] = RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemIn[i - 1].strName;
                        }
                        else
                            io_in[i-1] = "";
                    }
                    //int nIoOut = RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemOut.Count;
                    int nIoOut = RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemOut.Count;
                    for (int i = 1; i <= 5; i++)
                    {
                        if (i <= nIoOut)
                        {
                            //io_out[i-1] = string.Format("{0}.{1}",
                            //    RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemOut[i - 1].nCard,
                            //    RobotMgr.GetInstance().m_dicRobot[strRbtName].m_listIoSystemOut[i - 1].nBit);
                            //io_out[i - 1] = string.Format("{0}.{1}",
                            //    RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemOut[i - 1].nCard,
                            //    RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemOut[i - 1].nBit);
                            io_out[i - 1] = RobotMgr.GetInstance().m_listRobot[m_nIndex].m_listIoSystemOut[i - 1].strName;
                        }
                        else
                            io_out[i - 1] = "";
                    }
                    BindIoButton();
                    ManaulTool.updateIoText(btns_in, io_in);
                    ManaulTool.updateIoText(btns_out, io_out, false);
                }
            }
            if (nTemp<0)
            {
                m_nIndex = -1;
                return;
            }
            if ( (m_nIndex < nEth && -1 == nLinkMode) || 0 == nLinkMode) //网口通讯
            {
                TcpLink t = TcpMgr.GetInstance().GetTcpLink(nIndex);
                string strInfo = string.Format("{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}",
                    TcpMgr.m_strDescribe[0], t.m_nIndex.ToString(),
                    TcpMgr.m_strDescribe[1], t.m_strName,
                    TcpMgr.m_strDescribe[2], t.m_strIP,
                    TcpMgr.m_strDescribe[3], t.m_nPort.ToString(),
                    TcpMgr.m_strDescribe[4], t.m_nTime.ToString(),
                    TcpMgr.m_strDescribe[5], t.m_strLineFlag
                    );
                textBox_info.Text = strInfo;
                bIsTcp = true;
            }
            else if (m_nIndex < nEth + nCom || 1 == nLinkMode) //串口通讯
            {
                ComLink t;
                if (1==nLinkMode)
                    t = ComMgr.GetInstance().GetComLink(nIndex);
                else
                {
                    t = ComMgr.GetInstance().GetComLink(nIndex-nEth);
                    m_nIndex = nIndex - nEth;
                }
                string strInfo = string.Format("{0}: {1}\r\n{2}: {3}\r\n{4}: {5}\r\n{6}: {7}\r\n{8}: {9}\r\n{10}: {11}\r\n{12}: {13}\r\n{14}: {15}\r\n{16}: {17}\r\n{18}: {19}",
                ComMgr.m_strDescribe[0], t.m_nComNo,
                ComMgr.m_strDescribe[1], t.m_strName,
                ComMgr.m_strDescribe[2], t.m_nBaudRate,
                ComMgr.m_strDescribe[3], t.m_nDataBit,
                ComMgr.m_strDescribe[4], t.m_strPartiy,
                ComMgr.m_strDescribe[5], t.m_strStopBit,
                ComMgr.m_strDescribe[6], t.m_strFlowCtrl,
                ComMgr.m_strDescribe[7], t.m_nTime,
                ComMgr.m_strDescribe[8], t.m_nBufferSzie,
                ComMgr.m_strDescribe[9], t.m_strLineFlag
                );
                textBox_info.Text = strInfo;
                bIsTcp = false;
            }
            //m_nIndex = nTemp;
        }

        /// <summary>
        /// 自由通讯初始化界面
        /// </summary>
        public void SelectFreeCommunicate(string[] ioIn, string[] ioOut)
        {
            listBox_cmd.Items.Clear();
            m_comtSts = CommiSts.FreeCommi;
            int nEth = TcpMgr.GetInstance().Count;
            int nCom = ComMgr.GetInstance().Count;
            comboBox_EthOrCom.Items.Clear();
            for (int i = 0; i < nEth; i++)
            {
                comboBox_EthOrCom.Items.Add(TcpMgr.GetInstance().GetTcpLink(i).m_strName);
            };
            for (int i = 0; i < nCom; i++)
            {
                comboBox_EthOrCom.Items.Add(ComMgr.GetInstance().GetComLink(i).m_strName);
            };
            if (comboBox_EthOrCom.Items.Count > 0)
                comboBox_EthOrCom.SelectedIndex = 0;

            BindIoButton();
            io_in = ioIn;
            io_out = ioOut;
            ManaulTool.updateIoText(btns_in, io_in);
            ManaulTool.updateIoText(btns_out, io_out, false);
        }

        /// <summary>
        /// 机器人通讯初始化界面
        /// </summary>
        public void SelectRobotCommunicate()
        {
            m_comtSts = CommiSts.RbtCommi;
            int nCount = RobotMgr.GetInstance().m_listRobot.Count;
            comboBox_EthOrCom.Items.Clear();

            for (int i = 0; i < nCount; i++)
            {
                comboBox_EthOrCom.Items.Add(RobotMgr.GetInstance().m_listRobot[i].strName);
            };
            if (comboBox_EthOrCom .Items.Count> 0)
                comboBox_EthOrCom.SelectedIndex = 0;
        }
        //todo:增加当为自动流程时,不需要自已读通讯口, 只需要关联到相关的通讯口即可监控.

        private void OnDataReceive(byte []data,int length)
        { 

            this.BeginInvoke((MethodInvoker)delegate
            {
                string strText = "";
                if (checkBox_hex.Checked)
                {
                    for(int i = 0; i < length;i++)
                    {
                        strText += data[i].ToString("X2") + " ";
                    }
                }
                else
                {
                    strText = System.Text.Encoding.Default.GetString(data,0,length);
                }
                listBox_receive.Items.Add(strText);
            });
        }
    }
}
