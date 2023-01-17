using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using CommonTool;
using System.Text.RegularExpressions;


namespace AutoFrame
{
    class PLCServer
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        string strPath = SystemMgr.GetInstance().GetParamString("PLCDir");
        public enum Head
        {
            Error,
            Data,
            State,
        }

        public void PlcServer(string name)
        {
            IPAddress address = GetAddress(name);
            //int port = GetPort(DS);
            tcpListener = new TcpListener(address, 8001);
            listenThread = new Thread(() => ListenForClients(name));
            listenThread.Start();
            SavePlcServerLog($"Server started at {address} :{8001} @ {DateTime.Now.ToString()}",name);

        }

        private int GetPort(DataStyle DS)
        {
            int port = 0;
            switch (DS)
            {
                case DataStyle.ErrorData:
                    port = 8001;
                    break;
                case DataStyle.MachineData:
                    port = 8002;
                    break;
                case DataStyle.MachineState:
                    port = 8003;
                    break;
                default:
                    port = 0;
                    break;
            }
            return port;
        }

        private IPAddress GetAddress(string name)
        {
            return IPAddress.Parse(SystemMgr.GetInstance().GetParamString(name));
        }

        private void ListenForClients(string name)
        {
            tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = tcpListener.AcceptTcpClient();

                //create a thread to handle communication with connected client               
                Thread clientThread = new Thread(() => HandleClientComm(client,name));
                clientThread.Start();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void HandleClientComm(object client, string name)
        {
            TcpClient tcpClient = (TcpClient)client;
            SavePlcServerLog("Client @[{ tcpClient.Client.LocalEndPoint}] connected @{ DateTime.Now.ToString()}",name);

            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead = 0;
            //bool isRight=false;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    SavePlcServerLog("Error:receive msg error",name);
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    SavePlcServerLog($"Client @[{tcpClient.Client.LocalEndPoint}] disconnect @{ DateTime.Now.ToString()}",name);
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
                string recvstr = encoder.GetString(message, 0, bytesRead);
                SavePlcServerLog($"Recv:[{ tcpClient.Client.LocalEndPoint}]:msg:@[{recvstr}] @{DateTime.Now.ToString()}",name);
                string[] head = recvstr.Split('#');//切割头部

                string[] data = head[1].Split(',');//切割内容

                string Message = GetPlcMessage((DataStyle)Enum.Parse(typeof(DataStyle), head[0]), name);//获取当前数据类型的message配置文件

                var paramNum = Regex.Matches(Message, "{\\d*}"); //正则表达式，匹配格式项
                if (data.Length != paramNum.Count)
                {
                    //缺少数据，需要重新匹配
                    continue;
                }
                Message = string.Format(Message, data);
                string datastyle = GetDataStyle((Head)Enum.Parse(typeof(Head), head[0]));
                string strFileSavePath = strPath +"\\"+name+"\\"+"uploadfiles\\" + datastyle + System.Guid.NewGuid().ToString() + ".json";
                if (!File.Exists(strFileSavePath))
                    //路径不存在，新建路径并关闭
                    using (new FileStream(strFileSavePath, FileMode.Create, FileAccess.ReadWrite)) { }
                File.AppendAllText(strFileSavePath, Message);

                //#region send msg to client                
                //string sendstr = "Server OK";
                //if (recvstr == "101")
                //{
                //    //isRight = true;
                //    sendstr = "202";
                //    Console.ForegroundColor = ConsoleColor.Red;
                //}
                //else
                //{
                //    Console.ForegroundColor = ConsoleColor.White;
                //}

                //byte[] buffer = encoder.GetBytes(sendstr);
                //clientStream.Write(buffer, 0, buffer.Length);
                //clientStream.Flush();
                //SavePlcServerLog("Sent:[{tcpClient.Client.LocalEndPoint}]:msg:@[{sendstr}] @{DateTime.Now.ToString()}\r\n");
                //#endregion
            }

            tcpClient.Close();
        }

        private string GetDataStyle(Head head)
        {
            string path = "";
            switch (head)
            {
                case Head.Error:
                    path = "errordata";
                    break;
                case Head.Data:
                    path = "machinedata";
                    break;
                case Head.State:
                    path = "machinestate";
                    break;
                default:
                    break;
            }
            return path;
        }

        private string GetPlcMessage(DataStyle DS, string name)
        {
            string Message = "";
            switch (DS)
            {
                case DataStyle.ErrorData:
                    Message = "{\"message\":\"{0}\",\"code\":\"{1}\",\"severity\":\"{2}\",\"data\":\"{3}\",\"occurrence_Time\":\"{4}\",\"resolved_Time\":\"{5}\"}";
                    //Message = SystemMgr.GetInstance().GetParamString($"{name}ErrorDataMessage");
                    break;
                case DataStyle.MachineData:
                    Message = "{\"sn\":\"{0}\",\"code\":\"{1}\",\"pass\":\"{2}\",\"input_time\":\"{3}\",\"output_time\":\"{4}\",\"data\":\"{5}\"}";
                    //Message = SystemMgr.GetInstance().GetParamString($"{name}MachineDataMessage");
                    break;
                case DataStyle.MachineState:
                    Message = "{\"machineState\":\"{0}\",\"state_change_time\":\"{1}\",\"data\":{\"code\":\"{2}\",\"message\":{3}}}";
                    //Message = SystemMgr.GetInstance().GetParamString($"{name}MachineStateMessage");
                    break;

                default:
                    Message = "";
                    break;
            }
            return Message;
        }

        private bool SavePlcServerLog(string Message, string name)
        {
            try
            {
                string strSavePath = strPath + "\\" +name+"\\"+ "PlcServerlog";
                string strFileSavePath = "";
                DateTime TimeNow = DateTime.Now;
                if (!Directory.Exists(strSavePath))
                    Directory.CreateDirectory(strSavePath);

                strFileSavePath = strSavePath + "\\" + TimeNow.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(strFileSavePath))
                {
                    FileStream fs1 = new FileStream(strFileSavePath, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                try
                {
                    File.AppendAllText(strFileSavePath, Message);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
    }
}
