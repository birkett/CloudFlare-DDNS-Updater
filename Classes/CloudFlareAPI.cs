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
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Provides functions for interfacing with the CloudFlare REST API
    /// </summary>
    class CloudFlareAPI
    {


        /// <summary>
        /// Logic to get the external address and CloudFlare records
        /// </summary>
        public static JsonResponse fetchRecords()
        {
            JsonResponse fetchedRecords = null;

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
        public static void updateRecords(JsonResponse fetchedRecords)
        {
            if (fetchedRecords == null) //Dont attempt updates if the fetch failed
                return;

            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
            string[] selectedHosts = SettingsManager.getSetting("SelectedHosts").Split(';');

            for (int i = 0; i < Convert.ToInt32(fetchedRecords.response.recs.count); i++)
            {
                //Skip over anything that is not checked
                if ((Array.IndexOf(selectedHosts, fetchedRecords.response.recs.objs[i].display_name) >= 0) != true)
                {
                    ignored++;
                    continue;
                }

                //Skip over MX and CNAME records
                //TODO: Dont skip them :)
                if (fetchedRecords.response.recs.objs[i].type != "A")
                {
                    skipped++;
                    continue;
                }

                //Skip over anything that doesnt need an update
                if (fetchedRecords.response.recs.objs[i].content == SettingsManager.getSetting("ExternalAddress"))
                {
                    up_to_date++;
                    continue;
                }

                string strResponse = CloudFlareAPI.updateCloudflareRecords(fetchedRecords.response.recs.objs[i]);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JsonResponse resp = serializer.Deserialize<JsonResponse>(strResponse);

                if (resp.result != "success")
                {
                    failed++;
                    Logger.log("Failed to update " + fetchedRecords.response.recs.objs[i].name + " " + resp.msg, Logger.Level.Error);
                }
                else
                {
                    updated++;
                }
            }

            Logger.log("Update at " + DateTime.Now + " - " + updated.ToString() + " updated, " + up_to_date.ToString() + " up to date, " + skipped.ToString() + " skipped, " + ignored.ToString() + " ignored, " + failed.ToString() + " failed", Logger.Level.Info);

        }//end updateRecords()


        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public static string getExternalAddress()
        {
            string strResponse = webRequest(Method.Get, "http://checkip.dyndns.org", null);
            string[] strResponse2 = strResponse.Split(':');
            string strResponse3 = strResponse2[1].Substring(1);
            string new_external_address = strResponse3.Split('<')[0];

            if (new_external_address == null)
                return null; //Bail if failied, keeping the current address in settings

            if (new_external_address != SettingsManager.getSetting("ExternalAddress"))
            {
                SettingsManager.setSetting("ExternalAddress", new_external_address);
                SettingsManager.saveSettings();
            }

            return new_external_address;

        }//end getExternalAddress()


        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public static string getCloudflareRecords()
        {
            string postData = "a=rec_load_all";
            postData += "&tkn=" + SettingsManager.getSetting("APIKey");
            postData += "&email=" + SettingsManager.getSetting("EmailAddress");
            postData += "&z=" + SettingsManager.getSetting("Domain");

            return webRequest(Method.Post, "https://www.cloudflare.com/api_json.html", postData);

        }//end getCloudflareRecords()


        /// <summary>
        /// Run an update on the given record
        /// </summary>
        /// <param name="FetchedRecord"></param>
        /// <returns></returns>
        public static string updateCloudflareRecords(DnsRecord FetchedRecord)
        {
            string postData = "a=rec_edit";
            postData += "&tkn=" + SettingsManager.getSetting("APIKey");
            postData += "&id=" + FetchedRecord.rec_id;
            postData += "&email=" + SettingsManager.getSetting("EmailAddress");
            postData += "&z=" + SettingsManager.getSetting("Domain");
            postData += "&type=" + FetchedRecord.type;
            postData += "&name=" + FetchedRecord.name;
            postData += "&content=" + SettingsManager.getSetting("ExternalAddress");
            postData += "&service_mode=" + FetchedRecord.service_mode;
            postData += "&ttl=" + FetchedRecord.ttl;

            return webRequest(Method.Post, "https://www.cloudflare.com/api_json.html", postData);

        }//end updateCloudflareRecords()


        /// <summary>
        /// Enum to contain web request types (GET or POST)
        /// </summary>
        private enum Method
        {
            Get = 0,
            Post,

        } //end enum


        /// <summary>
        /// Make a web request via GET or POST
        /// </summary>
        /// <param name="MethodType"></param>
        /// <param name="szUrl"></param>
        /// <param name="szData"></param>
        /// <returns></returns>
        private static string webRequest(Method MethodType, string szUrl, string szData)
        {
            WebRequest webrequest = WebRequest.Create(szUrl);
            byte[] data = null;

            if(szData != null)
                data = Encoding.ASCII.GetBytes(szData);

            if(MethodType == Method.Post)
            {
                webrequest.Method = "POST";
                webrequest.ContentType = "application/x-www-form-urlencoded";
                webrequest.ContentLength = data.Length;
            }

            string strResponse = null;
            try
            {
                if (MethodType == Method.Post)
                {
                    using (Stream stream = webrequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

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
                Logger.log(e.ToString(), Logger.Level.Error);
                return null;
            }

            return strResponse;

        }//end webRequest()


    }//end class
}//end namespace
