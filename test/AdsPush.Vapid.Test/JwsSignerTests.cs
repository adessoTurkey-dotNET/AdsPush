using System.Reflection;
using System.Text;

namespace AdsPush.Vapid.Test;

public class JwsSignerTests
{
    [Fact]
    public void GenerateSignatureProducesCompactToken()
    {
        var assembly = typeof(VapidHelper).Assembly;
        var keys = VapidHelper.GenerateVapidKeys();

        var urlBase64Type = assembly.GetType("AdsPush.Vapid.Util.UrlBase64")!;
        var decodeMethod = urlBase64Type.GetMethod("Decode", BindingFlags.Public | BindingFlags.Static)!;
        var privateKeyBytes = (byte[])decodeMethod.Invoke(null, new object[] { keys.PrivateKey })!;

        var ecKeyHelperType = assembly.GetType("AdsPush.Vapid.Util.ECKeyHelper")!;
        var getPrivateKeyMethod =
            ecKeyHelperType.GetMethod("GetPrivateKey", BindingFlags.Public | BindingFlags.Static)!;
        var privateKey = getPrivateKeyMethod.Invoke(null, new object[] { privateKeyBytes })!;

        var signerType = assembly.GetType("AdsPush.Vapid.Util.JwsSigner")!;
        var signer = Activator.CreateInstance(signerType, privateKey)!;

        var header = new Dictionary<string, object> { { "typ", "JWT" }, { "alg", "ES256" } };

        var payload = new Dictionary<string, object>
        {
            { "aud", "https://example.com" }, { "sub", "mailto:test@example.com" }
        };

        var generateSignature = signerType.GetMethod("GenerateSignature", BindingFlags.Public | BindingFlags.Instance)!;
        var signature = (string)generateSignature.Invoke(signer, new object[] { header, payload })!;

        Assert.Equal(2, signature.Count(c => c == '.'));
        Assert.False(string.IsNullOrEmpty(signature));
    }

    [Fact]
    public void PrivateHelpersReturnExpectedValues()
    {
        var assembly = typeof(VapidHelper).Assembly;
        var signerType = assembly.GetType("AdsPush.Vapid.Util.JwsSigner")!;

        var secureInput = signerType.GetMethod("SecureInput", BindingFlags.NonPublic | BindingFlags.Static)!;
        var secureValue = (string)secureInput.Invoke(null,
            new object[]
            {
                new Dictionary<string, object> { { "alg", "ES256" } },
                new Dictionary<string, object> { { "aud", "https://example.com" } }
            })!;
        Assert.Contains('.', secureValue);

        var byteArrayPadLeft = signerType.GetMethod("ByteArrayPadLeft", BindingFlags.NonPublic | BindingFlags.Static)!;
        var padded = (byte[])byteArrayPadLeft.Invoke(null, new object[] { new byte[] { 0x1 }, 4 })!;
        Assert.Equal(4, padded.Length);
        Assert.Equal(0x1, padded[^1]);

        var sha256Hash = signerType.GetMethod("Sha256Hash", BindingFlags.NonPublic | BindingFlags.Static)!;
        var hash = (byte[])sha256Hash.Invoke(null, new object[] { Encoding.UTF8.GetBytes("payload") })!;
        Assert.Equal(32, hash.Length);
    }
}
