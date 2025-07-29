using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Services;

public class EncryptPasswordService
{
    private readonly byte[] _key;

    public EncryptPasswordService(string key)
    {
        _key = CreateKey(key);
    }

    private static byte[] CreateKey(string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length == 16 || keyBytes.Length == 24 || keyBytes.Length == 32)
            return keyBytes;

        var resizedKey = new byte[32];
        Array.Copy(keyBytes, resizedKey, Math.Min(keyBytes.Length, resizedKey.Length));
        return resizedKey;
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        var encrypted = ms.ToArray();
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = _key;

        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        var cipher = new byte[fullCipher.Length - iv.Length];
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
