using System.Net.Http;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// Error codes that return from APNS.
    /// </summary>
    public class APNSError
    {
        /// <summary>
        /// APNS Error reason.
        /// <see cref="APNSErrorReasonCode"/>
        /// </summary>
        public APNSErrorReasonCode Reason {get; set;}
        
        /// <summary>
        /// 
        /// </summary>
        public long? Timestamp {get; set; }
        
        /// <summary>
        /// APNS Response.
        /// <see cref="HttpResponseMessage"/>
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
