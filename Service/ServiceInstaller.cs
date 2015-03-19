using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Handle the installation of the service
    /// </summary>
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceInstaller()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "CloudFlare Dynamic DNS Client";
            this.serviceInstaller1.DisplayName = "CloudFlare DDNS";
            this.serviceInstaller1.ServiceName = "CloudFlareDDNS";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;


    }
}
