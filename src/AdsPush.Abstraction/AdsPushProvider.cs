namespace AdsPush.Abstraction
{
    /// <summary>
    /// Supported notification server provider.
    /// </summary>
    public enum AdsPushProvider
    {
        /// <summary>
        /// Apple push Notification Service
        /// </summary>
        Apns,
        /// <summary>
        /// FCM - Firebase Cloud Messaging.
        /// </summary>
        Firebase,

        /// <summary>
        /// Web Push
        /// </summary>
        VapidWebPush,
    }
}
