using System.Net.Http;

namespace AdsPush.Abstraction.Vapid
{
    /// <summary>
    /// Vapid error.
    /// </summary>
    public class VapidError
    {
        public VapidError()
        {
        }

        public VapidError(
            VapidErrorReasonCode reasonCode,
            HttpResponseMessage httpResponse)
        {
            this.ReasonCode = reasonCode;
            this.HttpResponse = httpResponse;
        }

        /// <summary>
        /// Vapid error reason.
        /// </summary>
        public VapidErrorReasonCode ReasonCode { get; set; }

        /// <summary>
        /// APNS Response.
        /// <see cref="HttpResponseMessage"/>
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
