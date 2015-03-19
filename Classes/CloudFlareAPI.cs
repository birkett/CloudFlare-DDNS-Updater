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

namespace CloudFlareDDNS
{
    /// <summary>
    /// Provides functions for interfacing with the CloudFlare REST API
    /// </summary>
    class CloudFlareAPI
    {


        /// <summary>
        /// Return the current external network address, using the default gateway
        /// </summary>
        /// <returns>IP address as a string, null on error</returns>
        public static string getExternalAddress()
        {
            string strResponse = webRequest(Method.Get, "http://checkip.dyndns.org", null);
            string[] strResponse2 = strResponse.Split(':');
            string strResponse3 = strResponse2[1].Substring(1);
            return strResponse3.Split('<')[0];

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
