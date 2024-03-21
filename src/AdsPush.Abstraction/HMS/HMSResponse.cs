using AdsPush.Abstraction.APNS;

namespace AdsPush.Abstraction.HMS
{
    public class HMSResponse
    {
        /// <summary>
        /// The service response success or not.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Represents error if occurrences. <seealso cref="APNSError"/>
        /// </summary>
        public HMSError Error { get; set; }

    }
}
