using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AutoFrameDll;
using CommonTool;
using ToolEx;
using Communicate;
using System.Reflection;

namespace AutoFrame
{
    class StationTemplate : StationEx
    {  
        enum POINT
        {
            安全点=1,
        }

        private AsyncSocketTCPServer m_tcpServer;

        /// <summary>
        /// 构造函数，需要设置站位当前的IO输入，IO输出，轴方向及轴名称，以显示在手动页面方便操作
        /// </summary>
        /// <param name="strName"></param>
        public StationTemplate(string strName) : base(strName)
        {
            //配置站位界面显示输入
            io_in = new string[] { };
            //配置站位界面显示输出
            io_out = new string[] { };
            //配置站位界面显示气缸
            m_cylinders = new string[] { };

            //配置手动调试时的伺服的运动方向，如果发现界面上的方向和实际的方向相反，则把对应的轴的方向改为false
            InverseAxisPositiveByAxisNo(AxisZ);

            //给轴重命名
            RenameAxisName(0, "NewX");

            //配置此站中用到的机器人，如果没有则不需要配置
            m_Robot = RobotMgrEx.GetInstance().GetRobot("xxx机器人");

            m_tcpServer = TcpServerMgr.GetInstance().GetTcpServer(0);

            m_tcpServer.DataReceived += M_tcpServer_DataReceived;

        }

        private void M_tcpServer_DataReceived(object sender, AsyncSocketEventArgs e)
        {
            string strData;
            strData = Encoding.Default.GetString(e.m_state.RecvDataBuffer, 0, e.m_state.Length);

            string[] strSlits = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < strSlits.Length;i++)
            {
                if (strSlits[i].Contains("SN"))
                {
                    //
                }
                else if (strSlits[i].Contains("TIME"))
                {
                    //
                }
            }
        }

        /// <summary>
        /// 站位初始化，用来添加伺服上电，打开网口，站位回原点等动作
        /// </summary>
        public override void StationInit()
        {

            //设置位寄存器状态
            SetBit(SysBitReg.xxx, false);

            //气缸回位
            CylBack("xxx气缸");

            //伺服使能
            AxisEnable(true);

            //伺服回原点，可以指定优先级
            AxisHome(MotionPriority.Z_FIRST);

            //伺服运动到点位，可以指定优先级
            AxisGoTo((int)POINT.安全点, MotionPriority.Z_FIRST);

            //机器人初始化，如果有其他站位公用此机器人，需要判断站位是否使能，并把以某个站位为主
            if (!m_Robot.IsInited)
            {
                if (!m_Robot.InitRobot())
                {
                    ShowLog("xxx机器人初始化失败");
                    WarningMgr.GetInstance().Error(ErrorType.Err_Robot, "xxx机器人", "xxx机器人初始化失败");
                }
            }

            m_tcpServer.Start();

            ShowLog("初始化完成");
            
        }
        /// <summary>
        /// 站位退出退程时调用，用来关闭伺服，关闭网口等动作
        /// </summary>
        public override void StationDeinit()
        {
            m_tcpServer.Stop();
        }

         //当所有站位均为全自动运行模式时，不需要重载该函数
        //当所有站位为半自动运行模式时，也不需要重载该函数， 只需要在站位流程开始时插入WaitBegin()即可保证所有站位同步开始。
        //当所有站位中，有的为半自动，有的为全自动时，半自动的站位不重载该函数，使用WaitBegin()控制同步，全自动的站位重载该函数返回true即可。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public override bool IsReady()
        //{
        //    return true;
        //}


        /// <summary>
        /// 初始化时设置站位安全状态，用来设备初始化时各站位之间相互卡站，
        /// 控制各站位先后动作顺序用，比如流水线组装，肯定需要组装的Z轴升
        /// 起后，流水线才能动作这种情况
        /// </summary>
        public override void InitSecurityState()
        {
            //对于绝对值式的伺服驱动器，由于没有正负限位和原点，采用软件限位
            ConfigSoftLimit();
        }

        /// <summary>
        /// 正常运行
        /// </summary>
        protected override void NormalRun()
        {
            
        }

        /// <summary>
        /// 空跑
        /// </summary>
        protected override void DryRun()
        {
            
        }

        /// <summary>
        /// 自动标定
        /// </summary>
        protected override void AutoCalib()
        {
            string strCalib = SystemMgrEx.GetInstance().CurrentCalib;

            ShowMessage(string.Format("开始自动标定 - {0},按启动键开始", strCalib),true);

            WaitIo("启动", true,-1);

            RunModeInfo info;
            if (SystemMgrEx.GetInstance().m_dictCalibs.TryGetValue(strCalib, out info))
            {
                MethodInfo method = GetType().GetMethod(info.m_strMethod);

                if (method != null)
                {
                    method.Invoke(this, null);
                }
                else
                {
                    ShowMessage("标定方法错误，请确认", true);
                }
            }

            ShowLog("标定完成");
            ShowMessage("标定完成", true);
        }

        /// <summary>
        /// GRR验证
        /// </summary>
        protected override void GrrRun()
        {
            string strGRR = SystemMgrEx.GetInstance().CurrentGrr;

            RunModeInfo info;
            if (SystemMgrEx.GetInstance().m_dictGrrs.TryGetValue(strGRR, out info))
            {
                MethodInfo method = GetType().GetMethod(info.m_strMethod);

                if (method != null)
                {
                    method.Invoke(this, null);
                }
                else
                {
                    ShowMessage("GRR方法错误，请确认", true);
                }
            }
        }

    }
}
