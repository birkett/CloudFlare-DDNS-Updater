using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

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
        private System.Timers.Timer autoUpdateTimer;


        /// <summary>
        /// Used for updating the log control
        /// </summary>
        private System.Timers.Timer logUpdateTimer;


        /// <summary>
        /// Stores the fetched records in an accessible place
        /// </summary>
        private JSONResponse FetchedRecords = null;


        /// <summary>
        /// Called from another thread, get if a list entry is selected by the user
        /// </summary>
        /// <param name="szEntryName"></param>
        /// <returns></returns>
        private bool isEntryChecked(string szEntryName)
        {
            return (listViewRecords.FindItemWithText(szEntryName).Checked == true);
        }


        /// <summary>
        /// Deletgate for isEntryChecked()
        /// </summary>
        /// <param name="szEntryName"></param>
        /// <returns></returns>
        delegate bool isEntryCheckedInvoker(string szEntryName);


        /// <summary>
        /// Called from another thread, select an entry in the list
        /// </summary>
        /// <param name="szEntryName"></param>
        private void checkTickEntry(string szEntryName)
        {
            listViewRecords.FindItemWithText(szEntryName).Checked = true;
        }


        /// <summary>
        /// Deletgate for checkTickEntry()
        /// </summary>
        /// <param name="szEntryName"></param>
        delegate void checkTickEntryInvoker(string szEntryName);


        /// <summary>
        /// Called from another thread, adds an entry to the list control
        /// </summary>
        /// <param name="entry"></param>
        private void addHostEntry(DNSRecord entry)
        {
            ListViewItem row = new ListViewItem();
            row.SubItems.Add(entry.type);
            row.SubItems.Add(entry.display_name);
            row.SubItems.Add(entry.display_content);
            listViewRecords.Items.Add(row);
        }


        /// <summary>
        /// Delegate for addHostEntry()
        /// </summary>
        /// <param name="entry"></param>
        delegate void addHostEntryInvoker(DNSRecord entry);


        /// <summary>
        /// Called from another thread, adds an entry to the log control
        /// </summary>
        /// <param name="entry"></param>
        private void addLogEntry(ListViewItem entry)
        {
            listViewLog.Items.Add(entry);
        }


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
        }


        /// <summary>
        /// Delegate for updateAddress()
        /// </summary>
        /// <param name="szAddress"></param>
        delegate void updateAddressInvoker(string szAddress);


        /// <summary>
        /// Logic to get the external address and CloudFlare records
        /// </summary>
        private void FetchRecords()
        {
            FetchedRecords = null;
            string new_external_address = CloudFlareAPI.GetExternalAddress();

            if (new_external_address == null)
                return;

            SettingsManager.setSetting("ExternalAddress", new_external_address);

            this.Invoke(new updateAddressInvoker(updateAddress), new_external_address);

            string records = CloudFlareAPI.GetCloudflareRecords();
            if (records == null)
                return;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            FetchedRecords = serializer.Deserialize<JSONResponse>(records);

            if (FetchedRecords.result != "success")
            {
                Logger.log(FetchedRecords.msg, Logger.Level.Error);
                return;
            }

            //Itterate through the current list, saving checked items.
            for(int j = 0; j < listViewRecords.Items.Count; j++)
            {
                //Add checked items to a list to be saved
                if (listViewRecords.FindItemWithText(FetchedRecords.response.recs.objs[j].display_name).Checked == true)
                {
                    //Item has been selected by the user, store it for later
                    SettingsManager.setSetting("SelectedHosts", SettingsManager.getSetting("SelectedHosts") + FetchedRecords.response.recs.objs[j].display_name + ";");
                }
                else 
                {
                    //Make sure to clean up any old entries in the settings
                    string new_selected = SettingsManager.getSetting("SelectedHosts").Replace(FetchedRecords.response.recs.objs[j].display_name + ';', "");
                    SettingsManager.setSetting("SelectedHosts", new_selected);
                }
            }

            SettingsManager.saveSettings(); //Save the selected host list accross sessions
            listViewRecords.Items.Clear();

            string[] selectedHosts = SettingsManager.getSetting("SelectedHosts").Split(';');

            for (int i = 0; i < Convert.ToInt32(FetchedRecords.response.recs.count); i++)
            {
                this.Invoke(new addHostEntryInvoker(addHostEntry), FetchedRecords.response.recs.objs[i]);

                foreach (string host in selectedHosts)
                {
                    if (host == FetchedRecords.response.recs.objs[i].display_name)
                    {
                        this.Invoke(new checkTickEntryInvoker(checkTickEntry), FetchedRecords.response.recs.objs[i].display_name);
                    }
                }
            }
        }


        /// <summary>
        /// Logic to update records
        /// </summary>
        private void UpdateRecords()
        {
            if (FetchedRecords == null) //Dont attempt updates if the fetch failed
                return;
       
            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;

            for (int i = 0; i < Convert.ToInt32(FetchedRecords.response.recs.count); i++)
            {
                //Skip over anything that is not checked
                if (Convert.ToBoolean(this.Invoke(new isEntryCheckedInvoker(isEntryChecked), FetchedRecords.response.recs.objs[i].display_name)) == false)
                {
                    //Log("Ignoring " + FetchedRecords.response.recs.objs[i].name + " - not checked by user", 1);
                    ignored++;
                    continue;
                }

                //Skip over MX and CNAME records
                //TODO: Dont skip them :)
                if (FetchedRecords.response.recs.objs[i].type != "A")
                {
                    //Log("Skipping " + FetchedRecords.response.recs.objs[i].name + " - is not an A record", 1);
                    skipped++;
                    continue;
                }

                //Skip over anything that doesnt need an update
                if (FetchedRecords.response.recs.objs[i].content == SettingsManager.getSetting("ExternalAddress"))
                {
                    //Log("Skipping " + FetchedRecords.response.recs.objs[i].name + " - no update needed", 0);
                    up_to_date++;
                    continue;
                }

                string strResponse = CloudFlareAPI.UpdateCloudflareRecords(FetchedRecords.response.recs.objs[i]);
 
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JSONResponse resp = serializer.Deserialize<JSONResponse>(strResponse);

                if (resp.result != "success")
                {
                    failed++;
                    Logger.log("Failed to update " + FetchedRecords.response.recs.objs[i].name + " " + resp.msg, Logger.Level.Error);
                }
                else
                {
                    updated++;
                }
            }

            Logger.log("Update at " + DateTime.Now + " - " + updated.ToString() + " updated, " + up_to_date.ToString() + " up to date, " + skipped.ToString() + " skipped, " + ignored.ToString() + " ignored, " + failed.ToString() + " failed", Logger.Level.Info);
        }


        /// <summary>
        /// Form entry point
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            autoUpdateTimer = new System.Timers.Timer(Convert.ToInt32(SettingsManager.getSetting("FetchTime")) * 60000); //Minutes to milliseconds
            autoUpdateTimer.Elapsed += autoUpdateTimer_Tick;
            autoUpdateTimer.Enabled = true;

            logUpdateTimer = new System.Timers.Timer(1000); //Refresh the log every second
            logUpdateTimer.Elapsed += logUpdateTimer_Tick;
            logUpdateTimer.Enabled = true;

            Logger.log("Starting auto updates every " + SettingsManager.getSetting("FetchTime") + " minutes for domain " + SettingsManager.getSetting("Domain"), Logger.Level.Info);
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
            trayIcon.Dispose();
        }


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
        }


        /// <summary>
        /// Get current external address and CloudFlare records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fetchDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread fetchThread = new Thread(new ThreadStart(FetchRecords));
            fetchThread.Start();
        }


        /// <summary>
        /// Update the selected records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread updateThread = new Thread(new ThreadStart(UpdateRecords));
            updateThread.Start();
        }


        /// <summary>
        /// Open the settings box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings settingsForm = new frmSettings(); settingsForm.Show();
        }


        /// <summary>
        /// Minimise the appplication to the task tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimiseToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        /// <summary>
        /// Thread to run updates every x minutes
        /// </summary>
        private void timerUpdateThread()
        {
            FetchRecords();
            UpdateRecords();
        }


        /// <summary>
        /// Auto update every x minutes, creates a new timerUpdateThread() thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(timerUpdateThread));
            thread.Start();
        }


        /// <summary>
        /// Tick every 1000ms to add new log entries to the listview control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logUpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach(ListViewItem ListItem in Logger.m_LogItems)
            {
                this.Invoke(new addLogEntryInvoker(addLogEntry), ListItem);
            }
            Logger.m_LogItems.Clear();
        }


        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Restore when double clicking the tray icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }


        /// <summary>
        /// Show the about form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.Show();
        }


    }
}
