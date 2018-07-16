﻿/*
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
    /// Logic to update records
    /// </summary>
    public void updateRecords(JsonResponse fetchedRecords)
    {
      if (fetchedRecords == null) //Dont attempt updates if the fetch failed
        return;

      int up_to_date = 0, skipped = 0, failed = 0, updated = 0, ignored = 0;
      string[] selectedHosts = Program.settingsManager.getSetting("SelectedHosts").ToString().Split(';');

      for (int i = 0; i < fetchedRecords.response.recs.count; i++)
      {
        //Skip over MX and CNAME records
        //TODO: Dont skip them :)
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

        JsonResponse resp = serializer.Deserialize<JsonResponse>(strResponse);

        if (resp.result != "success")
        {
          failed++;
          Logger.log(Properties.Resources.Logger_Failed + " " + fetchedRecords.response.recs.objs[i].name + " " + resp.msg, Logger.Level.Error);
        }
        else
        {
          updated++;
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
      string strResponse = webRequest(Method.GET, "http://checkip.dyndns.org");
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
    /// GET the listed records from Cloudflare using their API
    /// </summary>
    /// <returns>JSON stream of records, null on error</returns>
    public JsonResponse getCloudFlareRecords()
    {
      JsonResponse fetchedRecords = null;

      string zoneId = Program.settingsManager.getSetting("ZoneID").ToString();
      string url = "https://api.cloudflare.com/client/v4/zones/" + zoneId + "/dns_records";
      url += "?type=A&per_page=100";

      string records = webRequest(Method.GET, url);
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
      
      return webRequest(Method.POST, "https://api.cloudflare.com/client/v4/");

    }//end updateCloudflareRecords()


    /// <summary>
    /// Enum to contain web request types (GET or POST)
    /// </summary>
    private enum Method
    {
      GET,
      POST,
      PUT,
      DELETE
    } //end enum


    /// <summary>
    /// Make a web request via GET or POST
    /// </summary>
    /// <param name="MethodType"></param>
    /// <param name="szUrl"></param>
    /// <returns></returns>
    private string webRequest(Method MethodType, string szUrl)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      WebRequest webrequest = WebRequest.Create(szUrl);
      
      string email = Program.settingsManager.getSetting("EmailAddress").ToString();
      string key = Program.settingsManager.getSetting("APIKey").ToString();

      webrequest.ContentType = "application/json";

      webrequest.Headers.Add("X-Auth-Email", email);
      webrequest.Headers.Add("X-Auth-Key", key);

      webrequest.Method = MethodType.ToString();

      string strResponse = null;
      try
      {
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
