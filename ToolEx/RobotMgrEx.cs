using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using Communicate;
using System.Windows.Forms;
using System.Xml;
using AutoFrameDll;
using System.Threading;
using System.Text.RegularExpressions;

#pragma warning disable 0168

namespace ToolEx
{
    /// <summary>
    /// 通信方式
    /// </summary>
    public enum CommMode
    {
        /// <summary>
        /// 网络通信
        /// </summary>
        Tcp,
        /// <summary>
        /// 串口通信
        /// </summary>
        Com, 
    }

    /// <summary>
    /// 有效电平
    /// </summary>
    public enum ActiveLevel
    {
        /// <summary>
        /// 低电平
        /// </summary>
        Low = 0,

        /// <summary>
        /// 高电平
        /// </summary>
        High,

        /// <summary>
        /// 脉冲
        /// </summary>
        Pluse, 
    }

    /// <summary>
    /// 机械手品牌
    /// </summary>
    public enum RobotVendor
    {
        /// <summary>
        /// 爱普生
        /// </summary>
        Epson = 0,
        /// <summary>
        /// 三菱
        /// </summary>
        Mitsubishi,
        /// <summary>
        /// 雅马哈
        /// </summary>
        Yamaha,

        /// <summary>
        /// ABB
        /// </summary>
        ABB,

        /// <summary>
        /// 东芝
        /// </summary>
        Toshiba,
    }

    /// <summary>
    /// 机器人远程控制IO
    /// </summary>
    public struct RobotSysIo
    {
        /// <summary>
        /// 功能
        /// </summary>
        public string m_strIoFuction;

        /// <summary>
        /// IO名称
        /// </summary>
        public string m_strIoName;

        /// <summary>
        /// 有效方式
        /// </summary>
        public ActiveLevel m_activeLevel;

        /// <summary>
        /// 脉冲宽度，单位ms
        /// </summary>
        public int m_nPluseWidthMs;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool m_bEnable;     
    }

    /// <summary>
    /// 机器人IO
    /// </summary>
    public struct RobotIo
    {
        /// <summary>
        /// IO点位
        /// </summary>
        public int m_nIndex;

        /// <summary>
        /// IO名称
        /// </summary>
        public string m_strName; 
    }

    /// <summary>
    /// 机器人Point
    /// </summary>
    public struct RobotPoint
    {
        /// <summary>
        /// 点位
        /// </summary>
        public int m_nIndex;

        /// <summary>
        /// 点位名称
        /// </summary>
        public string m_strName;
    }

    /// <summary>
    /// 机器人命令
    /// </summary>
    public struct RobotCmd
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string m_strCmd;

        /// <summary>
        /// 命令描述
        /// </summary>
        public string m_strDesc;

        /// <summary>
        /// 参数长度
        /// </summary>
        public int m_nLength;

