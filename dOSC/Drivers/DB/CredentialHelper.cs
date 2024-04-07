using System.Security.Cryptography;
using System.Text;

namespace dOSCEngine.Services;


public static class CredentialHelper
{
    public static string HashData(string data, byte[] salt)
    {
        using (var hmac = new HMACSHA512(salt))
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var hashedData = hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(hashedData);
        }
    }

    public static byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return saltBytes;
        }
    }

    public static bool VerifyData(string inputData, string storedData)
    {
        var parts = storedData.Split(':');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hashedData = HashData(inputData, salt);

        return hashedData == parts[1];
    }
}