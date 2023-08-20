namespace AdsPush.Abstraction.Settings
{
    public class AdsPushVapidSettings
    {
        /// <summary>
        /// Gets or sets the public key for VAPID authentication. This should be a URL-safe base64 encoded string.
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the private key for VAPID authentication. This should be a URL-safe base64 encoded string.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the subject for VAPID authentication. This should be a mailto or a URL.
        /// </summary>
        public string Subject { get; set; }
    }
}
