using System;
using System.ServiceProcess;
using System.Threading;
using System.Web.Script.Serialization;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Run the update process as a Windows Service
    /// </summary>
    public partial class Service : ServiceBase
    {


        /// <summary>
        /// Used for auto updating
        /// </summary>
        private System.Timers.Timer autoUpdateTimer = null;


        /// <summary>
        /// Constructor
        /// </summary>
        public Service()
        {
            this.CanPauseAndContinue = true;
            this.ServiceName = "CloudFlareDDNS";

        }//end Service()


        /// <summary>
        /// Service startup
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            autoUpdateTimer = new System.Timers.Timer(Convert.ToInt32(SettingsManager.getSetting("FetchTime")) * 60000); //Minutes to milliseconds
            autoUpdateTimer.Elapsed += autoUpdateTimer_Tick;
            autoUpdateTimer.Enabled = true;

            Logger.log("Starting auto updates every " + SettingsManager.getSetting("FetchTime") + " minutes for domain " + SettingsManager.getSetting("Domain"), Logger.Level.Info);
        
        }//end OnStart()


        /// <summary>
        /// Service stop
        /// </summary>
        protected override void OnStop()
        {
            autoUpdateTimer.Enabled = false;
            Logger.log("Service stopping", Logger.Level.Info);

        }//end OnStop()


        /// <summary>
        /// Pause the service
        /// </summary>
        protected override void OnPause()
        {
            autoUpdateTimer.Enabled = false;
            base.OnPause();

        }//end OnPause()


        /// <summary>
        /// Resume the service
        /// </summary>
        protected override void OnContinue()
        {
            autoUpdateTimer.Enabled = true;
            base.OnContinue();

        }//end OnContinue()


        /// <summary>
        /// Thread to run updates every x minutes
        /// </summary>
        private void timerUpdateThread()
        {
            updateRecords(fetchRecords());

        }//end timerUpdateThread()


        /// <summary>
        /// Auto update every x minutes, creates a new timerUpdateThread() thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(timerUpdateThread));
            thread.Start();

        }//end autoUpdateTimer_Tick()


        /// <summary>
        /// Logic to get the external address and CloudFlare records
        /// </summary>
        private static JsonResponse fetchRecords()
        {
            JsonResponse fetchedRecords = new JsonResponse();
            string new_external_address = CloudFlareAPI.getExternalAddress();

            if (new_external_address == null)
                return null;

            SettingsManager.setSetting("ExternalAddress", new_external_address);
            SettingsManager.saveSettings();

            string records = CloudFlareAPI.getCloudflareRecords();
            if (records == null)
                return null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            fetchedRecords = serializer.Deserialize<JsonResponse>(records);

            if (fetchedRecords.result != "success")
            {
                Logger.log(fetchedRecords.msg, Logger.Level.Error);
                return null;
            }

            return fetchedRecords;

        }//end fetchRecords()


        /// <summary>
        /// Logic to update records
        /// </summary>
        private static void updateRecords(JsonResponse FetchedRecords)
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

                string strResponse = CloudFlareAPI.updateCloudflareRecords(FetchedRecords.response.recs.objs[i]);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JsonResponse resp = serializer.Deserialize<JsonResponse>(strResponse);

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
        
        }//end updateRecords()


    }//end class
}//end namespace
