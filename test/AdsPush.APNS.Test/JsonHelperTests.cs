using AdsPush.APNS.Helpers;

namespace AdsPush.APNS.Test;

public class JsonHelperTests
{
    private sealed class Sample
    {
        public int ValueOne { get; set; }

        public string? Ignored { get; set; }
    }

    [Fact]
    public void SerializeUsesCamelCaseAndIgnoresNull()
    {
        var json = JsonHelper.Serialize(new Sample { ValueOne = 42, Ignored = null });

        Assert.Equal("{\"valueOne\":42}", json);
    }

    [Fact]
    public void DeserializeRoundTripsObject()
    {
        const string json = "{\"valueOne\":7}";

        var result = JsonHelper.Deserialize<Dictionary<string, int>>(json);

        Assert.Equal(7, result["valueOne"]);
    }
}
