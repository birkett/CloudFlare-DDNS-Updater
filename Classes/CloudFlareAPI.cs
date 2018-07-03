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
        public List<Result> updateRecords(Cloudflare_get_dns_records_Response fetchedRecords)
        {
            //List for the Updated IPs
            List<Result> return_updated_list = new List<Result>();

            if (fetchedRecords == null) //Dont attempt updates if the fetch failed
                return return_updated_list;

            int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
            string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');
 
            foreach (Result r in fetchedRecords.result)
            {
                //Skip over MX and CNAME records
                //TODO: Dont skip them :)
                bool NeedIp = false;
                switch (r.type)
                {
                    case "A":
                        NeedIp = true;
                        break;
                    case "AAAA":
                        NeedIp = true;
                        break;
                }
                if (NeedIp == false)
                {
                    skipped++;
                    continue;
                }


                //Ignore anything that is not checked
                if ((Array.IndexOf(selectedHosts, r.name) >= 0) != true)
                {
                    ignored++;
                    continue;
                }

                //Skip over anything that doesnt need an update
                if (r.content == Program.settingsManager.getSetting("ExternalAddressIPV4").ToString() || r.content == Program.settingsManager.getSetting("ExternalAddressIPV6").ToString())
                {
                    up_to_date++;
                    continue;
                }
                string strResponse = "";
                try
                {
                    strResponse = this.updateCloudflareRecords(r);
                }
                catch (Exception) {
                    Logger.log("Failed to Update " + r.name, Logger.Level.Error);
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
        public Cloudflare_get_dns_records_Response getCloudFlareRecords(string SelectedZone)
        {
            WebHeaderCollection headerData = new WebHeaderCollection();
            string url = Program.settingsManager.getSetting("APIUrl").ToString();
            if (!string.IsNullOrEmpty(SelectedZone))
            {
                url += "/zones/" + SelectedZone + "/dns_records?page=1&per_page=50&order=type&direction=asc";

                headerData.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());
                headerData.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());


                //Check if request still get value.
                string records = "";
                try
                {
                    records = webRequest(Method.Get, url, headerData);
                }
                catch (Exception)
                {
                    Logger.log("Cant reach" + url, Logger.Level.Error);
                    return null;
                }
                if (records == null || records == "error")
                    return null;

                JavaScriptSerializer jss = new JavaScriptSerializer();

                return jss.Deserialize<Cloudflare_get_dns_records_Response>(records);
            }
            throw new Exception();
        }//end getCloudflareRecords()

        public Cloudflare_get_Zone_list_Response getCloudFlareZones()
        {
            WebHeaderCollection headerData = new WebHeaderCollection();
            string url = Program.settingsManager.getSetting("APIUrl").ToString();

            url += "/zones?status=active&page=1&per_page=50&order=status&direction=desc&match=all";

            headerData.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());
            headerData.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());


            //Check if request still get value.
            string records = "";
            try
            {
                records = webRequest(Method.Get, url, headerData);
            }
            catch (Exception)
            {
                Logger.log("Cant reach" + url, Logger.Level.Error);
                return null;
            }
            if (records == null || records == "error")
                return null;

            JavaScriptSerializer jss = new JavaScriptSerializer();

            return jss.Deserialize<Cloudflare_get_Zone_list_Response>(records);
        }

        /// <summary>
        /// Run an update on the given record
        /// </summary>
        /// <param name="FetchedRecord"></param>
        /// <returns></returns>
        private string updateCloudflareRecords(Result FetchedRecord)
        {
            WebHeaderCollection headerData = new WebHeaderCollection();
            string url = Program.settingsManager.getSetting("APIUrl").ToString();
            string ip = "";
            if (!string.IsNullOrEmpty(FetchedRecord.zone_id) && !string.IsNullOrEmpty(FetchedRecord.id))
            {
                url += "/zones/" + FetchedRecord.zone_id + "/dns_records/" + FetchedRecord.id;


                headerData.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString());
                headerData.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString());

                //Switch IPV4 and IPV6 if its A or AAAA
                if (FetchedRecord.type.ToString() == "A")
                {
                    if (string.IsNullOrEmpty(Program.settingsManager.getSetting("ExternalAddressIPV4").ToString()))
                    {
                        throw new Exception();
                    }
                    ip = Program.settingsManager.getSetting("ExternalAddressIPV4").ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(Program.settingsManager.getSetting("ExternalAddressIPV6").ToString()))
                    {
                        throw new Exception();
                    }
                    ip = Program.settingsManager.getSetting("ExternalAddressIPV6").ToString();
                }

                try
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(new { type = FetchedRecord.type, name = FetchedRecord.name, content = ip });
                    webRequest(Method.Put, url, headerData, data);
                    return FetchedRecord.name;
                }
                catch (Exception)
                {
                    Logger.log("Cant reach " + url, Logger.Level.Error);
                    return null;
                }
            }
            throw new Exception();
        }//end updateCloudflareRecords()


        /// <summary>
        /// Enum to contain web request types (GET or POST)
        /// </summary>
        private enum Method
        {
            Get = 0,
            Post = 1,
            Put = 2,
        } //end enum



        /// <summary>
        /// Make a web request via GET or POST
        /// </summary>
        /// <param name="MethodType"></param>
        /// <param name="szUrl"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <param name="szData"></param>
        /// <returns></returns>
        private string webRequest(Method MethodType, string szUrl, WebHeaderCollection headers,string data=null)
        {
            WebRequest webrequest = WebRequest.Create(szUrl);

            if (MethodType == Method.Post)
            {
                webrequest.Method = "POST";
                webrequest.ContentType = "application/json";
            }
            if (MethodType == Method.Get)
            {
                webrequest.ContentType = "application/json";
                if (headers != null)
                {
                    webrequest.Headers = headers;
                }
            }
            string strResponse = null;
            try
            {
                if (MethodType == Method.Post)
                {
                    using (Stream stream = webrequest.GetRequestStream())
                    {
                        //stream.Write(data, 0, data.Length);
                    }

                    using (WebResponse webresponse = webrequest.GetResponse())
                    {
                        using (StreamReader readStream = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
                        {
                            strResponse = readStream.ReadToEnd();
                        }
                    }
                }
                if (MethodType == Method.Get)
                {
                    using (System.IO.Stream s = webrequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            strResponse = sr.ReadToEnd();
                            Console.WriteLine(String.Format("Response: {0}", strResponse));
                        }
                    }
                }
                if (MethodType == Method.Put)
                {
                    try
                    {
                        byte[] sentData = Encoding.UTF8.GetBytes(data);
                        using (var client = new System.Net.WebClient())
                        {
                            client.Headers = headers;
                            client.UploadData(szUrl, "PUT", sentData);
                        }
                        return "no error";
                    }catch(Exception e)
                    {
                        Logger.log("[Exception info below]IP Updated failed:" + szUrl, Logger.Level.Error);
                        Logger.log(e.ToString(), Logger.Level.Error);
                        throw new Exception();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.log("[Exception info below]Exception on request:" + szUrl, Logger.Level.Error);
                Logger.log(e.ToString(), Logger.Level.Error);
                return "error";
            }

            return strResponse;

        }//end webRequest()


    }//end class

    public class Cloudflare_get_Zone_list_Response
    {
        public Result[] result { get; set; }
        public Result_Info result_info { get; set; }
        public bool success { get; set; }
        public object[] errors { get; set; }
        public object[] messages { get; set; }
    }

    public class Result_Info
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_pages { get; set; }
        public int count { get; set; }
        public int total_count { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }

        public string content { get; set; }
        public bool proxiable { get; set; }
        public bool proxied { get; set; }
        public int ttl { get; set; }
        public string zone_id { get; set; }

        public bool paused { get; set; }
        public string type { get; set; }
        public int development_mode { get; set; }
        public string[] name_servers { get; set; }
        public string[] original_name_servers { get; set; }
        public object original_registrar { get; set; }
        public object original_dnshost { get; set; }
        public DateTime modified_on { get; set; }
        public DateTime created_on { get; set; }
        public Meta meta { get; set; }
        public Owner owner { get; set; }
        public Account account { get; set; }
        public string[] permissions { get; set; }
        public Plan plan { get; set; }
    }

    public class Meta
    {
        public int step { get; set; }
        public bool wildcard_proxiable { get; set; }
        public int custom_certificate_quota { get; set; }
        public int page_rule_quota { get; set; }
        public bool phishing_detected { get; set; }
        public bool multiple_railguns_allowed { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string type { get; set; }
        public string email { get; set; }
    }

    public class Account
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Plan
    {
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string currency { get; set; }
        public string frequency { get; set; }
        public bool is_subscribed { get; set; }
        public bool can_subscribe { get; set; }
        public string legacy_id { get; set; }
        public bool legacy_discount { get; set; }
        public bool externally_managed { get; set; }
    }

    public class Cloudflare_get_dns_records_Response
    {
        public Result[] result { get; set; }
        public bool success { get; set; }
        public object[] errors { get; set; }
        public object[] messages { get; set; }
        public Result_Info result_info { get; set; }
    }
}//end namespace
