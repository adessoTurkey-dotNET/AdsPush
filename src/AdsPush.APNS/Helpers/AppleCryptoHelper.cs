﻿using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace AdsPush.APNS.Helpers
{
    public static class AppleCryptoHelper
    {
        public static ECDsa GetEllipticCurveAlgorithm(string privateKey)
        {
            var keyParams = (ECPrivateKeyParameters) PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            var q = keyParams.Parameters.G.Multiply(keyParams.D).Normalize();

            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.CreateFromValue(keyParams.PublicKeyParamSet.Id),
                D = keyParams.D.ToByteArrayUnsigned(),
                Q =
                {
                    X = q.XCoord.GetEncoded(),
                    Y = q.YCoord.GetEncoded()
                }
            });
        }
    }
}
