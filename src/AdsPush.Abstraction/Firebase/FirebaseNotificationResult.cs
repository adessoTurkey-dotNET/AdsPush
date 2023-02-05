
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static FirebaseNotificationResult CreateSuccessResult(
            string messageId) => new FirebaseNotificationResult(true, messageId, null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firebaseMessagingException"></param>
        /// <returns></returns>
        public static FirebaseNotificationResult CreateFailedResult(
            FirebaseMessagingException firebaseMessagingException)
            => new FirebaseNotificationResult(false, string.Empty, firebaseMessagingException);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendResponse"></param>
        /// <returns></returns>
        public static FirebaseNotificationResult CreateUsingFirebaseSendResponse(
            SendResponse sendResponse)
            => new FirebaseNotificationResult(sendResponse.IsSuccess, sendResponse.MessageId, sendResponse.Exception);

        /// <summary>
        /// The request is successful. 
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// Unique message is.
        /// </summary>
        public string MessageId { get; }
        
        /// <summary>
        /// Firebase exception.
        /// <see cref="FirebaseMessagingException"/>
        /// </summary>
        public FirebaseMessagingException MessagingException { get; }
    }
}
