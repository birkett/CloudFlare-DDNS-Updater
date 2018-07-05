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

using System.Collections.Generic;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Provides functions for interfacing with the CloudFlare REST API
    /// </summary>
    class CloudFlareAPI
    {


        /// <summary>
        /// Logic to update records
        /// </summary>
        public void updateRecords(JsonResponse[] fetchedRecordsArray)
        {
            if (fetchedRecordsArray == null) //Dont attempt updates if the fetch failed
                return;

            for (int j = 0; j < fetchedRecordsArray.Length; j++)
            {
                JsonResponse fetchedRecords = fetchedRecordsArray[j];

                if (fetchedRecords != null) { 
                    int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
                    string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

                    for (int i = 0; i < fetchedRecords.response.recs.count; i++)
                    {
                        //Skip over MX and CNAME records
                        if (fetchedRecords.response.recs.objs[i].type != "A")
                        {
                            skipped++;
                            continue;
                        }

                        //Ignore anything that is not checked
                        if ((Array.IndexOf(selectedHosts, fetchedRecords.response.recs.objs[i].display_name) >= 0) != true)
                        {
                            ignored++;
                            continue;
                        }

                        //Skip over anything that doesnt need an update
                        if (fetchedRecords.response.recs.objs[i].content == Program.settingsManager.getSetting("ExternalAddress").ToString())
                        {
                            up_to_date++;
                            continue;
                        }

                        string strResponse = this.updateCloudflareRecords(fetchedRecords.response.recs.objs[i]);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        dnsUpdateResponseRootObject resp = serializer.Deserialize<dnsUpdateResponseRootObject>(strResponse);

                        if (!resp.success)
                        {
                            failed++;
                            //TODO
                            //Logger.log(Properties.Resources.Logger_Failed + " " + fetchedRecords.response.recs.objs[i].name + " " + resp.msg, Logger.Level.Error);
                            Logger.log(Properties.Resources.Logger_Failed + " " + fetchedRecords.response.recs.objs[i].name, Logger.Level.Error);
                        }
                        else
                        {
                            updated++;
                            Logger.log(fetchedRecords.response.recs.objs[i].name + ": update at " + DateTime.Now + " - " + updated.ToString(Program.cultureInfo) + " updated, " + up_to_date.ToString(Program.cultureInfo) + " up to date, " + skipped.ToString(Program.cultureInfo) + " skipped, " + ignored.ToString(Program.cultureInfo) + " ignored, " + failed.ToString(Program.cultureInfo) + " failed", Logger.Level.Info);

                        }
                    }
                }
            }
        }//end updateRecords()


        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public string getExternalAddress()
        {
            string new_external_address = webRequest(Method.Get, "https://api.ipify.org/", null, "application/x-www-form-urlencoded");

            if (new_external_address == null)
                return null; //Bail if failied, keeping the current address in settings

            if (new_external_address != Program.settingsManager.getSetting("ExternalAddress").ToString())
            {
                Program.settingsManager.setSetting("ExternalAddress", new_external_address);
                Program.settingsManager.saveSettings();
            }

            return new_external_address;

        }//end getExternalAddress()


        public zoneRootObject getCloudFlareZones()
        {
          zoneRootObject cloudFlareZones;

          WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
          myWebHeaderCollection.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());
          myWebHeaderCollection.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());

          string records = webRequest(Method.Get, "https://api.cloudflare.com/client/v4/zones", null, "application/json", myWebHeaderCollection);
          if (records == null)
            return null;

          JavaScriptSerializer serializer = new JavaScriptSerializer();

          cloudFlareZones = serializer.Deserialize<zoneRootObject>(records);

          if (!cloudFlareZones.success)
          {
            //TODO
            //Logger.log(fetchedRecords.messages, Logger.Level.Error);
            Logger.log("CloudFlare DDNS was not able to get zone from cloud flare servers", Logger.Level.Error);
            return null;
          }

          return cloudFlareZones;
        }

        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public JsonResponse[] getCloudFlareRecords()
        {
          zoneRootObject cloudFlareZones = getCloudFlareZones();
          JsonResponse[] fetchedRecordsArray = cloudFlareZones.ToJsonResponse();

          WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
          myWebHeaderCollection.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());
          myWebHeaderCollection.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());

          for (int i = 0; i < cloudFlareZones.result.Count; i++)
          {
            if (shouldUpdateRecord(cloudFlareZones.result[i].name))
            {
              string records = webRequest(Method.Get, "https://api.cloudflare.com/client/v4/zones/" + cloudFlareZones.result[i].id + "/dns_records?match=all",
                null, "application/json", myWebHeaderCollection);

              if (records == null)
                return null;

              JavaScriptSerializer serializer = new JavaScriptSerializer();

              dnsRootObject dnsRecords;
              dnsRecords = serializer.Deserialize<dnsRootObject>(records);

              if (!dnsRecords.success)
              {
                //TODO
                //Logger.log(fetchedRecords.messages, Logger.Level.Error);
                Logger.log("Error retrieving the DNS records for " + cloudFlareZones.result[i].name, Logger.Level.Error);
                return null;
              }

              fetchedRecordsArray[i].response = dnsRecords.ToDnsResponse();
            }
          }

          return fetchedRecordsArray;

        }//end getCloudflareRecords()

        private bool shouldUpdateRecord(string szDnsRecordName) {
          string[] selectedHosts = Program.settingsManager.getSetting("Domain").ToString().Split(';');

          for (int i = 0; i < selectedHosts.Length; i++)
          {
            if (selectedHosts[i].ToLower() == szDnsRecordName.ToLower()) return true;
          }

          return false;
        }   

        /// <summary>
        /// Run an update on the given record
        /// </summary>
        /// <param name="dnsRecord"></param>
        /// <returns></returns>
        private string updateCloudflareRecords(DnsRecord dnsRecord)
        {
            WebHeaderCollection myWebHeaderCollection = new WebHeaderCollection();
            myWebHeaderCollection.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());
            myWebHeaderCollection.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());

            string szRequestURL = "https://api.cloudflare.com/client/v4/zones/" + dnsRecord.zone_id + "/dns_records/" + dnsRecord.rec_id;
            string szRequestData = "{\"type\":\"A\",\"name\":\"" + dnsRecord.zone_name + "\",\"content\":\"" + Program.settingsManager.getSetting("ExternalAddress").ToString() + "\",\"ttl\":" + dnsRecord.ttl + ",\"proxied\":" + dnsRecord.proxied.ToString().ToLower() + "}";
            return webRequest(Method.Put, szRequestURL, szRequestData, "application/json", myWebHeaderCollection);

        } //end updateCloudflareRecords()


    /// <summary>
    /// Enum to contain web request types (GET or POST)
    /// </summary>
    private enum Method
    {
        Get = 0,
        Post,
        Put

    } //end enum


    /// <summary>
    /// Make a web request via GET or POST
    /// </summary>
    /// <param name="MethodType"></param>
    /// <param name="szUrl"></param>
    /// <param name="szData"></param>
    /// <param name="szContentType"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    private string webRequest(Method MethodType, string szUrl, string szData, string szContentType, WebHeaderCollection headers = null)
        {
            WebRequest webrequest = WebRequest.Create(szUrl);
            byte[] data = null;

            if (szData != null)
            {
              data = Encoding.UTF8.GetBytes(szData);
            }

            if (headers != null)
            {
              webrequest.Headers.Add(headers);
            }

            webrequest.ContentType = szContentType;

            switch (MethodType)
            {
              case Method.Post:
                webrequest.Method = "POST";
                break;
              case Method.Put:
                webrequest.Method = "PUT";
                break;
              case Method.Get:
              default:
                webrequest.Method = "GET";
                break;
            }

            string strResponse = null;
            try
            {
                if (MethodType == Method.Post || MethodType == Method.Put)
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
