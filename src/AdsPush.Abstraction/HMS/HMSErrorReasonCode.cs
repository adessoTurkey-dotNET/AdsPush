namespace AdsPush.Abstraction.HMS
{
    public enum HMSErrorReasonCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 80000000,

        /// <summary>
        /// The message is successfully sent to some tokens. Tokens identified by illegal_tokens
        /// are those to which the message failed to be sent
        /// </summary>
        FailedIllegalTokens = 80100000,

        /// <summary>
        /// Some request parameters are incorrect.
        /// </summary>
        UnsupportedRequestParameters = 80100001,

        /// <summary>
        /// Incorrect message structure.
        /// </summary>
        BadMessageStructure = 80100003,

        /// <summary>
        /// The message expiration time is earlier than the current time.
        /// </summary>
        BadExpiryTime = 80100004,

        /// <summary>
        /// The collapse_key message field is invalid.
        /// </summary>
        BadCollapseKey = 80100013,

        /// <summary>
        /// A maximum of 100 topic-based messages can be sent at the same time.
        /// </summary>
        TopicOverload = 80100017,

        /// <summary>
        /// OAuth authentication error.
        /// </summary>
        FailedOAuth = 80200001,

        /// <summary>
        /// OAuth token expired.
        /// </summary>
        ExpiredOAuthToken = 80200003,

        /// <summary>
        /// The current app does not have the permission to send messages.
        /// </summary>
        NotPermitted = 80300002,

        /// <summary>
        /// All tokens are invalid.
        /// </summary>
        FailedAllTokens = 80300007,

        /// <summary>
        /// The message body size (excluding the token) exceeds the default value (4096 bytes).
        /// </summary>
        BodyOverlaod = 80300008,

        /// <summary>
        /// The number of tokens in the message body exceeds the default value.
        /// </summary>
        TokenCountOverload = 80300010,

        /// <summary>
        /// Invalid receipt URL.
        /// </summary>
        InvalidReceiptUrl = 80300013,

        /// <summary>
        /// Failed to request the OAuth service.
        /// </summary>
        FailedOAuthRequest = 80600003,

        /// <summary>
        /// An internal error of the system occurs.
        /// </summary>
        InternalError = 81000001
    }
}
