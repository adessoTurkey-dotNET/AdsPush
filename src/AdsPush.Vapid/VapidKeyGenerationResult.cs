namespace AdsPush.Vapid
{
    /// <summary>
    /// Returns from key generation request.
    /// <see cref="VapidHelper"/>
    /// </summary>
    public class VapidKeyGenerationResult
    {
        public VapidKeyGenerationResult(
            string publicLey,
            string privateKey)
        {
            this.PublicLey = publicLey;
            this.PrivateKey = privateKey;
        }

        /// <summary>
        /// Public key that's required for client operation.
        /// </summary>
        public string PublicLey { get; }

        /// <summary>
        /// Private key that should be used in server-side encryption.
        /// </summary>
        public string PrivateKey { get; }
    }
}
