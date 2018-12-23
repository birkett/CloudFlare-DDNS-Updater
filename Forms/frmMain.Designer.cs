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
            this.LastChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ItemUpdateIp = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.IPv4_text = new System.Windows.Forms.Label();
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
            this.IPv6_text = new System.Windows.Forms.Label();
            this.labeltxtExternalAddressIPV6 = new System.Windows.Forms.Label();
            this.labeltxtExternalAddressIPV4 = new System.Windows.Forms.Label();
            this.button_close = new System.Windows.Forms.Button();
            this.button_minimize = new System.Windows.Forms.Button();
            this.listViewContextMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewRecords
            // 
            this.listViewRecords.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.listViewRecords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewRecords.CheckBoxes = true;
            this.listViewRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.LastChange});
            this.listViewRecords.ContextMenuStrip = this.listViewContextMenu;
            this.listViewRecords.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewRecords.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.listViewRecords.FullRowSelect = true;
            this.listViewRecords.Location = new System.Drawing.Point(12, 90);
            this.listViewRecords.MultiSelect = false;
            this.listViewRecords.Name = "listViewRecords";
            this.listViewRecords.Size = new System.Drawing.Size(702, 342);
            this.listViewRecords.TabIndex = 0;
            this.listViewRecords.UseCompatibleStateImageBehavior = false;
            this.listViewRecords.View = System.Windows.Forms.View.Details;
            this.listViewRecords.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewRecords_DrawColumnHeader);
            this.listViewRecords.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listHostsCheck);
            this.listViewRecords.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewRecords_MouseDown);
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
            this.columnHeader3.Width = 309;
            // 
            // LastChange
            // 
            this.LastChange.Text = global::CloudFlareDDNS.Properties.Resources.Main_List_LastChange;
            this.LastChange.Width = 127;
            // 
            // listViewContextMenu
            // 
            this.listViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemUpdateIp,
            this.ItemDisable,
            this.ItemEnable});
            this.listViewContextMenu.Name = "listViewContextMenu";
            this.listViewContextMenu.Size = new System.Drawing.Size(126, 70);
            // 
            // ItemUpdateIp
            // 
            this.ItemUpdateIp.Name = "ItemUpdateIp";
            this.ItemUpdateIp.Size = new System.Drawing.Size(125, 22);
            this.ItemUpdateIp.Text = "Update IP";
            // 
            // ItemDisable
            // 
            this.ItemDisable.Name = "ItemDisable";
            this.ItemDisable.Size = new System.Drawing.Size(125, 22);
            this.ItemDisable.Text = "Disable";
            this.ItemDisable.Visible = false;
            // 
            // ItemEnable
            // 
            this.ItemEnable.Name = "ItemEnable";
            this.ItemEnable.Size = new System.Drawing.Size(125, 22);
            this.ItemEnable.Text = "Enable";
            // 
            // listViewLog
            // 
            this.listViewLog.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewLog.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.listViewLog.Location = new System.Drawing.Point(12, 438);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(702, 98);
            this.listViewLog.SmallImageList = this.imageList1;
            this.listViewLog.TabIndex = 1;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            this.listViewLog.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewLog_DrawColumnHeader);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = global::CloudFlareDDNS.Properties.Resources.Main_LogControl_Header1;
            this.columnHeader4.Width = 54;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = global::CloudFlareDDNS.Properties.Resources.Main_LogControl_Header2;
            this.columnHeader5.Width = 518;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "information.png");
            this.imageList1.Images.SetKeyName(1, "warning.png");
            this.imageList1.Images.SetKeyName(2, "error.png");
            // 
            // IPv4_text
            // 
            this.IPv4_text.AutoSize = true;
            this.IPv4_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPv4_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.IPv4_text.Location = new System.Drawing.Point(12, 64);
            this.IPv4_text.Name = "IPv4_text";
            this.IPv4_text.Size = new System.Drawing.Size(181, 13);
            this.IPv4_text.TabIndex = 5;
            this.IPv4_text.Text = "Current External IPv4 Address:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MaximumSize = new System.Drawing.Size(150, 50);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(150, 30);
            this.menuStrip1.TabIndex = 7;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimiseToTrayToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 26);
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
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 26);
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
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
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
            // IPv6_text
            // 
            this.IPv6_text.AutoSize = true;
            this.IPv6_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPv6_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.IPv6_text.Location = new System.Drawing.Point(12, 40);
            this.IPv6_text.Name = "IPv6_text";
            this.IPv6_text.Size = new System.Drawing.Size(181, 13);
            this.IPv6_text.TabIndex = 8;
            this.IPv6_text.Text = "Current External IPv6 Address:";
            // 
            // labeltxtExternalAddressIPV6
            // 
            this.labeltxtExternalAddressIPV6.AutoSize = true;
            this.labeltxtExternalAddressIPV6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeltxtExternalAddressIPV6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.labeltxtExternalAddressIPV6.Location = new System.Drawing.Point(200, 40);
            this.labeltxtExternalAddressIPV6.Name = "labeltxtExternalAddressIPV6";
            this.labeltxtExternalAddressIPV6.Size = new System.Drawing.Size(27, 14);
            this.labeltxtExternalAddressIPV6.TabIndex = 9;
            this.labeltxtExternalAddressIPV6.Text = "IPv6";
            // 
            // labeltxtExternalAddressIPV4
            // 
            this.labeltxtExternalAddressIPV4.AutoSize = true;
            this.labeltxtExternalAddressIPV4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeltxtExternalAddressIPV4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.labeltxtExternalAddressIPV4.Location = new System.Drawing.Point(200, 64);
            this.labeltxtExternalAddressIPV4.Name = "labeltxtExternalAddressIPV4";
            this.labeltxtExternalAddressIPV4.Size = new System.Drawing.Size(27, 14);
            this.labeltxtExternalAddressIPV4.TabIndex = 11;
            this.labeltxtExternalAddressIPV4.Text = "IPv4";
            // 
            // button_close
            // 
            this.button_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.button_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_close.Font = new System.Drawing.Font("Arial", 9F);
            this.button_close.ForeColor = System.Drawing.Color.White;
            this.button_close.Location = new System.Drawing.Point(639, 7);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 12;
            this.button_close.Text = global::CloudFlareDDNS.Properties.Resources.Main_Close;
            this.button_close.UseVisualStyleBackColor = false;
            this.button_close.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_minimize
            // 
            this.button_minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.button_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button_minimize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_minimize.Font = new System.Drawing.Font("Arial", 10F);
            this.button_minimize.ForeColor = System.Drawing.Color.White;
            this.button_minimize.Location = new System.Drawing.Point(558, 7);
            this.button_minimize.Name = "button_minimize";
            this.button_minimize.Size = new System.Drawing.Size(75, 23);
            this.button_minimize.TabIndex = 13;
            this.button_minimize.Text = "Minimize";
            this.button_minimize.UseVisualStyleBackColor = false;
            this.button_minimize.Click += new System.EventHandler(this.button_minimize_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(726, 548);
            this.Controls.Add(this.button_minimize);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.labeltxtExternalAddressIPV4);
            this.Controls.Add(this.labeltxtExternalAddressIPV6);
            this.Controls.Add(this.IPv6_text);
            this.Controls.Add(this.IPv4_text);
            this.Controls.Add(this.listViewLog);
            this.Controls.Add(this.listViewRecords);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "CloudFlare DDNS Updater v2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.listViewContextMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }//end InitializeComponent()

        #endregion

        private System.Windows.Forms.ListView listViewRecords;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.Label IPv4_text;
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
        private System.Windows.Forms.Label IPv6_text;
        private System.Windows.Forms.Label labeltxtExternalAddressIPV6;
        private System.Windows.Forms.Label labeltxtExternalAddressIPV4;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_minimize;
        private System.Windows.Forms.ColumnHeader LastChange;
        private System.Windows.Forms.ContextMenuStrip listViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ItemUpdateIp;
        private System.Windows.Forms.ToolStripMenuItem ItemDisable;
        private System.Windows.Forms.ToolStripMenuItem ItemEnable;
    }//end class
}//end namespace

