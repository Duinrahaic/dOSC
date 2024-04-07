using System;
using System.Security.Cryptography;

namespace dOSC.Shared.Utilities;

public static class SecureKeyGenerator
{
    public static string GenerateSecureKey()
    {
        using (var aesAlgorithm = Aes.Create())
        {
            aesAlgorithm.KeySize = 256;
            aesAlgorithm.GenerateKey();
            return Convert.ToBase64String(aesAlgorithm.Key);
            ;
        }
    }
}