using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudFlare_DDNS
{
    /// <summary>
    /// Settings form - basic configuration 
    /// </summary>
    public partial class frmSettings : Form
    {
        /// <summary>
        /// Form entry point
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the saved values on open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtDomainName.Text = SettingsManager.getSetting("Domain");
            txtEmailAddress.Text = SettingsManager.getSetting("EmailAddress");
            txtAPIKey.Text = SettingsManager.getSetting("APIKey");
            txtFetchTime.Text = SettingsManager.getSetting("FetchTime");
        }

        /// <summary>
        /// Save the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            SettingsManager.setSetting("Domain", txtDomainName.Text);
            SettingsManager.setSetting("EmailAddress", txtEmailAddress.Text);
            SettingsManager.setSetting("APIKey", txtAPIKey.Text);
            SettingsManager.setSetting("FetchTime", txtFetchTime.Text);
            SettingsManager.saveSettings();
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
