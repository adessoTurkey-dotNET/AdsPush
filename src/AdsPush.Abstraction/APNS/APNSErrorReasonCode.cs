namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// https://developer.apple.com/library/archive/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/CommunicatingwithAPNs.html#//apple_ref/doc/uid/TP40008194-CH11-SW15
    /// </summary>
    public enum APNSErrorReasonCode
    {
        /// <summary>
        /// 
        /// </summary>
        BadCollapseId,
        /// <summary>
        /// 
        /// </summary>
        BadDeviceToken,
        /// <summary>
        /// 
        /// </summary>
        BadExpirationDate,
        /// <summary>
        /// 
        /// </summary>
        BadMessageId,
        /// <summary>
        /// 
        /// </summary>
        BadPriority,
        /// <summary>
        /// 
        /// </summary>
        BadTopic,
        /// <summary>
        /// 
        /// </summary>
        DeviceTokenNotForTopic,
        /// <summary>
        /// 
        /// </summary>
        DuplicateHeaders,
        /// <summary>
        /// 
        /// </summary>
        IdleTimeout,
        /// <summary>
        /// 
        /// </summary>
        MissingDeviceToken,
        /// <summary>
        /// 
        /// </summary>
        MissingTopic,
        /// <summary>
        /// 
        /// </summary>
        PayloadEmpty,
        /// <summary>
        /// 
        /// </summary>
        TopicDisallowed,
        /// <summary>
        /// 
        /// </summary>
        BadCertificate,
        /// <summary>
        /// 
        /// </summary>
        BadCertificateEnvironment,
        /// <summary>
        /// 
        /// </summary>
        ExpiredProviderToken,
        /// <summary>
        /// 
        /// </summary>
        Forbidden,
        /// <summary>
        /// 
        /// </summary>
        InvalidProviderToken,
        /// <summary>
        /// 
        /// </summary>
        MissingProviderToken,
        /// <summary>
        /// 
        /// </summary>
        BadPath,
        /// <summary>
        /// 
        /// </summary>
        MethodNotAllowed,
        /// <summary>
        /// 
        /// </summary>
        Unregistered,
        /// <summary>
        /// 
        /// </summary>
        PayloadTooLarge,
        /// <summary>
        /// 
        /// </summary>
        TooManyProviderTokenUpdates,
        /// <summary>
        /// 
        /// </summary>
        TooManyRequests,
        /// <summary>
        /// 
        /// </summary>
        InternalServerError,
        /// <summary>
        /// 
        /// </summary>
        ServiceUnavailable,
        /// <summary>
        /// 
        /// </summary>
        Shutdown,
    }
}
