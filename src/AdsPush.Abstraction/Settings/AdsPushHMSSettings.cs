namespace AdsPush.Abstraction.Settings
{
    public class AdsPushHMSSettings
    {
        /// <summary>
        /// A Url that helps to send request to get request access token
        /// "https://oauth-login.cloud.huawei.com/oauth2/v3/token"
        /// </summary>
        public string IdentityUrl { get; set; }


        /// <summary>
        /// A Url that helps to send request to get request access token
        /// "https://push-api.cloud.huawei.com/v1/{clientId}/messages:send"
        /// </summary>
        public string ApiBaseUrl { get; set; }

        //Message Type - Notification or Data


        /// <summary>
        /// Gets or sets the APP ID from AppGallery Connect.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets orset the App Secret from AppGallery Connect.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}

