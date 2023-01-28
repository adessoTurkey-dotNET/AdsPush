using System.Collections.Generic;
using AdsPush.Abstraction.Settings;

namespace AdsPush.Firebase.Settings
{
    /// <summary>
    /// Configuration section to be able use <see cref="IFirebasePushNotificationSenderFactory"/>.
    /// <seealso cref="AdsPushFirebaseSettings"/>.
    /// </summary>
    public class FirebaseAppSettingsSection : Dictionary<string, AdsPushFirebaseSettings>
    {
    }
}
