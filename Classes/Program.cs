using System;
using System.Windows.Forms;
using System.ServiceProcess;

namespace CloudFlare_DDNS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "-service")
                {
                    RunService();
                    return;
                }
            }

            RunGUI();
            return;
        }

        static void RunGUI()
        {
            Logger.log("Starting CloudFlare DDNS updater GUI", Logger.Level.Info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void RunService()
        {
            Logger.log("Starting CloudFlare DDNS updater service", Logger.Level.Info);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
