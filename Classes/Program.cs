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
        public static System.Globalization.CultureInfo cultureInfo;


        /// <summary>
        /// Global instance of the settingsManager
        /// </summary>
        public static SettingsManager settingsManager;


        /// <summary>
        /// Global instance of the CloudFlareAPI class
        /// </summary>
        public static CloudFlareAPI cloudFlareAPI;


        /// <summary>
        /// The main entry point for the application.
        /// This will decide if to run the service, GUI, or install / uninstall helpers for the service
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Set up the crash dump handler.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(unhandledException);

            cultureInfo = new System.Globalization.CultureInfo("en-GB");
            settingsManager = new SettingsManager();
            cloudFlareAPI = new CloudFlareAPI();

            if (args.Length > 0)
            {
                if (args[0] == "/service")
                {
                    runService();
                    return;
                }

                if (args[0] == "/install")
                {
                    if(!isAdmin)
                    {
                        AttachConsole( -1 /*ATTACH_PARENT_PROCESS*/ );
                        Console.WriteLine("Need to be running from an elevated (Administrator) command prompt.");
                        FreeConsole();
                        return;
                    }

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
                    if (!isAdmin)
                    {
                        AttachConsole(-1 /*ATTACH_PARENT_PROCESS*/ );
                        Console.WriteLine("Need to be running from an elevated (Administrator) command prompt.");
                        FreeConsole();
                        return;
                    }

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
        /// Call into kernel32.dll for AttachConsole()
        /// </summary>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);


        /// <summary>
        /// Call into kernel32.dll for FreeConsole()
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();


        /// <summary>
        /// Call into dbghelp.dll for MiniDumpWriteDump()
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="ProcessId"></param>
        /// <param name="hFile"></param>
        /// <param name="DumpType"></param>
        /// <param name="ExceptionParam"></param>
        /// <param name="UserStreamParam"></param>
        /// <param name="CallackParam"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("dbghelp.dll")]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, int DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);


        /// <summary>
        /// Create a dump file when crashing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void unhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            long unixTimestamp = DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;

            string filename = "ExceptionDump-" + unixTimestamp + ".dmp";

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
            {
                using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    MiniDumpWriteDump(process.Handle, process.Id, fs.SafeFileHandle.DangerousGetHandle(), 0x00000002, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }//end unhandledException()


        /// <summary>
        /// Check if he program is running as Administrator
        /// </summary>
        public static bool isAdmin
        {
            get
            {
                return new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Run the application with the GUI
        /// </summary>
        static void runGUI()
        {
            Logger.log(Properties.Resources.Logger_RunGUI, Logger.Level.Info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

        }//end runGUI()


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
