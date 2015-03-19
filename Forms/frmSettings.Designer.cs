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
            this.label1 = new System.Windows.Forms.Label();
            this.cbEventLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(285, 156);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(12, 156);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(12, 16);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(77, 13);
            this.lblDomain.TabIndex = 2;
            this.lblDomain.Text = "Domain Name:";
            // 
            // txtDomainName
            // 
            this.txtDomainName.Location = new System.Drawing.Point(95, 12);
            this.txtDomainName.Name = "txtDomainName";
            this.txtDomainName.Size = new System.Drawing.Size(265, 20);
            this.txtDomainName.TabIndex = 3;
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(95, 38);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(265, 20);
            this.txtEmailAddress.TabIndex = 5;
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Location = new System.Drawing.Point(12, 41);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(76, 13);
            this.lblEmailAddress.TabIndex = 4;
            this.lblEmailAddress.Text = "Email Address:";
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Location = new System.Drawing.Point(95, 64);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(265, 20);
            this.txtAPIKey.TabIndex = 7;
            // 
            // lblAPIKey
            // 
            this.lblAPIKey.AutoSize = true;
            this.lblAPIKey.Location = new System.Drawing.Point(12, 67);
            this.lblAPIKey.Name = "lblAPIKey";
            this.lblAPIKey.Size = new System.Drawing.Size(48, 13);
            this.lblAPIKey.TabIndex = 6;
            this.lblAPIKey.Text = "API Key:";
            // 
            // txtFetchTime
            // 
            this.txtFetchTime.Location = new System.Drawing.Point(152, 90);
            this.txtFetchTime.Name = "txtFetchTime";
            this.txtFetchTime.Size = new System.Drawing.Size(208, 20);
            this.txtFetchTime.TabIndex = 9;
            // 
            // lblFetchTime
            // 
            this.lblFetchTime.AutoSize = true;
            this.lblFetchTime.Location = new System.Drawing.Point(12, 93);
            this.lblFetchTime.Name = "lblFetchTime";
            this.lblFetchTime.Size = new System.Drawing.Size(134, 13);
            this.lblFetchTime.TabIndex = 8;
            this.lblFetchTime.Text = "Auto Fetch Time (Minutes):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Use Windows Event Log:";
            // 
            // cbEventLog
            // 
            this.cbEventLog.AutoSize = true;
            this.cbEventLog.Location = new System.Drawing.Point(152, 117);
            this.cbEventLog.Name = "cbEventLog";
            this.cbEventLog.Size = new System.Drawing.Size(15, 14);
            this.cbEventLog.TabIndex = 11;
            this.cbEventLog.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 191);
            this.Controls.Add(this.cbEventLog);
            this.Controls.Add(this.label1);
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

        }

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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbEventLog;
    }//end class
}//end namespace