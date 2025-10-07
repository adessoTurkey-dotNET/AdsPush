using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;

namespace AdsPush.APNS.Test;

internal static class ApnsTestHelper
{
    internal static AdsPushAPNSSettings CreateSettings(
        APNSEnvironmentType environmentType = APNSEnvironmentType.Development,
        string? appBundleIdentifier = null)
    {
        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var pkcs8 = ecdsa.ExportPkcs8PrivateKey();

        appBundleIdentifier ??= $"com.example.app{Guid.NewGuid():N}";

        return new AdsPushAPNSSettings
        {
            P8PrivateKey = Convert.ToBase64String(pkcs8),
            P8PrivateKeyId = RandomAlphaNumeric(10),
            TeamId = RandomAlphaNumeric(10),
            AppBundleIdentifier = appBundleIdentifier,
            EnvironmentType = environmentType
        };
    }

    internal static void ClearJwtTokenCache()
    {
        var field = typeof(ApplePushNotificationSender).GetField("Tokens",
            BindingFlags.NonPublic | BindingFlags.Static);
        if (field?.GetValue(null) is ConcurrentDictionary<string, Tuple<string, DateTime>> dictionary)
        {
            dictionary.Clear();
        }
    }

    internal static void SetCachedToken(
        string bundleId,
        string token,
        DateTime timestamp)
    {
        var field = typeof(ApplePushNotificationSender).GetField("Tokens",
            BindingFlags.NonPublic | BindingFlags.Static);
        if (field?.GetValue(null) is ConcurrentDictionary<string, Tuple<string, DateTime>> dictionary)
        {
            dictionary[bundleId] = new Tuple<string, DateTime>(token, timestamp);
        }
    }

    private static string RandomAlphaNumeric(
        int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
}
