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
using System.Collections.Generic;
using System;

namespace CloudFlareDDNS
{
    // These structs define the JSON response for rec_load_all
    //  This is all here for serialization - JSON to struct

    /// <summary>
    /// Each record contains a set of properties
    /// The API will return 0 / 1 values for these, not valud boolean true / false - using short type
    /// </summary>
    public struct DnsRecordProps
    {
        /// <summary>
        /// Can be proxied
        /// </summary>
        public short proxiable { get; set; }
        /// <summary>
        /// Are CloudFlare services enabled for this record
        /// </summary>
        public short cloud_on { get; set; }
        /// <summary>
        /// Is CloudFlare accepting requests to this host
        /// </summary>
        public short cf_open { get; set; }
        /// <summary>
        /// Is SSL enabled
        /// </summary>
        public short ssl { get; set; }
        /// <summary>
        /// Has the SSL certificate expired
        /// </summary>
        public short expired_ssl { get; set; }
        /// <summary>
        /// Is the SSL certificate due to expire
        /// </summary>
        public short expiring_ssl { get; set; }
        /// <summary>
        /// Is the SSL certificate pending update
        /// </summary>
        public short pending_ssl { get; set; }
        /// <summary>
        /// CloudFlare vanity service
        /// </summary>
        public short vanity_lock { get; set; }
    }//end DnsRecordProps


