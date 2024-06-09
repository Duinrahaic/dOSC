using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dOSC.Utilities;

public class EncryptionHelper
{
    public static string Key { get; private set; }

    public static string Encrypt(string dataToEncrypt)
    {
        ArgumentNullException.ThrowIfNull(Key, "Encryption key");

        if (string.IsNullOrEmpty(dataToEncrypt) || string.IsNullOrWhiteSpace(dataToEncrypt)) return null;

        byte[] encrypted;

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(dataToEncrypt);
            }

            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(string dataToDecrypt)
    {
        ArgumentNullException.ThrowIfNull(Key, "Encryption key");

        if (string.IsNullOrEmpty(dataToDecrypt) || string.IsNullOrWhiteSpace(dataToDecrypt)) return null;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        var descriptor = aes.CreateDecryptor(aes.Key, aes.IV);

        var buffer = Convert.FromBase64String(dataToDecrypt);
        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, descriptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }

    public static void SetEncryptionKey(string key)
    {
        Key = key;
    }
}