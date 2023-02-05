using System;
using System.Net.Http;

namespace AdsPush.Abstraction
{
    /// <summary>
    /// The basic AdsPush exception.
    /// <see cref="Exception"/>
    /// </summary>
    public class AdsPushException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorType"></param>
        /// <param name="httpResponse"></param>
        public AdsPushException(
            string message,
            AdsPushErrorType errorType,
            HttpResponseMessage httpResponse)
            : base(message)
        {
            this.ErrorType = errorType;
            this.HttpResponse = httpResponse;
        }

        /// <summary>
        /// Error group.
        /// <see cref="AdsPushErrorType"/>
        /// </summary>
        public AdsPushErrorType ErrorType {  get; }
        
        /// <summary>
        /// Related service http response.
        /// <see cref="HttpResponseMessage"/> 
        /// </summary>
        public HttpResponseMessage HttpResponse { get; }
    }
}
