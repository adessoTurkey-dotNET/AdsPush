using System.Security.Cryptography;
using AdsPush.APNS.Helpers;

namespace AdsPush.APNS.Test;

public class AppleCryptoHelperTests
{
    [Fact]
    public void GetEllipticCurveAlgorithmReturnsSignableKey()
    {
        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var pkcs8 = ecdsa.ExportPkcs8PrivateKey();
        var privateKey = Convert.ToBase64String(pkcs8);

        using var signer = AppleCryptoHelper.GetEllipticCurveAlgorithm(privateKey);
        var data = new byte[] { 1, 2, 3, 4 };
        var signature = signer.SignData(data, HashAlgorithmName.SHA256);

        Assert.NotNull(signature);
        Assert.True(signature.Length > 0);
        Assert.True(signer.VerifyData(data, signature, HashAlgorithmName.SHA256));
    }

    [Fact]
    public void GetEllipticCurveAlgorithmWithInvalidKeyThrows()
    {
        Assert.ThrowsAny<Exception>(() => AppleCryptoHelper.GetEllipticCurveAlgorithm("not-base64"));
    }
}
