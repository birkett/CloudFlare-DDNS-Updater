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
        /// Has of the full record
        /// </summary>
        public string rec_hash { get; set; }
        /// <summary>
        /// Zone name
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


    /// <summary>
    /// Main response format
    /// </summary>
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


}//end namespace
