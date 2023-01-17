using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Windows.Forms;

namespace AutoFrame
{
    enum controller_name
    {
        Load,
    }
    class Robot_ABB
    {

        private static NetworkScanner scanner;
        private static Controller[] controller=new Controller[1];
      
        public Robot_ABB()
        {

        }
        public static void Init_All_Robot()
        {
            if (scanner == null)
            {
                scanner = new NetworkScanner();
            }
            scanner.Scan();

            ControllerInfoCollection controls = scanner.Controllers;

            foreach (ControllerInfo info in controls)
            {
                if (info.ControllerName == "Load")
                {
                    if (info.Availability == Availability.Available)
                    {
                        if (controller[(int)controller_name.Load] != null)
                        {
                            controller[(int)controller_name.Load].Logoff();
                            controller[(int)controller_name.Load].Dispose();
                            controller[(int)controller_name.Load] = null;
                        }
                        controller[(int)controller_name.Load] = ControllerFactory.CreateFrom(info);                  
                    }
                }
            }

            foreach (Controller ctl in controller)
            {
                Controller ctl1 = ctl;
                if (ctl == null)
                    MessageBox.Show("未查找到机器人控制器，请检查！");
            }
        }

        public static bool Robot_Start(int index)
        {
            LoginToController(index);
            Thread.Sleep(100);
            Robot_Stop(index);
            Thread.Sleep(100);
            PPToMain(index);
            Thread.Sleep(100);
            MotorsOn(index);
            Thread.Sleep(100);
            return Start(index);
        }

        public static bool Robot_Resume(int index)
        {
            return Start(index);
        }

        private static void LoginToController(int index)
        {
            if(controller[index] != null)
                controller[index].Logon(UserInfo.DefaultUser);
        }

        private static void LogoffToController(int index)
        {
            if (controller[index] != null)
            {
                controller[index].Logoff();
                controller[index].Dispose();
            }
        }

        private static void MotorsOn(int index)
        {
            if(controller[index]!=null)
            {
                if( controller[index].State == ControllerState.MotorsOn)
                {

                }
                else
                {
                    controller[index].State = ControllerState.MotorsOn;
                }                
            }
        }

        private static void PPToMain(int index)
        {
            if (controller[index] != null)
            {
                if(controller[index].State == ControllerState.MotorsOff)
                {
                    UserAuthorizationSystem uas = controller[index].AuthenticationSystem;

                    if (uas.CheckDemandGrant(Grant.ExecuteRapid))
                    {
                        using (Mastership.Request(controller[index].Rapid))
                        {
                            ABB.Robotics.Controllers.RapidDomain.Task[] tasks = controller[index].Rapid.GetTasks();
                            tasks[0].ResetProgramPointer();
                        }
                    }
                }                
            }
        }



        private static bool Start(int index)
        {
            if (controller[index] != null)
            {
                UserAuthorizationSystem uas = controller[index].AuthenticationSystem;

                if (uas.CheckDemandGrant(Grant.ExecuteRapid))
                {
                    using (Mastership.Request(controller[index].Rapid))
                    {
                        if(StartResult.Ok == controller[index].Rapid.Start())
                          return true;
                    }
                }
            }
            return false;
        }

        public static void Robot_Pause(int index)
        {
            if (controller[index] != null)
            {
                UserAuthorizationSystem uas = controller[index].AuthenticationSystem;
                if (uas.CheckDemandGrant(Grant.ExecuteRapid))
                {
                    using (Mastership.Request(controller[index].Rapid))
                    {
                        controller[index].Rapid.Stop(StopMode.Immediate);
                    }
                }
            }
        }

        public static void Robot_Stop(int index)
        {
            try
            {
                if (controller[index] != null)
                {
                    UserAuthorizationSystem uas = controller[index].AuthenticationSystem;
                    if (uas.CheckDemandGrant(Grant.ExecuteRapid))
                    {
                        //using (Mastership.Request(controller[index].Rapid))
                        {
                            controller[index].Rapid.Stop(StopMode.Immediate);
                            try
                            {
                                controller[index].State = ControllerState.MotorsOff;
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch
            { }
        }
    }
}
