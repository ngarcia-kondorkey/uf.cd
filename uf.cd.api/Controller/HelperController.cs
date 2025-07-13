using System;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;

public class PasswordEncryptor
{
    private readonly string key;
    private readonly string ivkey;

    public PasswordEncryptor(string? secretKey, string? ivKey)
    {
        key = secretKey ?? GenerateRandomKey();
        ivkey = ivKey ?? key;
    }

    // Función principal que encripta el password
    public string GetHashPassword(string password)
    {
        string hashedKey = GetSha256Hash(key);
        string hashedIv = GetSha256Hash(ivkey).Substring(0, 16);

        string hashedPassword = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(GetMd5Hash(GetSha1Hash(password)))
        );

        return EncryptStringAES(hashedPassword, hashedKey, hashedIv);
    }

    // Función auxiliar para generar un hash SHA-1
    private string GetSha1Hash(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }
    }

    // Función auxiliar para generar un hash MD5
    private string GetMd5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }
    }

    // Función auxiliar para generar un hash SHA-256
    private string GetSha256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }
    }

    // Encriptación AES-256-CBC
    private string EncryptStringAES(string plainText, string key, string iv)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (var msEncrypt = new System.IO.MemoryStream())
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                byte[] toEncrypt = Encoding.UTF8.GetBytes(plainText);
                csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                csEncrypt.FlushFinalBlock();
                encrypted = msEncrypt.ToArray();
            }
        }

        return Convert.ToBase64String(encrypted);
    }

    // Generador de clave aleatoria
    public static string GenerateRandomKey(int len = 20)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            string input = Guid.NewGuid().ToString() + new Random().Next();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Base36Encode(hash).Substring(0, len);
        }
    }

    // Conversión base36 (similar a base_convert de PHP)
    private static string Base36Encode(byte[] bytes)
    {
        // Agregamos un byte 0 al final para evitar problemas con signo en BigInteger
        BigInteger value = new BigInteger(bytes.Concat(new byte[] { 0 }).ToArray());
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
        string result = "";

        while (value > 0)
        {
            BigInteger remainder;
            value = BigInteger.DivRem(value, 36, out remainder);
            result = chars[(int)remainder] + result;
        }

        return result;
    }
}
