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
namespace CloudFlareDDNS
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }//end Dispose()

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.btnClose = new System.Windows.Forms.Button();
      this.btnApply = new System.Windows.Forms.Button();
      this.lblDomain = new System.Windows.Forms.Label();
      this.txtDomainName = new System.Windows.Forms.TextBox();
      this.txtEmailAddress = new System.Windows.Forms.TextBox();
      this.lblEmailAddress = new System.Windows.Forms.Label();
      this.txtAPIKey = new System.Windows.Forms.TextBox();
      this.lblAPIKey = new System.Windows.Forms.Label();
      this.txtFetchTime = new System.Windows.Forms.TextBox();
      this.lblFetchTime = new System.Windows.Forms.Label();
      this.lblEventlog = new System.Windows.Forms.Label();
      this.cbEventLog = new System.Windows.Forms.CheckBox();
      this.lblZoneID = new System.Windows.Forms.Label();
      this.txtZoneID = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(285, 168);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 21);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = global::CloudFlareDDNS.Properties.Resources.Settings_Close;
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnApply
      // 
      this.btnApply.Location = new System.Drawing.Point(12, 168);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new System.Drawing.Size(75, 21);
      this.btnApply.TabIndex = 1;
      this.btnApply.Text = global::CloudFlareDDNS.Properties.Resources.Settings_Apply;
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
      // 
      // lblDomain
      // 
      this.lblDomain.AutoSize = true;
      this.lblDomain.Location = new System.Drawing.Point(12, 15);
      this.lblDomain.Name = "lblDomain";
      this.lblDomain.Size = new System.Drawing.Size(75, 12);
      this.lblDomain.TabIndex = 2;
      this.lblDomain.Text = "Domain Name:";
      // 
      // txtDomainName
      // 
      this.txtDomainName.Location = new System.Drawing.Point(95, 11);
      this.txtDomainName.Name = "txtDomainName";
      this.txtDomainName.Size = new System.Drawing.Size(265, 22);
      this.txtDomainName.TabIndex = 3;
      // 
      // txtEmailAddress
      // 
      this.txtEmailAddress.Location = new System.Drawing.Point(95, 59);
      this.txtEmailAddress.Name = "txtEmailAddress";
      this.txtEmailAddress.Size = new System.Drawing.Size(265, 22);
      this.txtEmailAddress.TabIndex = 5;
      // 
      // lblEmailAddress
      // 
      this.lblEmailAddress.AutoSize = true;
      this.lblEmailAddress.Location = new System.Drawing.Point(12, 62);
      this.lblEmailAddress.Name = "lblEmailAddress";
      this.lblEmailAddress.Size = new System.Drawing.Size(75, 12);
      this.lblEmailAddress.TabIndex = 4;
      this.lblEmailAddress.Text = "Email Address:";
      // 
      // txtAPIKey
      // 
      this.txtAPIKey.Location = new System.Drawing.Point(95, 83);
      this.txtAPIKey.Name = "txtAPIKey";
      this.txtAPIKey.Size = new System.Drawing.Size(265, 22);
      this.txtAPIKey.TabIndex = 7;
      // 
      // lblAPIKey
      // 
      this.lblAPIKey.AutoSize = true;
      this.lblAPIKey.Location = new System.Drawing.Point(12, 86);
      this.lblAPIKey.Name = "lblAPIKey";
      this.lblAPIKey.Size = new System.Drawing.Size(48, 12);
      this.lblAPIKey.TabIndex = 6;
      this.lblAPIKey.Text = "API Key:";
      // 
      // txtFetchTime
      // 
      this.txtFetchTime.Location = new System.Drawing.Point(152, 107);
      this.txtFetchTime.Name = "txtFetchTime";
      this.txtFetchTime.Size = new System.Drawing.Size(208, 22);
      this.txtFetchTime.TabIndex = 9;
      // 
      // lblFetchTime
      // 
      this.lblFetchTime.AutoSize = true;
      this.lblFetchTime.Location = new System.Drawing.Point(12, 110);
      this.lblFetchTime.Name = "lblFetchTime";
      this.lblFetchTime.Size = new System.Drawing.Size(134, 12);
      this.lblFetchTime.TabIndex = 8;
      this.lblFetchTime.Text = "Auto Fetch Time (Minutes):";
      // 
      // lblEventlog
      // 
      this.lblEventlog.AutoSize = true;
      this.lblEventlog.Location = new System.Drawing.Point(12, 133);
      this.lblEventlog.Name = "lblEventlog";
      this.lblEventlog.Size = new System.Drawing.Size(124, 12);
      this.lblEventlog.TabIndex = 10;
      this.lblEventlog.Text = "Use Windows Event Log:";
      // 
      // cbEventLog
      // 
      this.cbEventLog.AutoSize = true;
      this.cbEventLog.Location = new System.Drawing.Point(152, 132);
      this.cbEventLog.Name = "cbEventLog";
      this.cbEventLog.Size = new System.Drawing.Size(15, 14);
      this.cbEventLog.TabIndex = 11;
      this.cbEventLog.UseVisualStyleBackColor = true;
      // 
      // lblZoneID
      // 
      this.lblZoneID.AutoSize = true;
      this.lblZoneID.Location = new System.Drawing.Point(12, 38);
      this.lblZoneID.Name = "lblZoneID";
      this.lblZoneID.Size = new System.Drawing.Size(44, 12);
      this.lblZoneID.TabIndex = 12;
      this.lblZoneID.Text = "Zone ID:";
      // 
      // txtZoneID
      // 
      this.txtZoneID.Location = new System.Drawing.Point(95, 35);
      this.txtZoneID.Name = "txtZoneID";
      this.txtZoneID.Size = new System.Drawing.Size(265, 22);
      this.txtZoneID.TabIndex = 13;
      // 
      // frmSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(372, 198);
      this.Controls.Add(this.txtZoneID);
      this.Controls.Add(this.lblZoneID);
      this.Controls.Add(this.cbEventLog);
      this.Controls.Add(this.lblEventlog);
      this.Controls.Add(this.txtFetchTime);
      this.Controls.Add(this.lblFetchTime);
      this.Controls.Add(this.txtAPIKey);
      this.Controls.Add(this.lblAPIKey);
      this.Controls.Add(this.txtEmailAddress);
      this.Controls.Add(this.lblEmailAddress);
      this.Controls.Add(this.txtDomainName);
      this.Controls.Add(this.lblDomain);
      this.Controls.Add(this.btnApply);
      this.Controls.Add(this.btnClose);
      this.Name = "frmSettings";
      this.Text = "Settings";
      this.Load += new System.EventHandler(this.frmSettings_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

        }//end InitializeComponent()

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.TextBox txtDomainName;
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.Label lblEmailAddress;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.Label lblAPIKey;
        private System.Windows.Forms.TextBox txtFetchTime;
        private System.Windows.Forms.Label lblFetchTime;
        private System.Windows.Forms.Label lblEventlog;
        private System.Windows.Forms.CheckBox cbEventLog;
        private System.Windows.Forms.Label lblZoneID;
        private System.Windows.Forms.TextBox txtZoneID;
  }//end class
}//end namespace