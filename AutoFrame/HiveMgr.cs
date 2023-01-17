using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using CommonTool;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using RestSharp;
using System.Windows.Forms;

namespace AutoFrame
{
    #region 数据类型
    /// <summary>
    /// Error Information
    /// </summary>
    public struct ErrorMessage
    {
        public string code;
        public string Level;
        public string strMsgChinese;
        public string strMsgEnglish;

        public DateTime tm_begin;
        public DateTime tm_end;

        public Dictionary<string, string> data;
    }

    /// <summary>
    /// Error Machine State(包含报error 时的信息)
    /// 当状态为5时，可以保存error信息
    /// 用来暂存状态信息
    /// </summary>
    public struct StateMessage
    {
        /// <summary>
        /// 机台之前状态
        /// </summary>
        public int MachineState;
        /// <summary>
        /// 故障代码
        /// </summary>
        public string code;
        /// <summary>
        /// 故障信息
        /// </summary>       
        public string strMsg;
        /// <summary>
        /// 故障等级
        /// </summary>       
        public string strLevel;
    }

    /// <summary>
    /// 机台状态（5种状态）
    /// </summary>
    public enum MachineState
    {
        Running = 1,
        Idle = 2,
        Engineering = 3,
        PlannedDown = 4,
        UnplannedDown = 5,
    }

    /// <summary>
    /// 用于上传的4大数据类型
    /// （MachineData,MachineState,ErrorData,EnvironmentData）
    /// </summary>
    public enum DataStyle
    {
        MachineData = 1,
        MachineState = 2,
        ErrorData = 3,
        EnvironmentData = 4,
    }
    #endregion 数据类型

    #region 捕获数据
    /// <summary>
    /// 捕获数据(固定时间捕获一段时间的数据)
    /// 给出定时开关变量PostPause
    /// </summary>
    class PostMgr
    {
        public System.Timers.Timer HiveTimerStop;
        /// <summary>
        /// 每5分钟10秒停止上传数据
        /// </summary>
        public static bool PostPause = true;

        public System.Timers.Timer HiveTimerStart;

        public PostMgr()
        {
            HiveTimerStop = new System.Timers.Timer();
            HiveTimerStop.Enabled = true;
            HiveTimerStop.Interval = 300000;//5分钟
            HiveTimerStop.Elapsed += MachineDataPauseStop;

            HiveTimerStart = new System.Timers.Timer();
            HiveTimerStart.Enabled = true;
            HiveTimerStart.Interval = 10000;//10S
            HiveTimerStart.Elapsed += MachineDataPauseStart;
        }

        private void MachineDataPauseStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            HiveTimerStop.Stop();
            PostPause = false;

            HiveTimerStart.Start();
        }

