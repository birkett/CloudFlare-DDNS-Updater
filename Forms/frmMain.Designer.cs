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
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.listViewRecords = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtExternalAddress = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimiseToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fetchRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewRecords
            // 
            this.listViewRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRecords.CheckBoxes = true;
            this.listViewRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewRecords.FullRowSelect = true;
            this.listViewRecords.Location = new System.Drawing.Point(12, 82);
            this.listViewRecords.Name = "listViewRecords";
            this.listViewRecords.Size = new System.Drawing.Size(525, 195);
            this.listViewRecords.TabIndex = 0;
            this.listViewRecords.UseCompatibleStateImageBehavior = false;
            this.listViewRecords.View = System.Windows.Forms.View.Details;
            this.listViewRecords.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listHostsCheck);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = global::CloudFlareDDNS.Properties.Resources.Main_HostsList_Header1;
            this.columnHeader6.Width = 57;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = global::CloudFlareDDNS.Properties.Resources.Main_HostsList_Header2;
            this.columnHeader1.Width = 77;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = global::CloudFlareDDNS.Properties.Resources.Main_HostsList_Header3;
            this.columnHeader2.Width = 127;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = global::CloudFlareDDNS.Properties.Resources.Main_HostsList_Header4;
            this.columnHeader3.Width = 300;
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.Location = new System.Drawing.Point(12, 291);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(525, 191);
            this.listViewLog.SmallImageList = this.imageList1;
            this.listViewLog.TabIndex = 1;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = global::CloudFlareDDNS.Properties.Resources.Main_LogControl_Header1;
            this.columnHeader4.Width = 74;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = global::CloudFlareDDNS.Properties.Resources.Main_LogControl_Header2;
            this.columnHeader5.Width = 434;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "information.png");
            this.imageList1.Images.SetKeyName(1, "warning.png");
            this.imageList1.Images.SetKeyName(2, "error.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current External IP Address:";
            // 
            // txtExternalAddress
            // 
            this.txtExternalAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExternalAddress.Location = new System.Drawing.Point(157, 48);
            this.txtExternalAddress.Name = "txtExternalAddress";
            this.txtExternalAddress.Size = new System.Drawing.Size(380, 20);
            this.txtExternalAddress.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(549, 24);
            this.menuStrip1.TabIndex = 7;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimiseToTrayToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_File;
            // 
            // minimiseToTrayToolStripMenuItem
            // 
            this.minimiseToTrayToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.application_double;
            this.minimiseToTrayToolStripMenuItem.Name = "minimiseToTrayToolStripMenuItem";
            this.minimiseToTrayToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.minimiseToTrayToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Minimise;
            this.minimiseToTrayToolStripMenuItem.Click += new System.EventHandler(this.minimiseToTrayToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Exit;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fetchRecordsToolStripMenuItem,
            this.updateRecordsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Tools;
            // 
            // fetchRecordsToolStripMenuItem
            // 
            this.fetchRecordsToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.arrow_down;
            this.fetchRecordsToolStripMenuItem.Name = "fetchRecordsToolStripMenuItem";
            this.fetchRecordsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.fetchRecordsToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Fetch;
            this.fetchRecordsToolStripMenuItem.Click += new System.EventHandler(this.fetchDataToolStripMenuItem_Click);
            // 
            // updateRecordsToolStripMenuItem
            // 
            this.updateRecordsToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.arrow_up;
            this.updateRecordsToolStripMenuItem.Name = "updateRecordsToolStripMenuItem";
            this.updateRecordsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.updateRecordsToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Update;
            this.updateRecordsToolStripMenuItem.Click += new System.EventHandler(this.updateRecordsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.cog;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.settingsToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Settings;
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_Help;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::CloudFlareDDNS.Properties.Resources.information;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = global::CloudFlareDDNS.Properties.Resources.Main_MenuItem_About;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipText = global::CloudFlareDDNS.Properties.Resources.Tooltip_Text;
            this.trayIcon.BalloonTipTitle = global::CloudFlareDDNS.Properties.Resources.Main_Title;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = global::CloudFlareDDNS.Properties.Resources.Main_Title;
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 494);
            this.Controls.Add(this.txtExternalAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewLog);
            this.Controls.Add(this.listViewRecords);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "CloudFlare DDNS Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_Closing);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }//end InitializeComponent()

        #endregion

        private System.Windows.Forms.ListView listViewRecords;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExternalAddress;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fetchRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimiseToTrayToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }//end class
}//end namespace

