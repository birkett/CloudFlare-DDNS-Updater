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
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;

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
        /// Called from another thread
        /// Populate the list hosts control with the new returned hosts
        /// </summary>
        /// <param name="fetchedRecords"></param>
        private void updateHostsList(JsonResponse fetchedRecords)
        {
            if (fetchedRecords == null)
                return; //bail if the fetch failed

            listViewRecords.Items.Clear();

            string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

            for (int i = 0; i < Convert.ToInt32(fetchedRecords.response.recs.count); i++)
            {
                ListViewItem row = new ListViewItem();
                row.SubItems.Add(fetchedRecords.response.recs.objs[i].type);
                row.SubItems.Add(fetchedRecords.response.recs.objs[i].display_name);
                row.SubItems.Add(fetchedRecords.response.recs.objs[i].display_content);

                //Only check this if it's an A record. MX records may have the same name as the primary A record, but should never be updated with an IP.
                if ((Array.IndexOf(selectedHosts, fetchedRecords.response.recs.objs[i].display_name) >= 0) == true && fetchedRecords.response.recs.objs[i].type == "A")
                {
                    row.Checked = true;
                }

                listViewRecords.Items.Add(row);
            }

            //Grey out anything that isn't an A record.
            for(int j = 0; j < listViewRecords.Items.Count; j++)
            {
                if (listViewRecords.Items[j].SubItems[1].Text != "A")
                {
                    listViewRecords.Items[j].ForeColor = System.Drawing.Color.LightGray;
                }
            }

        }//end updateHostsList()


        /// <summary>
        /// Delegate for updateHostsList()
        /// </summary>
        /// <param name="fetchedRecords"></param>
        delegate void updateHostsListInvoker(JsonResponse fetchedRecords);


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
        delegate void addLogEntryInvoker(ListViewItem entry);


        /// <summary>
        /// Called from another thread, update the textbox control with the new external address
        /// </summary>
        /// <param name="szAddress"></param>
        private void updateAddress(string szAddress)
        {
            txtExternalAddress.Text = szAddress;

        }//end updateAddress()


        /// <summary>
        /// Delegate for updateAddress()
        /// </summary>
        /// <param name="szAddress"></param>
        delegate void updateAddressInvoker(string szAddress);


        /// <summary>
        /// Form entry point
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            autoUpdateTimer = new System.Timers.Timer(Program.settingsManager.getSetting("FetchTime").ToInt() * 60000); //Minutes to milliseconds
            autoUpdateTimer.Elapsed += autoUpdateTimer_Tick;
            autoUpdateTimer.AutoReset = true;
            autoUpdateTimer.Enabled = true;

            logUpdateTimer = new System.Timers.Timer(1000); //Refresh the log every second
            logUpdateTimer.Elapsed += logUpdateTimer_Tick;
            logUpdateTimer.AutoReset = true;
            logUpdateTimer.Enabled = true;

            Logger.log(Properties.Resources.Logger_Start + " " + Program.settingsManager.getSetting("FetchTime").ToString() + " " + Properties.Resources.Logger_Interval + " " + Program.settingsManager.getSetting("Domain").ToString(), Logger.Level.Info);

        }//end frmMain()


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
                trayIcon.Visible = true;
                trayIcon.ShowBalloonTip(1000);
            }

        }//end frmMain_Resize()


        /// <summary>
        /// A host in the list contol has been checked or unchecked
        /// Update the config to reflect the change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listHostsCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem item = listViewRecords.Items[e.Index];

            //Do nothing for items that are not A records.
            if(item.SubItems[1].Text != "A")
            {
                e.NewValue = CheckState.Unchecked;
                return;
            }

            if (e.CurrentValue == CheckState.Unchecked)
            {
                //Item has been selected by the user, store it for later
                if (Program.settingsManager.getSetting("SelectedHosts").ToString().IndexOf(item.SubItems[2].Text, StringComparison.CurrentCulture) >= 0)
                {
                    //Item is already in the settings list, do nothing.
                    return;
                }
                Program.settingsManager.setSetting("SelectedHosts", Program.settingsManager.getSetting("SelectedHosts").ToString() + item.SubItems[2].Text + ";");
            }
            else if (e.CurrentValue == CheckState.Checked)
            {
                //Make sure to clean up any old entries in the settings
                string new_selected = Program.settingsManager.getSetting("SelectedHosts").ToString().Replace(item.SubItems[2].Text + ';', "");
                Program.settingsManager.setSetting("SelectedHosts", new_selected);
            }

            Program.settingsManager.saveSettings(); //Save the selected host list accross sessions

        }//end listHostsCheck()


        /// <summary>
        /// Thread to fetch records, nothing else
        /// </summary>
        private void threadFetchOnly()
        {
            this.Invoke(new updateAddressInvoker(updateAddress), Program.cloudFlareAPI.getExternalAddress());
            this.Invoke(new updateHostsListInvoker(updateHostsList), Program.cloudFlareAPI.getCloudFlareRecords());

        }//end threadFetchOnly()


        /// <summary>
        /// Thread to run updates every x minutes
        /// </summary>
        private void threadFetchUpdate()
        {
            this.Invoke(new updateAddressInvoker(updateAddress), Program.cloudFlareAPI.getExternalAddress());
            JsonResponse records = Program.cloudFlareAPI.getCloudFlareRecords();
            this.Invoke(new updateHostsListInvoker(updateHostsList), records);
            Program.cloudFlareAPI.updateRecords(records);

        }//end timerUpdateThread()


        /// <summary>
        /// GET current external address and CloudFlare records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fetchDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fetchThread = new Thread(new ThreadStart(threadFetchOnly));
            fetchThread.Start();

        }//end fetchDataToolStripMenuItem_Click()


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
            foreach(Logger.Entry logItem in Logger.items)
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


    }//end class
}//end namespace
