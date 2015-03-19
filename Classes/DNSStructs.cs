using System.Runtime.Serialization;

namespace CloudFlareDDNS
{
    // These structs define the JSON response for rec_load_all
    //  This is all here for serialization - JSON to struct

    /// <summary>
    /// Each record contains a set of properties
    /// </summary>
    [DataContract]
    public struct DnsRecordProps
    {
        /// <summary>
        /// Can be proxied
        /// </summary>
        [DataMember]
        public string proxiable;
        /// <summary>
        /// Are CloudFlare services enabled for this record
        /// </summary>
        [DataMember]
        public string cloud_on;
        /// <summary>
        /// Is CloudFlare accepting requests to this host
        /// </summary>
        [DataMember]
        public string cf_open;
        /// <summary>
        /// Is SSL enabled
        /// </summary>
        [DataMember]
        public string ssl;
        /// <summary>
        /// Has the SSL certificate expired
        /// </summary>
        [DataMember]
        public string expired_ssl;
        /// <summary>
        /// Is the SSL certificate due to expire
        /// </summary>
        [DataMember]
        public string expiring_ssl;
        /// <summary>
        /// Is the SSL certificate pending update
        /// </summary>
        [DataMember]
        public string pending_ssl;
        /// <summary>
        /// CloudFlare vanity service
        /// </summary>
        [DataMember]
        public string vanity_lock;
    }//end DnsRecordProps

    /// <summary>
    /// The actual DNS record
    /// </summary>
    [DataContract]
    public struct DnsRecord
    {
        /// <summary>
        /// Record ID
        /// </summary>
        [DataMember]
        public string rec_id;
        /// <summary>
        /// Has of the full record
        /// </summary>
        [DataMember]
        public string rec_hash;
        /// <summary>
        /// Zone name
        /// </summary>
        [DataMember]
        public string zone_name;
        /// <summary>
        /// Record full name
        /// </summary>
        [DataMember]
        public string name;
        /// <summary>
        /// Record short name
        /// </summary>
        [DataMember]
        public string display_name;
        /// <summary>
        /// Type of record (A, CNAME, MX, SRV, TXT)
        /// </summary>
        [DataMember]
        public string type;
        /// <summary>
        /// MX priority
        /// </summary>
        [DataMember]
        public string prio;
        /// <summary>
        /// IP address or host which this record points to
        /// </summary>
        [DataMember]
        public string content;
        /// <summary>
        /// Friendly IP or host
        /// </summary>
        [DataMember]
        public string display_content;
        /// <summary>
        /// Time To Live in seconds
        /// </summary>
        [DataMember]
        public string ttl;
        /// <summary>
        /// Maximum TTL value
        /// </summary>
        [DataMember]
        public string ttl_ceil;
        /// <summary>
        /// SSL certificate ID
        /// </summary>
        [DataMember]
        public string ssl_id;
        /// <summary>
        /// Is SSL enabled
        /// </summary>
        [DataMember]
        public string ssl_status;
        /// <summary>
        /// SSL certificate expiry date
        /// </summary>
        [DataMember]
        public string ssl_expires_on;
        /// <summary>
        /// Automatic TTL configuration
        /// </summary>
        [DataMember]
        public string auto_ttl;
        /// <summary>
        /// Are CloudFlare services enabled for this record
        /// </summary>
        [DataMember]
        public string service_mode;
        /// <summary>
        /// Properties for this record
        /// </summary>
        [DataMember]
        public DnsRecordProps props;
    }//end DnsRecord

    /// <summary>
    /// The JSON field "recs" contains a count and an array of actual records
    /// </summary>
    [DataContract]
    public struct DnsRecords
    {
        /// <summary>
        /// Is this all of the records, or are more available
        /// </summary>
        [DataMember]
        public string has_more;
        /// <summary>
        /// How many records have been returned
        /// </summary>
        [DataMember]
        public string count;
        /// <summary>
        /// List of  records
        /// </summary>
        [DataMember]
        public DnsRecord[] objs;
    }//end DnsRecords

    /// <summary>
    /// Just a padding field, wraps up the records
    /// </summary>
    [DataContract]
    public struct DnsResponse
    {
        /// <summary>
        /// Padding for the records
        /// </summary>
        [DataMember]
        public DnsRecords recs;
    }//end DnsResponse

    /// <summary>
    /// The type of request that was made
    /// </summary>
    [DataContract]
    public struct DnsRequest
    {
        /// <summary>
        /// The type of request this response is for (rec_edit, rec_load_all)
        /// </summary>
        [DataMember]
        public string act;
    }//end DnsRequest

    /// <summary>
    /// Main response format
    /// </summary>
    [DataContract]
    public class JsonResponse
    {
        /// <summary>
        /// Request information
        /// </summary>
        [DataMember]
        public DnsRequest request;
        /// <summary>
        /// Response information
        /// </summary>
        [DataMember]
        public DnsResponse response;
        /// <summary>
        /// success or error
        /// </summary>
        [DataMember]
        public string result;
        /// <summary>
        /// Error message, blank on success
        /// </summary>
        [DataMember]
        public string msg;
    }//end JsonResponse
}