        /// <summary>
        /// 命令返回
        /// </summary>
        public string m_strRespond; 
    }


    /// <summary>
    /// 机器人通信
    /// </summary>
    public struct RobotComm
    {
        /// <summary>
        /// 通信模式，Tcp、Com
        /// </summary>
        public CommMode m_comMode;

        /// <summary>
        /// 远程控制通信索引
        /// </summary>
        public int m_nRemoteComm;

        /// <summary>
        /// 手动控制通信索引
        /// </summary>
        public int m_nManulComm;

        /// <summary>
        /// 监控通信索引
        /// </summary>
        public int m_nMonitorComm;
    }

    /// <summary>
    /// 机器人类
    /// </summary>
    public class Robot
    {
        private string m_strRobotName = ""; //机械手名称
        private RobotVendor m_RobotVendor; //机械手品牌
        private RobotComm m_robotComm; //机械手通信方式
        private Dictionary<string, RobotSysIo> m_dictRobotSysIoIn = new Dictionary<string, RobotSysIo>(); //SysIoIn
        private Dictionary<string, RobotSysIo> m_dictRobotSysIoOut = new Dictionary<string, RobotSysIo>();//SysIoOut
        private Dictionary<string, RobotCmd> m_dictRobotCmd = new Dictionary<string, RobotCmd>(); //CMD
        private Dictionary<string, RobotIo> m_dictRobotIoIn = new Dictionary<string, RobotIo>(); //IoIn
        private Dictionary<string, RobotIo> m_dictRobotIoOut = new Dictionary<string, RobotIo>();//IoOut
        private Dictionary<string, RobotPoint> m_dictRobotPoint = new Dictionary<string, RobotPoint>();//Point

        private object m_linkRemote = null;
        private object m_linkManul = null;
        private object m_linkMonitor = null;

        private object m_lock = new object();

        //机器人监控字符串格式定义
        //Pos,axis,tool,x,y,z,u,v,w\r\n
        //In,len,0/1,...0/1\r\n
        //Out,len,0/1,...0/1\r\n
        private int m_nRobotAxis = 6;
        private double[] m_dbCurPos = new double[10];
        private bool[] m_bInStatus = new bool[100];
        private bool[] m_bOutStatus = new bool[100];
        private int m_nTool = 0;
        private int m_nSpeedFactor = 100;
        private bool m_bMotorOn = false;
        private bool m_bPowerHigh = false;

        /// <summary>
        /// 运动模式
        /// </summary>
        public enum MoveMode
        {
            /// <summary>
            /// 关节运动跳转
            /// </summary>
            Jump,

            /// <summary>
            /// 关节运动Go
            /// </summary>
            Go,

            /// <summary>
            /// 直线运动Move
            /// </summary>
            Move,
        }


        /// <summary>
        /// 机器人轴数量
        /// </summary>
        public int RobotAxis
        {
            get
            {
                return m_nRobotAxis;
            }
        }

        /// <summary>
        /// 机器人X坐标
        /// </summary>
        public double X
        {
            get
            {
                return m_dbCurPos[0];
            }
        }

        /// <summary>
        /// 机器人Y坐标
        /// </summary>
        public double Y
        {
            get
            {
                return m_dbCurPos[1];
            }
        }

        /// <summary>
        /// 机器人Z坐标
        /// </summary>
        public double Z
        {
            get
            {
                return m_dbCurPos[2];
            }
        }

        /// <summary>
        /// 机器人U坐标
        /// </summary>
        public double U
        {
            get
            {
                return m_dbCurPos[3];
            }
        }

        /// <summary>
        /// 机器人V坐标
        /// </summary>
        public double V
        {
            get
            {
                return m_dbCurPos[4];
            }
        }

        /// <summary>
        /// 机器人W坐标
        /// </summary>
        public double W
        {
            get
            {
                return m_dbCurPos[5];
            }
        }

        /// <summary>
        /// 机器人工具
        /// </summary>
        public int Tool
        {
            get
            {
                return m_nTool;
            }
        }

        /// <summary>
        /// 输入状态
        /// </summary>
        public bool[] IoIn
        {
            get
            {
                return m_bInStatus;
            }
        }

        /// <summary>
        /// 输出状态
        /// </summary>
        public bool[] IoOut
        {
            get
            {
                return m_bOutStatus;
            }
        }

        /// <summary>
        /// 速度百分比
        /// </summary>
        public int SpeedFactor
        {
            get
            {
                return m_nSpeedFactor;
            }
        }

        /// <summary>
        /// 机器上电状态
        /// </summary>
        public bool Motor
        {
            get
            {
                return m_bMotorOn;
            }
        }


        /// <summary>
        /// 机器人高功率状态
        /// </summary>
        public bool Power
        {
            get
            {
                return m_bPowerHigh;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strName">机器人名称</param>
        /// <param name="comm">通信方式</param>
        /// <param name="vendor">品牌</param>
        public Robot(string strName,RobotComm comm,RobotVendor vendor)
        {
            m_strRobotName = strName;
            m_RobotVendor = vendor;
            m_robotComm = comm;

            //只允许有固定功能的IO
            m_dictRobotSysIoIn.Clear();
            m_dictRobotSysIoOut.Clear();

            if (m_robotComm.m_comMode == CommMode.Com)
            {
                m_linkRemote = ComMgr.GetInstance().GetComLink(m_robotComm.m_nRemoteComm);
                m_linkManul = ComMgr.GetInstance().GetComLink(m_robotComm.m_nManulComm);
                m_linkMonitor = ComMgr.GetInstance().GetComLink(m_robotComm.m_nMonitorComm);
            }
            else if(m_robotComm.m_comMode == CommMode.Tcp)
            {
                m_linkRemote = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nRemoteComm);
                m_linkManul = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nManulComm);
                m_linkMonitor = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nMonitorComm);
            }
        }

        /// <summary>
        /// 机械手名称
        /// </summary>
        public string RobotName
        {
            get
            {
                return m_strRobotName;
            }

            set
            {
                m_strRobotName = value;
            }
        }

        /// <summary>
        /// 机器人品牌
        /// </summary>
        public RobotVendor Vendor
        {
            get
            {
                return m_RobotVendor;
            }

            set
            {
                m_RobotVendor = value;        
            }
        }

        /// <summary>
        /// 机器人通信方式
        /// </summary>
        public RobotComm Comm
        {
            get
            {
                return m_robotComm;
            }

            set
            {
                m_robotComm = value;

                if (m_robotComm.m_comMode == CommMode.Com)
                {
                    m_linkRemote = ComMgr.GetInstance().GetComLink(m_robotComm.m_nRemoteComm);
                    m_linkManul = ComMgr.GetInstance().GetComLink(m_robotComm.m_nManulComm);
                    m_linkMonitor = ComMgr.GetInstance().GetComLink(m_robotComm.m_nMonitorComm);
                }
                else if (m_robotComm.m_comMode == CommMode.Tcp)
                {
                    m_linkRemote = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nRemoteComm);
                    m_linkManul = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nManulComm);
                    m_linkMonitor = TcpMgr.GetInstance().GetTcpLink(m_robotComm.m_nMonitorComm);
                }
            }
        }

        /// <summary>
        /// 机器人远程输入IO
        /// </summary>
        public Dictionary<string,RobotSysIo> RobotSysIoIns
        {
            get
            {
                return m_dictRobotSysIoIn;
            }
        }

        /// <summary>
        /// 机器人远程输出IO
        /// </summary>
        public Dictionary<string,RobotSysIo> RobotSysIoOuts
        {
            get
            {
                return m_dictRobotSysIoOut;
            }
        }

        /// <summary>
        /// 机器人输入IO
        /// </summary>
        public Dictionary<string, RobotIo> RobotIoIns
        {
            get
            {
                return m_dictRobotIoIn;
            }
        }

        /// <summary>
        /// 机器人输出IO
        /// </summary>
        public Dictionary<string, RobotIo> RobotIoOuts
        {
            get
            {
                return m_dictRobotIoOut;
            }
        }

        /// <summary>
        /// 机器人运行点位
        /// </summary>
        public Dictionary<string,RobotPoint> RobotPoints
        {
            get
            {
                return m_dictRobotPoint;
            }
        }


        /// <summary>
        /// 机器人命令
        /// </summary>
        public Dictionary<string,RobotCmd> RobotCmds
        {
            get
            {
                return m_dictRobotCmd;
            }
        }

        /// <summary>
        /// 更新机器人输入IO信息
        /// </summary>
        /// <param name="strFunction">IO功能</param>
        /// <param name="info">IO信息</param>
        public void UpdateRobotIoIn(string strFunction,RobotSysIo info)
        {

            if (m_dictRobotSysIoIn.ContainsKey(strFunction))
            {
                m_dictRobotSysIoIn[strFunction] = info;
            }
            else
            {
                m_dictRobotSysIoIn.Add(strFunction, info);
            }

        }

        /// <summary>
        /// 更新机器人输入IO信息
        /// </summary>
        /// <param name="strIoName">IO名称</param>
        /// <param name="io">IO信息</param>
        public void UpdateRobotIoIn(string strIoName, RobotIo io)
        {

            if (m_dictRobotIoIn.ContainsKey(strIoName))
            {
                m_dictRobotIoIn[strIoName] = io;
            }
            else
            {
                m_dictRobotIoIn.Add(strIoName, io);
            }

        }

        /// <summary>
        /// 更新机器人输出IO信息
        /// </summary>
        /// <param name="strFunction">IO功能</param>
        /// <param name="info">IO信息</param>
        public void UpdateRobotIoOut(string strFunction, RobotSysIo info)
        {

            if (m_dictRobotSysIoOut.ContainsKey(strFunction))
            {
                m_dictRobotSysIoOut[strFunction] = info;
            }
            else
            {
                m_dictRobotSysIoOut.Add(strFunction, info);
            }

        }

        /// <summary>
        /// 更新机器人输出IO信息
        /// </summary>
        /// <param name="strIoName">IO名称</param>
        /// <param name="io">io信息</param>
        public void UpdateRobotIoOut(string strIoName, RobotIo io)
        {

            if (m_dictRobotIoOut.ContainsKey(strIoName))
            {
                m_dictRobotIoOut[strIoName] = io;
            }
            else
            {
                m_dictRobotIoOut.Add(strIoName, io);
            }

        }

        /// <summary>
        /// 更新机器人点位
        /// </summary>
        /// <param name="strPointName">点位名称</param>
        /// <param name="pt">点位</param>
        public void UpdateRobotPoint(string strPointName,RobotPoint pt)
        {
            if (m_dictRobotPoint.ContainsKey(strPointName))
            {
                m_dictRobotPoint[strPointName] = pt;
            }
            else
            {
                m_dictRobotPoint.Add(strPointName, pt);
            }
        }

        /// <summary>
        /// 更新机器人命令
        /// </summary>
        /// <param name="strCmd"></param>
        /// <param name="cmd"></param>
        public void UpdateRobotCmd(string strCmd,RobotCmd cmd)
        {
            if (m_dictRobotCmd.ContainsKey(strCmd))
            {
                m_dictRobotCmd[strCmd] = cmd;
            }
            else
            {
                m_dictRobotCmd.Add(strCmd, cmd);
            }
        }

        /// <summary>
        /// 远程通信连接对象
        /// </summary>
        public object LinkRemote
        {
            get { return m_linkRemote; }
        }


        /// <summary>
        /// 手动控制通信连接对象
        /// </summary>
        public object LinkManul
        {
            get { return m_linkManul; }
        }

        /// <summary>
        /// 监控通信连接对象
        /// </summary>
        public object LinkMonitor
        {
            get { return m_linkMonitor; }
        }

        /// <summary>
        /// 通信方式
        /// </summary>
        public CommMode Mode
        {
            get
            {
                return m_robotComm.m_comMode;
            }
        }

        /// <summary>
        /// 机器人是否在运行状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                bool bIsRun = true;

                if (m_dictRobotSysIoIn["运行"].m_bEnable)
                {
                    bIsRun = IoMgr.GetInstance().ReadIoInBit(m_dictRobotSysIoIn["运行"].m_strIoName);
                }

                return bIsRun;
            }
        }

        /// <summary>
        /// 机器人是否初始化
        /// </summary>
        public bool IsInited
        {
            get
            {
                //判断机械手是否打开
                bool bOpen = false;
                if (Mode == CommMode.Com)
                {
                    bOpen = ((ComLink)m_linkRemote).IsOpen();
                }
                else
                {
                    bOpen = ((TcpLink)m_linkRemote).IsOpen();
                }

                return (bOpen && IsRunning);
            }
        }

        /// <summary>
        /// 触发IO
        /// </summary>
        /// <param name="strFunction"></param>
        public void TriggerIo(string strFunction)
        {
            RobotSysIo info;
            if (m_dictRobotSysIoOut.TryGetValue(strFunction,out info))
            {
                //是否启用
                if (info.m_bEnable)
                {
                    switch (info.m_activeLevel)
                    {
                        case ActiveLevel.High:
                            IoMgr.GetInstance().WriteIoBit(info.m_strIoName, true);
                            break;

                        case ActiveLevel.Low:
                            IoMgr.GetInstance().WriteIoBit(info.m_strIoName, false);
                            break;

                        case ActiveLevel.Pluse:
                            IoMgr.GetInstance().WriteIoBit(info.m_strIoName, true);
                            Thread.Sleep(info.m_nPluseWidthMs);
                            IoMgr.GetInstance().WriteIoBit(info.m_strIoName, false);
                            break;
                    }
                }
            }
            

        }

        /// <summary>
        /// 机器人停止
        /// </summary>
        public void RobotStop()
        {
            TriggerIo("停止");
        }

        /// <summary>
        /// 机器人暂停
        /// </summary>
        public void RobotPause()
        {
            TriggerIo("暂停");
        }

        /// <summary>
        /// 机器人继续
        /// </summary>
        public void RobotContinue()
        {
            TriggerIo("继续");
        }

        /// <summary>
        /// 等待IO
        /// </summary>
        /// <param name="strFunction"></param>
        /// <param name="nTimeOut"></param>
        /// <returns></returns>
        public bool WaitIo(string strFunction,int nTimeOut = 0)
        {

            RobotSysIo info = m_dictRobotSysIoIn[strFunction];
            //是否启用
            if (info.m_bEnable)
            {
                bool bValid = info.m_activeLevel == ActiveLevel.High ? true : false;

                //等待
                int ticks = Environment.TickCount;
                if (nTimeOut == 0)
                {
                    nTimeOut = SystemMgr.GetInstance().GetParamInt("IoTimeOut");
                }

                while (true && StationMgr.GetInstance().IsAutoRunning())
                {
                    Thread.Sleep(20);

                    if (IoMgr.GetInstance().ReadIoInBit(info.m_strIoName) == bValid)
                    {
                        return true;
                    }

                    if (nTimeOut == -1)
                    {
                        continue;
                    }

                    if (Environment.TickCount - ticks > nTimeOut * 1000)
                    {
                        return false;
                    }
                }


            }


            return true;
        }

        /// <summary>
        /// 初始化机器人
        /// </summary>
        /// <returns></returns>
        public bool InitRobot()
        {
            lock (m_lock)
            {
                if (!IsInited)
                {
                    //先断开连接
                    ConnectRemote(false);

                    switch (m_RobotVendor)
                    {
                        case RobotVendor.Epson:
                            if (!EpsonInit())
                            {
                                return false;
                            }
                            break;
                    }


                    //打开连接
                    return ConnectRemote(true);
                }

                return true;
            }   
        }

        /// <summary>
        /// 机器人反初始化
        /// </summary>
        public void DeInitRobot()
        {
            //机械手停止
            TriggerIo("停止");

            UnLock();

            //断开连接
            ConnectRemote(false);
        }

        /// <summary>
        /// 打开、断开远程连接
        /// </summary>
        /// <param name="connect">打开、断开</param>
        /// <returns>连接结果</returns>
        public bool ConnectRemote(bool connect)
        {
            if (connect)
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkRemote).Open();
                }
                else
                {
                    return ((TcpLink)m_linkRemote).Open();
                }
            }
            else
            {
                //断开连接
                if (Mode == CommMode.Com)
                {
                    if (m_linkRemote != null)
                    {
                        ((ComLink)m_linkRemote).Close();
                    }
                }
                else
                {
                    if (m_linkRemote != null)
                    {
                        ((TcpLink)m_linkRemote).Close();
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 远程是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsRemoteConnected
        {
            get
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkRemote).IsOpen();
                }
                else
                {
                    return ((TcpLink)m_linkRemote).IsOpen();
                }
            }
        }

        /// <summary>
        /// 打开、断开手动连接
        /// </summary>
        /// <param name="connect">打开、断开</param>
        /// <returns>连接结果</returns>
        public bool ConnectManul(bool connect)
        {
            if (connect)
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkManul).Open();
                }
                else
                {
                    return ((TcpLink)m_linkManul).Open();
                }
            }
            else
            {
                //断开连接
                if (Mode == CommMode.Com)
                {
                    if (m_linkRemote != null)
                    {
                        ((ComLink)m_linkManul).Close();
                    }
                }
                else
                {
                    if (m_linkRemote != null)
                    {
                        ((TcpLink)m_linkManul).Close();
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 手动控制是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsManulConnected
        {
            get
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkManul).IsOpen();
                }
                else
                {
                    return ((TcpLink)m_linkManul).IsOpen();
                }
            }
        }

        /// <summary>
        /// 打开、断开监视连接
        /// </summary>
        /// <param name="connect">打开、断开</param>
        /// <returns>连接结果</returns>
        public bool ConnectMonitor(bool connect)
        {
            if (connect)
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkMonitor).Open();
                }
                else
                {
                    return ((TcpLink)m_linkMonitor).Open();
                }
            }
            else
            {
                //断开连接
                if (Mode == CommMode.Com)
                {
                    if (m_linkRemote != null)
                    {
                        ((ComLink)m_linkMonitor).Close();
                    }
                }
                else
                {
                    if (m_linkRemote != null)
                    {
                        ((TcpLink)m_linkMonitor).Close();
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 监控是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsMonitorConnected
        {
            get
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkMonitor).IsOpen();
                }
                else
                {
                    return ((TcpLink)m_linkMonitor).IsOpen();
                }
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public int ReadLine(out string strData)
        {
            lock(m_lock)
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkRemote).ReadLine(out strData);
                }
                else
                {
                    return ((TcpLink)m_linkRemote).ReadLine(out strData);
                }
            }
            
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool WriteLine(string strData)
        {
            lock(m_lock)
            {
                if (Mode == CommMode.Com)
                {
                    return ((ComLink)m_linkRemote).WriteLine(strData);
                }
                else
                {
                    return ((TcpLink)m_linkRemote).WriteLine(strData);
                }
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="link"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        private int ReadLine(object link,out string strData)
        {

            if (Mode == CommMode.Com)
            {
                return ((ComLink)link).ReadLine(out strData);
            }
            else
            {
                return ((TcpLink)link).ReadLine(out strData);
            }

        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="link"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        private bool WriteLine(object link,string strData)
        {
            if (Mode == CommMode.Com)
            {
                return ((ComLink)link).WriteLine(strData);
            }
            else
            {
                return ((TcpLink)link).WriteLine(strData);
            }
        }

        /// <summary>
        /// 锁定
        /// </summary>
        public void Lock()
        {
            System.Threading.Monitor.Enter(m_lock);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLock()
        {
            if (System.Threading.Monitor.IsEntered(m_lock))
            {
                System.Threading.Monitor.Exit(m_lock);
            }
        }

        /// <summary>
        /// 设置输出
        /// </summary>
        /// <param name="index"></param>
        /// <param name="on"></param>
        public void SetDO(int index,bool on)
        {
            if (IsManulConnected)
            {
                string strCmd = String.Format("Out,{0},{1}", index, on ? 1 : 0);
                WriteLine(m_linkManul, strCmd);
            }
        }

        /// <summary>
        /// 获取输入状态
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetDI(int index)
        {
            return m_bInStatus[index];
        }

        
        /// <summary>
        /// 获取输出状态
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetDO(int index)
        {
            return m_bOutStatus[index];
        }

        /// <summary>
        /// 机器人上电
        /// </summary>
        /// <param name="bOn"></param>
        public void MotorOn(bool bOn)
        {
            if (IsManulConnected)
            {
                if (bOn)
                {
                    WriteLine(m_linkManul, "MotorOn");
                }
                else
                {
                    WriteLine(m_linkManul, "MotorOff");
                }
            }
        }

        /// <summary>
        /// 设置机器人功率
        /// </summary>
        /// <param name="bOn"></param>
        public void PowerHigh(bool bOn)
        {
            if (IsManulConnected)
            {
                if (bOn)
                {
                    WriteLine(m_linkManul, "PowerHigh");
                }
                else
                {
                    WriteLine(m_linkManul, "PowerLow");
                }
            }
        }

        /// <summary>
        /// 机器人重置
        /// </summary>
        public void Reset()
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "Reset");
            }
        }

        /// <summary>
        /// 设置工具坐标
        /// </summary>
        /// <param name="tool"></param>
        public void SetTool(int tool)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "Tool," + tool);
            }
        }

        /// <summary>
        /// 设置Local坐标
        /// </summary>
        /// <param name="local"></param>
        public void SetLocal(int local)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "Local," + local);
            }
        }

        /// <summary>
        /// 设置速度比
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeedFactor(int speed)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "SpeedFactor," + speed);
            }
        }

        /// <summary>
        /// Jump到点
        /// </summary>
        /// <param name="pos"></param>
        public void Jump(int pos)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "JumpPos," + pos);
            }
        }

        /// <summary>
        /// Jump到绝对位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        public void Jump(double x,double y,double z,double u)
        {
            if (IsManulConnected)
            {
                string strCmd = String.Format("JumpAbs,{0},{1},{2},{3}", x, y, z, u);
                WriteLine(m_linkManul, strCmd);
            }
        }

        /// <summary>
        /// Go到点
        /// </summary>
        /// <param name="pos"></param>
        public void Go(int pos)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "GoPos," + pos);
            }
        }

        /// <summary>
        /// Go到绝对位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void Go(double x, double y, double z, double u,double v,double w)
        {
            if (IsManulConnected)
            {
                string strCmd = String.Format("GoAbs,{0},{1},{2},{3},{4},{5}"
                    , x, y, z, u,v,w);
                WriteLine(m_linkManul, strCmd);
            }
        }

        /// <summary>
        /// Move到点
        /// </summary>
        /// <param name="pos"></param>
        public void Move(int pos)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "MovePos," + pos);
            }
        }

        /// <summary>
        /// Move到绝对位置
        /// </summary>
        /// <param name="bAbs">是否绝对运动</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void Move(bool bAbs,double x, double y, double z, double u, double v, double w)
        {
            if (IsManulConnected)
            {
                string strCmd;

                if (bAbs)
                {
                    strCmd = String.Format("MoveAbs,{0},{1},{2},{3},{4},{5}", x, y, z, u,v,w);
                }
                else
                {
                    strCmd = String.Format("MoveRel,{0},{1},{2},{3},{4},{5}", x, y, z, u,v,w);
                }

                WriteLine(m_linkManul, strCmd);
            }
        }

        /// <summary>
        /// 连续运动开始
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void ContinueOn(double x, double y, double z, double u, double v, double w)
        {
            if (IsManulConnected)
            {
                string strCmd;

                strCmd = String.Format("ContinueOn,{0},{1},{2},{3},{4},{5}", x, y, z, u, v, w);
               

                WriteLine(m_linkManul, strCmd);
            }
        }


        /// <summary>
        /// 连续运动结束
        /// </summary>
        public void ContinueOff()
        {
            if (IsManulConnected)
            {
                string strCmd;

                strCmd = String.Format("ContinueOff");


                WriteLine(m_linkManul, strCmd);
            }
        }

        /// <summary>
        /// 示教点位
        /// </summary>
        /// <param name="pos"></param>
        public void TeachPoint(int pos)
        {
            if (IsManulConnected)
            {
                WriteLine(m_linkManul, "Teach," + pos);
            }
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartMonitor()
        {
            if (Mode == CommMode.Com)
            {
                ComLink t = (ComLink)m_linkMonitor;

                t.BeginAsynReceive(OnDataReceivedEvent);
            }
            else
            {
                TcpLink t = (TcpLink)m_linkMonitor;

                t.BeginAsynReceive(OnDataReceivedEvent);
            }
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopMonitor()
        {
            if (Mode == CommMode.Com)
            {
                ComLink t = (ComLink)m_linkMonitor;

                t.EndAsynReceive();
            }
            else
            {
                TcpLink t = (TcpLink)m_linkMonitor;

                t.EndAsynReceive();
            }
        }

        private string m_strMonitorReceive = "";
        private void OnDataReceivedEvent(byte[] data, int length)
        {
            if (length > 0)
            {
                m_strMonitorReceive += System.Text.Encoding.Default.GetString(data, 0, length);

                string[] strSplits;
                int nLength = 0;
                //判断接收字符串是否完整，是否以\r\n结尾
                if (m_strMonitorReceive.EndsWith("\r\n"))
                {
                    //以"\r\n"分割字符串
                    strSplits = m_strMonitorReceive.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    m_strMonitorReceive = "";

                    nLength = strSplits.Length;
                }
                else
                {
                    strSplits = m_strMonitorReceive.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    m_strMonitorReceive = strSplits[strSplits.Length-1].Trim();

                    nLength = strSplits.Length - 1;
                }

                //解析字符串
                for (int i = 0; i < nLength;i++)
                {
                    string str = strSplits[i];

                    //用','分割字符串
                    string[] strTT = str.Split(',');

                    switch (strTT[0])
                    {
                        //Rob,motor,power,tool,speed\r\n
                        //Pos,axis,x,y,z,u,v,w\r\n
                        //In,len,0/1,...0/1\r\n
                        //Out,len,0/1,...0/1\r\n
                        case "Rob":
                            {
                                m_bMotorOn = Convert.ToInt32(strTT[1]) == 1;
                                m_bPowerHigh = Convert.ToInt32(strTT[2]) == 1;
                                m_nTool = Convert.ToInt32(strTT[3]);
                                m_nSpeedFactor = Convert.ToInt32(strTT[4]);
                            }
                            break;

                        case "Pos":
                            {
                                m_nRobotAxis = Convert.ToInt32(strTT[1]);

                                for (int k = 0; k < m_nRobotAxis;k++)
                                {
                                    m_dbCurPos[k] = Convert.ToDouble(strTT[2 + k]);
                                }
                            }
                            break;

                        case "In":
                            {
                                int len = Convert.ToInt32(strTT[1]);

                                for (int k = 0; k < len;k++)
                                {
                                    m_bInStatus[k] = Convert.ToInt32(strTT[2 + k]) == 1;
                                }
                            }
                            break;

                        case "Out":
                            {
                                int len = Convert.ToInt32(strTT[1]);

                                for (int k = 0; k < len; k++)
                                {
                                    m_bOutStatus[k] = Convert.ToInt32(strTT[2 + k]) == 1;
                                }
                            }
                            break;
                    }

                }

            }
        }

        /// <summary>
        /// 爱普生机器人初始化
        /// </summary>
        /// <returns></returns>
        private bool EpsonInit()
        {
            //停止，复位，启动
            TriggerIo("停止");
            TriggerIo("复位");

            //等待机械手Ready
            if (!WaitIo("就绪"))
            {
                return false;
            }

            TriggerIo("启动");

            //等待机械手Running
            if (!WaitIo("运行"))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 机器人管理类
    /// </summary>
    public class RobotMgrEx : SingletonTemplate<RobotMgrEx>
    {
        /// <summary>
        /// 机器人数据
        /// </summary>
        public Dictionary<string, Robot> m_dictRobot = new Dictionary<string, Robot>();

        /// <summary>
        /// 从xml文件中读取定义的Data信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictRobot.Clear();
            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Robot");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    #region 遍历所有机器人
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        #region 遍历单个机器人信息
                        XmlElement xe = (XmlElement)xn;

                        int nRobotNum = Convert.ToInt32(xe.GetAttribute("序号").Trim());
                        string strRobotName = xe.GetAttribute("名称").Trim();
                        RobotVendor vendor = RobotVendor.Epson;

                        RobotComm comm = new RobotComm();

                        if (!Enum.TryParse(xe.GetAttribute("品牌").Trim(), out vendor))
                        {
                            vendor = RobotVendor.Epson;
                        }

                        comm.m_comMode = (CommMode)Convert.ToInt32(xe.GetAttribute("通信方式").Trim());
                        comm.m_nRemoteComm = Convert.ToInt32(xe.GetAttribute("远程").Trim());
                        comm.m_nManulComm = Convert.ToInt32(xe.GetAttribute("手控").Trim());
                        comm.m_nMonitorComm = Convert.ToInt32(xe.GetAttribute("监视").Trim());

                        Robot robot = new Robot(strRobotName, comm,vendor);

                        //获取远程输入IO信息
                        XmlNode xmlNode = xn.SelectSingleNode("SysIoIn");
                        #region 远程输入IO
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                string strFunction = xeItem.GetAttribute("功能描述").Trim();
                                string strIoName = xeItem.GetAttribute("点位名称").Trim();
                                ActiveLevel activeLevel = (ActiveLevel)Convert.ToInt32(xeItem.GetAttribute("有效电平").Trim());
                                bool bEnable = (Convert.ToInt32(xeItem.GetAttribute("启用").Trim()) == 1);

                                RobotSysIo info = new RobotSysIo();
                                info.m_strIoFuction = strFunction;
                                info.m_strIoName = strIoName;
                                info.m_activeLevel = activeLevel;
                                info.m_bEnable = bEnable;

                                robot.UpdateRobotIoIn(strFunction, info);
                            }
                        }
                        #endregion

                        //获取远程输出IO信息
                        xmlNode = xn.SelectSingleNode("SysIoOut");
                        #region 远程输出IO
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                string strFunction = xeItem.GetAttribute("功能描述").Trim();
                                string strIoName = xeItem.GetAttribute("点位名称").Trim();
                                ActiveLevel activeLevel = (ActiveLevel)Convert.ToInt32(xeItem.GetAttribute("有效电平").Trim());
                                int nPluseWidth = Convert.ToInt32(xeItem.GetAttribute("脉冲宽度").Trim());
                                bool bEnable = (Convert.ToInt32(xeItem.GetAttribute("启用").Trim()) == 1);

                                RobotSysIo info = new RobotSysIo();
                                info.m_strIoFuction = strFunction;
                                info.m_strIoName = strIoName;
                                info.m_activeLevel = activeLevel;
                                info.m_nPluseWidthMs = nPluseWidth;
                                info.m_bEnable = bEnable;

                                robot.UpdateRobotIoOut(strFunction, info);
                            }
                        }
                        #endregion

                        //获取输入IO信息
                        xmlNode = xn.SelectSingleNode("IoIn");
                        #region 输入IO
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                int index = Convert.ToInt32(xeItem.GetAttribute("点序号").Trim());
                                string strIoName = xeItem.GetAttribute("点位名称").Trim();

                                RobotIo info = new RobotIo();
                                info.m_nIndex = index;
                                info.m_strName = strIoName;

                                robot.UpdateRobotIoIn(strIoName, info);
                            }
                        }
                        #endregion

                        //获取输出IO信息
                        xmlNode = xn.SelectSingleNode("IoOut");
                        #region 输出IO
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                int index = Convert.ToInt32(xeItem.GetAttribute("点序号").Trim());
                                string strIoName = xeItem.GetAttribute("点位名称").Trim();

                                RobotIo info = new RobotIo();
                                info.m_nIndex = index;
                                info.m_strName = strIoName;

                                robot.UpdateRobotIoOut(strIoName, info);
                            }
                        }
                        #endregion

                        //获取POINT信息
                        xmlNode = xn.SelectSingleNode("Point");
                        #region POINT
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                if (item.NodeType != XmlNodeType.Element)
                                {
                                    continue;
                                }

                                XmlElement xeItem = (XmlElement)item;

                                int index = Convert.ToInt32(xeItem.GetAttribute("点序号").Trim());
                                string strName = xeItem.GetAttribute("点位名称").Trim();

                                RobotPoint info = new RobotPoint();
                                info.m_nIndex = index;
                                info.m_strName = strName;

                                robot.UpdateRobotPoint(strName, info);
                            }
                        }
                        #endregion

                        //获取Cmd信息
                        xmlNode = xn.SelectSingleNode("Cmd");
                        #region 命令
                        if (xmlNode != null)
                        {
                            foreach (XmlNode item in xmlNode.ChildNodes)
                            {
                                XmlElement xeItem = (XmlElement)item;

                                string strCmd = xeItem.GetAttribute("命令").Trim();
                                string strDesc = xeItem.GetAttribute("描述").Trim();
                                int nLength = Convert.ToInt32(xeItem.GetAttribute("参数数量").Trim());
                                string strRespond = xeItem.GetAttribute("回复").Trim();

                                RobotCmd cmd = new RobotCmd();
                                cmd.m_strCmd = strCmd;
                                cmd.m_strDesc = strDesc;
                                cmd.m_nLength = nLength;
                                cmd.m_strRespond = strRespond;

                                robot.UpdateRobotCmd(strCmd, cmd);
                            }
                        }
                        #endregion

                        m_dictRobot.Add(strRobotName, robot);

                        #endregion
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 更新内存参数到表格数据
        /// </summary>
        /// <param name="strRobotName"></param>
        /// <param name="gridSysIoIn"></param>
        /// <param name="gridSysIoOut"></param>
        /// <param name="gridIoIn"></param>
        /// <param name="gridIoOut"></param>
        /// <param name="gridPoint"></param>
        /// <param name="gridCmd"></param>
        public void UpdateGridFromParam(string strRobotName,
            DataGridView gridSysIoIn, DataGridView gridSysIoOut,
            DataGridView gridIoIn, DataGridView gridIoOut, 
            DataGridView gridPoint, DataGridView gridCmd)
        {
            if (!m_dictRobot.ContainsKey(strRobotName))
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(String.Format("There is no robot {0}", strRobotName));
                }
                else
                {
                    MessageBox.Show(String.Format("不存在机器人{0}", strRobotName));
                }
                return;
            }

            Robot robot = m_dictRobot[strRobotName];

            DataGridView grid;

            #region 远程IO输入
            grid = gridSysIoIn;
            grid.Rows.Clear();
            foreach (var data in robot.RobotSysIoIns.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoFuction;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoName;
                grid.Rows[nRow].Cells[nCol++].Value = (int)data.m_activeLevel;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_bEnable ? 1 : 0;
            }
            #endregion

            #region IO输出
            grid = gridSysIoOut;
            grid.Rows.Clear();
            foreach (var data in robot.RobotSysIoOuts.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoFuction;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strIoName;
                grid.Rows[nRow].Cells[nCol++].Value = (int)data.m_activeLevel;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nPluseWidthMs;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_bEnable ? 1 : 0;
            }
            #endregion

            #region IO输入
            grid = gridIoIn;
            grid.Rows.Clear();
            foreach (var data in robot.RobotIoIns.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nIndex;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;
            }
            #endregion

            #region IO输出
            grid = gridIoOut;
            grid.Rows.Clear();
            foreach (var data in robot.RobotIoOuts.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nIndex;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;
            }
            #endregion

            #region POINT
            grid = gridPoint;
            grid.Rows.Clear();
            foreach (var data in robot.RobotPoints.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nIndex;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strName;
            }
            #endregion

            #region 命令
            grid = gridCmd;
            grid.Rows.Clear();
            foreach (var data in robot.RobotCmds.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strCmd;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strDesc;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_nLength;
                grid.Rows[nRow].Cells[nCol++].Value = data.m_strRespond;
            }
            #endregion
        }


        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="strRobotName"></param>
        /// <param name="comm"></param>
        /// <param name="vendor"></param>
        /// <param name="gridSysIoIn"></param>
        /// <param name="gridSysIoOut"></param>
        /// <param name="gridIoIn"></param>
        /// <param name="gridIoOut"></param>
        /// <param name="gridPoint"></param>
        /// <param name="gridCmd"></param>
        public void UpdateParamFromGrid(string strRobotName,RobotComm comm,int vendor,
            DataGridView gridSysIoIn, DataGridView gridSysIoOut,
            DataGridView gridIoIn, DataGridView gridIoOut,
            DataGridView gridPoint, DataGridView gridCmd)
        {
            if (!m_dictRobot.ContainsKey(strRobotName))
            {
                m_dictRobot.Add(strRobotName, new Robot(strRobotName, comm,(RobotVendor)vendor));
            }

            Robot robot = m_dictRobot[strRobotName];

            robot.Comm = comm;
            robot.Vendor = (RobotVendor)vendor;

            #region 远程IO输入
            robot.RobotSysIoIns.Clear();
            foreach (DataGridViewRow row in gridSysIoIn.Rows)
            {
                RobotSysIo info = new RobotSysIo();
                int nCol = 0;

                try
                {
                    info.m_strIoFuction = row.Cells[nCol++].Value.ToString();

                    info.m_strIoName = row.Cells[nCol++].Value.ToString();

                    info.m_activeLevel = (ActiveLevel)Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    info.m_bEnable = Convert.ToInt32(row.Cells[nCol++].Value.ToString()) == 1;

                    robot.UpdateRobotIoIn(info.m_strIoFuction, info);
                }
                catch (Exception ex)
                {
                }

            }
            #endregion

            #region 远程IO输出
            robot.RobotSysIoOuts.Clear();
            foreach (DataGridViewRow row in gridSysIoOut.Rows)
            {
                RobotSysIo info = new RobotSysIo();
                int nCol = 0;

                try
                {
                    info.m_strIoFuction = row.Cells[nCol++].Value.ToString();

                    info.m_strIoName = row.Cells[nCol++].Value.ToString();

                    info.m_activeLevel = (ActiveLevel)Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    info.m_nPluseWidthMs = Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    info.m_bEnable = Convert.ToInt32(row.Cells[nCol++].Value.ToString()) == 1;

                    robot.UpdateRobotIoOut(info.m_strIoFuction, info);

                }
                catch (Exception ex)
                {

                }

            }
            #endregion

            #region IO输入
            robot.RobotIoIns.Clear();
            foreach (DataGridViewRow row in gridIoIn.Rows)
            {
                RobotIo info = new RobotIo();
                int nCol = 0;

                try
                {
                    info.m_nIndex = Convert.ToInt32(row.Cells[nCol++].Value.ToString());
                    info.m_strName = row.Cells[nCol++].Value.ToString();

                    robot.UpdateRobotIoIn(info.m_strName, info);
                }
                catch
                {

                }
                
            }
            #endregion

            #region IO输出
            robot.RobotIoOuts.Clear();
            foreach (DataGridViewRow row in gridIoOut.Rows)
            {
                RobotIo info = new RobotIo();
                int nCol = 0;

                try
                {
                    info.m_nIndex = Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    info.m_strName = row.Cells[nCol++].Value.ToString();

                    robot.UpdateRobotIoOut(info.m_strName, info);

                }
                catch
                {

                }

            }
            #endregion

            #region POINT
            robot.RobotPoints.Clear();
            foreach (DataGridViewRow row in gridPoint.Rows)
            {
                RobotPoint info = new RobotPoint();
                int nCol = 0;

                try
                {
                    info.m_nIndex = Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    info.m_strName = row.Cells[nCol++].Value.ToString();

                    robot.UpdateRobotPoint(info.m_strName, info);

                }
                catch
                {

                }

            }
            #endregion

            #region 命令
            robot.RobotCmds.Clear();
            foreach (DataGridViewRow row in gridCmd.Rows)
            {
                RobotCmd cmd = new RobotCmd();
                int nCol = 0;

                try
                {
                    cmd.m_strCmd = row.Cells[nCol++].Value.ToString();

                    cmd.m_strDesc = row.Cells[nCol++].Value.ToString();

                    cmd.m_nLength = Convert.ToInt32(row.Cells[nCol++].Value.ToString());

                    cmd.m_strRespond = row.Cells[nCol++].Value.ToString();

                    robot.UpdateRobotCmd(cmd.m_strCmd, cmd);

                }
                catch
                {

                }

            }
            #endregion
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");

            XmlNode root = xnl.SelectSingleNode("Robot");
            if (root == null)
            {
                root = doc.CreateElement("Robot");

                xnl.AppendChild(root);
            }

            root.RemoveAll();

            int nRobot = 1;
            foreach (var robot in m_dictRobot.Values)
            {
                XmlElement xeRobot = doc.CreateElement("Robot");

                xeRobot.SetAttribute("序号", (nRobot++).ToString());
                xeRobot.SetAttribute("名称", robot.RobotName);
                xeRobot.SetAttribute("品牌", robot.Vendor.ToString());
                xeRobot.SetAttribute("通信方式", ((int)robot.Comm.m_comMode).ToString());
                xeRobot.SetAttribute("远程", robot.Comm.m_nRemoteComm.ToString());
                xeRobot.SetAttribute("手控", robot.Comm.m_nManulComm.ToString());
                xeRobot.SetAttribute("监视", robot.Comm.m_nMonitorComm.ToString());

                #region 远程IO输入
                XmlNode xn = doc.CreateElement("SysIoIn");
                foreach (var data in robot.RobotSysIoIns)
                {
                    XmlElement xe = doc.CreateElement("SysIoIn");

                    xe.SetAttribute("功能描述", data.Value.m_strIoFuction);
                    xe.SetAttribute("点位名称", data.Value.m_strIoName);
                    xe.SetAttribute("有效电平", ((int)data.Value.m_activeLevel).ToString());
                    xe.SetAttribute("启用", data.Value.m_bEnable ? "1" : "0");

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion

                #region 远程IO输出
                xn = doc.CreateElement("SysIoOut");
                foreach (var data in robot.RobotSysIoOuts)
                {
                    XmlElement xe = doc.CreateElement("SysIoOut");

                    xe.SetAttribute("功能描述", data.Value.m_strIoFuction);
                    xe.SetAttribute("点位名称", data.Value.m_strIoName);
                    xe.SetAttribute("有效电平", ((int)data.Value.m_activeLevel).ToString());
                    xe.SetAttribute("脉冲宽度", data.Value.m_nPluseWidthMs.ToString());
                    xe.SetAttribute("启用", data.Value.m_bEnable ? "1" : "0");

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion

                #region IO输入
                xn = doc.CreateElement("IoIn");
                foreach (var data in robot.RobotIoIns)
                {
                    XmlElement xe = doc.CreateElement("IoIn");

                    xe.SetAttribute("点序号", data.Value.m_nIndex.ToString());
                    xe.SetAttribute("点位名称", data.Value.m_strName);

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion

                #region IO输出
                xn = doc.CreateElement("IoOut");
                foreach (var data in robot.RobotIoOuts)
                {
                    XmlElement xe = doc.CreateElement("IoOut");

                    xe.SetAttribute("点序号", data.Value.m_nIndex.ToString());
                    xe.SetAttribute("点位名称", data.Value.m_strName);

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion

                #region POINT
                xn = doc.CreateElement("Point");
                foreach (var data in robot.RobotPoints)
                {
                    XmlElement xe = doc.CreateElement("Point");

                    xe.SetAttribute("点序号", data.Value.m_nIndex.ToString());
                    xe.SetAttribute("点位名称", data.Value.m_strName);

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion

                #region 命令
                xn = doc.CreateElement("Cmd");
                foreach (var data in robot.RobotCmds)
                {
                    XmlElement xe = doc.CreateElement("Cmd");

                    xe.SetAttribute("命令", data.Value.m_strCmd);
                    xe.SetAttribute("描述", data.Value.m_strDesc);
                    xe.SetAttribute("参数数量", data.Value.m_nLength.ToString());
                    xe.SetAttribute("回复", data.Value.m_strRespond);

                    xn.AppendChild(xe);
                }
                xeRobot.AppendChild(xn);
                #endregion


                root.AppendChild(xeRobot);
            }
        }

        /// <summary>
        /// 获取机器人对象
        /// </summary>
        /// <param name="strRobotName"></param>
        /// <returns></returns>
        public Robot GetRobot(string strRobotName)
        {
            try
            {
                return m_dictRobot[strRobotName];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());

                return null;
            }
        }
    }
}
