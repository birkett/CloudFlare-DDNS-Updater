using System;
using System.ServiceProcess;
using System.Threading;
using System.Web.Script.Serialization;

namespace CloudFlare_DDNS
{
    /// <summary>
    /// Run the update process as a Windows Service
    /// </summary>
    public partial class Service : ServiceBase
    {


        /// <summary>
        /// Constructor
        /// </summary>
        public Service()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Service startup
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            autoUpdateTimer.Interval = Convert.ToInt32(SettingsManager.getSetting("FetchTime")) * 60000; //Minutes to milliseconds
            autoUpdateTimer.Start();
        }


        /// <summary>
        /// Service stop
        /// </summary>
        protected override void OnStop()
        {
        }


        /// <summary>
        /// Thread to run updates every x minutes
        /// </summary>
        private void timerUpdateThread()
        {
            JSONResponse records = FetchRecords();
            UpdateRecords(records);
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
        /// Logic to get the external address and CloudFlare records
        /// </summary>
        private static JSONResponse FetchRecords()
        {
            JSONResponse FetchedRecords = new JSONResponse();
            string new_external_address = CloudFlareAPI.GetExternalAddress();

            if (new_external_address == null)
                return null;

            SettingsManager.setSetting("ExternalAddress", new_external_address);
            SettingsManager.saveSettings();

            string records = CloudFlareAPI.GetCloudflareRecords();
            if (records == null)
                return null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            FetchedRecords = serializer.Deserialize<JSONResponse>(records);

            if (FetchedRecords.result != "success")
            {
                Logger.log(FetchedRecords.msg, Logger.Level.Error);
                return null;
            }

            return FetchedRecords;
        }


        /// <summary>
        /// Logic to update records
        /// </summary>
        private static void UpdateRecords(JSONResponse FetchedRecords)
        {
            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
            string[] selectedHosts = SettingsManager.getSetting("SelectedHosts").Split(';');

            for (int i = 0; i < Convert.ToInt32(FetchedRecords.response.recs.count); i++)
            {
                //Skip over anything that was not checked
                if ((Array.IndexOf(selectedHosts, FetchedRecords.response.recs.objs[i].display_name) >= 0) != true)
                {
                    ignored++;
                    continue;
                }

                //Skip over MX and CNAME records
                //TODO: Dont skip them :)
                if (FetchedRecords.response.recs.objs[i].type != "A")
                {
                    skipped++;
                    continue;
                }

                //Skip over anything that doesnt need an update
                if (FetchedRecords.response.recs.objs[i].content == SettingsManager.getSetting("ExternalAddress"))
                {
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


    }
}
