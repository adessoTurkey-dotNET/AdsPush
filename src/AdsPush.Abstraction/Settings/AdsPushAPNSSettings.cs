using AdsPush.Abstraction.APNS;

namespace AdsPush.Abstraction.Settings
{
    /// <summary>
    /// The required settings to be able to connect to the Apple Push Notification Server.
    /// See for configuration: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/establishing_a_token-based_connection_to_apns
    /// </summary>
    public class AdsPushAPNSSettings
    {
        /// <summary>
        /// p8 certificate string without any space and start and end tags.
        /// </summary>
        public string P8PrivateKey { get; set; }

        /// <summary>
        /// 10 digit p8 certificate id. Usually a part of a downloadable certificate filename.
        /// </summary>
        public string P8PrivateKeyId { get; set; }

        /// <summary>
        /// Apple 10 digit team id shown in Apple Developer Membership Page.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// App slug / bundle name
        /// </summary>
        public string AppBundleIdentifier { get; set; }

        /// <summary>
        /// Development or Production server.
        /// <seealso cref="ApnServerType"/>
        /// </summary>
        public APNSEnvironmentType EnvironmentType { get; set; }
    }
}
