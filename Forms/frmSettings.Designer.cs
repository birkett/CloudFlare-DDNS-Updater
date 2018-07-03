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
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.txtAPIKey = new System.Windows.Forms.TextBox();
            this.lblAPIKey = new System.Windows.Forms.Label();
            this.txtFetchTime = new System.Windows.Forms.TextBox();
            this.lblFetchTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEventLog = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IPV6UpdateURL = new System.Windows.Forms.TextBox();
            this.IPV4UpdateURL = new System.Windows.Forms.TextBox();
            this.IPV6RESET = new System.Windows.Forms.Button();
            this.IPV4RESET = new System.Windows.Forms.Button();
            this.StartMinimized = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cloudflare_api_url_default_button = new System.Windows.Forms.Button();
            this.cloudflare_api_url_input = new System.Windows.Forms.TextBox();
            this.cloudflare_api_url_label = new System.Windows.Forms.Label();
            this.Update = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DomainNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ZoneUpdateList = new System.Windows.Forms.ListView();
            this.ZoneID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UseInternalIP_input = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.HideSRV_input = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(370, 521);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = global::CloudFlareDDNS.Properties.Resources.Settings_Close;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(12, 521);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = global::CloudFlareDDNS.Properties.Resources.Settings_Apply;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(95, 12);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(350, 20);
            this.txtEmailAddress.TabIndex = 5;
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Location = new System.Drawing.Point(12, 15);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(76, 13);
            this.lblEmailAddress.TabIndex = 4;
            this.lblEmailAddress.Text = "Email Address:";
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Location = new System.Drawing.Point(95, 38);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(350, 20);
            this.txtAPIKey.TabIndex = 7;
            // 
            // lblAPIKey
            // 
            this.lblAPIKey.AutoSize = true;
            this.lblAPIKey.Location = new System.Drawing.Point(12, 41);
            this.lblAPIKey.Name = "lblAPIKey";
            this.lblAPIKey.Size = new System.Drawing.Size(48, 13);
            this.lblAPIKey.TabIndex = 6;
            this.lblAPIKey.Text = "API Key:";
            // 
            // txtFetchTime
            // 
            this.txtFetchTime.Location = new System.Drawing.Point(152, 64);
            this.txtFetchTime.Name = "txtFetchTime";
            this.txtFetchTime.Size = new System.Drawing.Size(293, 20);
            this.txtFetchTime.TabIndex = 9;
            // 
            // lblFetchTime
            // 
            this.lblFetchTime.AutoSize = true;
            this.lblFetchTime.Location = new System.Drawing.Point(12, 67);
            this.lblFetchTime.Name = "lblFetchTime";
            this.lblFetchTime.Size = new System.Drawing.Size(134, 13);
            this.lblFetchTime.TabIndex = 8;
            this.lblFetchTime.Text = "Auto Fetch Time (Minutes):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Use Windows Event Log:";
            // 
            // cbEventLog
            // 
            this.cbEventLog.AutoSize = true;
            this.cbEventLog.Location = new System.Drawing.Point(152, 91);
            this.cbEventLog.Name = "cbEventLog";
            this.cbEventLog.Size = new System.Drawing.Size(15, 14);
            this.cbEventLog.TabIndex = 11;
            this.cbEventLog.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "IPV6 Update URL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "IPV4 Update URL";
            // 
            // IPV6UpdateURL
            // 
            this.IPV6UpdateURL.Location = new System.Drawing.Point(152, 190);
            this.IPV6UpdateURL.Name = "IPV6UpdateURL";
            this.IPV6UpdateURL.Size = new System.Drawing.Size(225, 20);
            this.IPV6UpdateURL.TabIndex = 14;
            // 
            // IPV4UpdateURL
            // 
            this.IPV4UpdateURL.Location = new System.Drawing.Point(152, 164);
            this.IPV4UpdateURL.Name = "IPV4UpdateURL";
            this.IPV4UpdateURL.Size = new System.Drawing.Size(225, 20);
            this.IPV4UpdateURL.TabIndex = 15;
            // 
            // IPV6RESET
            // 
            this.IPV6RESET.Location = new System.Drawing.Point(384, 190);
            this.IPV6RESET.Name = "IPV6RESET";
            this.IPV6RESET.Size = new System.Drawing.Size(61, 20);
            this.IPV6RESET.TabIndex = 20;
            this.IPV6RESET.Text = "Default";
            this.IPV6RESET.UseVisualStyleBackColor = true;
            this.IPV6RESET.Click += new System.EventHandler(this.IPV6RESET_Click);
            // 
            // IPV4RESET
            // 
            this.IPV4RESET.Location = new System.Drawing.Point(384, 164);
            this.IPV4RESET.Name = "IPV4RESET";
            this.IPV4RESET.Size = new System.Drawing.Size(61, 20);
            this.IPV4RESET.TabIndex = 21;
            this.IPV4RESET.Text = "Default";
            this.IPV4RESET.UseVisualStyleBackColor = true;
            this.IPV4RESET.Click += new System.EventHandler(this.IPV4RESET_Click);
            // 
            // StartMinimized
            // 
            this.StartMinimized.AutoSize = true;
            this.StartMinimized.Location = new System.Drawing.Point(99, 495);
            this.StartMinimized.Name = "StartMinimized";
            this.StartMinimized.Size = new System.Drawing.Size(15, 14);
            this.StartMinimized.TabIndex = 22;
            this.StartMinimized.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 495);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Start Minimized:";
            // 
            // cloudflare_api_url_default_button
            // 
            this.cloudflare_api_url_default_button.Location = new System.Drawing.Point(384, 216);
            this.cloudflare_api_url_default_button.Name = "cloudflare_api_url_default_button";
            this.cloudflare_api_url_default_button.Size = new System.Drawing.Size(61, 20);
            this.cloudflare_api_url_default_button.TabIndex = 26;
            this.cloudflare_api_url_default_button.Text = "Default";
            this.cloudflare_api_url_default_button.UseVisualStyleBackColor = true;
            this.cloudflare_api_url_default_button.Click += new System.EventHandler(this.cloudflare_api_url_default_button_Click);
            // 
            // cloudflare_api_url_input
            // 
            this.cloudflare_api_url_input.Location = new System.Drawing.Point(152, 216);
            this.cloudflare_api_url_input.Name = "cloudflare_api_url_input";
            this.cloudflare_api_url_input.Size = new System.Drawing.Size(225, 20);
            this.cloudflare_api_url_input.TabIndex = 25;
            // 
            // cloudflare_api_url_label
            // 
            this.cloudflare_api_url_label.AutoSize = true;
            this.cloudflare_api_url_label.Location = new System.Drawing.Point(12, 219);
            this.cloudflare_api_url_label.Name = "cloudflare_api_url_label";
            this.cloudflare_api_url_label.Size = new System.Drawing.Size(90, 13);
            this.cloudflare_api_url_label.TabIndex = 24;
            this.cloudflare_api_url_label.Text = "Cloudflare API Url";
            // 
            // Update
            // 
            this.Update.Text = global::CloudFlareDDNS.Properties.Resources.Main_HostsList_Header1;
            this.Update.Width = 50;
            // 
            // DomainNames
            // 
            this.DomainNames.Text = "Domain Name";
            this.DomainNames.Width = 191;
            // 
            // ZoneUpdateList
            // 
            this.ZoneUpdateList.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.ZoneUpdateList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.ZoneUpdateList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ZoneUpdateList.CheckBoxes = true;
            this.ZoneUpdateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Update,
            this.DomainNames,
            this.ZoneID});
            this.ZoneUpdateList.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoneUpdateList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ZoneUpdateList.Location = new System.Drawing.Point(12, 242);
            this.ZoneUpdateList.Name = "ZoneUpdateList";
            this.ZoneUpdateList.Size = new System.Drawing.Size(433, 247);
            this.ZoneUpdateList.TabIndex = 27;
            this.ZoneUpdateList.UseCompatibleStateImageBehavior = false;
            this.ZoneUpdateList.View = System.Windows.Forms.View.Details;
            // 
            // ZoneID
            // 
            this.ZoneID.Text = "Zone ID";
            this.ZoneID.Width = 192;
            // 
            // UseInternalIP_input
            // 
            this.UseInternalIP_input.AutoSize = true;
            this.UseInternalIP_input.Location = new System.Drawing.Point(152, 142);
            this.UseInternalIP_input.Name = "UseInternalIP_input";
            this.UseInternalIP_input.Size = new System.Drawing.Size(15, 14);
            this.UseInternalIP_input.TabIndex = 29;
            this.UseInternalIP_input.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Use Internal IP:";
            // 
            // HideSRV_input
            // 
            this.HideSRV_input.AutoSize = true;
            this.HideSRV_input.Location = new System.Drawing.Point(152, 117);
            this.HideSRV_input.Name = "HideSRV_input";
            this.HideSRV_input.Size = new System.Drawing.Size(15, 14);
            this.HideSRV_input.TabIndex = 31;
            this.HideSRV_input.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Hide SRV:";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 556);
            this.Controls.Add(this.HideSRV_input);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.UseInternalIP_input);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ZoneUpdateList);
            this.Controls.Add(this.cloudflare_api_url_default_button);
            this.Controls.Add(this.cloudflare_api_url_input);
            this.Controls.Add(this.cloudflare_api_url_label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StartMinimized);
            this.Controls.Add(this.IPV4RESET);
            this.Controls.Add(this.IPV6RESET);
            this.Controls.Add(this.IPV4UpdateURL);
            this.Controls.Add(this.IPV6UpdateURL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbEventLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFetchTime);
            this.Controls.Add(this.lblFetchTime);
            this.Controls.Add(this.txtAPIKey);
            this.Controls.Add(this.lblAPIKey);
            this.Controls.Add(this.txtEmailAddress);
            this.Controls.Add(this.lblEmailAddress);
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
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.Label lblEmailAddress;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.Label lblAPIKey;
        private System.Windows.Forms.TextBox txtFetchTime;
        private System.Windows.Forms.Label lblFetchTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbEventLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IPV6UpdateURL;
        private System.Windows.Forms.TextBox IPV4UpdateURL;
        private System.Windows.Forms.Button IPV6RESET;
        private System.Windows.Forms.Button IPV4RESET;
        private System.Windows.Forms.CheckBox StartMinimized;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cloudflare_api_url_default_button;
        private System.Windows.Forms.TextBox cloudflare_api_url_input;
        private System.Windows.Forms.Label cloudflare_api_url_label;
        private new System.Windows.Forms.ColumnHeader Update;
        private System.Windows.Forms.ColumnHeader DomainNames;
        private System.Windows.Forms.ListView ZoneUpdateList;
        private System.Windows.Forms.ColumnHeader ZoneID;
        private System.Windows.Forms.CheckBox UseInternalIP_input;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox HideSRV_input;
        private System.Windows.Forms.Label label6;
    }//end class
}//end namespace