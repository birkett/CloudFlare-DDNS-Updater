/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2014-2015 Anthony Birkett
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Configuration.Install;

namespace CloudFlareDDNS
{


    /// <summary>
    /// Program entry logic
    /// </summary>
    static class Program
    {


        /// <summary>
        /// Used for localising strings and conversions.
        /// </summary>
        public static System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-GB");


        /// <summary>
        /// The main entry point for the application.
        /// This will decide if to run the service, GUI, or install / uninstall helpers for the service
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
                    ti.Dispose();
                    return;
                }

                if (args[0] == "/uninstall")
                {
                    TransactedInstaller ti = new TransactedInstaller();
                    ti.Installers.Add(new ServiceInstaller());
                    ti.Context = new System.Configuration.Install.InstallContext("", null);
                    ti.Context.Parameters["assemblypath"] = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" /service";
                    ti.Uninstall(null);
                    ti.Dispose();
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
            Logger.log(Properties.Resources.Logger_RunGUI, Logger.Level.Info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

        }// end runGUI()


        /// <summary>
        /// Run the application silently as a service
        /// </summary>
        static void runService()
        {
            Logger.log(Properties.Resources.Logger_RunService, Logger.Level.Info);
            ServiceBase.Run(new Service());

        }//end runService()


    }//end class
}//end namespace
