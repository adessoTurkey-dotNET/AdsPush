using Newtonsoft.Json.Linq;

namespace AdsPush.Vapid.Test;

public class VapidSubscriptionTests
{
    [Fact]
    public void FromParametersCreatesSubscription()
    {
        var subscription = VapidSubscription.FromParameters(
            "https://example.com/endpoint",
            "publicKey",
            "authKey");

        Assert.Equal("https://example.com/endpoint", subscription.Endpoint);
        Assert.Equal("publicKey", subscription.P256dh);
        Assert.Equal("authKey", subscription.Auth);
    }

    [Fact]
    public void FromSubscriptionJsonParsesKeys()
    {
        var json = new JObject
        {
            ["endpoint"] = "https://example.com/endpoint",
            ["keys"] = new JObject { ["p256dh"] = "publicKey", ["auth"] = "authKey" }
        }.ToString();

        var subscription = VapidSubscription.FromSubscriptionJson(json);

        Assert.Equal("publicKey", subscription.P256dh);
        Assert.Equal("authKey", subscription.Auth);
    }

    [Fact]
    public void FromBase64EncodedSubscriptionJsonDecodes()
    {
        var json = "{\"endpoint\":\"https://example.com\",\"keys\":{\"p256dh\":\"pk\",\"auth\":\"ak\"}}";
        var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

        var subscription = VapidSubscription.FromBase64EncodedSubscriptionJson(encoded);

        Assert.Equal("https://example.com", subscription.Endpoint);
        Assert.Equal("pk", subscription.P256dh);
        Assert.Equal("ak", subscription.Auth);
    }

    [Fact]
    public void ToAdsPushTokenProducesStructuredJson()
    {
        var subscription = VapidSubscription.FromParameters(
            "https://example.com/endpoint",
            "publicKey",
            "authKey");

        var tokenJson = subscription.ToAdsPushToken();
        var token = JObject.Parse(tokenJson);

        Assert.Equal("https://example.com/endpoint", token.Value<string>("endpoint"));
        Assert.Equal("publicKey", token["keys"]!.Value<string>("p256dh"));
        Assert.Equal("authKey", token["keys"]!.Value<string>("auth"));
    }

    [Fact]
    public void FromSubscriptionJsonHandlesMissingValues()
    {
        var subscription = VapidSubscription.FromSubscriptionJson("{}");

        Assert.Null(subscription.Endpoint);
        Assert.Null(subscription.P256dh);
        Assert.Null(subscription.Auth);
    }
}
