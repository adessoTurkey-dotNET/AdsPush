using System.Net.Http;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// Generic service response.
    /// </summary>
    public class APNSResponse
    {
        /// <summary>
        /// The service response success or not.
        /// </summary>
        public bool IsSuccess { get; set;  }

        /// <summary>
        /// Represents error if occurrences. <seealso cref="APNSError"/>
        /// </summary>
        public APNSError Error { get; set; }
    }
}
