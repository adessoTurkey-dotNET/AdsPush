using System.Text;
using AdsPush.Abstraction.Settings;
using Newtonsoft.Json.Linq;

namespace AdsPush.Vapid.Test;

public class VapidHelperTests
{
    [Fact]
    public void GetVapidHeadersWithExplicitExpirationEncodesPayload()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var futureExpiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 120;

        var headers = VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey,
            futureExpiration);

        Assert.NotNull(headers);
        Assert.Equal("p256ecdsa=" + keys.PublicLey, headers["Crypto-Key"]);

        var jwt = headers["Authorization"].Substring("WebPush ".Length);
        var payloadJson = DecodeBase64Url(jwt.Split('.')[1]);
        var payload = JObject.Parse(payloadJson);

        Assert.Equal(futureExpiration, payload.Value<long>("exp"));
        Assert.Equal("https://example.com", payload.Value<string>("aud"));
        Assert.Equal("mailto:person@example.com", payload.Value<string>("sub"));
    }

    [Fact]
    public void GetVapidHeadersAcceptsSubjectAsUrl()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var headers = VapidHelper.GetVapidHeaders(
            "https://example.com",
            "https://example.com/contact",
            keys.PublicLey,
            keys.PrivateKey);

        Assert.NotNull(headers);
        Assert.StartsWith("WebPush ", headers["Authorization"]);
    }

    [Fact]
    public void GetVapidHeadersThrowsForAudienceMissing()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            null!,
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForAudienceEmptyString()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            string.Empty,
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForAudienceNotUrl()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "not-a-url",
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForSubjectMissing()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            null!,
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForSubjectEmptyString()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            string.Empty,
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForSubjectInvalidUrl()
    {
        var keys = VapidHelper.GenerateVapidKeys();

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "invalid-subject",
            keys.PublicLey,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForPublicKeyLength()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            keys.PublicLey.Substring(0, 10),
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForPublicKeyMissing()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            null!,
            keys.PrivateKey));
    }

    [Fact]
    public void GetVapidHeadersThrowsForPrivateKeyLength()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey.Substring(0, 10)));
    }

    [Fact]
    public void GetVapidHeadersThrowsForPrivateKeyMissing()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            keys.PublicLey,
            null!));
    }

    [Fact]
    public void GetVapidHeadersThrowsForPastExpiration()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var pastExpiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 5;

        Assert.Throws<ArgumentException>(() => VapidHelper.GetVapidHeaders(
            "https://example.com",
            "mailto:person@example.com",
            keys.PublicLey,
            keys.PrivateKey,
            pastExpiration));
    }

    [Fact]
    public void GenerateVapidKeysProducesRoundTripKeys()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var section = new Dictionary<string, AdsPushVapidSettings>
        {
            ["app"] = new AdsPushVapidSettings
            {
                PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:person@example.com"
            }
        };

        Assert.True(section.ContainsKey("app"));
        Assert.False(string.IsNullOrEmpty(section["app"].PublicKey));
        Assert.False(string.IsNullOrEmpty(section["app"].PrivateKey));
    }

    private static string DecodeBase64Url(
        string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2:
                padded += "==";
                break;
            case 3:
                padded += "=";
                break;
        }

        var bytes = Convert.FromBase64String(padded);
        return Encoding.UTF8.GetString(bytes);
    }
}
