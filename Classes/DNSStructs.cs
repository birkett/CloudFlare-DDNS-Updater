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
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudFlareDDNS
{
  // These structs define the JSON response
  // This is all here for serialization - JSON to struct

  /// <summary>
  /// Each record contains a set of properties
  /// </summary>
  public struct DnsRecordMeta
  {
    /// <summary>
    /// Auto added?
    /// </summary>
    public bool auto_added { get; set; }
    /// <summary>
    /// Managed by apps?
    /// </summary>
    public bool managed_by_apps { get; set; }
    /// <summary>
    /// Managed by argo tunnel?
    /// </summary>
    public bool managed_by_argo_tunnel { get; set; }
  }//end DnsRecordMeta


  /// <summary>
  /// The DNS record
  /// </summary>
  public struct DnsRecord
  {
    /// <summary>
    /// Record ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// Type of record (A, AAAA, CNAME, MX, SRV, TXT)
    /// </summary>
    public string type { get; set; }
    /// <summary>
    /// Record full name
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// IP address or host which this record points to
    /// </summary>
    public string content { get; set; }
    /// <summary>
    /// Proxiable
    /// </summary>
    public bool proxiable { get; set; }
    /// <summary>
    /// Proxied
    /// </summary>
    public bool proxied { get; set; }
    /// <summary>
    /// Time To Live in seconds
    /// </summary>
    public int ttl { get; set; }
    /// <summary>
    /// Locked
    /// </summary>
    public bool locked { get; set; }
    /// <summary>
    /// Zone ID
    /// </summary>
    public string zone_id { get; set; }
    /// <summary>
    /// Zone name
    /// </summary>
    public string zone_name { get; set; }
    /// <summary>
    /// DNS Record Modified time
    /// </summary>
    public string modified_on { get; set; }
    /// <summary>
    /// DNS Record Created time
    /// </summary>
    public string created_on { get; set; }
    /// <summary>
    /// Properties for this record
    /// </summary>
    public DnsRecordMeta meta { get; set; }
  }//end DnsRecord

  /// <summary>
  /// DNS Details Error
  /// </summary>
  public struct DnsErrorChain
  {
    /// <summary>
    /// DNS Details Error Code
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// DNS Details Error Message
    /// </summary>
    public string message { get; set; }

  }//end DnsErrorChain

  /// <summary>
  /// CloudFlare API Error Messages
  /// </summary>
  public struct DnsError
  {
    /// <summary>
    /// Error Code
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// Error Message
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// Error Chain
    /// </summary>
    public List<DnsErrorChain> error_chain { get; set; }
  }//end DnsErrors


  /// <summary>
  /// DNS Result Info
  /// </summary>
  public struct DnsResultInfo
  {
    /// <summary>
    /// Pages of records
    /// </summary>
    public int page { get; set; }
    /// <summary>
    /// Amounts of records in this page
    /// </summary>
    public int per_page { get; set; }
    /// <summary>
    /// Total amounts of records
    /// </summary>
    public int total_pages { get; set; }
    /// <summary>
    /// Total amounts of records in this page
    /// </summary>
    public int count { get; set; }
    /// <summary>
    /// Total amounts of records
    /// </summary>
    public int total_count { get; set; }
  }//end DnsResultInfo


  /// <summary>
  /// Main response format
  /// </summary>
  public class JsonResponse
  {
    /// <summary>
    /// Result information
    /// </summary>
    [JsonProperty("result")]
    public dynamic result { get; set; }

    /// <summary>
    /// success or error
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// Error message, blank on success
    /// </summary>
    public List<DnsError> errors { get; set; }
    /// <summary>
    /// Messages, most of the time it will be blank
    /// </summary>
    public List<string> messages { get; set; }
    /// <summary>
    /// Request information
    /// </summary>
    public DnsResultInfo result_info { get; set; }
  }//end JsonResponse


}//end namespace
