
using FirebaseAdmin.Messaging;

namespace AdsPush.Abstraction.Firebase
{
    /// <summary>
    /// FCM Notification send response.
    /// </summary>
    public sealed class FirebaseNotificationResult
    {
        private FirebaseNotificationResult(
            bool isSuccess,
            string messageId,
            FirebaseMessagingException messagingException)
        {
            this.IsSuccess = isSuccess;
            this.MessageId = messageId;
            this.MessagingException = messagingException;
        }

        public static FirebaseNotificationResult CreateSuccessResult(
            string messageId) => new FirebaseNotificationResult(true, messageId, null);

        public static FirebaseNotificationResult CreateFailedResult(
            FirebaseMessagingException firebaseMessagingException)
            => new FirebaseNotificationResult(false, string.Empty, firebaseMessagingException);

        public static FirebaseNotificationResult CreateUsingFirebaseSendResponse(
            SendResponse sendResponse)
            => new FirebaseNotificationResult(sendResponse.IsSuccess, sendResponse.MessageId, sendResponse.Exception);

        public bool IsSuccess { get; }
        public string MessageId { get; }
        public FirebaseMessagingException MessagingException { get; }
    }
}
