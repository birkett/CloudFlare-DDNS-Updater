using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Helpers;

namespace CloudFlare_DDNS
{
    /// <summary>
    /// Main form - all main functionality
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Stores the fetched records in an accessible place
        /// </summary>
        public dynamic FetchedRecords = null;

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
            txtExternalAddress.Text = new_external_address;

            string records = CloudFlareAPI.GetCloudflareRecords();
            if (records == null)
                return;

            FetchedRecords = Json.Decode(records);

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
                ListViewItem row = listViewRecords.Items.Add("");
                row.SubItems.Add(FetchedRecords.response.recs.objs[i].type);
                row.SubItems.Add(FetchedRecords.response.recs.objs[i].display_name);
                row.SubItems.Add(FetchedRecords.response.recs.objs[i].display_content);

                foreach(string host in selectedHosts)
                {
                    if(host == FetchedRecords.response.recs.objs[i].display_name)
                    {
                        listViewRecords.FindItemWithText(FetchedRecords.response.recs.objs[i].display_name).Checked = true;
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
                if (listViewRecords.FindItemWithText(FetchedRecords.response.recs.objs[i].display_name).Checked == false)
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
                dynamic resp = System.Web.Helpers.Json.Decode(strResponse);

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
            autoupdateTimer.Interval = Convert.ToInt32(SettingsManager.getSetting("FetchTime")) * 60000; //Minutes to milliseconds
            autoupdateTimer.Start();
            Logger.setTargetControl(ref this.listViewLog);
            Logger.log("Starting auto updates every " + SettingsManager.getSetting("FetchTime") + " minutes for domain " + SettingsManager.getSetting("Domain"), Logger.Level.Info);
        }

        /// <summary>
        /// Clean up the tray icon when closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Closing(object sender, FormClosingEventArgs e)
        {
            autoupdateTimer.Dispose();
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
            FetchRecords();
        }

        /// <summary>
        /// Update the selected records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateRecords();
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
        /// Auto update every x minutes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            FetchRecords();
            UpdateRecords();
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
    }
}
