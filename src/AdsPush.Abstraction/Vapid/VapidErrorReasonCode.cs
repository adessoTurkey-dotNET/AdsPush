namespace AdsPush.Abstraction.Vapid
{
    public enum VapidErrorReasonCode
    {
        /// <summary>
        /// Unknown error, possibly due to an unexpected scenario.
        /// </summary>
        UnknownError = 0,

        /// <summary>
        /// Push token is invalid or expired.
        /// </summary>
        InvalidToken = 1,

        /// <summary>
        /// The push notification service is unavailable or unreachable.
        /// </summary>
        ServiceUnavailable = 2,

        /// <summary>
        /// One or more of the provided arguments is invalid or missing.
        /// </summary>
        InvalidArgument = 3,

        /// <summary>
        /// The authentication configuration is missing or incorrect.
        /// </summary>
        InvalidAuthConfiguration = 4,
    }
}
