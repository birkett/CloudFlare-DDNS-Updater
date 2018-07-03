namespace CloudFlareDDNS.Classes.JsonObjects.Cloudflare
{
    internal class Get_zone_list_response
    {
        /// <summary>
        ///
        /// </summary>
        public Result[] result { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Result_Info result_info { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object[] errors { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object[] messages { get; set; }
    }
}