    /// <summary>
    /// The actual DNS record
    /// </summary>
    public struct DnsRecord
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public string rec_id { get; set; }
        /// <summary>
        /// Hash of the full record
        /// </summary>
        public string rec_hash { get; set; }
        /// <summary>
        /// Zone name
        /// </summary>
        public string domain { get; set; }
        /// <summary>
        /// record domain
        /// </summary>
        public string zone_name { get; set; }
        /// <summary>
        /// Record full name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Record short name
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// Type of record (A, CNAME, MX, SRV, TXT)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// MX priority
        /// </summary>
        public int? prio { get; set; } //nullable int
        /// <summary>
        /// IP address or host which this record points to
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// Friendly IP or host
        /// </summary>
        public string display_content { get; set; }
        /// <summary>
        /// Time To Live in seconds
        /// </summary>
        public int ttl { get; set; }
        /// <summary>
        /// Maximum TTL value
        /// </summary>
        public int ttl_ceil { get; set; }
        /// <summary>
        /// SSL certificate ID
        /// </summary>
        public string ssl_id { get; set; }
        /// <summary>
        /// Is SSL enabled
        /// </summary>
        public string ssl_status { get; set; }
        /// <summary>
        /// SSL certificate expiry date
        /// </summary>
        public string ssl_expires_on { get; set; }
        /// <summary>
        /// Automatic TTL configuration
        /// </summary>
        public string auto_ttl { get; set; }
        /// <summary>
        /// Are CloudFlare services enabled for this record
        /// </summary>
        public string service_mode { get; set; }
        /// <summary>
        /// Properties for this record
        /// </summary>
        public DnsRecordProps props { get; set; }
        /// <summary>
        /// Zone (domain) of the record
        /// </summary>
        public string zone_id { get; set; }
        /// <summary>
        /// Whether the record is proxied or not
        /// </summary>
        public bool proxied { get; set; }

  }//end DnsRecord


  /// <summary>
  /// The JSON field "recs" contains a count and an array of actual records
  /// </summary>
  public struct DnsRecords
    {
        /// <summary>
        /// Is this all of the records, or are more available
        /// </summary>
        public bool has_more { get; set; }
        /// <summary>
        /// How many records have been returned
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// List of  records
        /// </summary>
        public DnsRecord[] objs { get; set; }
    }//end DnsRecords


    /// <summary>
    /// Just a padding field, wraps up the records
    /// </summary>
    public struct DnsResponse
    {
        /// <summary>
        /// Padding for the records
        /// </summary>
        public DnsRecords recs { get; set; }
    }//end DnsResponse


    /// <summary>
    /// The type of request that was made
    /// </summary>
    public struct DnsRequest
    {
        /// <summary>
        /// The type of request this response is for (rec_edit, rec_load_all)
        /// </summary>
        public string act { get; set; }
    }//end DnsRequest


  ///// <summary>
  ///// Main response format
  ///// </summary>
  public class JsonResponse
  {
    /// <summary>
    /// Request information
    /// </summary>
    public DnsRequest request { get; set; }
    /// <summary>
    /// Response information
    /// </summary>
    public DnsResponse response { get; set; }
    /// <summary>
    /// success or error
    /// </summary>
    public string result { get; set; }
    /// <summary>
    /// Error message, blank on success
    /// </summary>
    public string msg { get; set; }
  }//end JsonResponse

  //DNS Update response
  //Created using http://json2csharp.com/
  public class dnsUpdateResponseMeta
  {
    public bool auto_added { get; set; }
    public bool managed_by_apps { get; set; }
    public bool managed_by_argo_tunnel { get; set; }
  }

  public class Result
  {
    public string id { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string content { get; set; }
    public bool proxiable { get; set; }
    public bool proxied { get; set; }
    public int ttl { get; set; }
    public bool locked { get; set; }
    public string zone_id { get; set; }
    public string zone_name { get; set; }
    public DateTime modified_on { get; set; }
    public DateTime created_on { get; set; }
    public dnsUpdateResponseMeta meta { get; set; }
  }

  public class dnsUpdateResponseRootObject
  {
    public Result result { get; set; }
    public bool success { get; set; }
    public List<object> errors { get; set; }
    public List<object> messages { get; set; }
  }

  //Zone Structure
  //Created using http://json2csharp.com/
  public class zoneMeta
  {
    public int step { get; set; }
    public bool wildcard_proxiable { get; set; }
    public int custom_certificate_quota { get; set; }
    public int page_rule_quota { get; set; }
    public bool phishing_detected { get; set; }
    public bool multiple_railguns_allowed { get; set; }
  }

  public class zoneOwner
  {
    public string id { get; set; }
    public string type { get; set; }
    public string email { get; set; }
  }

  public class zoneAccount
  {
    public string id { get; set; }
    public string name { get; set; }
  }

  public class zonePlan
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

  public class zoneResult
  {
    public string id { get; set; }
    public string name { get; set; }
    public string status { get; set; }
    public bool paused { get; set; }
    public string type { get; set; }
    public int development_mode { get; set; }
    public List<string> name_servers { get; set; }
    public List<string> original_name_servers { get; set; }
    public string original_registrar { get; set; }
    public string original_dnshost { get; set; }
    public DateTime modified_on { get; set; }
    public DateTime created_on { get; set; }
    public zoneMeta meta { get; set; }
    public zoneOwner owner { get; set; }
    public zoneAccount account { get; set; }
    public List<string> permissions { get; set; }
    public zonePlan plan { get; set; }
  }

  public class zoneResultInfo
  {
    public int page { get; set; }
    public int per_page { get; set; }
    public int total_pages { get; set; }
    public int count { get; set; }
    public int total_count { get; set; }
  }

  public class zoneRootObject
  {
    public List<zoneResult> result { get; set; }
    public zoneResultInfo result_info { get; set; }
    public bool success { get; set; }
    public List<object> errors { get; set; }
    public List<object> messages { get; set; }

    public JsonResponse[] ToJsonResponse()
    {
      JsonResponse[] jsonResponseArray = new JsonResponse[result.Count];

      for (int i = 0; i < result.Count; i++) jsonResponseArray[i] = new JsonResponse();

      return jsonResponseArray;
    }
  }


  //DNS record structure
  //Created using http://json2csharp.com/
  public class dnsMeta
  {
    public bool auto_added { get; set; }
    public bool managed_by_apps { get; set; }
    public bool managed_by_argo_tunnel { get; set; }
  }

  public class dnsResult
  {
    public string id { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string content { get; set; }
    public bool proxiable { get; set; }
    public bool proxied { get; set; }
    public int ttl { get; set; }
    public bool locked { get; set; }
    public string zone_id { get; set; }
    public string zone_name { get; set; }
    public DateTime modified_on { get; set; }
    public DateTime created_on { get; set; }
    public dnsMeta meta { get; set; }
  }

  public class dnsResultInfo
  {
    public int page { get; set; }
    public int per_page { get; set; }
    public int total_pages { get; set; }
    public int count { get; set; }
    public int total_count { get; set; }
  }

  public class dnsRootObject
  {
    public List<dnsResult> result { get; set; }
    public dnsResultInfo result_info { get; set; }
    public bool success { get; set; }
    public List<object> errors { get; set; }
    public List<object> messages { get; set; }


    public DnsResponse ToDnsResponse()
    {
      DnsResponse response = new DnsResponse();
      DnsRecords dnsRecords = new DnsRecords();

      dnsRecords.count = result.Count;

      DnsRecord[] objs = new DnsRecord[result.Count];

      for (int i = 0; i < result.Count; i++)
      {
        objs[i].type = result[i].type;
        objs[i].name = result[i].name;
        objs[i].display_name = result[i].name;
        objs[i].ttl = result[i].ttl;
        objs[i].zone_name = result[i].zone_name;
        objs[i].rec_id = result[i].id;
        objs[i].zone_id = result[i].zone_id;
        objs[i].domain = result[i].zone_name;
        objs[i].proxied = result[i].proxied;
      }

      dnsRecords.objs = objs;
      response.recs = dnsRecords;

      return response;
    }
  }

}//end namespace
