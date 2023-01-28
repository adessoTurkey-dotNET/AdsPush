namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// https://developer.apple.com/library/archive/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/CommunicatingwithAPNs.html#//apple_ref/doc/uid/TP40008194-CH11-SW15
    /// </summary>
    public enum APNSErrorReasonCode
    {
        BadCollapseId,
        BadDeviceToken,
        BadExpirationDate,
        BadMessageId,
        BadPriority,
        BadTopic,
        DeviceTokenNotForTopic,
        DuplicateHeaders,
        IdleTimeout,
        MissingDeviceToken,
        MissingTopic,
        PayloadEmpty,
        TopicDisallowed,
        BadCertificate,
        BadCertificateEnvironment,
        ExpiredProviderToken,
        Forbidden,
        InvalidProviderToken,
        MissingProviderToken,
        BadPath,
        MethodNotAllowed,
        Unregistered,
        PayloadTooLarge,
        TooManyProviderTokenUpdates,
        TooManyRequests,
        InternalServerError,
        ServiceUnavailable,
        Shutdown,
    }
}
