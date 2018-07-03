using System;

namespace CloudFlareDDNS.Classes.JsonObjects.Cloudflare
{
    /// <summary>
    ///
    /// </summary>
    public class Result_Info
    {
        /// <summary>
        ///
        /// </summary>
        public int page { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int per_page { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int total_pages { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int count { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int total_count { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Result
    {
        /// <summary>
        /// dnsrecord id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Domain name (example.com)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Current IP
        /// </summary>
        public string content { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool proxiable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool proxied { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ttl { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string zone_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool paused { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int development_mode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[] name_servers { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[] original_name_servers { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object original_registrar { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object original_dnshost { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime modified_on { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime created_on { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Meta meta { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Owner owner { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Account account { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[] permissions { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Plan plan { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Meta
    {
        /// <summary>
        ///
        /// </summary>
        public int step { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool wildcard_proxiable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int custom_certificate_quota { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int page_rule_quota { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool phishing_detected { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool multiple_railguns_allowed { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Owner
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string email { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Account
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Plan
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int price { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string frequency { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool is_subscribed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool can_subscribe { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string legacy_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool legacy_discount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool externally_managed { get; set; }
    }
}