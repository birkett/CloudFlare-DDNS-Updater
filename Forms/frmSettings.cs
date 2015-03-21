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

namespace CloudFlareDDNS
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

        }//end frmSettings()


        /// <summary>
        /// Load the saved values on open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtDomainName.Text = Program.settingsManager.getSetting("Domain").ToString();
            txtEmailAddress.Text = Program.settingsManager.getSetting("EmailAddress").ToString();
            txtAPIKey.Text = Program.settingsManager.getSetting("APIKey").ToString();
            txtFetchTime.Text = Program.settingsManager.getSetting("FetchTime").ToString();
            cbEventLog.Checked = Program.settingsManager.getSetting("UseEventLog").ToBool();

        }//end frmSettings_Load()


        /// <summary>
        /// Save the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            Program.settingsManager.setSetting("Domain", txtDomainName.Text);
            Program.settingsManager.setSetting("EmailAddress", txtEmailAddress.Text);
            Program.settingsManager.setSetting("APIKey", txtAPIKey.Text);
            Program.settingsManager.setSetting("FetchTime", txtFetchTime.Text);
            Program.settingsManager.setSetting("UseEventLog", cbEventLog.Checked.ToString());
            Program.settingsManager.saveSettings();

        }//end btnApply_Click()


        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }//end btnClose_Click()


    }//end class
}//end namespace
