using System;
using System.Windows.Forms;

namespace CloudFlare_DDNS
{
    /// <summary>
    /// About box form
    /// </summary>
    public partial class frmAbout : Form
    {
        /// <summary>
        /// Form entry point
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();

            rtfAbout.Text = "CloudFlare Dynamic DNS client for Windows - Version 1.0.0.0\n"
                           + "Copyright © Anthony Birkett 2014\n";
        }

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
