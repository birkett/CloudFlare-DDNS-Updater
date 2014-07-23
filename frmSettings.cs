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
            txtDomainName.Text = Properties.Settings.Default.Domain;
            txtEmailAddress.Text = Properties.Settings.Default.EmailAddress;
            txtAPIKey.Text = Properties.Settings.Default.APIKey;
            txtFetchTime.Text = Properties.Settings.Default.AutoFetchTime;
        }

        /// <summary>
        /// Save the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Domain = txtDomainName.Text;
            Properties.Settings.Default.EmailAddress = txtEmailAddress.Text;
            Properties.Settings.Default.APIKey = txtAPIKey.Text;
            Properties.Settings.Default.AutoFetchTime = txtFetchTime.Text;
            Properties.Settings.Default.Save();
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
