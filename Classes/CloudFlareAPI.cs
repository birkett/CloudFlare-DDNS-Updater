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
        /// And return changes
        /// </summary>
        public List<DnsRecord> updateRecords(JsonResponse fetchedRecords)
        {
            //List for the Updated IPs
            List<DnsRecord> return_updated_list = new List<DnsRecord>();

            if (fetchedRecords == null) //Dont attempt updates if the fetch failed
                return return_updated_list;

            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
            string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

            for (int i = 0; i < fetchedRecords.response.recs.count; i++)
            {
                //Skip over MX and CNAME records
                //TODO: Dont skip them :)
                bool NeedIp = false;
                switch (fetchedRecords.response.recs.objs[i].type)
                {
                    case "A":
                        NeedIp = true;
                        break;
                    case "AAAA":
                        NeedIp = true;
                        break;
                }
                if (NeedIp ==false)
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
                if (fetchedRecords.response.recs.objs[i].content == Program.settingsManager.getSetting("ExternalAddressIPV4").ToString() || fetchedRecords.response.recs.objs[i].content == Program.settingsManager.getSetting("ExternalAddressIPV6").ToString())
                {
                    up_to_date++;
                    continue;
                }
                string strResponse = "";
                try
                {
                     strResponse = this.updateCloudflareRecords(fetchedRecords.response.recs.objs[i]);
                }
                catch (Exception) { }

                if (!string.IsNullOrEmpty(strResponse))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    JsonResponse resp = serializer.Deserialize<JsonResponse>(strResponse);

                    if (resp.result != "success")
                    {
                        failed++;
                        Logger.log(Properties.Resources.Logger_Failed + " " + fetchedRecords.response.recs.objs[i].name + " " + resp.msg, Logger.Level.Error);
                    }
                    else
                    {

                        return_updated_list.Add(fetchedRecords.response.recs.objs[i]);
                        updated++;
                    }
                }
                else
                {
                    failed++;
                    Logger.log(Properties.Resources.Logger_Failed + " " + fetchedRecords.response.recs.objs[i].name + "Unknown Error 001, maybe no IPV4 or IPV6 ", Logger.Level.Error);
                }
            }

            Logger.log("Update at " + DateTime.Now + " - " + updated.ToString(Program.cultureInfo) + " updated, " + up_to_date.ToString(Program.cultureInfo) + " up to date, " + skipped.ToString(Program.cultureInfo) + " skipped, " + ignored.ToString(Program.cultureInfo) + " ignored, " + failed.ToString(Program.cultureInfo) + " failed", Logger.Level.Info);
            return return_updated_list;
        }//end updateRecords()


        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public void getExternalAddress()
        {
            //UPDATE IPV4
            string new_external_addressIPV4;
                string strResponseIPV4 = webRequest(Method.Get, Program.settingsManager.getSetting("IPV4UpdateURL").ToString(), null);
                if (Program.settingsManager.getSetting("IPV4UpdateURL").ToString().Contains("checkip.dyndns.org"))
                {
                    string[] strResponse2 = strResponseIPV4.Split(':');
                    string strResponse3 = strResponse2[1].Substring(1);
                    new_external_addressIPV4 = strResponse3.Split('<')[0];
                }
                else
                {
                    new_external_addressIPV4 = System.Text.RegularExpressions.Regex.Replace(strResponseIPV4, "<.*?>", String.Empty).Trim();
                }
            if (new_external_addressIPV4 != null)
            {

                if (new_external_addressIPV4 != Program.settingsManager.getSetting("ExternalAddressIPV4").ToString())
                {
                    Program.settingsManager.setSetting("ExternalAddressIPV4", new_external_addressIPV4);
                    Program.settingsManager.saveSettings();
                }
            }
            //UPDATE IPV6
            string new_external_addressIPV6;
            string strResponseIPV6 = webRequest(Method.Get, Program.settingsManager.getSetting("IPV6UpdateURL").ToString(), null);
                new_external_addressIPV6 = System.Text.RegularExpressions.Regex.Replace(strResponseIPV6, "<.*?>", String.Empty).Trim();
            if (new_external_addressIPV6 != null)
            {
                if (new_external_addressIPV6 != Program.settingsManager.getSetting("ExternalAddressIPV6").ToString())
                {
                    Program.settingsManager.setSetting("ExternalAddressIPV6", new_external_addressIPV6);
                    Program.settingsManager.saveSettings();
                }
            }

            }//end getExternalAddress()


        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public JsonResponse getCloudFlareRecords()
        {
            JsonResponse fetchedRecords = null;

            string postData = "a=rec_load_all";
            postData += "&tkn=" + Program.settingsManager.getSetting("APIKey").ToString();
            postData += "&email=" + Program.settingsManager.getSetting("EmailAddress").ToString();
            postData += "&z=" + Program.settingsManager.getSetting("Domain").ToString();

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
            //Switch IPV4 and IPV6 if its A or AAAA
            if (FetchedRecord.type.ToString() == "A")
            {
                if (string.IsNullOrEmpty(Program.settingsManager.getSetting("ExternalAddressIPV4").ToString()))
                {
                    throw new Exception();
                }
                postData += "&content=" + Program.settingsManager.getSetting("ExternalAddressIPV4").ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(Program.settingsManager.getSetting("ExternalAddressIPV6").ToString()))
                {
                    throw new Exception();
                }
                postData += "&content=" + Program.settingsManager.getSetting("ExternalAddressIPV6").ToString();
            }
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
