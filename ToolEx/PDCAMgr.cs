using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using Communicate;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ToolEx
{
    /// <summary>
    /// PDCA上传管理
    /// </summary>
    public class PDCAMgr : SingletonTemplate<PDCAMgr>
    {
        private TcpLink m_tcpPDCA;

        private string m_strSN;

        /// <summary>
        /// 
        /// </summary>
        public static object m_lock = new object();

        /// <summary>
        /// 连接PDCA
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool Connect(TcpLink link)
        {
            m_tcpPDCA = link;

            if (m_tcpPDCA != null)
            {
                return m_tcpPDCA.Open();
            }

            return false;
        }

        /// <summary>
        /// 断开PDCA连接
        /// </summary>
        public void Disconnect()
        {
            if (m_tcpPDCA != null)
            {
                m_tcpPDCA.Close();
            }
        }

        /// <summary>
        /// 创建开始信息 eg: CCQM7000FY1K@start
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public string CreateStartMessage(string serialNumber)
        {
            m_strSN = serialNumber;      // Set SN instance variable
            return String.Format("{0}@start\n", m_strSN);
        }

        /// <summary>
        /// 创建开始时间信息 eg: CCQM7000FY1K@start_time@2020-02-27 13:00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string CreateStartTimeMessage(DateTime time)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@start_time@{1}\n", m_strSN,time.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 创建开始时间信息 eg: CCQM7000FY1K@start_time@2020-02-27 13@yyyy-MM-dd HH
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public string CreateStartTimeMessage(DateTime time,string timeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@start_time@{1}@{2}\n", m_strSN, time.ToString(timeFormat),timeFormat);
        }

        /// <summary>
        /// 创建结束时间信息  CCQM7000FY1K@stop_time@2020-02-27 14:00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string CreateStopTimeMessage( DateTime time)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@stop_time@{1}\n", m_strSN, time.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 创建开始时间信息 eg: CCQM7000FY1K@stop_time@2020-02-27 14@yyyy-MM-dd HH
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public string CreateStopTimeMessage(DateTime time, string timeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@stop_time@{1}@{2}\n", m_strSN, time.ToString(timeFormat), timeFormat);
        }

        /// <summary>
        /// 创建DUT_POS信息 eg: CCQMTESTFY1K@dut_pos@BUEN01@H1
        /// </summary>
        /// <param name="fixtureID">最长16个字符</param>
        /// <param name="headID">最长8个字符</param>
        /// <returns></returns>
        public string CreateDUT_POSMessage(string fixtureID, string headID)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw(new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@dut_pos@{1}@{2}\n", m_strSN, fixtureID, headID);
        }

        /// <summary>
        /// 创建数据信息 eg: CCQMTESTFY1K@pdata@second@0.01
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public string CreatePDataMessage(string testName, float testValue)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}\n", m_strSN, testName, testValue);
        }

        /// <summary>
        /// 创建数据信息 eg: CCQMTESTFY1K@pdata@second@0.01@0.1@0.11
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="testValue"></param>
        /// <param name="lowerLimit"></param>
        /// <param name="upperLimit"></param>
        /// <returns></returns>
        public string CreatePDataMessage(string testName, double testValue, double lowerLimit, double upperLimit)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}@{3}@{4}\n", m_strSN, testName, testValue, lowerLimit, upperLimit);
        }

        /// <summary>
        /// 创建数据信息 eg: CCQMTESTFY1K@pdata@second@0.01@0.1@0.11@dec
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="testValue"></param>
        /// <param name="lowerLimit"></param>
        /// <param name="upperLimit"></param>
        /// <param name="measurementUnit"></param>
        /// <returns></returns>
        public string CreatePDataMessage(string testName, float testValue, float lowerLimit, float upperLimit, string measurementUnit)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            return String.Format("{0}@pdata@{1}@{2}@{3}@{4}@{5}\n", m_strSN, testName, testValue, lowerLimit, upperLimit, measurementUnit);
        }

        /// <summary>
        /// 创建属性信息 eg:CCQMTESTFY1K@attr@MLBSN@Hello
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public string CreateAtrrMessage(string testName, float testValue)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@attr@{1}@{2}\n", m_strSN, testName, testValue);
        }

        /// <summary>
        /// 创建上传日志信息 eg:CCQMTESTFY1K@log@we are now logging to pudding using Bali protocol
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public string CreateLogMessage(string log)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@log@{1}\n", m_strSN, log);
        }

        /// <summary>
        /// 创建上传日志文件信息 eg:CCQMTESTFY1K@log_ﬁle@http://thebrainfever.com/images/apple-logos/Silhouette.png
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string CreateLogFileMessage(string path)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@log_ﬁle@{1}\n", m_strSN, path);
        }

        /// <summary>
        /// 创建上传日志文件信息 eg:CCQMTESTFY1K@log_ﬁle@smb://127.0.0.1/Downloads/gitbox-1.6.2-ml.zip@uid@pwd
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string CreateLogFileMessage(string path,string userid,string password)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@log_ﬁle@{1}@{2}@{3}\n", m_strSN, path,userid,password);
        }

        /// <summary>
        /// 创建提交信息 eg:CCQMTESTFY1K@submit@1.0
        /// </summary>
        /// <param name="SWVersion"></param>
        /// <returns></returns>
        public string CreateSubmitMessage(string SWVersion)
        {
            if (string.IsNullOrEmpty(m_strSN))
            {
                throw (new Exception("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }

            string serialNum = m_strSN;
            m_strSN = null;                  // Reset SN for next DUT
            return String.Format("{0}@submit@{1}\n", serialNum, SWVersion);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            if (m_tcpPDCA != null)
            {
                m_tcpPDCA.WriteString(msg);
            }
        }

        /// <summary>
        /// 获取单个数据的回复
        /// </summary>
        /// <returns></returns>
        public string GetSingleReplay()
        {
            string strReturn = string.Empty;

            if (m_tcpPDCA != null && m_tcpPDCA.IsOpen())
            {
                // Initialize
                StringBuilder reply = new StringBuilder();
                int BUFSIZE = 32;
                Byte[] buf;
                bool done = false;

                // Get Reply by reading BUFSIZE-byte chunks from the TCP socket until
                // we get the reply termination string - "}@\n"
                while (!done)
                {
                    // Reinitialize our buffer
                    buf = new Byte[BUFSIZE];
                    // Read BUFSIZE bytes from TCP socket
                    int bytesRead = m_tcpPDCA.ReadData(buf, buf.Length);
                    // Check if any bytes were read
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    // Convert buf from bytes to string
                    String tmpReply = System.Text.Encoding.Default.GetString(buf);
                    // Concatenate this portion of the reply to the full reply
                    reply.Append(tmpReply);
                    // Check if this portion of the reply contains termination string "}@\n"
                    if (reply.ToString().Contains("}@\n"))
                    {
                        // Cut off all characters after termination string
                        int lengthOfValidString = reply.ToString().LastIndexOf("}@\n") + 3;
                        strReturn = reply.ToString().Substring(0, lengthOfValidString);
                        // Signal that we're done
                        done = true;
                    }
                }

            }

            return strReturn;
        }

        /// <summary>
        /// 获取数据组的回复
        /// </summary>
        /// <returns></returns>
        public string GetMutiReplay()
        {
            string strReturn = string.Empty;

            if (m_tcpPDCA != null && m_tcpPDCA.IsOpen())
            {
                // Initialize
                StringBuilder reply = new StringBuilder();
                int BUFSIZE = 32;
                Byte[] buf;
                bool done = false;

                // Get Reply by reading BUFSIZE-byte chunks from the TCP socket until
                // we get the reply termination string - "}\n"
                while (!done)
                {
                    // Reinitialize our buffer
                    buf = new Byte[BUFSIZE];
                    // Read BUFSIZE bytes from TCP socket
                    int bytesRead = m_tcpPDCA.ReadData(buf, buf.Length);
                    // Check if any bytes were read
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    // Convert buf from bytes to string
                    String tmpReply = System.Text.Encoding.Default.GetString(buf);
                    // Concatenate this portion of the reply to the full reply
                    reply.Append(tmpReply);
                    // Check if this portion of the reply contains termination string "}\n"
                    if (reply.ToString().Contains("}\n"))
                    {
                        // Cut off all characters after termination string
                        int lengthOfValidString = reply.ToString().LastIndexOf("}\n") + 2;
                        strReturn = reply.ToString().Substring(0, lengthOfValidString);
                        // Signal that we're done
                        done = true;
                    }
                }

            }

            return strReturn;
        }

        /// <summary>
        /// 判断PDCA回复是否成功
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public bool IsSuccess(string reply)
        {
            string[] strReplyArray = reply.Split('\n');

            bool bSuccess = true;

            foreach(string str in strReplyArray)
            {
                string[] strSplits = str.Split('@');

                if (strSplits.Length > 0)
                {
                    if (strSplits[0].ToLower().Contains("bad")
                        || strSplits[0].ToLower().Contains("err"))
                    {
                        bSuccess = false;
                        break;
                    }
                }
            }

            return bSuccess;
        }

        /// <summary>
        /// 解析单个回复数据
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public string ParseSingleReply(string reply)
        {
            // Initialize
            string replyType = String.Empty;
            string replyMsg = String.Empty;
            // Split Bali Reply by @
            string[] replyComponents = reply.Split('@');
            // Verify that we have 3 entries ([<replyType>,<replyMsg>,"\n"])
            if (replyComponents.Length != 3)
            {
                throw (new Exception(String.Format("接收信息格式错误 <{0}>", reply)));
            }

            // Capture replyType and replyMsg
            replyType = replyComponents[0];
            replyMsg = replyComponents[1];
            //Console.WriteLine(String.Format("ParseReply: <{0}> : <{1}>", replyType, replyMsg));

            // Check replyType for Bad or Err
            if (replyType.ToLower().Equals("bad"))
            {
                throw (new Exception(replyMsg));
            }
            else if (replyType.ToLower().Equals("err"))
            {
                throw (new Exception(replyMsg));
            }

            // If we get down to here, the Bali reply was 'ok',
            // so we can just return the replyMsg

            // Return
            return replyMsg;
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="strSN"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string QueryRecord(string strSN,string strName)
        {
            string strMessage = string.Format("sfc_post@c=Query_record&sn={0}&p={1}\n", strSN,strName);

            try
            {
                if (m_tcpPDCA.WriteLine(strMessage))
                {
                    Thread.Sleep(10);

                    //ok@{0 SFC_OK[fgsn = C39123456XYZ]}@
                    string strReply = GetSingleReplay();

                    if (!string.IsNullOrEmpty(strReply))
                    {
                        //{0 SFC_OK[fgsn = C39123456XYZ]}
                        string strMsg = ParseSingleReply(strReply);

                        if (!string.IsNullOrEmpty(strMsg))
                        {
                            int nStartIndex = strMsg.IndexOf('[') + 1;
                            int nLength = strMsg.LastIndexOf(']') - nStartIndex;

                            string strKeyValue = strMsg.Substring(nStartIndex, nLength);

                            //fgsn = C39123456XYZ
                            string[] strSplits = strKeyValue.Split('=');

                            if (strSplits.Length == 2)
                            {
                                return strSplits[1].Trim();
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return "";
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strText"></param>
        public void WriteFile(string strFileName,string strText)
        {
            using (StreamWriter sw = new StreamWriter(strFileName, true))
            {
                sw.WriteLine(strText);
                sw.Flush();
            }
        }
    }
}
