using System.Net.Http;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// Error codes that return from APNS.
    /// </summary>
    public class APNSError
    {
        public APNSErrorReasonCode Reason {get; set;}
        public long? Timestamp {get; set; }
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
