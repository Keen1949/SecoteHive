//2019-05-07 Binggoo 1.加入开机启动画面。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;
using AutoFrameDll;
using AutoFrameVision;
using System.Xml;
using Communicate;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    class AutoTool
    {
        public AutoTool()
        {

        }

        public static bool ConfigAll()
        {
            string str1 = "系统参数配置文件名获取失败\n将启用默认文件systemParam";
            string str2 = "系统初始化错误";
            string str3 = "加载系统配置。。。";
            string str4 = "加载扩展系统配置。。。";
            string str5 = "加载系统参数。。。";
            string str6 = "加载日志配置。。。";
            string str7 = "加载点位配置。。。";
            string str8 = "加载站位配置。。。";
            string str9 = "加载视觉配置。。。";
            string str10 = "系统配置文件systemCfg.xml加载失败";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "System parameter configuration file name acquisition failed \n will enable the default file systemparam";
                str2 = "System initialization error";
                str3 = "Load system configuration...";
                str4 = "Load extended system configuration...";
                str5 = "Loading system parameters...";
                str6 = "Load log configuration...";
                str7 = "Load point configuration...";
                str8 = "Load station configuration...";
                str9 = "Load vision configuration...";
                str10 = "System configuration file systemcfg.xml failed to load";
            }

            try
            {


                ErrorCodeMgr.GetInstance();
                //先初始化报警类,避免后续其它类初始化时异常,造成循环调用.
                WarningMgr.GetInstance();
                //初始化设置为OP权限
                //     Security.ChangeOpMode();
                //最先在systemCfg.xml文件中得到系统参数配置文件名
                if (!SystemMgr.GetInstance().GetSystemParamName("systemCfg.xml"))
                {
                    MessageBox.Show(str1, str2, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SystemMgr.GetInstance().AppendSystemParamName("systemCfg.xml");
                }
                //开始读取配置
                FormSplash.UpdateUI(str3, 10);
                if (ConfigMgr.GetInstance().LoadCfgFile("SystemCfg.xml"))
                {
                    FormSplash.UpdateUI(str4, 20);
                    LoadSystemCfgEx();

                    //读取系统参数配置, 站位点位文件
                    FormSplash.UpdateUI(str5, 30);
                    SystemMgr.GetInstance().LoadParamFile(/*"systemParam.xml"*/SystemMgr.GetInstance().m_strSystemParamName);
                    //站位点位文件的加载需要放到产品信息加载之后,通过AutoTool的响应事件加载               
                    //     StationMgr.GetInstance().LoadPointFile("Point.xml");

                    //读取日志系统配置
                    FormSplash.UpdateUI(str6, 40);
                    WarningMgr.GetInstance().ReadXmlConfig("Log4Net.config");
                    //读取产品配置信息
                    //FormSplash.UpdateUI(str7, 50);
                    StationMgr.GetInstance().LoadPointFile("Point.xml");

                    //加入自定义站位类和视觉流程类
                    FormSplash.UpdateUI(str8, 60);
                    AddStation();

                    //FormSplash.UpdateUI(str9, 70);
                    // AddVisionStep();

                    ManulSafeMgr.GetInstance();

                    //自行决定是否需要从资源文件中追加配置
                    //AppendConfigFromResource();

                    return true;
                }
                else
                {
                    MessageBox.Show(str10, str2, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), str2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        ///向站位管理器添加自定议的站位,同时为站位类关联手动调试窗口界面类 
        /// </summary>
        public static void AddStation()
        {
            //StationMgr.GetInstance().AddStation(new Form_sta_templete_ex(), new StationGlue("点胶站"));
            //StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationRotTable("转盘站"));

            //StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationLoad("机械手站"));
            if (SystemMgr.GetInstance().GetParamInt("HiveMode")==0)//单台PLC设备（CSV）
            {
                StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationCSV("PLC1"));
            }
            else if (SystemMgr.GetInstance().GetParamInt("HiveMode") == 1)//单台PLC设备（TCP）
            {
                StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationTcp("PLC1"));
            }
            else if (SystemMgr.GetInstance().GetParamInt("HiveMode") == 2)//多台PLC设备（CSV）
            {
                StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationCSV("PLC1"));
                StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationCSV("PLC2"));
            }
            StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationDowntime("Downtime"));
            StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationParam("Param"));
            StationMgrEx.GetInstance().AddStation(1, new Form_sta_templete_ex(), new StationTossing("Tossing"));

        }

        /// <summary>
        /// 向视觉管理器添加自定义的步骤,同时为其绑定指定的相机采集类 
        /// </summary>
        public static void AddVisionStep()
        {
            //加入一个相机采集类, 加入步骤前必须先添加相机采集
            //VisionMgr.GetInstance().AddCamera(new CameraBaumer("Cam_0"));
            //VisionMgr.GetInstance().AddCamera(new CameraGige("Cam_2"));
            //VisionMgr.GetInstance().AddCamera(new CameraGige("Cam_3"));
            //VisionMgr.GetInstance().AddCamera(new CameraGige("Cam_4"));
            //VisionMgr.GetInstance().AddCamera(new CameraGige("Cam_5"));

            // 文件采集类示例
            string strImagePath = AppDomain.CurrentDomain.BaseDirectory + "image";
            //VisionMgr.GetInstance().AddCamera(new CameraFile(strImagePath));
            //VisionMgr.GetInstance().AddVisionStep(strImagePath, new Vision_T1("T1"));
            //VisionMgr.GetInstance().AddVisionStep(strImagePath, new Vision_Std("T2"));

            //加入视觉步骤类, 并将其绑定到指定的相机采集类上
            //VisionMgr.GetInstance().AddVisionStep("Cam_1", new Vision_CalibT1("CalibT1"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_1", new Vision_T1_1("T1_1"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_1", new Vision_T1_2("T1_2"));

            //VisionMgr.GetInstance().AddVisionStep("Cam_2", new Vision_CalibT2("CalibT2"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_2", new Vision_T2("T2"));

            //VisionMgr.GetInstance().AddVisionStep("Cam_3", new Vision_CalibT3("CalibT3"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_3", new Vision_T3_1("T3_1"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_3", new Vision_T3_2("T3_2"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_3", new Vision_T3_3("T3_3"));

            //VisionMgr.GetInstance().AddVisionStep("Cam_4", new Vision_CalibT4("CalibT4"));
            //VisionMgr.GetInstance().AddVisionStep("Cam_4", new Vision_T4("T4"));

        }

        /// <summary>
        /// 初始化硬件
        /// </summary>
        public static void InitSystem()
        {
            Action<object> action = (object obj) =>
            {
                string str1 = "初始化运动板卡。。。";
                string str2 = "初始化IO板卡。。。";
                string str3 = "程序初始化完成";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "Initialize the motion board...";
                    str2 = "Initialize IO board...";
                    str3 = "Program initialization complete";
                }
                //延迟一秒初始化硬件，便于窗口接收初始化异常
                Task.Delay(1000);
                FormSplash.UpdateUI(str1, 80);
                MotionMgr.GetInstance().InitAllCard();

                FormSplash.UpdateUI(str2, 90);
                IoMgr.GetInstance().InitAllCard();
                //PlcMgr.GetInstance().InitAllPlc();

                FormSplash.UpdateUI(str3, 100);
                //系统管理器线程，负责清除过期文件数据，检查系统是否空闲无操作
                //SystemMgr.GetInstance().StartMonitor();

                //如果需要程序一启动就给轴上电，可以在此处添加代码
                //todo: tcp, com, vision
            };
            Task t1 = new Task(action, "");
            t1.Start();
        }

        public static void DeinitSystem()
        {
            LightMgr.GetInstance().DeInit();

            MotionMgr.GetInstance().DeinitAllCard();
            IoMgr.GetInstance().DeinitAllCard();

            //SystemMgr.GetInstance().StopMonitor();
            //todo: tcp, com, vision
            //PlcMgr.GetInstance().StopMonitor();
            //PlcMgr.GetInstance().DeinitAllPlc();
        }

        /// <summary>
        /// INI文件操作样板函数
        /// </summary>
        private void TestIniOperation()
        {

            string file = "F:\\TestIni.ini";


            //写入/更新键值
            IniOperation.WriteValue(file, "Desktop", "Color", "Red");
            IniOperation.WriteValue(file, "Desktop", "Width", "3270");

            IniOperation.WriteValue(file, "Toolbar", "Items", "Save,Delete,Open");
            IniOperation.WriteValue(file, "Toolbar", "Dock", "True");

            //写入一批键值
            IniOperation.WriteItems(file, "Menu", "File=文件\0View=视图\0Edit=编辑");

            //获取文件中所有的节点
            string[] sections = IniOperation.GetAllSectionNames(file);

            //获取指定节点中的所有项
            string[] items = IniOperation.GetAllItems(file, "Menu");

            //获取指定节点中所有的键
            string[] keys = IniOperation.GetAllItemKeys(file, "Menu");

            //获取指定KEY的值
            string value = IniOperation.GetStringValue(file, "Desktop", "color", null);

            //删除指定的KEY
            IniOperation.DeleteKey(file, "desktop", "color");

            //删除指定的节点
            IniOperation.DeleteSection(file, "desktop");

            //清空指定的节点
            IniOperation.EmptySection(file, "toolbar");

        }

        /// <summary>
        /// 加载扩展配置，此步主要用于框架之外的配置
        /// </summary>
        public static void LoadSystemCfgEx()
        {
            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            if (File.Exists(cfg))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfg);

                //OpcMgr.GetInstance().ReadCfgFromXml(doc);
                DataMgr.GetInstance().ReadCfgFromXml(doc);
                RobotMgrEx.GetInstance().ReadCfgFromXml(doc);
                LightMgr.GetInstance().ReadCfgFromXml(doc);
                CylinderMgr.GetInstance().ReadCfgFromXml(doc);
                SystemMgrEx.GetInstance().ReadCfgFromXml(doc);
                //TcpServerMgr.GetInstance().ReadCfgFromXml(doc);
            }
        }

        public static void SaveSystemCfgEx()
        {
            string cfg = Application.StartupPath + "\\SystemCfgEx.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(cfg);

            OpcMgr.GetInstance().SaveCfgXML(doc);

            doc.Save(cfg);
        }

        //public static void AppendConfigFromResource()
        //{
        //    //2020-05-04 Binggoo 从资源文件中追加系统配置
        //    if (SystemMgr.GetInstance().AppendParamFromResource(SystemResource.systemParam))
        //    {
        //        //合并成最新的配置文件
        //        SystemMgr.GetInstance().SaveParamFile(SystemMgr.GetInstance().m_strSystemParamName);
        //    }

        //    //2020-05-04 Binggoo 从资源文件中加载点位
        //    if (StationMgr.GetInstance().AppendPointFromResource(SystemResource.Point))
        //    {
        //        //保存点位
        //        StationMgr.GetInstance().SavePointFile();
        //    }

        //    //从资源文件中加载
        //    DataMgr.GetInstance().AppendDataCfgFromResource(SystemResource.SystemCfgEx);

        //}
    }
}
