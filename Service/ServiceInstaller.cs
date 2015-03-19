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
            InitializeComponent();
        }
    }
}
