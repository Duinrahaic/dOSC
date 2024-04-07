using System.Security.Cryptography;

namespace dOSCEngine.Utilities;

public static class SecureKeyGenerator
{
    public static string GenerateSecureKey()
    {
        using (Aes aesAlgorithm = Aes.Create())
        {
            aesAlgorithm.KeySize = 256;
            aesAlgorithm.GenerateKey();
            return Convert.ToBase64String(aesAlgorithm.Key); ;
        }
    }
}