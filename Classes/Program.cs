using System;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Configuration.Install;

namespace CloudFlareDDNS
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
                if (args[0] == "/service")
                {
                    runService();
                    return;
                }
                   
                if (args[0] == "/install")
                {
                    TransactedInstaller ti = new TransactedInstaller();
                    ti.Installers.Add(new ServiceInstaller());
                    ti.Context = new InstallContext("", null);
                    ti.Context.Parameters["assemblypath"] = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" /service";
                    ti.Install(new System.Collections.Hashtable());
                    return;
                }

                if (args[0] == "/uninstall")
                {
                    TransactedInstaller ti = new TransactedInstaller();
                    ti.Installers.Add(new ServiceInstaller());
                    ti.Context = new System.Configuration.Install.InstallContext("", null);
                    ti.Context.Parameters["assemblypath"] = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" /service";
                    ti.Uninstall(null);
                    return;
                }
            }

            runGUI();
            return;

        }//end Main()


        /// <summary>
        /// Run the application with the GUI
        /// </summary>
        static void runGUI()
        {
            Logger.log("Starting CloudFlare DDNS updater GUI", Logger.Level.Info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

        }// end runGUI()


        /// <summary>
        /// Run the application silently as a service
        /// </summary>
        static void runService()
        {
            Logger.log("Starting CloudFlare DDNS updater service", Logger.Level.Info);
            ServiceBase.Run(new Service());

        }//end runService()


    }//end class
}//end namespace
