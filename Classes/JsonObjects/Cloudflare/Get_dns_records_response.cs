namespace CloudFlareDDNS.Classes.JsonObjects.Cloudflare
{
    internal class Get_dns_records_response
    {
        /// <summary>
        /// Get all DNS Records
        /// </summary>
        public Result[] result { get; set; }

        /// <summary>
        /// success msg
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// error msg's
        /// </summary>
        public object[] errors { get; set; }

        /// <summary>
        /// other msg's
        /// </summary>
        public object[] messages { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Result_Info result_info { get; set; }
    }
}