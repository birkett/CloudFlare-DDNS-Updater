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
        public void updateRecords(ICollection<DomainDnsJsonResponse> fetchedRecords)
        {
            if (fetchedRecords == null) //Dont attempt updates if the fetch failed
                return;

            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
            string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

            foreach (var fetchedRecord in fetchedRecords)
            {
                var record = fetchedRecord.Response.response;
                for (int i = 0; i < record.recs.count; i++)
                {
                    //Skip over MX and CNAME records
                    //TODO: Dont skip them :)
                    if (record.recs.objs[i].type != "A")
                    {
                        skipped++;
                        continue;
                    }


                    //Ignore anything that is not checked
                    if ((Array.IndexOf(selectedHosts, record.recs.objs[i].display_name) >= 0) != true)
                    {
                        ignored++;
                        continue;
                    }

                    //Skip over anything that doesnt need an update
                    if (record.recs.objs[i].content ==
                        Program.settingsManager.getSetting("ExternalAddress").ToString())
                    {
                        up_to_date++;
                        continue;
                    }

                    string strResponse = this.updateCloudflareRecords(record.recs.objs[i]);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    JsonResponse resp = serializer.Deserialize<JsonResponse>(strResponse);

                    if (resp.result != "success")
                    {
                        failed++;
                        Logger.log(
                            Properties.Resources.Logger_Failed + " " + record.recs.objs[i].name + " " +
                            resp.msg, Logger.Level.Error);
                    }
                    else
                    {
                        updated++;
                    }
                }
            }

            Logger.log("Update at " + DateTime.Now + " - " + updated.ToString(Program.cultureInfo) + " updated, " + up_to_date.ToString(Program.cultureInfo) + " up to date, " + skipped.ToString(Program.cultureInfo) + " skipped, " + ignored.ToString(Program.cultureInfo) + " ignored, " + failed.ToString(Program.cultureInfo) + " failed", Logger.Level.Info);

        }//end updateRecords()


        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public string getExternalAddress()
        {
            string strResponse = webRequest(Method.Get, "http://checkip.dyndns.org", null);
            string[] strResponse2 = strResponse.Split(':');
            string strResponse3 = strResponse2[1].Substring(1);
            string new_external_address = strResponse3.Split('<')[0];

            if (new_external_address == null)
                return null; //Bail if failied, keeping the current address in settings

            if (new_external_address != Program.settingsManager.getSetting("ExternalAddress").ToString())
            {
                Program.settingsManager.setSetting("ExternalAddress", new_external_address);
                Program.settingsManager.saveSettings();
            }

            return new_external_address;

        }//end getExternalAddress()

        /// <summary>
        /// Get the list of domains from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public DomainJsonResponse getCloudFlareDomains()
        {
            DomainJsonResponse fetchedRecords = null;

            string postData = "a=zone_load_multi";
            postData += "&tkn=" + Program.settingsManager.getSetting("APIKey").ToString();
            postData += "&email=" + Program.settingsManager.getSetting("EmailAddress").ToString();

            string records = webRequest(Method.Post, "https://www.cloudflare.com/api_json.html", postData);
            if (records == null)
                return null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            fetchedRecords = serializer.Deserialize<DomainJsonResponse>(records);

            if (fetchedRecords.result != "success")
            {
                Logger.log(fetchedRecords.msg, Logger.Level.Error);
                return null;
            }

            return fetchedRecords;

        }//end getCloudflareRecords()

        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public List<DomainDnsJsonResponse> getCloudFlareRecords()
        {
            var result = new List<DomainDnsJsonResponse>();
            var domain = Program.settingsManager.getSetting("Domain").ToString();
            if (String.IsNullOrEmpty(domain) || domain == "*")
            {
                var domainResponse = getCloudFlareDomains();
                if (domainResponse != null)
                {
                    var zones = domainResponse.response.zones.objs;
                    foreach (var zone in zones)
                    {
                        var zoneRecord = getCloudFlareRecords(zone.zone_name);
                        if (zoneRecord != null)
                        {
                            result.Add(new DomainDnsJsonResponse { Domain = zone.zone_name, Response = zoneRecord });
                        }
                    }
                }
            }
            else
            {
                var domainRecord = getCloudFlareRecords(domain);
                if (domainRecord != null)
                {
                    result.Add(new DomainDnsJsonResponse { Domain = domain, Response = domainRecord });
                }
            }

            return result;
        }//end getCloudflareRecords()

        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        private JsonResponse getCloudFlareRecords(string domain)
        {
            JsonResponse fetchedRecords = null;

            string postData = "a=rec_load_all";
            postData += "&tkn=" + Program.settingsManager.getSetting("APIKey");
            postData += "&email=" + Program.settingsManager.getSetting("EmailAddress");
            postData += "&z=" + domain;

            string records = webRequest(Method.Post, "https://www.cloudflare.com/api_json.html", postData);
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

        }//end getCloudflareRecords()


        /// <summary>
        /// Run an update on the given record
        /// </summary>
        /// <param name="FetchedRecord"></param>
        /// <returns></returns>
        private string updateCloudflareRecords(DnsRecord FetchedRecord)
        {
            string postData = "a=rec_edit";
            postData += "&tkn=" + Program.settingsManager.getSetting("APIKey").ToString();
            postData += "&id=" + FetchedRecord.rec_id;
            postData += "&email=" + Program.settingsManager.getSetting("EmailAddress").ToString();
            postData += "&z=" + Program.settingsManager.getSetting("Domain").ToString();
            postData += "&type=" + FetchedRecord.type;
            postData += "&name=" + FetchedRecord.name;
            postData += "&content=" + Program.settingsManager.getSetting("ExternalAddress").ToString();
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
        private string webRequest(Method MethodType, string szUrl, string szData)
        {
            WebRequest webrequest = WebRequest.Create(szUrl);
            byte[] data = null;

            if (szData != null)
                data = Encoding.ASCII.GetBytes(szData);

            if (MethodType == Method.Post)
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
