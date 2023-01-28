using System;

namespace AdsPush.Abstraction.APNS
{
    public class APNSExpiration
    {
        private APNSExpiration(long apnsExpiration)
        {
            this.ApnsExpirationValue = apnsExpiration;
        }

        public long ApnsExpirationValue { get; private set; }

        public static APNSExpiration SingeTry() => new APNSExpiration(0);

        public static APNSExpiration FromTimeSpan(TimeSpan timeSpan) => new APNSExpiration(
            DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (long)timeSpan.TotalSeconds);

        public static APNSExpiration FromDate(DateTimeOffset expiryDate) => new APNSExpiration(
            expiryDate.ToUnixTimeSeconds());

    }
}
