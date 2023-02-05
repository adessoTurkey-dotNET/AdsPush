using System;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// 
    /// </summary>
    public class APNSExpiration
    {
        private APNSExpiration(long apnsExpiration)
        {
            this.ApnsExpirationValue = apnsExpiration;
        }

        /// <summary>
        /// 
        /// </summary>
        public long ApnsExpirationValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static APNSExpiration SingeTry() => new APNSExpiration(0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static APNSExpiration FromTimeSpan(TimeSpan timeSpan) => new APNSExpiration(
            DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (long)timeSpan.TotalSeconds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiryDate"></param>
        /// <returns></returns>
        public static APNSExpiration FromDate(DateTimeOffset expiryDate) => new APNSExpiration(
            expiryDate.ToUnixTimeSeconds());

    }
}
