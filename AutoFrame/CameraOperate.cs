using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoFrameDll;
using CommonTool;
using Communicate;
using System.Windows.Forms;


namespace AutoFrame
{
    class CameraOperate
    {

        public delegate void ShowLogHandler(string strLog, LogLevel level);
        public delegate int WaitReceiveHandler(TcpLink tcplink, out string strData, string strStartWith, string strEndWith = "", int nTimeOut = -1, bool bShowDialog = true, bool bPause = true);


        private TcpLink myTcp;

        /// <summary>
        /// 接受相机返回的数据
        /// </summary>
        private string[] strData = new string[] { };

        public CameraOperate(int TcpIndex)
        {
            this.myTcp = TcpMgr.GetInstance().GetTcpLink(TcpIndex);
        }

        public bool InitTcp()
        {
            bool Return = false;
            if (this.myTcp != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    myTcp.Close();
                    Thread.Sleep(300);
                    if (this.myTcp.Open())
                    {
                        Return = true;
                        break;
                    }
                    Thread.Sleep(300);
                }
            }
            return Return;
        }

        public void Deinit()
        {
            this.myTcp.Close();
        }

        public string myTcpName
        {
            get
            {
                return myTcp.m_strName;
            }
        }
        /// <summary>
        /// 接受相机返回的数据
        /// </summary>
        public string[] ReceiveData
        {
            get { return strData; }
        }


        public bool 相机拍照(ShowLogHandler ShowLog, WaitReceiveHandler wait_receive_cmd, string strCmd, out string recev, params string[] paramList)
        {

            if (!SystemMgr.GetInstance().GetParamBool("VisionEnable"))
            {
                Thread.Sleep(200);
                recev = "未启用相机";
                return true;
            }

            string strSend = strCmd;

            if (paramList.Length > 0)
            {
                foreach (string str in paramList)
                {
                    strSend += "," + str;
                }
            }

            if (ShowLog != null)
            {
                ShowLog("给相机发送:" + strSend, LogLevel.Info);
            }

            if (!myTcp.IsOpen())
            {
                if (!myTcp.Open())
                {
                    WarningMgr.GetInstance().Error($"{myTcp.m_strName}网口打开失败 !");
                }
            }
            string strData;

            if (!myTcp.WriteLine(strSend))
            {
                recev = "999";
                if (ShowLog != null)
                {
                    ShowLog("发送命令失败", LogLevel.Error);
                }
                return false;
            }
            string Cmd_End = strCmd.Split(',')[0];
            wait_receive_cmd(myTcp, out strData, Cmd_End);
            strData = strData.Trim();
            ShowLog("从相机接收:" + strData, LogLevel.Info);
            string[] strSplitsRec = strData.Split(',');
            string[] strSplitsSend = strCmd.Split(',');

            recev = strData;
            if (strSplitsRec[0] != strSplitsSend[0] || strSplitsRec[1] != "1")
            {
                return false;
            }
            return true;
        }
    }
}
