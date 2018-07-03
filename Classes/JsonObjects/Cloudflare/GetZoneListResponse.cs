namespace CloudFlareDDNS.Classes.JsonObjects.Cloudflare
{
    internal class GetZoneListResponse
    {
        /// <summary>
        ///
        /// </summary>
        public Result[] result { get; set; }

        /// <summary>
        ///
        /// </summary>
        public ResultInfo result_info { get; set; }

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