//2019-05-22 Binggoo 1.完善自动化测试，加入开始测试事件
//                   2.运动控制加入上电和断电命令
//2019-05-28 Binggoo 1.完善自动化测试，加入限制时间和错误信息。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using ToolEx;
using System.Windows.Forms;
using AutoFrameDll;
using System.Threading;
using System.Xml;
using VTemplate.Engine;
using System.IO;

namespace AutoFrame
{
    public enum TestType
    {
        Cylinder,
        DI,
        DO,
        AxisMove,
    }

    public enum CylinderStatus
    {
        Out,
        Back,
    }

    public enum SignalStatus
    {
        Off,
        On,
    }

    public enum AxisMotion
    {
        On,
        Off,
        Home,
        PEL,
        MEL,
    }

    public class TestItem
    {
        /// <summary>
        /// 测试类型
        /// </summary>
        public TestType Type { get; set; }

        /// <summary>
        /// 测试名称
        /// </summary>
        public TValue Name { get; set; }

        /// <summary>
        /// 测试描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作模式
        /// </summary>
        public int OperationMode { get; set; }

        /// <summary>
        /// 操作模式
        /// </summary>
        public string Mode
        {
            get
            {
                string strMode = "";
                switch(Type)
                {
                    case TestType.Cylinder:
                        strMode = ((CylinderStatus)OperationMode).ToString();
                        break;
                    case TestType.DI:
                    case TestType.DO:
                        strMode = ((SignalStatus)OperationMode).ToString();
                        break;

                    case TestType.AxisMove:
                        strMode = ((AxisMotion)OperationMode).ToString();
                        break;
                }

                return strMode;
           }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime m_dtStartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime m_dtEndTime;

        /// <summary>
        /// 花费时间
        /// </summary>
        public double UsedTimeS
        {
            get
            {
                return (m_dtEndTime - m_dtStartTime).TotalSeconds;
            }
        }

        /// <summary>
        /// 测试结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 测试得分
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 限制时间
        /// </summary>
        public double LimitTimeS { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool Run(StationEx station)
        {
            bool bRet = true;
            Message = "";
            switch (Type)
            {
                case TestType.Cylinder:
                    {
                        string strOperation = "";
                        int nRet = 0;
                        m_dtStartTime = DateTime.Now;

                        if (OperationMode == (int)CylinderStatus.Out)
                        {
                            //伸出
                            strOperation = " Out ";
                            nRet = station.CylOut(Name.S);
                        }
                        else
                        {
                            //缩回
                            strOperation = " Back ";
                            nRet = station.CylBack(Name.S);
                        }

                        m_dtEndTime = DateTime.Now;

                        bRet = nRet == 0;

                        string str1 = "执行气缸动作{0}{1}完成，花费时间{2}s";
                        string str2 = "执行气缸动作{0}{1}超时";
                        string strName = Name.S;
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            str1 = "Execution of the cylinder action {0} {1} completed, taking {2} seconds";
                            str2 = "Execute cylinder action {0} {1} timeout";
                            strName = CylinderMgr.GetInstance().GetCylTranslate(strName);
                        }

                        if (bRet)
                        {
                            if (UsedTimeS > LimitTimeS)
                            {
                                Score = 1;
                            }
                            else
                            {
                                Score = 2;
                            }
                            station.ShowLog(string.Format(str1, strName, strOperation, UsedTimeS.ToString("F3")));    
                        }
                        else
                        {
                            Score = 0;

                            //错误信息，获取气缸的IO点
                            Cylinder cyl = CylinderMgr.GetInstance().GetCyLinder(Name.S);
                            if (OperationMode == (int)CylinderStatus.Out)
                            {
                                long num = 0;
                                //伸出
                                if (cyl.m_CylType == CylType.Single)
                                {
                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[0]];

                                    Message += String.Format("Check DO:{0}.{1}=On;", num >> 8,num & 0xFF);
                                }
                                else
                                {
                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[0]];
                                    Message += String.Format("Check DO:{0}.{1}=On;", num >> 8, num & 0xFF);

                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[1]];
                                    Message += String.Format("DO:{0}.{1}=Off;", num >> 8, num & 0xFF);
                                }

                                if (cyl.m_bEnableIns[0] && cyl.m_strIoIns[0].Length > 0)
                                {
                                    num = IoMgr.GetInstance().m_dicIn[cyl.m_strIoIns[0]];
                                    Message += String.Format("DI:{0}.{1}=On;", num >> 8, num & 0xFF);
                                }

                                if (cyl.m_bEnableIns[1] && cyl.m_strIoIns[1].Length > 0)
                                {
                                    num = IoMgr.GetInstance().m_dicIn[cyl.m_strIoIns[1]];
                                    Message += String.Format("DI:{0}.{1}=Off;", num >> 8, num & 0xFF);
                                }

                            }
                            else
                            {
                                //缩回
                                long num = 0;
                                if (cyl.m_CylType == CylType.Single)
                                {
                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[0]];

                                    Message += String.Format("Check DO:{0}.{1}=Off;", num >> 8, num & 0xFF);
                                }
                                else
                                {
                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[0]];
                                    Message += String.Format("Check DO:{0}.{1}=Off;", num >> 8, num & 0xFF);

                                    num = IoMgr.GetInstance().m_dicOut[cyl.m_strIoOuts[1]];
                                    Message += String.Format("DO:{0}.{1}=On;", num >> 8, num & 0xFF);
                                }

                                if (cyl.m_bEnableIns[0] && cyl.m_strIoIns[0].Length > 0)
                                {
                                    num = IoMgr.GetInstance().m_dicIn[cyl.m_strIoIns[0]];
                                    Message += String.Format("DI:{0}.{1}=Off", num >> 8, num & 0xFF);
                                }

                                if (cyl.m_bEnableIns[1] && cyl.m_strIoIns[1].Length > 0)
                                {
                                    num = IoMgr.GetInstance().m_dicIn[cyl.m_strIoIns[1]];
                                    Message += String.Format("DI:{0}.{1}=On;", num >> 8, num & 0xFF);
                                }
                            }

