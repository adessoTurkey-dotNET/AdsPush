using FirebaseAdmin.Messaging;

namespace AdsPush.Abstraction.HMS.WebPush
{
    public class Header
    {
        /// <summary>
        /// Message cache time, in seconds
        /// </summary>
        public string TTL { get; set; }
        /// <summary>
        /// Message ID, which can be used to overwrite undelivered messages.
        /// </summary>
        public string Topic { get; set; }
    }
}
