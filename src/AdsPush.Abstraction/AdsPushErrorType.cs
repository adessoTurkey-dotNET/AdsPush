namespace AdsPush.Abstraction
{
    /// <summary>
    /// AdsPush error categories.
    /// </summary>
    public enum AdsPushErrorType
    {
        /// <summary>
        /// Unknown 
        /// </summary>
        Unknown,
        /// <summary>
        /// Push token is invalid
        /// </summary>
        InvalidToken,
        /// <summary>
        /// could be be connect to the notification server.
        /// </summary>
        ServiceUnavailable,
        /// <summary>
        /// The passing argument(s) is invalid format or any required argument is missing. 
        /// </summary>
        InvalidArgument,
        /// <summary>
        /// Service certificate/credential is missing/wrong.
        /// </summary>
        InvalidAuthConfiguration
    }
}
