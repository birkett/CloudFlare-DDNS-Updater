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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using CloudFlareDDNS.Classes.JsonObjects.Cloudflare;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Provides functions for interfacing with the CloudFlare REST API
    /// </summary>
    internal class CloudFlareAPI
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        /// <summary>
        /// Logic to update records
        /// And return changes
        /// </summary>
        public List<Result> updateRecords(ListView lv,GetDnsRecordsResponse fetchedRecords)
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
                switch (r.type)
                {
                    case "A":
                    case "AAAA":
                        break;
                    default:
                         skipped++;
                         continue;
                }

                //Ignore anything that is not checked
                if ((Array.IndexOf(selectedHosts, r.id) >= 0) != true)
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

                    if (lv != null)
                        UpdateLastChange(lv, r, Properties.Resources.Main_Change_IP);

                    strResponse = this.updateCloudflareRecords(r);

                    System.Threading.Thread.Sleep(1000); //DontNeedTooSpamCloudflareServer

                    if (lv != null)
                        UpdateLastChange(lv, r, DateTime.Now.ToShortDateString()+" "+ DateTime.Now.ToShortTimeString());
                }
                catch (Exception)
                {
                    if (lv != null)
                        UpdateLastChange(lv, r, Properties.Resources.Main_Change_IP_Failed);

                    Logger.log(Properties.Resources.Error_FailedRecordUpdate + r.name, Logger.Level.Error);
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
            try
            {
                //UPDATE IPV4
                string new_external_addressIPV4 = null;
                if (!Program.settingsManager.getSetting("UseInternalIP").ToBool())
                {
                    if (string.IsNullOrEmpty(Program.settingsManager.getSetting("IPV4UpdateURL").ToString()))
                    {
                        Program.settingsManager.setSetting("IPV4UpdateURL", "http://checkip.dyndns.org");
                        Program.settingsManager.saveSettings();
                    }

                    string strResponseIPV4 = webRequest(Method.Get, Program.settingsManager.getSetting("IPV4UpdateURL").ToString(), null);
                    if (strResponseIPV4 != null)
                    {
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
                    }
                }
                else
                {
                    Program.settingsManager.setSetting("ExternalAddressIPV4", GetLocalIPAddress());
                    Program.settingsManager.saveSettings();
                }
                //UPDATE IPV6
                string new_external_addressIPV6;
                if (string.IsNullOrEmpty(Program.settingsManager.getSetting("IPV4UpdateURL").ToString()))
                {
                    Program.settingsManager.setSetting("IPV6UpdateURL", "http://myexternalip.com/raw");
                    Program.settingsManager.saveSettings();
                }
                string strResponseIPV6 = webRequest(Method.Get, Program.settingsManager.getSetting("IPV6UpdateURL").ToString(), null);
                if (strResponseIPV6 != null)
                {
                    new_external_addressIPV6 = System.Text.RegularExpressions.Regex.Replace(strResponseIPV6, "<.*?>", String.Empty).Trim();

                    if (new_external_addressIPV6 != null)
                    {
                        if (new_external_addressIPV6 != Program.settingsManager.getSetting("ExternalAddressIPV6").ToString())
                        {
                            Program.settingsManager.setSetting("ExternalAddressIPV6", new_external_addressIPV6);
                            Program.settingsManager.saveSettings();
                        }
                    }
                }
            }catch(Exception e)
            {
                Logger.log(Properties.Resources.Error_CantFindNewIp + " |" + e, Logger.Level.Error);
            }
        }//end getExternalAddress()

        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public GetDnsRecordsResponse getCloudFlareRecords(string SelectedZone)
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
                    Logger.log(Properties.Resources.Error_CantReach + url, Logger.Level.Error);
                    return null;
                }
                if (records == null || records == "error")
                    return null;

                return jss.Deserialize<GetDnsRecordsResponse>(records);
            }
            throw new Exception();
        }//end getCloudflareRecords()

        public GetZoneListResponse getCloudFlareZones()
        {
            WebHeaderCollection headerData = new WebHeaderCollection();
            string url = Program.settingsManager.getSetting("APIUrl").ToString();

            url += "/zones?status=active&page=1&per_page=50&order=status&direction=desc&match=all";

            headerData.Add("X-Auth-Key", Program.settingsManager.getSetting("APIKey").ToString().Trim());
            headerData.Add("X-Auth-Email", Program.settingsManager.getSetting("EmailAddress").ToString().Trim());

            //Check if request still get value.
            string records = "";
            try
            {
                records = webRequest(Method.Get, url, headerData);
            }
            catch (Exception)
            {
                Logger.log(Properties.Resources.Error_CantReach + url, Logger.Level.Error);
                return null;
            }
            if (records == null || records == "error")
                return null;

            return jss.Deserialize<GetZoneListResponse>(records);
        }

        /// <summary>
        /// Run an update on the given record
        /// </summary>
        /// <param name="FetchedRecord"></param>
        /// <returns></returns>
        public string updateCloudflareRecords(Result FetchedRecord)
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
                        Logger.log(Properties.Resources.Error_CantGet + "IPv4", Logger.Level.Error);
                        return null;
                    }
                    ip = Program.settingsManager.getSetting("ExternalAddressIPV4").ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(Program.settingsManager.getSetting("ExternalAddressIPV6").ToString()))
                    {
                        Logger.log(Properties.Resources.Error_CantGet + "IPv6", Logger.Level.Error);
                        return null;
                    }
                    ip = Program.settingsManager.getSetting("ExternalAddressIPV6").ToString();
                }

                try
                {
                    string data = new JavaScriptSerializer().Serialize(new { type = FetchedRecord.type, name = FetchedRecord.name, content = ip, proxied = FetchedRecord.proxied });
                    webRequest(Method.Put, url, headerData, data);
                    return FetchedRecord.name;
                }
                catch (Exception)
                {
                    Logger.log(Properties.Resources.Error_CantReach + url, Logger.Level.Error);
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
            Put = 1,
        } //end enum

        /// <summary>
        /// Make a web request via GET or POST
        /// </summary>
        /// <param name="MethodType"></param>
        /// <param name="szUrl"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string webRequest(Method MethodType, string szUrl, WebHeaderCollection headers, string data = null)
        {
            if (string.IsNullOrEmpty(szUrl))
                return null;

            WebRequest webrequest = WebRequest.Create(szUrl);

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
                if (MethodType == Method.Get)
                {
                    using (System.IO.Stream s = webrequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            strResponse = sr.ReadToEnd();
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
                    }
                    catch (Exception e)
                    {
                        Logger.log(Properties.Resources.Error_FailedIPUpdate + szUrl, Logger.Level.Error);
                        Logger.log(e.ToString(), Logger.Level.Error);
                        throw new Exception();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.log(Properties.Resources.Error_ExceptionOnRequest + szUrl, Logger.Level.Error);
                Logger.log(e.ToString(), Logger.Level.Error);
                return "error";
            }

            return strResponse;
        }//end webRequest()

        /// <summary>
        /// Get Local IP Adress
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            Logger.log(Properties.Resources.Error_ExceptionNoNetworkAdapterFound, Logger.Level.Error);
            throw new Exception(Properties.Resources.Error_ExceptionNoNetworkAdapterFound);
        }
        /// <summary>
        /// Change the LastChange section in listview or set a loading text.
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="r"></param>
        /// <param name="text"></param>
        public void UpdateLastChange(ListView lv,Result r, string text = null)
        {
            lv.Invoke((MethodInvoker)delegate {
                foreach (ListViewItem item in lv.Items)
                {
                    if ((item.Tag as Result).id == r.id)
                    {
                        if (text == null)
                            text = r.modified_on.ToShortDateString() + " " + r.modified_on.ToShortTimeString();

                        if (lv.InvokeRequired)
                        {

                        }
                        else
                        {
                            item.SubItems[4].Text = text;
                        }
                    }
                }
            });
        }
    }//end class
}//end namespace