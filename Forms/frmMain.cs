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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CloudFlareDDNS.Classes.JsonObjects.Cloudflare;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Main form - all main functionality
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Used for auto updates
        /// </summary>
        private System.Timers.Timer autoUpdateTimer = null;

        /// <summary>
        /// Used for updating the log control
        /// </summary>
        private System.Timers.Timer logUpdateTimer = null;

        /// <summary>
        /// Thread for fetching records
        /// </summary>
        private Thread fetchThread = null;

        /// <summary>
        /// Thread for updating records
        /// </summary>
        private Thread updateThread = null;

        /// <summary>
        /// Make borderless window move able
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// Make borderless window move able
        /// </summary>
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        /// <summary>
        /// Make borderless window move able
        /// </summary>
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        /// <summary>
        /// Make borderless window move able
        /// </summary>
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Called from another thread
        /// Populate the list hosts control with the new returned hosts
        /// </summary>
        /// <param name="resultList"></param>
        private void updateHostsList(List<Result> resultList)
        {
                if (resultList == null)
                {
                    Debug.WriteLine("fetchrecords failed");
                    return; //bail if the fetch failed
                }
                string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

                if (listViewRecords.Items.Count != selectedHosts.Length)
                {
                    if (listViewRecords.Items.Count != 0)
                    {
                        //listViewRecords.Clear(); -- Cant use Clear() it crashes the ListView i dont know why
                        foreach (ListViewItem item in listViewRecords.Items)
                        {
                            bool id_exist = false;
                            foreach (Result r in resultList)
                            {
                                if ((item.Tag as Result).id == r.id)
                                {
                                    id_exist = true;
                                    break;
                                }
                            }
                            if (!id_exist)
                                item.Remove();
                        }
                    }
                }

                foreach (Result r in resultList)
                {
                    bool exist = false;
                    ListViewItem listViewItem = null;
                    ListView.ListViewItemCollection items = null;
                    this.Invoke((MethodInvoker)delegate
                    {
                        items = listViewRecords.Items;
                    });
                    foreach (ListViewItem lvItem in items)
                    {
                        if ((lvItem.Tag as Result).id == r.id)
                        {
                            exist = true;
                            listViewItem = lvItem;
                            break;
                        }
                    }
                    if (exist)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            listViewItem.SubItems[1].Text = r.type.ToString();
                            listViewItem.SubItems[2].Text = r.name;
                            listViewItem.SubItems[3].Text = r.content;
                            listViewItem.SubItems[4].Text = r.modified_on.ToShortDateString() + " " + r.modified_on.ToShortTimeString();
                        });
                        continue;
                    }
                    ListViewItem row = new ListViewItem();
                    row.SubItems.Add(r.type);
                    row.SubItems.Add(r.name);
                    row.SubItems.Add(r.content);
                    row.SubItems.Add(r.modified_on.ToShortDateString() + " " + r.modified_on.ToShortTimeString());
                    row.Tag = r;
                    //Only check this if it's an A record. MX records may have the same name as the primary A record, but should never be updated with an IP.
                    bool NeedIp = false;
                    switch (r.type)
                    {
                        case "A":
                        case "AAAA":
                            NeedIp = true;
                            break;
                    }
                    if (NeedIp)
                    {
                        int pos = Array.IndexOf(selectedHosts, (row.Tag as Result).id);
                        if (pos > -1)
                        {
                            row.Checked = true;
                        }
                    }
                    try
                    {
                        if (string.IsNullOrEmpty(Program.settingsManager.getSetting("HideSRV").ToString()))
                        {
                            if (Program.settingsManager.getSetting("HideSRV").ToBool())
                            {
                                if (r.type != "SRV")
                                {
                                    listViewRecords.Items.Add(row);
                                    continue;
                                }
                            }
                        }
                        listViewRecords.Items.Add(row);
                        continue;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        listViewRecords.Items.Add(row);
                    }
                }

                //Grey out anything that isn't an A record.
                for (int j = 0; j < listViewRecords.Items.Count; j++)
                {
                    bool NeedIp = false;
                    switch (listViewRecords.Items[j].SubItems[1].Text)
                    {
                        case "A":
                        case "AAAA":
                            NeedIp = true;
                            break;
                    }
                    if (NeedIp == false)
                    {
                        listViewRecords.Items[j].ForeColor = System.Drawing.Color.Gray;
                    }
                }
        }//end updateHostsList()

        /// <summary>
        /// Delegate for updateHostsList()
        /// </summary>
        /// <param name="fetchedRecords"></param>
        private delegate void updateHostsListInvoker(List<Result> fetchedRecords);

        /// <summary>
        /// Called from another thread, adds a log entry to the list control
        /// </summary>
        /// <param name="entry"></param>
        private void addLogEntry(ListViewItem entry)
        {
            listViewLog.Items.Add(entry);
        }//end addLogEntry()

        /// <summary>
        /// Delegate for addLogEntry()
        /// </summary>
        /// <param name="entry"></param>
        private delegate void addLogEntryInvoker(ListViewItem entry);

        /// <summary>
        /// Called from another thread, update the textbox control with the new external address
        /// </summary>
        /// <param name="IPV4"></param>
        /// <param name="IPV6"></param>
        private void updateAddress(string IPV4, string IPV6)
        {
            labeltxtExternalAddressIPV4.Text = IPV4;
            labeltxtExternalAddressIPV6.Text = IPV6;
        }//end updateAddress()

        /// <summary>
        /// Delegate for updateAddress()
        /// </summary>
        /// <param name="IPV4"></param>
        /// <param name="IPV6"></param>
        private delegate void updateAddressInvoker(string IPV4, string IPV6);

        /// <summary>
        /// Form entry point
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            int i = 10;        
            if(!string.IsNullOrEmpty(Program.settingsManager.getSetting("FetchTime").ToString()))
                i = Program.settingsManager.getSetting("FetchTime").ToInt();

            autoUpdateTimer = new System.Timers.Timer( i * 60000); //Minutes to milliseconds
            autoUpdateTimer.Elapsed += autoUpdateTimer_Tick;
            autoUpdateTimer.AutoReset = true;
            autoUpdateTimer.Enabled = true;

            logUpdateTimer = new System.Timers.Timer(1000); //Refresh the log every second
            logUpdateTimer.Elapsed += logUpdateTimer_Tick;
            logUpdateTimer.AutoReset = true;
            logUpdateTimer.Enabled = true;
            Logger.log(Properties.Resources.Logger_Start + " " + Program.settingsManager.getSetting("FetchTime").ToString() + " " + Properties.Resources.Logger_Interval + " ", Logger.Level.Info);

        }//end frmMain()

        private void ListViewContextMenu_Opened(object sender, EventArgs e)
        {
            if(listViewRecords.SelectedItems[0] != null){
                if (listViewRecords.SelectedItems[0].Checked)
                {
                    listViewContextMenu.Items[1].Visible = true;
                    listViewContextMenu.Items[2].Visible = false;
                }
                else
                {
                    listViewContextMenu.Items[1].Visible = false;
                    listViewContextMenu.Items[2].Visible = true;
                }
                listViewContextMenu.Items[0].Click += FrmMain_ForceUpdateEntry;
                listViewContextMenu.Items[1].Click += FrmMain_DisableClick;
                listViewContextMenu.Items[2].Click += FrmMain_EnableClick;
            }
        }
        private void FrmMain_ForceUpdateEntry(object sender, EventArgs e)
        {
            fetchThread = new Thread(new ThreadStart(start_ForceUpdateEntry));
            fetchThread.Start();
        }

        private void start_ForceUpdateEntry()
        {
            ListViewItem item=null;
            this.Invoke((MethodInvoker)delegate
            {
                item = listViewRecords.SelectedItems[0];
            });
            if (item != null){
                    Result r = (item.Tag as Result);
                    Program.cloudFlareAPI.UpdateLastChange(listViewRecords, r, Properties.Resources.Main_Change_IP);

                    string strResponse = Program.cloudFlareAPI.updateCloudflareRecords(r);

                    Thread.Sleep(1000); //DontNeedTooSpamCloudflareServer

                    Program.cloudFlareAPI.UpdateLastChange(listViewRecords, r, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            }
        }

        private void FrmMain_DisableClick(object sender, EventArgs e)
        {
            if (listViewRecords.SelectedItems[0] != null)
            {
                listViewRecords.SelectedItems[0].Checked = false;
            }
        }
        private void FrmMain_EnableClick(object sender, EventArgs e)
        {
            if (listViewRecords.SelectedItems[0] != null)
            {
                listViewRecords.SelectedItems[0].Checked = true;
            }
        }

        /// <summary>
        /// Clean up the tray icon when closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Closing(object sender, FormClosingEventArgs e)
        {
            autoUpdateTimer.Enabled = false;
            logUpdateTimer.Enabled = false;

            //Stop the threads if they are active
            if (fetchThread != null && fetchThread.IsAlive)
            {
                fetchThread.Abort();
            }

            if (updateThread != null && updateThread.IsAlive)
            {
                updateThread.Abort();
            }

            trayIcon.Dispose();
        }//end frmMain_Closing()

        /// <summary>
        /// Minimise to the tray instead of the taskbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                trayIcon.Visible = true;
                trayIcon.ShowBalloonTip(1000);
            }
        }//end frmMain_Resize()

        /// <summary>
        /// Thread to fetch records, nothing else
        /// </summary>
        private void threadFetchOnly()
        {
            Program.cloudFlareAPI.getExternalAddress();
            this.Invoke(new updateAddressInvoker(updateAddress), Program.settingsManager.getSetting("ExternalAddressIPV4").ToString(), Program.settingsManager.getSetting("ExternalAddressIPV6").ToString());
            if (!string.IsNullOrEmpty(Program.settingsManager.getSetting("SelectedZones").ToString()))
            {
                List<Result> resultList = new List<Result>();
                foreach (string SelectedZones in Program.settingsManager.getSetting("SelectedZones").ToString().Split(';'))
                {

                    GetDnsRecordsResponse records = Program.cloudFlareAPI.getCloudFlareRecords(SelectedZones);
                    records.result.All(x => { resultList.Add(x); return true; });
                }
                if (resultList != null)
                {
                    this.Invoke(new updateHostsListInvoker(updateHostsList), resultList);
                }
            }
        }//end threadFetchOnly()

        /// <summary>
        /// Thread to run updates every x minutes
        /// </summary>
        private void threadFetchUpdate()
        {
            Program.cloudFlareAPI.getExternalAddress();
            this.Invoke(new updateAddressInvoker(updateAddress), Program.settingsManager.getSetting("ExternalAddressIPV4").ToString(), Program.settingsManager.getSetting("ExternalAddressIPV6").ToString());
            if (!string.IsNullOrEmpty(Program.settingsManager.getSetting("SelectedZones").ToString()))
            {
                List<Result> resultList = new List<Result>();
                foreach (string SelectedZones in Program.settingsManager.getSetting("SelectedZones").ToString().Split(';'))
                {
                    
                    GetDnsRecordsResponse records = Program.cloudFlareAPI.getCloudFlareRecords(SelectedZones);
                    records.result.All(x => { resultList.Add(x); return true; });
                }
                if (resultList != null)
                {
                    this.Invoke(new updateHostsListInvoker(updateHostsList), resultList);
                    List<Result> Ldns = Program.cloudFlareAPI.updateRecords(listViewRecords, resultList);
                }
                start_fetchThread();
            }
        }//end timerUpdateThread()

        /// <summary>
        /// Get current external address and CloudFlare records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fetchDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start_fetchThread();
        }//end fetchDataToolStripMenuItem_Click()

        private void start_fetchThread()
        {
            fetchThread = new Thread(new ThreadStart(threadFetchOnly));
            fetchThread.Start();
        }

        /// <summary>
        /// Update the selected records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateThread = new Thread(new ThreadStart(threadFetchUpdate));
            updateThread.Start();
        }//end updateRecordsToolStripMenuItem_Click()

        /// <summary>
        /// Open the settings box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings settingsForm = new frmSettings();
            settingsForm.Show();
        }//end settingsToolStripMenuItem_Click()

        /// <summary>
        /// Minimise the appplication to the task tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimiseToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }//end minimiseToTrayToolStripMenuItem_Click()

        /// <summary>
        /// Auto update every x minutes, creates a new timerUpdateThread() thread
        /// NOTE: This already runs in its own thread!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            threadFetchUpdate();
        }//end autoUpdateTimer_Tick()

        /// <summary>
        /// Tick every 1000ms to add new log entries to the listview control
        /// NOTE: This already runs in its own thread!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Logger.Entry logItem in Logger.items)
                {
                    ListViewItem row = new ListViewItem();

                    switch (logItem.Level)
                    {
                        case Logger.Level.Warning:
                            row.ImageIndex = 1;
                            break;

                        case Logger.Level.Error:
                            row.ImageIndex = 2;
                            break;

                        default: //Logger.Level.Info
                            row.ImageIndex = 0;
                            break;
                    }
                    row.SubItems.Add(logItem.Message);
                    this.Invoke(new addLogEntryInvoker(addLogEntry), row);
                }
                Logger.reset(); //Clear the log
            }
            catch (Exception)
            {
            }
        }//end logUpdateTimer_Tick()

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }//end exitToolStripMenuItem_Click()

        /// <summary>
        /// Restore when double clicking the tray icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }//end notifyIcon1_MouseDoubleClick()

        /// <summary>
        /// Show the about form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.Show();
        }//end aboutToolStripMenuItem_Click()

        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.cloudFlareAPI.getExternalAddress();
            this.Invoke(new updateAddressInvoker(updateAddress), Program.settingsManager.getSetting("ExternalAddressIPV4").ToString(), Program.settingsManager.getSetting("ExternalAddressIPV6").ToString());
            try
            {
                start_fetchThread();
            }
            catch (Exception) { }

                
        }

        private void listViewLog_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkOrange, e.Bounds);
            e.DrawText();
        }

        private void listViewRecords_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkOrange, e.Bounds);
            e.DrawText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void listViewRecords_MouseDown(object sender, MouseEventArgs e)
        {
        }
        /// <summary>
        /// A host in the list contol has been checked or unchecked
        /// Update the config to reflect the change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void listHostsCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                ListViewItem item = listViewRecords.Items[e.Index];
                bool NeedIp = false;
                switch (item.SubItems[1].Text)
                {
                    case "A":
                    case "AAAA":
                        NeedIp = true;
                        break;
                    default:
                        NeedIp = false;
                        break;
                }
                //Do nothing for items that are not A records.
                if (NeedIp == false)
                {
                    e.NewValue = CheckState.Unchecked;
                    return;
                }
                string selectedHosts = "";
                if (!string.IsNullOrEmpty(Program.settingsManager.getSetting("SelectedHosts").ToString()))
                {
                    selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString();
                }
                //Add Item if checked delete item if unchecked
                string[] selectedHostsArray = selectedHosts.Split(';');
                if (!item.Checked)
                {
                    int pos = Array.IndexOf(selectedHostsArray, (item.Tag as Result).id);
                    if (!(pos > -1))
                    {
                        if (!string.IsNullOrEmpty(selectedHosts.Trim()))
                            selectedHosts += ";";

                        selectedHosts += (item.Tag as Result).id;
                    }
                }
                else
                {
                    selectedHosts= selectedHosts.Replace(";" + (item.Tag as Result).id, "");
                    selectedHosts = selectedHosts.Replace((item.Tag as Result).id+";", "");
                    selectedHosts = selectedHosts.Replace(";" + item.SubItems[2].Text, "");
                }

                    List<String> myStringList = new List<string>();
                    string clearedSelectedHosts = "";
                    foreach (string s in selectedHosts.Split(';'))
                    {
                        if (!myStringList.Contains(s))
                        {
                            myStringList.Add(s);
                            clearedSelectedHosts += ";" + s;
                        }
                    }
                clearedSelectedHosts=clearedSelectedHosts.TrimStart(';');
                     
                Debug.WriteLine(clearedSelectedHosts);
                Program.settingsManager.setSetting("SelectedHosts", clearedSelectedHosts);
                Program.settingsManager.saveSettings(); //Save the selected host list accross sessions
            }
            catch (Exception) { }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {

            if (Program.settingsManager.getSetting("StartMinimized").ToBool())
            {
                this.WindowState = FormWindowState.Minimized;
            }
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        /// <summary>
        /// Set/Read the Item Tag from listView
        /// </summary>
    }//end class
}//end namespace