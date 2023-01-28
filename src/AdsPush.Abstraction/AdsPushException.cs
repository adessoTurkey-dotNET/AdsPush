using System;
using System.Net.Http;

namespace AdsPush.Abstraction
{
    public class AdsPushException : Exception
    {
        public AdsPushException(
            string message,
            AdsPushErrorType errorType,
            HttpResponseMessage httpResponse)
            : base(message)
        {
            this.ErrorType = errorType;
            this.HttpResponse = httpResponse;
        }

        public AdsPushErrorType ErrorType { get; set; }
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
