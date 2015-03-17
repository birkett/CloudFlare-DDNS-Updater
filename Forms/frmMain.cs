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
using System.Runtime.Serialization.Json;
using System.Diagnostics;

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
        public JSONResponse FetchedRecords = null;

        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        private string GetExternalAddress()
        {
            WebRequest webrequest = WebRequest.Create("http://checkip.dyndns.org");
            string strResponse;

            try
            {
                using (WebResponse webresponse = webrequest.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        strResponse = readStream.ReadToEnd();
                    }
                }
            }
            catch (Exception e) 
            { 
                Log(e.ToString(), 2); 
                return null; 
            }

            string[] strResponse2 = strResponse.Split(':');
            string strResponse3 = strResponse2[1].Substring(1);
            string[] strResponse4 = strResponse3.Split('<');

            return strResponse4[0];
        }

        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        private string GetCloudflareRecords()
        {
            WebRequest webrequest = WebRequest.Create("https://www.cloudflare.com/api_json.html");
            string postData = "a=rec_load_all";
            postData += "&tkn=" + SettingsManager.getSetting("APIKey");
            postData += "&email=" + SettingsManager.getSetting("EmailAddress");
            postData += "&z=" + SettingsManager.getSetting("Domain");
            byte[] data = Encoding.ASCII.GetBytes(postData);

            webrequest.Method = "POST";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.ContentLength = data.Length;

            string retval;
            try
            {
                using (Stream stream = webrequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (WebResponse webresponse = webrequest.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        retval = readStream.ReadToEnd();
                    }
                }
            }
            catch (Exception e) 
            { 
                Log(e.ToString(), 2); 
                return null; 
            }

            return retval;
        }

        /// <summary>
        /// Logic to get the external address and CloudFlare records
        /// </summary>
        private void FetchRecords()
        {
            listViewRecords.Items.Clear();
            FetchedRecords = null;

            string new_external_address = GetExternalAddress();

            if (new_external_address != null)
            {
                SettingsManager.setSetting("ExternalAddress", new_external_address);
                txtExternalAddress.Text = new_external_address;

                string records = GetCloudflareRecords();
                if (records != null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JSONResponse));
                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(records));
                    FetchedRecords = (JSONResponse)ser.ReadObject(stream);

                    if (FetchedRecords.result != "success")
                    {
                        Log(FetchedRecords.msg, 2);
                    }
                    else
                    {
                        for (int i = 0; i < Convert.ToInt32(FetchedRecords.response.recs.count); i++)
                        {
                            ListViewItem row = listViewRecords.Items.Add("");
                            row.SubItems.Add(FetchedRecords.response.recs.objs[i].type);
                            row.SubItems.Add(FetchedRecords.response.recs.objs[i].display_name);
                            row.SubItems.Add(FetchedRecords.response.recs.objs[i].display_content);
                        }
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
       
            int up_to_date = 0, skipped = 0, failed = 0, updated = 0;

            for (int i = 0; i < Convert.ToInt32(FetchedRecords.response.recs.count); i++)
            {
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

                //Skip over anything that is not checked
                if (listViewRecords.FindItemWithText(FetchedRecords.response.recs.objs[i].display_name).Checked == false)
                {
                    skipped++;
                    continue;
                }

                WebRequest webrequest = WebRequest.Create("https://www.cloudflare.com/api_json.html");
                string postData = "a=rec_edit";
                postData += "&tkn="          + SettingsManager.getSetting("APIKey");
                postData += "&id="           + FetchedRecords.response.recs.objs[i].rec_id;
                postData += "&email="        + SettingsManager.getSetting("EmailAddress");
                postData += "&z="            + SettingsManager.getSetting("Domain");
                postData += "&type="         + FetchedRecords.response.recs.objs[i].type;
                postData += "&name="         + FetchedRecords.response.recs.objs[i].name;
                postData += "&content="      + SettingsManager.getSetting("ExternalAddress");
                postData += "&service_mode=" + FetchedRecords.response.recs.objs[i].service_mode;
                postData += "&ttl="          + FetchedRecords.response.recs.objs[i].ttl;

                byte[] data = Encoding.ASCII.GetBytes(postData);

                webrequest.Method = "POST";
                webrequest.ContentType = "application/x-www-form-urlencoded";
                webrequest.ContentLength = data.Length;

                try
                {
                    using (Stream stream = webrequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    using (WebResponse webresponse = webrequest.GetResponse())
                    {
                        using (StreamReader readStream = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JSONResponse));
                            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(readStream.ReadToEnd()));
                            JSONResponse resp = (JSONResponse)ser.ReadObject(stream);

                            if (resp.result != "success")
                            {
                                failed++;
                                Log("Failed to update " + FetchedRecords.response.recs.objs[i].name + " " + resp.msg);
                            }
                            else
                            {
                                updated++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.ToString(), 2);
                }
            }

            if(FetchedRecords != null) //Dont log the summary if we did nothing
                Log("Update at " + DateTime.Now + " - " + updated.ToString() + " updated, " + up_to_date.ToString() + " up to date, " + skipped.ToString() + " skipped, " + failed.ToString() + " failed");
        }

        /// <summary>
        /// Add messages to the log view
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, int level = 0)
        {
            string sSource = "CloudFlare DDNS Updater";
            string sLog = "Application";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            ListViewItem row = listViewLog.Items.Add("");
            switch (level)
            {
                case 0: row.ImageIndex = 0; EventLog.WriteEntry(sSource, message, EventLogEntryType.Information); break;
                case 1: row.ImageIndex = 1; EventLog.WriteEntry(sSource, message, EventLogEntryType.Warning); break;
                case 2: row.ImageIndex = 2; EventLog.WriteEntry(sSource, message, EventLogEntryType.Error); break;
                default: row.ImageIndex = 0; EventLog.WriteEntry(sSource, message, EventLogEntryType.Information); break;
            }
            row.SubItems.Add(message);
        }

        /// <summary>
        /// Form entry point
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            notifyIcon1.BalloonTipText = "Updates will continue in the background";
            notifyIcon1.BalloonTipTitle = "CloudFlare DNS Updater";
            timer1.Interval = Convert.ToInt32(SettingsManager.getSetting("FetchTime")) * 60000; //Minutes to milliseconds
            timer1.Start();
            Log("Starting auto updates every " + SettingsManager.getSetting("FetchTime") + " minutes for domain " + SettingsManager.getSetting("Domain"));
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
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
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
            FetchRecords(); UpdateRecords();
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
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
    }
}
