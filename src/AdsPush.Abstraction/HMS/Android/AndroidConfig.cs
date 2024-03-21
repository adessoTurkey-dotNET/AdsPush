using FirebaseAdmin.Messaging;
using System;

namespace AdsPush.Abstraction.HMS.Android
{
    public class AndroidConfig
    {
        /// <summary>
        /// Tag of a message in a batch delivery task.
        /// </summary>
        public int Bi_Tag { get; set; }

        /// <summary>
        /// Mode for the Push Kit server to cache messages sent to an offline device.
        /// These cached messages will be delivered once the device goes online again.
        ///  can be 0, -1, 1-100
        /// </summary>
        public int CollapseKey { get; set; }

        //
        // Summary:
        //     Gets or sets the priority of the message.
        public Priority? Priority { get; set; }

        /// <summary>
        /// Message cache duration, in seconds.
        /// </summary>
        public TimeSpan? TimeToLive { get; set; }

        /// <summary>
        ///   Custom message payload
        ///   Gets or sets a collection of key-value pairs that will be added to the message
        ///   as data fields. Keys and the values must not be null. When set, overrides any
        ///   data fields set on the message.data field
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///   Android notification message structure
        /// </summary>
        public AndroidNotification Notification { get; set; }
    }
}
