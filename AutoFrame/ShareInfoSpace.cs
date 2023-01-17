using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.IO;
using AutoFrameDll;
using CommonTool;
using System.Data.SqlClient;
using System.Windows.Forms;
using ToolEx;
using Communicate;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace AutoFrame
{
    class ShareInfoSpace
    {

        public struct myStationInfo
        {
            public string CarrierCode;
            public bool DownCameraResult;
            public bool UpCamereResult;
            public bool KaGuanResult;
            public bool FinallyResult;
        }
        /// <summary>
        /// 存载具码，下相机结果，上相机结果，卡关结果
        /// </summary>
        public static myStationInfo[] StationInfo = new myStationInfo[2];

        public static double AxisPosition;
        

        /// <summary>
        /// 扫码结果
        /// </summary>
        public static bool ScanResult=true;
        public static void InitShareInfoSpace(int Index)
        {
            StationInfo[Index].CarrierCode = "";
            StationInfo[Index].DownCameraResult = true;
            StationInfo[Index].UpCamereResult = false;
            StationInfo[Index].KaGuanResult = true;
            StationInfo[Index].FinallyResult = false;
        }



        /// <summary>
        /// 保存ICT需要的数据
        /// </summary>
        /// <param name="strCoil">NC码</param>
        /// <param name="nCavity">穴位</param>
        public static void SaveShowFlowData(string NCCode, string NewCode, int nCavity, string DetectData1, string DetectData2, bool DetectResult, DateTime now)
        {
            if (NewCode.Length < 5)
                return;

            //保存记录
            string strRemoteFileName = String.Format("{0}\\P92 Magnetic moment test_{1}_{2}.csv"
                , SystemMgr.GetInstance().GetParamString("ICTDataPath")
                , NewCode
                , now.ToString("yyyy_MM_dd_HH_mm_ss"));

            Directory.CreateDirectory(SystemMgr.GetInstance().GetParamString("ICTDataPath"));
            string[] Heads = new string[]
            {
                    "exppid",
                    "Flux",//磁通
                    "Magnetic moment",//磁矩
                    "Line",
                    "Machine_number",
                    "Station_number",//站别，测试机的站别固定T0442
                    "Cavity_number",
                    "Station",//固定为  磁矩测试
                    "Result"
            };

            CsvOperationEx CS = new CsvOperationEx();

            CS.BQuota = false;//保存上没有引号

            int col = 0;
            foreach (string str in Heads)
            {
                CS[0, col++] = str;
            }
            col = 0;

            //  ITEMS Testpcid  Collect time    Shield SN   Flux        Magnetic moment    Line     Machine_number  Station_number  Cavity_number   Station     Result
            //  都是1 软件版本	                            磁通量	    磁矩	           线别L1   机台编号01	    T0442	        穴位	        磁矩测试	Pass

            CS[1, col++] = NewCode;
            CS[1, col++] = DetectData1;
            CS[1, col++] = DetectData2;
            CS[1, col++] = SystemMgr.GetInstance().GetParamString("LineID");
            CS[1, col++] = SystemMgr.GetInstance().GetParamString("MachineID");
            CS[1, col++] = "T0442";
            CS[1, col++] = nCavity.ToString();
            CS[1, col++] = "Magnetic moment test";
            CS[1, col++] = DetectResult ? "PASS" : "NG";
            CS.Save(strRemoteFileName);

        }


        public static void SaveLocalRecond(string Time, string IsHaveSTC1, string Code1, string IsHaveSTC2, string Code2)
        {
            try
            {
                //保存记录
                string strRemoteFileName = String.Format("d:\\exe\\Data\\静态数据{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));

                Directory.CreateDirectory("d:\\exe\\LocalRecond");
                CsvOperationEx CS = new CsvOperationEx();
                CS.BQuota = false;//保存上没有引号
                int col = 0;
                if (!File.Exists(strRemoteFileName))
                {
                    string[] Heads = new string[] { "时间", "1穴有无STC", "STC码", "2穴有无STC", "STC码" };

                    foreach (string str in Heads)
                    {
                        CS[0, col++] = str;
                    }
                    CS.Save(strRemoteFileName);
                }
                col = 0;
                CS[0, col++] = Time;
                CS[0, col++] = IsHaveSTC1;
                CS[0, col++] = Code1;
                CS[0, col++] = IsHaveSTC2;
                CS[0, col++] = Code2;

                CS.Save(strRemoteFileName);
            }
            catch
            {
            }
        }

        public static void SaveDropReason(DateTime now, string reason, string Code = "NULL")
        {
            try
            {
                //保存记录
                string strRemoteFileName = String.Format("d:\\exe\\DropReason\\DropReason{0}.csv", now.ToString("yyyy_MM_dd"));

                Directory.CreateDirectory("d:\\exe\\DropReason");
                CsvOperationEx CS = new CsvOperationEx();
                CS.BQuota = false;//保存上没有引号
                int col = 0;
                if (!File.Exists(strRemoteFileName))
                {
                    string[] Heads = new string[] { "时间", "原因", "STC码", };

                    foreach (string str in Heads)
                    {
                        CS[0, col++] = str;
                    }
                    CS.Save(strRemoteFileName);
                }
                col = 0;
                CS[0, col++] = now.ToString("yyyyMMdd HH:mm:ss");
                CS[0, col++] = reason;
                CS[0, col++] = Code;

                CS.Save(strRemoteFileName);
            }
            catch
            {
            }
        }
        public static void Func_记录缓存站给后站送载具时间(DateTime now, string str时间间隔)
        {
            try
            {
                //保存记录
                string strRemoteFileName = String.Format("d:\\exe\\缓存站给后站送载具时间\\缓存站给后站送载具时间{0}.csv", now.ToString("yyyyMMdd"));

                Directory.CreateDirectory("d:\\exe\\缓存站给后站送载具时间");
                CsvOperationEx CS = new CsvOperationEx();
                CS.BQuota = false;//保存上没有引号
                int col = 0;
                if (!File.Exists(strRemoteFileName))
                {
                    string[] Heads = new string[] { "时间" ,"时间间隔"};

                    foreach (string str in Heads)
                    {
                        CS[0, col++] = str;
                    }
                    CS.Save(strRemoteFileName);
                }
                col = 0;
                CS[0, col++] = now.ToString("yyyyMMdd HH:mm:ss");
                CS[0, col++] = str时间间隔;
                CS.Save(strRemoteFileName);
            }
            catch
            {
            }
        }

        public static void SaveScanerRecord(DateTime now, bool Result, string nCavity, string Code = "NULL")
        {
            try
            {
                //保存记录
                string strRemoteFileName = String.Format("d:\\exe\\ScanerRecord\\ScanerRecord{0}.csv", now.ToString("yyyy_MM_dd"));

                Directory.CreateDirectory("d:\\exe\\ScanerRecord");
                CsvOperationEx CS = new CsvOperationEx();
                CS.BQuota = false;//保存上没有引号
                int col = 0;
                if (!File.Exists(strRemoteFileName))
                {
                    string[] Heads = new string[] { "时间", "扫码结果", "扫码枪序号", "码" };

                    foreach (string str in Heads)
                    {
                        CS[0, col++] = str;
                    }
                    CS.Save(strRemoteFileName);
                }
                col = 0;

                CS[0, col++] = now.ToString("yyyyMMdd HH:mm:ss");
                CS[0, col++] = Result ? "扫码成功" : "扫码失败";
                CS[0, col++] = nCavity;
                CS[0, col++] = Code;

                CS.Save(strRemoteFileName);
            }
            catch
            {
            }
        }






        public static bool Func_捞取NC码(ProductData pd)
        {
            try
            {
                string strPath = @"D:\exe\StcCode\StcCode1";
                DirectoryInfo di = new DirectoryInfo(strPath);
                if (!di.Exists)
                    Directory.CreateDirectory(strPath);

                string strBackupPath = strPath + @"\backup\";
                if (!Directory.Exists(strBackupPath))
                    Directory.CreateDirectory(strBackupPath);

                foreach (var fi in di.EnumerateFiles())
                {
                    if (fi.Name.Length < 4)
                        continue;

                    if ("StcCode" == fi.Name.Substring(0, 7))
                    {
                        bool bRtn = false;

                        using (StreamReader sr = new StreamReader(new FileStream(fi.FullName, FileMode.Open, FileAccess.Read), Encoding.Default))
                        {
                            string str = sr.ReadToEnd();
                            str = str.Trim();
                            string[] strCut = str.Split(',');


                            strBackupPath += strCut[0] + ".csv";
                            bRtn = true;

                        }

                        if (bRtn)
                        {
                            fi.MoveTo(strBackupPath);
                            //ShowLog("Move File: " + pd.strCarrierSN + DateTime.Now.ToString("_yyyyMMdd_HHmmss"));
                            return true;
                        }
                    }
                }
                //ShowLog("Can not find File: " + pd.strCarrierSN + DateTime.Now.ToString("_yyyyMMdd_HHmmss"));
                return false;
            }
            catch (Exception ex)
            {
                //Alarm(ex.ToString());
                return false;
            }
        }


        #region 卡关


        public static bool Func_Stc卡关(string STCCode, out string Msg, bool Npass = true)
        {
            if (!SystemMgr.GetInstance().GetParamBool("IsKGStation1"))
            {
                Msg = "未启用卡关1";
                return true;
            }

            if (STCCode.Length < 3)
            {
                Msg = $"{STCCode}_长度不够";
                return false;
            }
            string SFCIP = "KSSFClisa.luxshare.com.cn";
            //string STCPreStationID = SystemMgr.GetInstance().GetParamString("STCPreStationID");
            string STCPreStationID = SystemMgr.GetInstance().GetParamString("KGStation1");
            try
            {
                string strMsg = "未查找到记录";
                //     string strMsg = "-1";
                string strConnection = "data source=" + SFCIP + ";initial catalog=MESDB; user id=dataquery; password=querydata";
                SqlConnection conn = new SqlConnection(strConnection);
                conn.Open();
                string strSql;
                if (Npass == true)
                    strSql = "select* from m_TestResult_t with(nolock)where stationid = '" +
                       STCPreStationID + "' and result = 'PASS' and Ppid = '" + STCCode.Trim() + "'";
                else
                    strSql = "select* from m_TestResult_t with(nolock)where stationid = '" +
                        STCPreStationID + "'and Ppid = '" + STCCode.Trim() + "'";
                SqlCommand comm = new SqlCommand(strSql, conn);
                SqlDataReader DataReader = comm.ExecuteReader();
                while (DataReader.Read())
                {
                    strMsg = DataReader[2].ToString();
                    Msg = strMsg;
                }
                Msg = strMsg;
                DataReader.Close();
                conn.Close();
                if (strMsg == "PASS")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                IoMgr.GetInstance().AlarmLight(LightState.红灯开);
                IoMgr.GetInstance().AlarmLight(LightState.蜂鸣开);
                WarningMgr.GetInstance().Info("STC卡关通讯失败-弹窗报警");
                MessageBox.Show(e.Message, "STC卡关通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IoMgr.GetInstance().AlarmLight(LightState.绿灯开);
                Msg = "STC卡关通讯失败";
                return false;
            }
        }
        public static bool Func_Stc抽检卡关(string StcCode, out string Msg, ref string strCode)
        {
            if (!SystemMgr.GetInstance().GetParamBool("IsKGStation2"))
            {
                Msg = "未启用卡关2";
                return true;
            }
            if (StcCode.Length < 3)
            {
                Msg = $"{StcCode}：长度不够";
                return false;
            }
            string strstccode = "";
            if (StcCode.Contains('+'))
            {
                try
                {
                    strstccode = StcCode.Split('+')[0];
                }
                catch
                {
                    strstccode = "-";
                    strCode = strstccode;
                    Msg = $"{StcCode}分割获取条码失败";
                    return false;
                }
                //strstccode = strCode.Split('+')[0];
            }
            else
            {
                strstccode = "-";
                strCode = strstccode;
                Msg = $"{StcCode}条码不包含+";
                return false;
            }
            string SFCIP = "KSSFClisa.luxshare.com.cn";
            //string STCPreStationID = SystemMgr.GetInstance().GetParamString("StePdcaPreID");
            string STCPreStationID = SystemMgr.GetInstance().GetParamString("KGStation2");
            try
            {
                string strMsg = "未查找到记录";
                string strConnection = "data source=" + SFCIP + ";initial catalog=MESDB; user id=dataquery; password=querydata";
                SqlConnection conn = new SqlConnection(strConnection);
                conn.Open();
                string strSql;
                strSql = "select result from m_testresult_t where ppid='" + strstccode + "' and stationid='" + STCPreStationID.Trim() + "' ";
                SqlCommand comm = new SqlCommand(strSql, conn);
                SqlDataReader DataReader = comm.ExecuteReader();
                while (DataReader.Read())
                {
                    strMsg = DataReader["result"].ToString();
                    Msg = strMsg;
                }
                Msg = strMsg;
                DataReader.Close();
                conn.Close();
                if (strMsg == "PASS")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                IoMgr.GetInstance().AlarmLight(LightState.红灯开);
                IoMgr.GetInstance().AlarmLight(LightState.蜂鸣开);
                WarningMgr.GetInstance().Info("STC卡关通讯失败-弹窗报警");
                MessageBox.Show(e.Message, "STC卡关通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                IoMgr.GetInstance().AlarmLight(LightState.绿灯开);
                Msg = "STC卡关通讯失败";
                return false;
            }
        }




        #endregion 卡关
    }
}