        private void MachineDataPauseStart(object sender, System.Timers.ElapsedEventArgs e)
        {
            HiveTimerStart.Stop();
            PostPause = true;

            HiveTimerStop.Start();
        }

    }
    #endregion 捕获数据


    class HiveMgr : SingletonTemplate<HiveMgr>
    {
        public static StateMessage machine_state = new StateMessage { MachineState = 2 };//用于暂存设备状态
        public static ErrorMessage LastError = new ErrorMessage();
        private static string Time_Format = "yyyy-MM-ddTHH:mm:ss.ffzzz";
        private static readonly object lock_log = new object();
        private static readonly object lock_1 = new object();
        private static readonly object lock_showlog = new object();
        private static readonly object lock_movefile = new object();

        private DateTime Form_MessageBeginTime = DateTime.Now;
        private bool Form_MessageFirst = true;

        public static int Hive_MachineState;//界面显示hive状态


        /// <summary>
        /// 0:MachineData 1:MachineState  2:ErrorData
        /// </summary>
        public static bool[] BackUp_SendFlag = new bool[] { false, false, false };

        /// <summary>
        /// IOS时间格式
        /// </summary>
        public static string TimeFormat
        {
            get
            {
                return HiveMgr.Time_Format;
            }
        }


        #region 扫描3个正常上传文件夹的三个线程
        private Thread threadUploadMachineData;
        private Thread threadUploadMachineState;
        private Thread threadUploadJsonErrorData;
        private Thread threadUploadToDB;
        #endregion

        public HiveMgr()
        {
            #region start 数据正常上传线程
            threadUploadMachineData = new Thread(() => postJsonFile(DataStyle.MachineData));
            threadUploadMachineData.IsBackground = true;
            threadUploadMachineData.Start();
            //Thread.Sleep(1000 * 60);
            threadUploadMachineState = new Thread(() => postJsonFile(DataStyle.MachineState));
            threadUploadMachineState.IsBackground = true;
            threadUploadMachineState.Start();
            //Thread.Sleep(1000 * 60);
            threadUploadJsonErrorData = new Thread(() => postJsonFile(DataStyle.ErrorData));
            threadUploadJsonErrorData.IsBackground = true;
            threadUploadJsonErrorData.Start();
            #endregion
        }

        public void StartUpload()
        {
            //if (threadUploadMachineData.ThreadState == ThreadState.Unstarted)
            //    threadUploadMachineData.Start();

            //if (threadUploadToDB.ThreadState == ThreadState.Unstarted)
            //    threadUploadToDB.Start();

        }

        public void CloseUpload()
        {
            try
            {
                threadUploadMachineData.Abort();

                threadUploadMachineState.Abort();

                threadUploadJsonErrorData.Abort();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 保存信息到本地文档
        /// </summary>
        /// <param name="Message">需要保存的信息</param>
        /// <returns></returns>
        public bool SaveHiveLog(string Message)
        {
            string path = SystemMgr.GetInstance().GetParamString("HiveDir");

            lock (lock_log)
            {
                try
                {
                    string strSavePath = path + "\\" + "hivelog";
                    string strFileSavePath = "";
                    DateTime TimeNow = DateTime.Now;

                    if (!Directory.Exists(strSavePath))
                    {
                        Directory.CreateDirectory(strSavePath);
                    }
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


        #region retry every 5 minutes for 3 days
        /// <summary>
        ///  发送数据的状态
        /// </summary>
        public class SendDataState
        {
            private static volatile SendDataState instance = null;
            private static readonly object padlock = new object();

            /// <summary>
            /// 构造函数
            /// </summary>
            private SendDataState()
            {
                foreach (DataStyle suit in Enum.GetValues(typeof(DataStyle)))
                {
                    if (SendDataStates == null)
                    {
                        SendDataStates = new List<SendDataStateModel>();
                    }
                    //Console.WriteLine((int)suit + ":" + suit);
                    SendDataStates.Add(new SendDataStateModel() { CanSend = true, vDataStyle = suit, PropertyName = suit.ToString(), ThreadIsFinish = true, ThreadStartTime = DateTime.Now });
                }
            }

            public static SendDataState Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (padlock)
                        {
                            if (instance == null)
                            {
                                instance = new SendDataState();
                            }
                        }
                    }
                    return instance;
                }
            }


            private List<SendDataStateModel> SendDataStates { get; set; }

            //public bool ErrorDataCanSend { get; set; }
            //public bool MachineDataCanSend { get; set; }
            //public bool MachineStateDataCanSend { get; set; }

            /// <summary>
            ///   推迟发送时间
            /// </summary>
            /// <param name="DelayTime"></param>
            /// <param name="_datastyle"></param>
            public void DelayTime(int DelayTime, DataStyle _datastyle)
            {
                //Thread t = new Thread(() => SetValueTrue(DelayTime, _datastyle),);
                //t.IsBackground = true;
                //t.Start();

                SendDataStateModel sdsm = SendDataStates.Where(n => n.PropertyName == _datastyle.ToString()).FirstOrDefault();

                if (sdsm == null || string.IsNullOrEmpty(sdsm.PropertyName))
                {
                    //为获取到指定的数据
                    return;
                }
                //线程是否结束
                if (!sdsm.ThreadIsFinish)
                {
                    //当前线程正在进行中，不能再次设置重试
                    return;
                }
                //设置不能回传数据
                sdsm.CanSend = false;
                sdsm.ReSetCanSend = new Thread(() => SetValueTrue(DelayTime, sdsm));
                sdsm.ReSetCanSend.IsBackground = true;
                sdsm.ReSetCanSend.Start();

            }

            /// <summary>
            /// 后台线程计时，超过指定时间，将开启回传状态
            /// </summary>
            /// <param name="DelayTime"></param>
            /// <param name="sdsm"></param>
            private void SetValueTrue(int DelayTime, SendDataStateModel sdsm)
            {
                try
                {
                    //设置计算时间戳是否超多指定的时间

                    long localMillisecondTimestamp = GetMillisecondTimestamp();
                    while (GetMillisecondTimestamp() - localMillisecondTimestamp < DelayTime)
                    {
                        Thread.Sleep(1000);
                    }
                    sdsm.CanSend = true;
                    if (sdsm.ReSetCanSend != null)
                    {
                        sdsm.ReSetCanSend.Abort();
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sdsm.ThreadIsFinish = true;
                    sdsm.ReSetCanSend = null;
                }
            }
            /// <summary>
            /// 获取毫秒级时间戳
            /// </summary>
            /// <returns></returns>
            public long GetMillisecondTimestamp()
            {
                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            }

            /// <summary>
            /// 当前是否可以回传数据
            /// </summary>
            /// <param name="vDataStyle"></param>
            /// <returns></returns>
            public bool IsCanSend(DataStyle vDataStyle)
            {
                try
                {
                    SendDataStateModel sdsm = SendDataStates.Where(n => n.PropertyName == vDataStyle.ToString()).FirstOrDefault();
                    if (sdsm == null || string.IsNullOrEmpty(sdsm.PropertyName))
                    {
                        //为获取到指定的数据
                        return false;
                    }
                    return sdsm.CanSend;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            /// <summary>
            /// 本地时间和历史时间对比，小时差
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public bool IsOverBindingTime(DateTime dt, double hourNum)
            {
                DateTime now = DateTime.Now;
                TimeSpan ts = now.Subtract(dt);
                double sec = ts.TotalHours;
                return System.Math.Abs(sec) > hourNum;
            }


            public class SendDataStateModel
            {
                /// <summary>
                /// 数据类型
                /// </summary>
                public DataStyle vDataStyle { get; set; }
                /// <summary>
                /// 是否可以发送数据
                /// </summary>
                public bool CanSend { get; set; }
                /// <summary>
                /// DataStyle 名字
                /// </summary>
                public string PropertyName { get; set; }
                /// <summary>
                /// 线程，设置CanSend 为true的。
                /// </summary>
                public Thread ReSetCanSend { get; set; }
                /// <summary>
                /// 线程是否结束
                /// </summary>
                public bool ThreadIsFinish { get; set; }
                /// <summary>
                /// 线程开始时间
                /// </summary>
                public DateTime ThreadStartTime { get; set; }

            }
        }
        #endregion retry every 5 minutes for 3 days

        #region 数据上传接口
        /// <summary>
        /// 上传Environment Data
        /// </summary>
        /// <param name="Sequence">The sequence number of the child station reporting data. A seperate mapping list will be provided to Apple DRI to map the child sequence on any backend analytics</param>
        /// <param name="MeasurementTime"> Time the measurement was taken. This should be in ISO format with timezone offset</param>
        /// <param name="Value">Measurement value</param>
        /// <param name="Unit">Unit of measurement</param>
        /// <param name="Data">any machine data to log in addition to the event</param>
        public void EnvironmentalDataUpload(out string message, int Sequence, DateTime MeasurementTime, string Value, string Unit, Dictionary<string, object> Data = null, bool CreateFile = true)
        {
            HiveMgr.EnvironmentalData ed = new EnvironmentalData();

            ed.sequence = Sequence >= 0 ? Sequence : 0;
            ed.value = Value;
            ed.unit = Unit;
            ed.measurement_time = MeasurementTime.ToString(HiveMgr.TimeFormat);

            if (Data != null)
            {
                foreach (KeyValuePair<string, object> dia in Data)
                {
                    ed.addData(dia.Key, dia.Value);
                }
            }

            HiveMgr.GetInstance().UploadInformation(ed, out message, CreateFile);
        }


        /// <summary>
        /// PLC machinedata
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pass"></param>
        /// <param name="input_time"></param>
        /// <param name="output_time"></param>
        /// <param name="Serials"></param>
        /// <param name="Data"></param>
        /// <param name="unit_s"></param>
        /// <param name="UseLimit"></param>
        /// <param name="limits"></param>
        /// <param name="onlyJson"></param>
        public void PlcMachineDataUpload(out string message, string pass, string Input_Time, string output_time, Dictionary<string, object> Serials, Dictionary<string, object> Data, string unit_s = null, bool UseLimit = false, Dictionary<string, object> limits = null, bool onlyJson = false)
        {
            HiveMgr.MachineDataDefine md;


            if (!UseLimit)
            {
                md = new HiveMgr.MachineDataDefine();
            }
            else
            {
                md = new HiveMgr.MachineDataLimit();
                HiveMgr.MachineDataLimit md1 = new HiveMgr.MachineDataLimit();

                if (limits != null)
                {
                    foreach (KeyValuePair<string, object> dia in limits)
                    {
                        md1.addlimit(dia.Key, dia.Value);
                    }
                }
                md = md1;
            }


            md.unit_sn = unit_s == null ? Guid.NewGuid().ToString() : unit_s;

            string upperValue = pass.ToUpper();
            if (upperValue.Equals("TRUE") || upperValue.Equals("FALSE"))
            {
                md.pass = pass.ToLower();
            }

            //md.Coil1_InputTime = Coil1_InputTime.ToString(HiveMgr.TimeFormat);
            md.input_time = Input_Time;
            md.output_time = output_time;


            foreach (KeyValuePair<string, object> dia in Serials)
            {
                md.addSerial(dia.Key, dia.Value);
            }

            foreach (KeyValuePair<string, object> dia in Data)
            {
                md.addData(dia.Key, dia.Value);
            }

            #region onlyJson
            if (onlyJson)
            {
                #region 排序
                var _jobject = JObject.FromObject(md);
                var _newJObject = new JObject();
                //----------------------------------------------------------------------------------------------
                //unit_sn = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //serials = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //pass = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //input_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //output_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //data = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //limit = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                #endregion 排序               
                message = JsonConvert.SerializeObject(_newJObject);
                return;
            }
            #endregion

            HiveMgr.GetInstance().UploadInformation(md, out message);
        }

        /// <summary>
        /// Machine 数据上传（可以DIY data）
        /// </summary>
        /// <param name="pass"> true / false based on unit process</param>
        /// <param name="input_time"> the time of state change in local time. This should be in ISO format with timezone offset</param>
        /// <param name="output_time">the time the production finished. This should be in ISO format with timezone offset</param>
        /// <param name="Serials"> the mapping list of component serials being assembled in the current process</param>
        /// <param name="Data">any machine data to log in addition to the event</param>
        /// <param name="unit_s">the unit serial number of the unit being produced or randomly generated ID</param>
        public void MachineDataUpload(out string message, string pass, DateTime input_time, DateTime output_time, Dictionary<string, object> Serials, Dictionary<string, object> Data, string unit_s = null, bool UseLimit = false, Dictionary<string, object> limits = null, bool onlyJson = false)
        {
            HiveMgr.MachineDataDefine md;


            if (!UseLimit)
            {
                md = new HiveMgr.MachineDataDefine();
            }
            else
            {
                md = new HiveMgr.MachineDataLimit();
                HiveMgr.MachineDataLimit md1 = new HiveMgr.MachineDataLimit();

                if (limits != null)
                {
                    foreach (KeyValuePair<string, object> dia in limits)
                    {
                        md1.addlimit(dia.Key, dia.Value);
                    }
                }
                md = md1;
            }


            md.unit_sn = unit_s == null ? Guid.NewGuid().ToString() : unit_s;

            string upperValue = pass.ToUpper();
            if (upperValue.Equals("TRUE") || upperValue.Equals("FALSE"))
            {
                md.pass = pass.ToLower();
            }

            md.input_time = input_time.ToString(HiveMgr.TimeFormat);
            md.output_time = output_time.ToString(HiveMgr.TimeFormat);


            foreach (KeyValuePair<string, object> dia in Serials)
            {
                md.addSerial(dia.Key, dia.Value);
            }

            foreach (KeyValuePair<string, object> dia in Data)
            {
                md.addData(dia.Key, dia.Value);
            }



            if (onlyJson)
            {
                #region 排序
                var _jobject = JObject.FromObject(md);
                var _newJObject = new JObject();
                //----------------------------------------------------------------------------------------------
                //unit_sn = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //serials = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //pass = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //input_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //output_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //data = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //limit = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                #endregion 排序               
                message = JsonConvert.SerializeObject(_newJObject);
                return;
            }

            HiveMgr.GetInstance().UploadInformation(md, out message);
        }

        /// <summary>
        /// Machine 数据上传(带Activity)
        /// </summary>
        /// <param name="pass"> true / false based on unit process</param>
        /// <param name="input_time"> the time of state change in local time. This should be in ISO format with timezone offset</param>
        /// <param name="output_time">the time the production finished. This should be in ISO format with timezone offset</param>
        /// <param name="Serials"> the mapping list of component serials being assembled in the current process</param>
        /// <param name="ActivityTemp"> a list of activities that are performed during machine process</param>
        /// <param name="unit_s">the unit serial number of the unit being produced or randomly generated ID</param>
        public void MachineDataUpload(out string message, string pass, DateTime input_time, DateTime output_time, Dictionary<string, object> Serials, List<HiveMgr.MachineData.Activity> ActivityTemp = null, string unit_s = null, bool onlyJson = false)
        {
            HiveMgr.MachineData md = new HiveMgr.MachineData(ActivityTemp == null);
            md.unit_sn = unit_s == null ? Guid.NewGuid().ToString() : unit_s;

            string upperValue = pass.ToUpper();
            if (upperValue.Equals("TRUE") || upperValue.Equals("FALSE"))
            {
                md.pass = pass.ToLower();
            }

            md.input_time = input_time.ToString(HiveMgr.TimeFormat);
            md.output_time = output_time.ToString(HiveMgr.TimeFormat);


            foreach (KeyValuePair<string, object> dia in Serials)
            {
                md.addSerial(dia.Key, dia.Value);
            }

            if (ActivityTemp != null)
            {
                foreach (HiveMgr.MachineData.Activity Act in ActivityTemp)
                {
                    md.addActivity(Act);
                }
            }

            if (onlyJson)
            {
                message = JsonConvert.SerializeObject(md);
                return;
            }
            HiveMgr.GetInstance().UploadInformation(md, out message);
        }

        /// <summary>
        /// 上传机台状态，并记录该状态
        /// </summary>
        /// <param name="state">机台状态</param>
        public static void ChangeState(StateMessage state, out string message, bool CreateFile = true, string Time = null, int oldstate = 0, string SCR = "other", string sw_version = "V2.1")
        {
            HiveMgr.MachineStateData MS = new HiveMgr.MachineStateData();
            Hive_MachineState = state.MachineState;
            if (oldstate != 0)
            {
                MS.data.Add("sw_version", sw_version);
                MS.data.Add("previous_state", oldstate);
                MS.data.Add("State_change_reason", SCR);
            }

            if (state.MachineState == 5)
            {
                GetErrorInfo.GetInstance().WriteMachineStateXML(state, false);
                MS.data.Add("code", state.code);
                MS.data.Add("error_message", state.strMsg);
            }
            else
            {
                GetErrorInfo.GetInstance().WriteMachineStateXML(state, false);
            }

            MS.machine_state = (AutoFrame.HiveMgr.MachineStateData.MachineState)state.MachineState;
            if (Time == null)
            {
                MS.state_change_time = DateTime.Now.ToString(HiveMgr.TimeFormat);
            }
            else
            {
                MS.state_change_time = Convert.ToDateTime(Time).ToString(HiveMgr.TimeFormat);
            }

            HiveMgr.GetInstance().UploadInformation(MS, out message, CreateFile);

        }
        #endregion 数据上传接口

        #region Json格式和String格式互相转换方法
        /// <summary>
        /// Json转换成str格式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string Compress(string json)
        {
            StringBuilder sb = new StringBuilder();
            using (StringReader reader = new StringReader(json))
            {
                int ch = -1;
                int lastch = -1;
                bool isQuoteStart = false;
                while ((ch = reader.Read()) > -1)
                {
                    if ((char)lastch != '\\' && (char)ch == '\"')
                    {
                        if (!isQuoteStart)
                        {
                            isQuoteStart = true;
                        }
                        else
                        {
                            isQuoteStart = false;
                        }
                    }
                    if (!Char.IsWhiteSpace((char)ch) || isQuoteStart)
                    {
                        sb.Append((char)ch);
                    }
                    lastch = ch;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// str转换成Json格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region ShowLog
        /// <summary>
        /// 手动界面测试使用
        /// </summary>
        /// <param name="RTB">控件</param>
        /// <param name="message">数据</param>               
        public void showLog(System.Windows.Forms.RichTextBox RTB, string message, string OldeMS = "Idle", string NewMS = "Running", bool state = false)
        {
            string temp = ConvertJsonString(message);

            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Upload information to the hive system successfully!\r\n");
            if (state)
            {
                RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Machine state changes from《{OldeMS}》to《{NewMS}》\r\n");
            }
            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system：\r\n");
            RTB.AppendText(temp + "\r\n");
            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************" + "\r\n");
            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Hive returns:{Disconnect Hive system，Success machinestate logged}\r\n" + "\r\n");

            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Upload information to the hive system successfully!\r\n");
            if (state)
            {
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Machine state changes from《{OldeMS}》to《{NewMS}》\r\n");
            }
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
            SaveHiveLog(temp + "\r\n");
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************\r\n");
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Hive returns:{Disconnect Hive system，Success machinestate logged}\r\n\r\n");


        }

        /// <summary>
        /// 手动界面测试使用
        /// </summary>
        /// <param name="RTB">控件</param>
        /// <param name="message">数据</param>
        /// <param name="result">上传结果</param>
        /// <param name="returnResult">Hive返回数据</param>
        public void showLog(System.Windows.Forms.RichTextBox RTB, string message, bool result, string returnResult, string OldeMS = "", string NewMS = "")
        {
            string temp = ConvertJsonString(message);

            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload information to the hive system {(result ? "successfully" : "failed")}!\r\n");

            if (OldeMS != "" && NewMS != "" && result)
            {
                RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Machine state changes from《{OldeMS}》to《{NewMS}》\r\n");
            }

            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system：\r\n");
            RTB.AppendText(temp + "\r\n");
            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************" + "\r\n");
            if (returnResult == null || returnResult == "")
            {
                RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Connect Hive system failed" + "\r\n");
            }
            if (result)
            {
                RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Success machinestate logged" + "\r\n");
            }
            RTB.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive returns:{returnResult}\r\n" + "\r\n");


            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload information to the hive system {(result ? "successfully" : "failed")}!\r\n");
            if (OldeMS != "" && NewMS != "" && result)
            {
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Machine state changes from《{OldeMS}》to《{NewMS}》\r\n");
            }
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
            SaveHiveLog(temp + "\r\n");
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************\r\n");
            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive returns:{returnResult}\r\n" + "\r\n");
        }

        /// <summary>
        /// 正常作料使用
        /// </summary>        
        /// <param name="message">数据</param>
        /// <param name="result">上传结果</param>
        /// <param name="returnResult">Hive返回数据</param>
        public void showLog(string message, bool result, string returnResult, string OldeMS = "", string NewMS = "")
        {
            string temp = ConvertJsonString(message);

            lock (lock_showlog)
            {
                if (returnResult == null || returnResult == "")
                {
                    SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Connect Hive system failed" + "\r\n");
                }
                if (result)
                {
                    SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Success machinestate logged" + "\r\n");
                }

                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload information to the hive system {(result ? "successfully" : "failed")}!\r\n");
                if (OldeMS != "" && NewMS != "" && result)
                {
                    SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Machine state changes from《{OldeMS}》to《{NewMS}》\r\n");
                }
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
                SaveHiveLog(temp + "\r\n");
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************\r\n");
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive returns:{returnResult}\r\n" + "\r\n");
            }

        }

        #endregion ShowLog

        /// <summary>
        /// Json时间有效性防呆
        /// </summary>
        /// <param name="DS"></param>
        /// <param name="jsonFileName"></param>
        /// <returns>false:Json 时间异常</returns>
        private bool JSONTime(DataStyle DS, string jsonFileName)
        {
            try
            {
                string content = jsonFileName.ReadJsonContentFromFile(); //(post json)读取一个文件。
                JObject jo = JObject.Parse(content);
                switch (DS)
                {
                    case DataStyle.MachineData:
                        if (((DateTime.Now - Convert.ToDateTime(jo["output_time"].ToString())).TotalHours > 71) || ((DateTime.Now - Convert.ToDateTime(jo["input_time"].ToString())).TotalHours > 71))
                        {
                            return false;
                        }
                        break;
                    case DataStyle.MachineState:
                        if ((DateTime.Now - Convert.ToDateTime(jo["state_change_time"].ToString())).TotalHours > 71)
                        {
                            return false;
                        }
                        break;
                    case DataStyle.ErrorData:
                        if ((DateTime.Now - Convert.ToDateTime(jo["occurrence_time"].ToString())).TotalHours > 71)
                        {
                            return false;
                        }
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// A function do loop post and check outdate file
        /// </summary>
        private void postJsonFile(DataStyle DS)
        {
            string directory = CreateFilePath(DS);
            var _SendDataState = SendDataState.Instance;
            while (true)
            {
                if (!SystemMgr.GetInstance().GetParamBool("EnableUploadJson"))
                {
                    Thread.Sleep(200);
                    continue;

                }

                directory = CreateFilePath(DS);

                Thread.Sleep(50);

                string[] files = Directory.GetFiles(directory);


                string[] backfiles = BackUp_SendFlag[(int)DS - 1] ? Directory.GetFiles(directory.Replace("uploadfiles", "backup")) : null;

                if (backfiles == null && BackUp_SendFlag[(int)DS - 1])
                {
                    BackUp_SendFlag[(int)DS - 1] = false;
                    SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Upload has done,close {DS.ToString()}upload enable\r\n");
                }

                files = backfiles == null ? files : files.Concat(backfiles).ToArray();


                #region 正常上传，按时间排序
                Dictionary<string, DateTime> DI = new Dictionary<string, DateTime>();

                foreach (string temp in files)
                {
                    DateTime t = File.GetCreationTime(temp);

                    if (!BackUp_SendFlag[(int)DS - 1] && (_SendDataState.IsOverBindingTime(t, 24 * 3 - 0.1) || !JSONTime(DS, temp)))
                    {
                        string DF = SystemMgr.GetInstance().GetParamString("HiveDir");
                        DF = DF + "\\backup\\" + DS.ToString() + "\\";

                        if (!Directory.Exists(DF))
                        {
                            Directory.CreateDirectory(DF);

                        }
                        MoveFile(temp, temp.Replace("uploadfiles", "backup"), DS);
                        //File.Move(temp, temp.Replace("uploadfiles", "backup"));
                    }
                    else
                    {
                        DI.Add(temp, t);
                    }

                }
                var disSort = from objDic in DI orderby objDic.Value select new { objDic.Key };
                files = disSort.Select(n => n.Key).ToArray();
                #endregion 正常上传，按时间排序

                List<string> jsonFiles = new List<string>();
                foreach (string fileName in files)
                {
                    if (Path.GetExtension(fileName).Equals(".json"))
                    {
                        jsonFiles.Add(fileName);
                    }
                }

                bool result = false;
                foreach (string jsonFileName in jsonFiles)
                {
                    if (!_SendDataState.IsCanSend(DS))
                    {
                        Thread.Sleep(1000);
                        //SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Forbid to send because of the previous bad return from Hive server:\r\n");
                        continue;
                    }
                    string content = jsonFileName.ReadJsonContentFromFile(); //(post json)读取一个文件。
                    if (string.IsNullOrEmpty(content))//若未读取到内容，跳过此文件
                    {
                        continue;//跳过当前文档，
                    }
                    string responseString = "";
                    result = false;
                    //判断当前状态是否允许回传 如果不允许回传，直接跳出当前 foreach 循环
                    if (!SystemMgr.GetInstance().GetParamBool("EnableUploadJson"))
                    {
                        Thread.Sleep(1000);
                        break;
                    }
                    //post data
                    result = this.post(SystemMgr.GetInstance().GetParamString($"{DS}Url"), content, ref responseString);

                    if (!result)
                    {
                        SystemMgr.GetInstance().WriteRegBit((int)SysBitReg.bit_Hive连接失败, true);
                        if (Form_MessageFirst)
                        {
                            Form_MessageFirst = false;
                            Form_MessageBeginTime = DateTime.Now;
                            Form_HiveMessage frm = new Form_HiveMessage("Hive数据上传失败，请检查Hive系统连接是否正常");
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.TopLevel = true;
                            frm.ShowDialog();
                        }
                        else if ((DateTime.Now - Form_MessageBeginTime).TotalMinutes > 30)
                        {
                            Form_MessageBeginTime = DateTime.Now;
                            Form_HiveMessage frm = new Form_HiveMessage("Hive数据上传失败，请检查Hive系统连接是否正常");
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.TopLevel = true;
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        Form_MessageFirst = true;
                    }

                    showLog(content, result, responseString);
                    HiveServerModel hsm = responseString.GetHiveServerModel();
                    /*pan*/
                    if (result && hsm != null)
                    {
                        SystemMgr.GetInstance().WriteRegBit((int)SysBitReg.bit_Hive连接失败, false);
                        if (hsm.Status.ToLower() == "error")
                        {
                            string DF = SystemMgr.GetInstance().GetParamString("HiveDir");
                            DF = DF + "\\code4files\\" + DS.ToString() + "\\";

                            if (!Directory.Exists(DF))
                            {
                                Directory.CreateDirectory(DF);

                            }
                            //try
                            //{
                            //    lock (lock_movefile)
                            //    {
                            //        File.Move(jsonFileName, DF + Path.GetFileName(jsonFileName));
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"{DS.ToString()}|{jsonFileName}Move异常：{ex.ToString()}");
                            //}
                            MoveFile(jsonFileName, DF + Path.GetFileName(jsonFileName), DS);


                        }
                        else//success
                        {
                            try
                            {
                                File.Delete(jsonFileName);
                            }
                            catch
                            {

                            }

                        }

                    }
                    else
                    {
                        //网络异常


                        _SendDataState.DelayTime(1000 * 60 * 5, DS);

                        lock (lock_1)
                        {
                            SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"{DS.ToString()} data will post again after 5 minutes!\r\n\r\n");
                            //SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Fail to upload to Hive system!\r\n");
                            //SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "The data send from IPC to Hive system:\r\n");
                            //SaveHiveLog(ConvertJsonString(content) + "\r\n");
                            //SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "**************************************\r\n");

                            //SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"Hive return:{{Disconnect Hive system，Success {DS.ToString().ToLower()} logged}}\r\n\r\n");
                        }



                        // SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Delay to send because of sending failed to Hive server:\r\n");

                    }



                    //if (result)
                    //{
                    //                           //File.Delete(jsonFileName);
                    //    //File.Move(jsonFileName, bakDirectory + Path.GetFileName(jsonFileName));
                    //}
                    //else if ((DateTime.Now - File.GetCreationTime(jsonFileName)).TotalDays > 3)
                    //{
                    //    File.Delete(jsonFileName);                       
                    //}
                }
                Thread.Sleep(100);
            }
        }

        public void MoveFile(string oldPath, string newPath, DataStyle DS)
        {
            try
            {
                lock (lock_movefile)
                {
                    File.Move(oldPath, newPath);
                }
            }
            catch (Exception ex)
            {
                SaveHiveLog($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + $"{DS.ToString()}|{oldPath}移动异常：{ex.ToString()}\r\n");
            }

        }


        /// <summary>
        /// Upload to HIVE server by http request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="responseString"></param>
        /// <returns></returns>  
        public bool post(string url, string postData, ref string responseString)
        {

            #region restsharp
            try
            {
                var client = new RestClient(url);
                client.Timeout = 1000 * 10;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", postData, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                responseString = response.Content;
                if (responseString == "")
                {
                    responseString = response.ErrorMessage;
                    return false;
                }


            }
            catch (Exception ex)
            {
                responseString = ex.ToString();
                return false;
            }

            #endregion
            #region 注释1
            //    try
            //    {
            //        var request = (HttpWebRequest)WebRequest.Create(url);

            //        var data = Encoding.ASCII.GetBytes(postData);
            //        request.Method = "POST";
            //        request.ContentType = "application/json";
            //        request.ContentLength = data.Length;



            //        using (var stream = request.GetRequestStream())
            //        {
            //            stream.Write(data, 0, data.Length);
            //        }

            //        var response = (HttpWebResponse)request.GetResponse();

            //        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            //    }
            //    catch (Exception ex)
            //    {
            //        responseString = ex.ToString();
            //        return false;
            //    }
            //}
            #endregion


            return true;
        }

        /// <summary>
        /// Create json file as data or state
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message">返回的Json信息</param>
        /// <param name="CreateFile">是否在本地生成Json（手动测试时，置false）</param>
        public void UploadInformation(object obj, out string message, bool CreateFile = true)
        {
            //if (!SystemMgr.GetInstance().GetParamBool("EnableUploadJson"))
            //{
            //    message = null;
            //    Thread.Sleep(50);
            //    return;
            //}


            //WarningMgr.GetInstance().Info("Hive save json file.");
            Type type = obj.GetType();
            string jsonss = null;

            string fileName = "";

            if (type.Equals(typeof(MachineData)))
            {
                MachineData mData = (MachineData)obj;
                fileName = CreateFilePath(DataStyle.MachineData) + System.Guid.NewGuid().ToString() + ".json";
                //string str1 = DateTime.Now.GetDateTimeFormats()[137];
                jsonss = JsonConvert.SerializeObject(mData);
                //File.AppendAllText(fileName, jsonss);
            }
            else if (type.Equals(typeof(MachineDataDefine)))
            {
                MachineDataDefine mData = (MachineDataDefine)obj;
                fileName = CreateFilePath(DataStyle.MachineData) + System.Guid.NewGuid().ToString() + ".json";
                jsonss = JsonConvert.SerializeObject(mData);
                //File.AppendAllText(fileName, jsonss);
            }
            else if (type.Equals(typeof(MachineDataLimit)))
            {
                MachineDataLimit mData = (MachineDataLimit)obj;
                fileName = CreateFilePath(DataStyle.MachineData) + System.Guid.NewGuid().ToString() + ".json";
                #region 排序
                var _jobject = JObject.FromObject(mData);
                var _newJObject = new JObject();
                //----------------------------------------------------------------------------------------------
                //unit_sn = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("unit_sn").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //serials = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("serials").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //pass = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("pass").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //input_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("input_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //output_time = string.Empty;
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("output_time").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //data = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("data").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                //limit = new Dictionary<string, object>();
                if (_jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList().Count > 0)
                {
                    var Propertys = _jobject.Properties().ToList().Where(n => n.Name.ToLower().Trim() == ("limit").ToLower().Trim()).ToList();
                    if (Propertys == null || Propertys.Count < 1) { }
                    else
                    {
                        _newJObject.Add(Propertys[0].Name, Propertys[0].Value);
                    }
                }
                #endregion 排序
                jsonss = JsonConvert.SerializeObject(_newJObject);
                //File.AppendAllText(fileName, jsonss);
            }
            else if (type.Equals(typeof(MachineStateData)))
            {
                MachineStateData msData = (MachineStateData)obj;
                fileName = CreateFilePath(DataStyle.MachineState) + /*msData.Guid*/ System.Guid.NewGuid().ToString() + ".json";
                jsonss = JsonConvert.SerializeObject(msData);
                //File.AppendAllText(fileName, jsonss);
                //fileName = directoryDB + System.Guid.NewGuid().ToString() + ".json";
                //jsonss = JsonConvert.SerializeObject(msData);
                //File.AppendAllText(fileName, jsonss);
            }
            else if (type.Equals(typeof(ErrorMessage)))
            {
                ErrorMessage eData = (ErrorMessage)obj;
                ErrorData temp = new ErrorData(eData.code);
                temp.message = eData.strMsgEnglish;
                temp.code = eData.code;
                temp.occurrence_time = eData.tm_begin.ToString(HiveMgr.TimeFormat);
                temp.resolved_time = eData.tm_end.ToString(HiveMgr.TimeFormat);
                temp.severity = eData.Level;

                //排查code 是否是null 或者空的情况
                if (string.IsNullOrEmpty(eData.code))
                {
                    message = "";
                    return;
                }
                fileName = CreateFilePath(DataStyle.ErrorData) + System.Guid.NewGuid().ToString() + ".json";
                jsonss = JsonConvert.SerializeObject(temp);
                //File.AppendAllText(fileName, jsonss);
            }
            else if (type.Equals(typeof(EnvironmentalData)))
            {
                EnvironmentalData eData = (EnvironmentalData)obj;

                fileName = CreateFilePath(DataStyle.EnvironmentData) + System.Guid.NewGuid().ToString() + ".json";
                jsonss = JsonConvert.SerializeObject(eData);
                //File.AppendAllText(fileName, jsonss);
            }
            else
            {
                message = jsonss;
                return;
            }

            if (CreateFile)
            {
                File.AppendAllText(fileName, jsonss);
            }

            message = jsonss;
        }

        /// <summary>
        /// 获取数据保存路径 (1：MachineData  2：MachineState  3：ErrorData 4:EnvironmentData)
        /// </summary>
        /// <param name="index">1：MachineData  2：MachineState  3：ErrorData 4:EnvironmentData</param>
        /// <returns></returns>
        public string CreateFilePath(DataStyle index)
        {
            string hiveDirectory = SystemMgr.GetInstance().GetParamString("HiveDir");
            string directory = SystemMgr.GetInstance().GetParamString("HiveDir");

            if (hiveDirectory.LastIndexOf("\\") != directory.Length - 1)
                hiveDirectory += "\\";

            if (PostMgr.PostPause)
            {
                directory = hiveDirectory + "uploadfiles\\";
            }
            else
            {

                directory = hiveDirectory + "uploadfilespause\\";

            }

            switch (index)
            {
                case DataStyle.MachineData:
                    directory = directory + "machinedata\\";
                    break;
                case DataStyle.MachineState:
                    directory = directory + "machinestate\\";
                    break;
                case DataStyle.ErrorData:
                    directory = directory + "errordata\\";
                    break;
                default:
                    directory = directory + "environmentdata\\";
                    break;
            }

            //检测文件夹是否存在，如果不存在创建一个该文件夹
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }
        private Dictionary<string, object> JsonStringToDictionary(string json)
        {
            Dictionary<string, object> dict;
            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                //WarningMgr.GetInstance().Info(json + " to dictionary exception!");
                //WarningMgr.GetInstance().Info(ex.ToString());
                return null;
            }
            return dict;
        }

        #region 4种数据类型

        #region Machine data
        /// <summary>
        /// 运行数据（带limit）
        /// </summary>
        public class MachineDataLimit : MachineDataDefine
        {
            //[JsonProperty]
            public Dictionary<string, object> limit { get; set; }

            public MachineDataLimit()
            {
                unit_sn = string.Empty;
                serials = new Dictionary<string, object>();
                pass = string.Empty;
                input_time = string.Empty;
                output_time = string.Empty;
                data = new Dictionary<string, object>();
                limit = new Dictionary<string, object>();
            }

            public void addlimit(string name, object serial)
            {
                if (!limit.Keys.Contains(name))
                    limit.Add(name, serial);
            }

        }

        /// <summary>
        /// 运行数据（可以用来DIY data）
        /// </summary>
        public class MachineDataDefine
        {
            public string unit_sn { get; set; }
            public Dictionary<string, object> serials { get; set; }
            public string pass { get; set; }
            public string input_time { get; set; }
            public string output_time { get; set; }
            public Dictionary<string, object> data { get; set; }

            private string guid { get; set; }

            public MachineDataDefine()
            {
                guid = System.Guid.NewGuid().ToString();
                unit_sn = string.Empty;
                serials = new Dictionary<string, object>();
                pass = string.Empty;
                input_time = string.Empty;
                output_time = string.Empty;
                data = new Dictionary<string, object>();

            }



            public void addSerial(string name, object serial)
            {
                if (!serials.Keys.Contains(name))
                    serials.Add(name, serial);
            }

            public void addData(string name, object serial)
            {
                if (!data.Keys.Contains(name))
                    data.Add(name, serial);
            }

            public Dictionary<string, object> ToDictionary()
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                return dic;
            }
        }

        /// <summary>
        /// 运行数据(带活动)
        /// </summary>
        public class MachineData
        {
            public string unit_sn { get; set; }
            public Dictionary<string, object> serials { get; set; }
            public string pass { get; set; }
            public string input_time { get; set; }
            public string output_time { get; set; }
            public SubData data { get; set; }

            private string guid { get; set; }

            public MachineData()
            {
                guid = System.Guid.NewGuid().ToString();
                unit_sn = string.Empty;
                serials = new Dictionary<string, object>();
                pass = string.Empty;
                input_time = string.Empty;
                output_time = string.Empty;
                data = new SubData();
            }

            public MachineData(bool actEnable)
            {
                guid = System.Guid.NewGuid().ToString();
                unit_sn = string.Empty;
                serials = new Dictionary<string, object>();
                pass = string.Empty;
                input_time = string.Empty;
                output_time = string.Empty;

                if (!actEnable)
                {
                    data = new SubData();
                }
            }


            public void addSerial(string name, object serial)
            {
                if (!serials.Keys.Contains(name))
                    serials.Add(name, serial);
            }

            public void addActivity(Activity activity)
            {
                data.activities.Add(activity);
            }

            public Dictionary<string, object> ToDictionary()
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                return dic;
            }

            public class SubData
            {
                private string swversion = "";
                public List<Activity> activities;


                public string sw_version
                {
                    get
                    {
                        return swversion;
                    }
                    set
                    {
                        if (value.Trim().Length > 0)
                            swversion = value;
                    }
                }


                public SubData()
                {
                    activities = new List<Activity>();
                }
            }

            public class Activity
            {
                public string stage = "";
                private string startTime = "";
                private string endTime = "";
                public string activity = "";

                public string start_time
                {
                    get
                    {
                        return startTime;
                    }
                    set
                    {
                        DateTime result;
                        if (DateTime.TryParse(value, out result))
                        {
                            startTime = result.ToString(HiveMgr.TimeFormat);
                        }
                    }
                }

                public string end_time
                {
                    get
                    {
                        return endTime;
                    }
                    set
                    {
                        DateTime result;
                        if (DateTime.TryParse(value, out result))
                        {
                            endTime = result.ToString(HiveMgr.TimeFormat);
                        }
                    }
                }

            }
        }
        #endregion Machine data

        #region Error data
        /// <summary>
        /// 故障数据
        /// </summary>
        public class ErrorData
        {
            //public ErrorSeverity severity = ErrorSeverity.error;
            public string message = null;
            public string code = null;
            public string severity = null;
            private string occurrence_Time = "";
            private string resolved_Time = "";
            public Dictionary<string, string> data;
            private string guid;

            //public string Guid
            //{
            //    get
            //    {
            //        return this.guid;
            //    }
            //}


            public string occurrence_time
            {
                get
                {
                    return occurrence_Time;
                }
                set
                {
                    DateTime result;
                    if (DateTime.TryParse(value, out result))
                    {
                        occurrence_Time = result.ToString(HiveMgr.TimeFormat);
                    }
                }
            }

            public string resolved_time
            {
                get
                {
                    return resolved_Time;
                }
                set
                {
                    DateTime result;
                    if (DateTime.TryParse(value, out result))
                    {
                        resolved_Time = result.ToString(HiveMgr.TimeFormat);
                    }
                }
            }

            public ErrorData(string code)
            {
                this.code = code;
                guid = System.Guid.NewGuid().ToString();
                if (data == null)
                    data = new Dictionary<string, string>();
            }

            public ErrorData(ErrorType errorType, ErrorComponent component, ErrorSubComponent subComponent, int errorIndex, RepairAction repairAction, params string[] args)
            {
                string errorInfo = string.Format("Create new error [ErrorType:{0},ErrorComponent:{1},ErrorSubComponent:{2},ErrorIndex:{3},RepairAction:{4}", errorType.ToString(), component.ToString(), subComponent.ToString(), errorIndex, repairAction.ToString());
                ////WarningMgr.GetInstance().Info(errorInfo);
                StringBuilder code = new StringBuilder("");
                int radixType = (int)errorType / 100;
                string typeCode = ((int)errorType % 100).ToString("00");
                switch (radixType)
                {
                    case 0:
                        code.Append("M");
                        break;
                    case 1:
                        code.Append("C");
                        break;
                    case 2:
                        code.Append("U");
                        break;
                    case 3:
                        code.Append("S");
                        break;
                    case 4:
                        code.Append("L");
                        break;
                    case 5:
                        code.Append("W");
                        break;
                    case 6:
                        code.Append("N");
                        break;
                    case 7:
                        code.Append("V");
                        break;
                    case 8:
                        code.Append("T");
                        break;
                    case 9:
                        code.Append("F");
                        break;
                    case 10:
                        code.Append("E");
                        break;
                    case 11:
                        code.Append("O");
                        break;
                }
                code.Append(typeCode);
                code.Append(((ErrorComponentCode)((int)component)).ToString());
                string subComponentCode = ((ErrorSubComponentCode)((int)subComponent)).ToString();
                if (subComponentCode.Length == 1)
                {
                    subComponentCode += args.Length > 0 ? args[0] : "1";
                }
                code.Append(subComponentCode);
                code.Append("-" + errorIndex.ToString("00"));

                if ((int)repairAction != 0)
                    code.Append("-" + ((int)repairAction).ToString("00"));
                this.code = code.ToString();
                if (data == null)
                    data = new Dictionary<string, string>();
                guid = System.Guid.NewGuid().ToString();
            }



            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public Dictionary<string, object> ToDictionary()
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(this.ToString());
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(jo.ToString());
            }



            public enum ErrorSeverity
            {
                warning = 1,
                error = 2,
                critical = 3,
            }

            public enum ErrorType
            {
                Motion_MotionOrRobotOverload = 001,
                Motion_NotInPosition = 002,
                Motion_Timeout = 003,
                Motion_NegativePosition = 004,
                Motion_PositivePosition = 005,
                Motion_MotorEnableError = 006,
                Motion_CommunicationError = 007,
                Motion_MaxAttemptsReached = 008,
                Motion_Other = 099,

                PneumaticCylinder_NegativePosition = 101,
                PneumaticCylinder_PositivePosition = 102,
                PneumaticCylinder_CommunicationError = 103,
                PneumaticCylinder_AirPressureOrAirFlowError = 104,
                PneumaticCylinder_Other = 199,

                Vacuum_VacuumAlarm = 201,
                Vacuum_Other = 299,

                Sensor_LoadCellForceAbnormal = 301,
                Sensor_SensorDetectedOrNotDetected = 302,
                Sensor_TemperatureAlarm = 303,
                Sensor_CommunicationError = 304,
                Sensor_Other = 399,

                Laser_LaserNotEnabled = 401,
                Laser_AbnormalData = 402,
                Laser_CommunicationError = 403,
                Laser_Other = 499,

                Screwing_TorqueAlarm = 501,
                Screwing_CycleAlarm = 502,
                Screwing_CommunicationError = 503,
                Screwing_Other = 599,

                SoftwareRelated_ValueOutOfRange = 601,
                SoftwareRelated_DataSaveFailure = 602,
                SoftwareRelated_CommunicationError = 603,
                SoftwareRelated_Other = 699,

                Vision_UnableToFindPattern = 701,
                Vision_AlignmentError = 702,
                Vision_CommunicationError = 703,
                Vision_Other = 704,

                Scanning_BarcodeScanError = 801,
                Scanning_TypeError = 802,
                Scanning_ImproperSerialNumberlength = 803,
                Scanning_MaterialMatchError = 804,
                Scanning_ProcessError = 805,
                Scanning_CommunicationError = 806,
                Scanning_Other = 807,

                Safety_E_StopPressed = 901,
                Safety_LightCurtainOrDoorAlarm = 902,
                Safety_Other = 999,

                MaterialShortage_OutOfMaterial = 1000,
                MaterialShortage_AlmostOutOfMaterialWarning = 1001,
                MaterialShortage_Other = 1099,

                Other_Other = 1199,
            }

            public enum ErrorComponent
            {
                Conveyor = 1,
                Dispense = 2,
                Electrice = 3,
                EndEffector = 4,
                EmergencyButtons = 5,
                Gantry = 6,
                IPCOrComputer = 7,
                LoadCell = 8,
                Loader = 9,
                PLC = 10,
                Pneumatics = 11,
                Press = 12,
                Robot = 13,
                SafetyInterlockOrDoorOrBarrier = 14,
                SensorOrSignal = 15,
                Unloader = 16,
                VacuumGenerator = 17,
                CCDOrOtherVisionSystem = 18,
                Other = 19,
            }

            private enum ErrorComponentCode
            {
                CV = 1,
                DS = 2,
                EE = 3,
                EF = 4,
                ES = 5,
                GA = 6,
                IP = 7,
                LC = 8,
                LO = 9,
                PL = 10,
                PN = 11,
                PS = 12,
                RB = 13,
                SC = 14,
                SS = 15,
                UL = 16,
                VG = 17,
                VS = 18,
                OO = 19,
            }

            public enum ErrorSubComponent
            {
                Belts = 1,
                Axis = 2,
                Bolt = 3,
                Bracket = 4,
                Cables = 5,
                ConnectorElectrical = 6,
                Clamp = 7,
                Coupler = 8,
                ConnectorPneumatic = 9,
                Cylinder = 10,
                DriverElectrical = 11,
                DriverRobot = 12,
                DriverVision = 13,
                HolderOrNestGeneral = 14,
                Mechanicanism = 15,
                Motor = 16,
                MechanicalPart = 17,
                Needle = 18,
                Nozzle = 19,
                PLCCable = 20,
                PLCConnector = 21,
                PLCCard = 22,
                Rollers = 23,
                RobotTool = 24,
                Screws = 25,
                SensorElectrical = 26,
                SensorFlow = 27,
                Shim = 28,
                SensorLoad_cell = 29,
                Stopper = 30,
                SensorPositioning = 31,
                SensorPressure = 32,
                Tubes = 33,
                VaveDirectional = 34,
                ValvePressure = 35,
                ValveVacuum = 36,
                Peeler = 37,
                Other = 38,
            }

            private enum ErrorSubComponentCode
            {
                BL = 1,
                A = 2,
                BO = 3,
                BR = 4,
                CA = 5,
                CE = 6,
                CL = 7,
                CO = 8,
                CP = 9,
                CY = 10,
                DE = 11,
                DR = 12,
                DV = 13,
                H = 14,
                ME = 15,
                M = 16,
                MP = 17,
                NE = 18,
                NO = 19,
                PC = 20,
                PO = 21,
                PR = 22,
                RO = 23,
                RT = 24,
                SC = 25,
                SE = 26,
                SF = 27,
                SH = 28,
                SL = 29,
                SO = 30,
                SP = 31,
                SR = 32,
                TU = 33,
                VD = 34,
                VP = 35,
                VV = 36,
                PL = 37,
                OO = 38,
            }

            public enum RepairAction
            {
                NoAction = 0,
                Adjusted = 1,
                Calibrated = 2,
                Checked = 3,
                Cleaned = 4,
                Erased = 5,
                Filled = 6,
                Fixed = 7,
                Lubricated = 8,
                Replaced = 9,
                Restored = 10,
                Stretched = 11,
                Tightened = 12,
                Trained = 13,
                Tuned = 14,
                Other = 15,
            }
        }
        #endregion Error data

        #region Machine state data
        /// <summary>
        /// 状态数据
        /// </summary>
        public class MachineStateData
        {
            public MachineState machine_state = MachineState.UnplannedDown;
            public string state_change_time = "";
            public Dictionary<string, object> data = null;
            private string guid = "";

            //public string Guid
            //{
            //    get
            //    {
            //        return guid;
            //    }
            //}

            public MachineStateData()
            {
                guid = System.Guid.NewGuid().ToString();
                data = new Dictionary<string, object>();
            }

            public MachineStateData(string guid)
            {
                this.guid = guid;
                data = new Dictionary<string, object>();
            }

            public Dictionary<string, object> ToDictionary()
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("machine_state", (int)machine_state);
                dict.Add("state_change_time", state_change_time);
                dict.Add("guid", guid);
                dict.Add("data", data);

                return dict;
            }



            public enum MachineState
            {
                Running = 1,
                Idle = 2,
                Engineering = 3,
                PlannedDown = 4,
                UnplannedDown = 5,
            }

        }
        #endregion Machine state data

        #region Evvironment data
        public class EnvironmentalData
        {
            public int sequence;
            public string measurement_time = "";
            public string value;
            public string unit;
            public Dictionary<string, object> data = null;
            private string guid = "";


            public EnvironmentalData()
            {
                guid = System.Guid.NewGuid().ToString();
                data = new Dictionary<string, object>();
            }

            public void addData(string name, object serial)
            {
                if (!data.Keys.Contains(name))
                    data.Add(name, serial);
            }

        }
        #endregion Evvironment data

        #endregion 4种数据类型


    }
    /// <summary>
    /// 公用方法扩展类
    /// </summary>
    public static class HiveMgrCommonEx
    {
        /// <summary>
        /// 读取一个json文档
        /// </summary>
        /// <param name="filepath">文档路径</param>
        /// <returns></returns>
        public static string ReadJsonContentFromFile(this string filepath)
        {
            try
            {
                return File.ReadAllText(filepath);
            }
            catch (Exception ex)
            {
                //可以将ex输出到hivelog日志，已提供查看
                return "";
            }
        }
    }

    /// <summary>
    /// hive 服务器返回信息的model
    /// </summary>
    [Serializable]
    public class HiveServerModel
    {
        //  ErrorCode
        //      ErrorText
        //      ErrorValidation
        //pu      Status
        /// <summary>
        /// 错误编码
        /// </summary>
        public Object ErrorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorText { get; set; }
        /// <summary>
        /// 错误校验明细
        /// </summary>
        public object ErrorValidation { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// hive 属性扩展
    /// </summary>
    public static class HiveMgrEx
    {
        /// <summary>
        /// json字符串转换为HiveServerModel
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static HiveServerModel GetHiveServerModel(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<HiveServerModel>(json);
            }
            catch (Exception ex)
            {
                //错误信息需要添加到界面展示
                return null;
            }

        }
    }


}
