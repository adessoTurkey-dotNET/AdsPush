using AdsPush.Abstraction.APNS;

namespace AdsPush.Abstraction.Vapid
{
    /// <summary>
    /// Generic service response
    /// </summary>
    public class VapidResponse
    {
        public VapidResponse()
        {
        }

        public VapidResponse(
            bool isSuccess,
            VapidError error)
        {
            this.IsSuccess = isSuccess;
            this.Error = error;
        }

        /// <summary>
        /// The service response success or not.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Represents error if occurrences. <seealso cref="VapidError"/>
        /// </summary>
        public VapidError Error { get; set; }
    }
}