                            //超时
                            station.ShowLog(string.Format(str2, strName, strOperation), LogLevel.Error);
                        }
                    }    
                    break;

                case TestType.DI:
                    {
                        string strName = Name.S;
                        string str1 = "等待DI:{0} = {1}";
                        string str2 = "等待DI:{0} = {1} 完成，花费时间{2}s";
                        string str3 = "等待DI:{0} = {1}超时";
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            strName = IoMgr.GetInstance().GetIoInTranslate(strName);
                            str1 = "Waiting for DI: {0} = {1}";
                            str2 = "Waiting for DI: {0} = {1} to complete, taking {2} seconds";
                            str3 = "Waiting for DI: {0} = {1} timed out";
                        }

                        int nRet = 0;
                        m_dtStartTime = DateTime.Now;
                        station.ShowLog(string.Format(str1, strName, OperationMode == 1 ? "On" : "Off"));
                        nRet = station.WaitIo(Name.S, OperationMode == 1,(int)LimitTimeS);
                        
                        m_dtEndTime = DateTime.Now;

                        bRet = nRet == 0;

                        if (bRet)
                        {
                            if (UsedTimeS > LimitTimeS)
                            {
                                Score = 0.5;
                            }
                            else
                            {
                                Score = 1;
                            }

                            station.ShowLog(string.Format(str2, strName, OperationMode == 1 ? "On" : "Off",UsedTimeS.ToString("F3")));
                        }
                        else
                        {
                            Score = 0;

                            //错误信息，获取IO点
                            long num = IoMgr.GetInstance().m_dicIn[Name.S];

                            Message = string.Format("Check DI:{0}.{1}={2};", num >> 8, num & 0xFF, OperationMode == 1 ? "On" : "Off");

                            //超时
                            station.ShowLog(string.Format(str3, strName, OperationMode == 1 ? "On" : "Off"));
                        }
                    }
                    break;

                case TestType.DO:
                    {
                        string strName = Name.S;
                        string str1 = "执行DO:{0} = {1}，等待确认";
                        string str2 = "执行DO:{0} = {1},请确认是否OK?";
                        string str3 = "执行DO:{0} = {1} 完成，花费时间{2}s";
                        string str4 = "执行DO:{0} = {1} NG";
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            strName = IoMgr.GetInstance().GetIoOutTranslate(strName);
                            str1 = "Execute DO: {0} = {1}, waiting for confirmation";
                            str2 = "Execute DO: {0} = {1}, are you sure you are OK?";
                            str3 = "DO execution: {0} = {1} complete, time {2} seconds";
                            str4 = "Execute DO: {0} = {1} NG";
                        }

                        //弹出对话框手动确认是否OK
                        m_dtStartTime = DateTime.Now;
                        station.ShowLog(string.Format(str1, strName, OperationMode == 1 ? "On" : "Off"));

                        while (true)
                        {
                            IoMgr.GetInstance().WriteIoBit(Name.S, OperationMode == 1);

                            DialogResult result = 
                                Form_Msg.ShowMessage(string.Format(str2, strName, OperationMode == 1 ? "On" : "Off") , "提示");

                            if (result == DialogResult.Retry)
                            {
                                IoMgr.GetInstance().WriteIoBit(Name.S, OperationMode == 0);

                                Thread.Sleep(500);
                            }
                            else
                            {
                                bRet = result == DialogResult.Yes;
                                break;
                            }
                        }

                        m_dtEndTime = DateTime.Now;

                        if (bRet)
                        {
                            if (UsedTimeS > LimitTimeS)
                            {
                                Score = 0.5;
                            }
                            else
                            {
                                Score = 1;
                            }
                            station.ShowLog(string.Format(str3, strName, OperationMode == 1 ? "On" : "Off", UsedTimeS.ToString("F3")));
                        }
                        else
                        {
                            Score = 0;

                            //错误信息，获取IO点
                            long num = IoMgr.GetInstance().m_dicOut[Name.S];

                            Message = string.Format("Check DO:{0}.{1}={2};", num >> 8, num & 0xFF, OperationMode == 1 ? "On" : "Off");

                            //超时
                            station.ShowLog(string.Format(str4, strName, OperationMode == 1 ? "On" : "Off"));
                        }

                    }
                    break;

                case TestType.AxisMove:
                    {
                        int speed = 1000;
                        AxisCfg cfg;
                        if (MotionMgr.GetInstance().GetAxisCfg(Name.I, out cfg))
                        {
                            speed = (int)cfg.dbSpeedMax;
                        }

                        MotionMgr.GetInstance().ServoOn(Name.I);

                        switch ((AxisMotion)OperationMode)
                        {
                            case AxisMotion.On:
                                {
                                    string str1 = "执行{0}上电，等待确认";
                                    string str2 = "执行{0}上电,请确认是否OK?";
                                    string str3 = "执行{0}上电完成，花费时间{1}s";
                                    string str4 = "执行{0}上电NG";
                                    if (LanguageMgr.GetInstance().LanguageID == 1)
                                    {
                                        str1 = "Execute {0} servo on, waiting for confirmation";
                                        str2 = "Execute {0} servo on, please confirm whether it is OK?";
                                        str3 = "Execute {0} servo on completion, taking {1} seconds";
                                        str4 = "Execute {0} servo on NG";
                                    }

                                    m_dtStartTime = DateTime.Now;
                                    station.ShowLog(string.Format(str1, Description));

                                    while (true)
                                    {
                                        MotionMgr.GetInstance().ServoOn(Name.I);

                                        DialogResult result =
                                            Form_Msg.ShowMessage(string.Format(str2, Description), "Tips");

                                        if (result == DialogResult.Retry)
                                        {
                                            MotionMgr.GetInstance().ServoOff(Name.I);

                                            Thread.Sleep(500);
                                        }
                                        else
                                        {
                                            bRet = result == DialogResult.Yes;
                                            break;
                                        }
                                    }

                                    m_dtEndTime = DateTime.Now;

                                    if (bRet)
                                    {
                                        station.ShowLog(string.Format(str3, Description, UsedTimeS.ToString("F3")));
                                    }
                                    else
                                    {
                                        Message = string.Format("Check {0} servo on", Description);
                                        //超时
                                        station.ShowLog(string.Format(str4, Description));
                                    }
                                }
                                break;

                            case AxisMotion.Off:
                                {
                                    string str1 = "执行{0}去电，等待确认";
                                    string str2 = "执行{0}去电,请确认是否OK?";
                                    string str3 = "执行{0}去电完成，花费时间{1}s";
                                    string str4 = "执行{0}去电NG";
                                    if (LanguageMgr.GetInstance().LanguageID == 1)
                                    {
                                        str1 = "Execute {0} servo off and wait for confirmation";
                                        str2 = "Execute {0} servo off, please confirm whether it is OK?";
                                        str3 = "Execute {0} servo off and take {1} seconds";
                                        str4 = "Execute {0} servo off NG";
                                    }

                                    m_dtStartTime = DateTime.Now;
                                    station.ShowLog(string.Format(str1, Description));

                                    while (true)
                                    {
                                        MotionMgr.GetInstance().ServoOff(Name.I);

                                        DialogResult result =
                                            Form_Msg.ShowMessage(string.Format(str2, Description), "Tips");

                                        if (result == DialogResult.Retry)
                                        {
                                            MotionMgr.GetInstance().ServoOn(Name.I);

                                            Thread.Sleep(500);
                                        }
                                        else
                                        {
                                            bRet = result == DialogResult.Yes;
                                            break;
                                        }
                                    }

                                    m_dtEndTime = DateTime.Now;

                                    if (bRet)
                                    {
                                        station.ShowLog(string.Format(str3, Description, UsedTimeS.ToString("F3")));
                                    }
                                    else
                                    {
                                        Message = string.Format("Check {0} servo off", Description);
                                        //超时
                                        station.ShowLog(string.Format(str4, Description));
                                    }
                                }
                                break;

                            case AxisMotion.Home:
                                {
                                    string str1 = "{0}检测原点信号";
                                    string str2 = "{0}检测原点信号完成，花费时间{1}s";
                                    string str3 = "{0}检测原点信号超时";
                                    if (LanguageMgr.GetInstance().LanguageID == 1)
                                    {
                                        str1 = "{0} detects the ORG signal";
                                        str2 = "{0} detection of the ORG signal completed, taking time {1} seconds";
                                        str3 = "{0} detection ORG signal timeout";
                                    }
                                    station.ShowLog(string.Format(str1, Description));
                                    m_dtStartTime = DateTime.Now;

                                    MotionMgr.GetInstance().Home(Name.I);
                                    bRet = station.WaitHome(Name.I,600) == 0;

                                    m_dtEndTime = DateTime.Now;

                                    if (bRet)
                                    {
                                        station.ShowLog(string.Format(str2, Description, UsedTimeS.ToString("F3")));
                                    }
                                    else
                                    {
                                        station.ShowLog(string.Format(str3, Description));
                                    }

                                }
                                break;

                            case AxisMotion.MEL:
                                {
                                    string str1 = "{0}检测负极限信号";
                                    string str2 = "{0}检测负极限信号完成,花费时间{1}s";
                                    string str3 = "检查{0}正负限位信号是否接反";
                                    string str4 = "{0}检测负极限信号失败，负极限配错";
                                    string str5 = "{0}检测负极限信号失败";
                                    if (LanguageMgr.GetInstance().LanguageID == 1)
                                    {
                                        str1 = "{0} detect MEL signal";
                                        str2 = "{0} detection of MEL signal completed, taking time {1} seconds";
                                        str3 = "Check whether the PEL and MEL signals of {0} are connected reversely";
                                        str4 = "{0} failed to detect MEL signal, MEL mismatch";
                                        str5 = "{0} failed to detect MEL signal";
                                    }

                                    station.ShowLog(string.Format(str1, Description));
                                    m_dtStartTime = DateTime.Now;

                                    while(MotionMgr.GetInstance().IsAxisMEL(Name.I) == 1)
                                    {
                                        MotionMgr.GetInstance().RelativeMove(Name.I, 1000, speed);
                                        station.WaitMotion(Name.I);
                                    }

                                    while (MotionMgr.GetInstance().IsAxisPEL(Name.I) == 1)
                                    {
                                        MotionMgr.GetInstance().RelativeMove(Name.I, -1000, speed);
                                        station.WaitMotion(Name.I);
                                    }

                                    MotionMgr.GetInstance().VelocityMove(Name.I,-speed);
                                    station.WaitEL(Name.I,true, (int)LimitTimeS);
                                    m_dtEndTime = DateTime.Now;

                                    MotionMgr.GetInstance().StopAxis(Name.I);

                                    if (MotionMgr.GetInstance().IsAxisMEL(Name.I) == 1 
                                        && MotionMgr.GetInstance().IsAxisPEL(Name.I) != 1)
                                    {
                                        bRet = true;
                                        station.ShowLog(string.Format(str2, Description, UsedTimeS.ToString("F3")));
                                    }
                                    else if(MotionMgr.GetInstance().IsAxisPEL(Name.I) == 1)
                                    {
                                        bRet = false;
                                        Message = string.Format(str3, Description);

                                        station.ShowLog(string.Format(str4, Description));
                                    }
                                    else
                                    {
                                        Message = string.Format("Check {0} MEL", Description);

                                        station.ShowLog(string.Format(str5, Description));
                                        bRet = false;
                                    }

                                }
                                break;

                            case AxisMotion.PEL:
                                {
                                    string str1 = "{0}检测正极限信号";
                                    string str2 = "{0}检测正极限信号完成,花费时间{1}s";
                                    string str3 = "检查{0}正负限位信号是否接反";
                                    string str4 = "{0}检测正极限信号失败，正极限配错";
                                    string str5 = "{0}检测正极限信号失败";
                                    if (LanguageMgr.GetInstance().LanguageID == 1)
                                    {
                                        str1 = "{0} detect PEL signal";
                                        str2 = "{0} detection of PEL signal completed, taking time {1} seconds";
                                        str3 = "Check whether the PEL and MEL signals of {0} are connected reversely";
                                        str4 = "{0} failed to detect PEL signal, PEL mismatch";
                                        str5 = "{0} failed to detect PEL signal";
                                    }

                                    station.ShowLog(string.Format(str1, Description));
                                    m_dtStartTime = DateTime.Now;

                                    while (MotionMgr.GetInstance().IsAxisMEL(Name.I) == 1)
                                    {
                                        MotionMgr.GetInstance().RelativeMove(Name.I, 1000, speed);
                                        station.WaitMotion(Name.I);
                                    }

                                    while (MotionMgr.GetInstance().IsAxisPEL(Name.I) == 1)
                                    {
                                        MotionMgr.GetInstance().RelativeMove(Name.I, -1000, speed);
                                        station.WaitMotion(Name.I);
                                    }

                                    MotionMgr.GetInstance().VelocityMove(Name.I, speed);
                                    station.WaitEL(Name.I, true, 600);
                                    m_dtEndTime = DateTime.Now;

                                    MotionMgr.GetInstance().StopAxis(Name.I);

                                    if (MotionMgr.GetInstance().IsAxisMEL(Name.I) != 1
                                        && MotionMgr.GetInstance().IsAxisPEL(Name.I) == 1)
                                    {
                                        bRet = true;
                                        station.ShowLog(string.Format(str2, Description, UsedTimeS.ToString("F3")));
                                    }
                                    else if (MotionMgr.GetInstance().IsAxisMEL(Name.I) == 1)
                                    {
                                        bRet = false;
                                        Message = string.Format(str3, Description);

                                        station.ShowLog(string.Format(str4, Description));
                                    }
                                    else
                                    {
                                        bRet = false;
                                        Message = string.Format("Check {0} PEL", Description);

                                        station.ShowLog(string.Format(str5, Description));
                                    }

                                }
                                break;
                        }

                        MotionMgr.GetInstance().ServoOff(Name.I);

                        if (bRet)
                        {
                            if (UsedTimeS > LimitTimeS)
                            {
                                Score = 1;
                            }
                            else
                            {
                                Score = 2;
                            }
                        }
                        else
                        {
                            Score = 0;
                        }
                    }
                    break;
            }

            return bRet;
        }

    }

    public class AutoTestMgr : SingletonTemplate<AutoTestMgr>
    {
        public static readonly string[] HEADS = { "类型", "名称","描述", "操作", "限制时间", "结果", "花费时间","得分","错误信息" };

        private List<TestItem> m_listTestItems = new List<TestItem>();

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double Score
        {
            get
            {
                double score = 0;
                foreach(var test in m_listTestItems)
                {
                    score += test.Score;
                }

                return score;
            }
        }

        private StationEx m_station = new StationEx("AutoTest");

        public delegate void TestItemChangedHandler(int index, TestItem item);
        public delegate void TestFinishedHandler();

        public event TestItemChangedHandler OnTestItemChanged;
        public event TestItemChangedHandler OnTestItemStart;
        public event TestFinishedHandler OnTestFinished;

        /// <summary>
        /// 从xml文件中读取定义的Data信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_listTestItems.Clear();
            XmlNodeList xnl = doc.SelectNodes("/AutoTest");

            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        try
                        {
                            int nCol = 0;
                            //类型
                            string strType = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //名称
                            string strName = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //描述
                            string strDesc = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //操作
                            string strOperator = xe.GetAttribute(HEADS[nCol++]).Trim();

                            //限制时间
                            string strLimitTimeS = xe.GetAttribute(HEADS[nCol++]).Trim();

                            TestItem item = new TestItem();

                            item.Type = (TestType)Enum.Parse(typeof(TestType), strType);

                            item.Name = strName;

                            item.Description = strDesc;

                            item.OperationMode = Convert.ToInt32(strOperator);

                            item.LimitTimeS = Convert.ToDouble(strLimitTimeS);

                            m_listTestItems.Add(item);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 跟新内存参数到表格数据
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();

            foreach (var item in m_listTestItems)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = item.Type.ToString();
                grid.Rows[nRow].Cells[nCol++].Value = item.Name.S;
                grid.Rows[nRow].Cells[nCol++].Value = item.Description;
                grid.Rows[nRow].Cells[nCol++].Value = item.Mode;
                
                grid.Rows[nRow].Cells[nCol++].Value = item.LimitTimeS.ToString();
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            int minLen = Math.Min(m_listTestItems.Count, grid.Rows.Count);

            for (int i = 0; i < minLen; i++)
            {
                DataGridViewRow row = grid.Rows[i];

                int nCol = 0;

                try
                {
                    //类型
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strType = row.Cells[nCol++].Value.ToString();

                    //名称
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strName = row.Cells[nCol++].Value.ToString();

                    //描述
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strDesc = row.Cells[nCol++].Value.ToString();

                    //操作
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strOperator = row.Cells[nCol++].Value.ToString();

                    TestType type = (TestType)Enum.Parse(typeof(TestType), strType);

                    //限制时间
                    if (row.Cells[nCol].Value == null)
                    {
                        switch (type)
                        {
                            case TestType.Cylinder:
                                row.Cells[nCol].Value = 10;
                                break;

                            case TestType.DI:
                                row.Cells[nCol].Value = 3 * 60;
                                break;

                            case TestType.DO:
                                row.Cells[nCol].Value = 60;
                                break;

                            case TestType.AxisMove:
                                row.Cells[nCol].Value = 3 * 60;
                                break;
                        }
                    }
                    string strLimitTime = row.Cells[nCol++].Value.ToString();

                    TestItem item = m_listTestItems[i];

                    item.Type = type;

                    item.Name = strName;

                    item.Description = strDesc;

                    switch(item.Type)
                    {
                        case TestType.Cylinder:
                            item.OperationMode = (int)Enum.Parse(typeof(CylinderStatus), strOperator);
                            break;
                        case TestType.DI:
                        case TestType.DO:
                            item.OperationMode = (int)Enum.Parse(typeof(SignalStatus), strOperator);
                            break;

                        case TestType.AxisMove:
                            item.OperationMode = (int)Enum.Parse(typeof(AxisMotion), strOperator);
                            break;

                    }

                    item.LimitTimeS = Convert.ToDouble(strLimitTime) ;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            for (int i = minLen; i < grid.Rows.Count; i++)
            {
                DataGridViewRow row = grid.Rows[i];

                int nCol = 0;

                try
                {
                    //类型
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strType = row.Cells[nCol++].Value.ToString();

                    //名称
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strName = row.Cells[nCol++].Value.ToString();

                    //描述
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strDesc = row.Cells[nCol++].Value.ToString();

                    //操作
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    string strOperator = row.Cells[nCol++].Value.ToString();

                    TestType type = (TestType)Enum.Parse(typeof(TestType), strType);

                    //限制时间
                    if (row.Cells[nCol].Value == null)
                    {
                        switch (type)
                        {
                            case TestType.Cylinder:
                                row.Cells[nCol].Value = 10;
                                break;

                            case TestType.DI:
                                row.Cells[nCol].Value = 3 * 60;
                                break;

                            case TestType.DO:
                                row.Cells[nCol].Value = 60;
                                break;

                            case TestType.AxisMove:
                                row.Cells[nCol].Value = 3 * 60;
                                break;
                        }
                    }
                    string strLimitTime = row.Cells[nCol++].Value.ToString();

                    TestItem item = new TestItem();

                    item.Type = type;

                    item.Name = strName;

                    item.Description = strDesc;

                    switch (item.Type)
                    {
                        case TestType.Cylinder:
                            item.OperationMode = (int)Enum.Parse(typeof(CylinderStatus), strOperator);
                            break;
                        case TestType.DI:
                        case TestType.DO:
                            item.OperationMode = (int)Enum.Parse(typeof(SignalStatus), strOperator);
                            break;

                        case TestType.AxisMove:
                            item.OperationMode = (int)Enum.Parse(typeof(AxisMotion), strOperator);
                            break;

                    }

                    item.LimitTimeS = Convert.ToDouble(strLimitTime);

                    m_listTestItems.Add(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("AutoTest");
            if (root == null)
            {
                root = doc.CreateElement("AutoTest");

                doc.AppendChild(root);
            }

            root.RemoveAll();

            foreach (var data in m_listTestItems)
            {
                XmlElement xe = doc.CreateElement("Item");
                int nItem = 0;
                xe.SetAttribute(HEADS[nItem++], data.Type.ToString());
                xe.SetAttribute(HEADS[nItem++], data.Name.ToString());
                xe.SetAttribute(HEADS[nItem++], data.Description.ToString());
                xe.SetAttribute(HEADS[nItem++], data.OperationMode.ToString());
                xe.SetAttribute(HEADS[nItem++], data.LimitTimeS.ToString());
                root.AppendChild(xe);
            }
        }

        private void OnStart()
        {
            try
            {
                //计算总分
                double totalScore = 0;
                foreach (var test in AutoTestMgr.GetInstance().m_listTestItems)
                {
                    switch (test.Type)
                    {
                        case TestType.Cylinder:
                            totalScore += 2;
                            break;
                        case TestType.DI:
                            totalScore += 1;
                            break;
                        case TestType.DO:
                            totalScore += 1;
                            break;
                        case TestType.AxisMove:
                            totalScore += 2;
                            break;
                    }
                }

                //计算权重
                double factor = 100 / totalScore;

                int nItem = 0;

                StartTime = DateTime.Now;

                foreach (var test in AutoTestMgr.GetInstance().m_listTestItems)
                {
                    if (m_station.CurState != StationState.STATE_AUTO)
                    {
                        break;
                    }

                    if (test.Result)
                    {
                        nItem++;
                        continue;
                    }

                    if (OnTestItemStart != null)
                    {
                        OnTestItemStart(nItem, test);
                    }

                    bool bRet = test.Run(m_station);

                    test.Result = bRet;

                    test.Score *= factor;

                    if (OnTestItemChanged != null)
                    {
                        OnTestItemChanged(nItem, test);
                    }

                    nItem++;
                }

                EndTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                EndTime = DateTime.Now;

                m_station.ShowLog(ex.Message);
                foreach (var item in MotionMgr.GetInstance().m_listCard)
                {
                    for (int i = item.GetMinAxisNo(); i <= item.GetMaxAxisNo(); i++)
                    {
                        MotionMgr.GetInstance().StopEmg(i);
                    }
                }
            }
            finally
            {
                if (OnTestFinished != null && m_listTestItems.Count > 0)
                {
                    OnTestFinished();
                }
            }
        }

        public void Start()
        {
            m_station.ManualRun(OnStart);
        }

        public void Stop()
        {
            if (m_station.IsManual())
            {
                m_station.StopManualRun();
            }
        }

        public void SetLogListBox(Control ctrl,LogHandler handler)
        {
            m_station.SetLogListBox(ctrl);
            m_station.LogEvent += handler;
        }

        public void GenerateHtmlReport(string strFile)
        {
            try
            {
                string strTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report.html");
                TemplateDocument doc = new TemplateDocument(strTemplate, Encoding.UTF8, TemplateDocumentConfig.Default);

                doc.Variables.SetValue("StartTime", StartTime.ToString());
                doc.Variables.SetValue("EndTime", EndTime.ToString());
                doc.Variables.SetValue("Score", Score.ToString("F2"));
                doc.Variables.SetValue("Heads", HEADS);
                doc.Variables.SetValue("TestItems", m_listTestItems);
                doc.RenderTo(strFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void ResetTestItems()
        {
            m_listTestItems.Clear();
        }
    }
}
