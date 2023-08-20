﻿namespace AdsPush.Vapid.Util
{
    // @LogicSoftware
    // Originally From: https://github.com/LogicSoftware/WebPushEncryption/blob/master/src/EncryptionResult.cs
    internal class EncryptionResult
    {
        public byte[] PublicKey { get; set; }
        public byte[] Payload { get; set; }
        public byte[] Salt { get; set; }

        public string Base64EncodePublicKey()
        {
            return UrlBase64.Encode(this.PublicKey);
        }

        public string Base64EncodeSalt()
        {
            return UrlBase64.Encode(this.Salt);
        }
    }
}
