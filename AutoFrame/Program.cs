//2019-06-03 Binggoo 1.加入程序运行参数，用于切换用户，方便调试。
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolEx;
using CommonTool;
using System.Text.RegularExpressions;

namespace AutoFrame
{
    static class Program
    {      
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
      
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //一个进程只打开一次
            Process process = HelpTool.GetCurrentInstance();
            if (process != null)
            {
                if (HelpTool.IsIconic(process.MainWindowHandle))
                {
                    HelpTool.ShowWindowAsync(process.MainWindowHandle, HelpTool.SW_RESTORE);
                }

                HelpTool.SetForegroundWindow(process.MainWindowHandle);

                Application.Exit();

                return;
            }

            SystemMgr.GetInstance();

            string str1 = "开始加载系统......";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Start loading system...";
            }
            FormSplash.UpdateUI(str1, 0);

            if (args.Length > 1)
            {
                switch (args[0])
                {
                    case "1":
                        Security.ChangeFaeMode(args[1]);
                        break;
                    case "2":
                        Security.ChangeAdjustorMode(args[1]);
                        break;
                    case "3":
                        Security.ChangeEngMode(args[1]);
                        break;
                }
                
            }

            if (AutoTool.ConfigAll())
            {
                AutoTool.InitSystem();

                Application.Run(new Form_Main());

                AutoTool.DeinitSystem();
            }
                        
        }
    }
}
