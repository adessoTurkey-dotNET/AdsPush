using System;
using System.Collections.Generic;
using System.Linq;

namespace AdsPush.Abstraction.Firebase
{
    /// <summary>
    /// Response for batch sending.
    /// </summary>
    public sealed class FirebaseNotificationBatchResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationResults"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FirebaseNotificationBatchResult(
            IList<FirebaseNotificationResult> notificationResults)
        {
            this.NotificationResults = notificationResults ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// The notification count that is
        /// successfully accepted by FCM.
        /// </summary>
        public int SuccessCount => this.NotificationResults.Count(x => x.IsSuccess);

        /// <summary>
        /// The notification count that is NOT accepted by FCM.
        /// </summary>
        public int FailedCount => this.NotificationResults.Count(x => !x.IsSuccess);

        /// <summary>
        /// The sent notifications response.
        /// <seealso cref="FirebaseNotificationResult"/>
        /// </summary>
        public IList<FirebaseNotificationResult> NotificationResults { get; private set; }
    }
}
