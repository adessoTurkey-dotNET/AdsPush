using System.Net.Http;

namespace AdsPush.Abstraction.HMS
{
    public class HMSError
    {
        public HMSError(
         HMSErrorReasonCode reasonCode,
         HttpResponseMessage httpResponse)
        {
            this.Reason = reasonCode;
            this.HttpResponse = httpResponse;
        }

        /// <summary>
        /// HMS error reason.
        /// </summary>
        public HMSErrorReasonCode Reason { get; set; }

        /// <summary>
        /// HMS Response.
        /// <see cref="HttpResponseMessage"/>
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
