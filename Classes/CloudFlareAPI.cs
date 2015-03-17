using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace CloudFlare_DDNS
{
    class CloudFlareAPI
    {
        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public static string GetExternalAddress()
        {
            string strResponse = webRequest(Method.Get, "http://checkip.dyndns.org", null);
            string[] strResponse2 = strResponse.Split(':');
            string strResponse3 = strResponse2[1].Substring(1);
            return strResponse3.Split('<')[0];
        }


        /// <summary>
        /// Get the listed records from Cloudflare using their API
        /// </summary>
        /// <returns>JSON stream of records, null on error</returns>
        public static string GetCloudflareRecords()
        {
            string postData = "a=rec_load_all";
            postData += "&tkn=" + SettingsManager.getSetting("APIKey");
            postData += "&email=" + SettingsManager.getSetting("EmailAddress");
            postData += "&z=" + SettingsManager.getSetting("Domain");

            return webRequest(Method.Post, "https://www.cloudflare.com/api_json.html", postData);
        }

        public static string UpdateCloudflareRecords(DNSRecord FetchedRecord)
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
        }

        /// <summary>
        /// Enum to contain web request types (GET or POST)
        /// </summary>
        private enum Method
        {
            Get = 0,
            Post,
        }

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
        }
    }
}
