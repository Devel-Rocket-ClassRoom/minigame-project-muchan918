using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoUtil
{
    // AES-256 키 (32바이트 = 16진수 64자). 수업용 하드코딩 — 실제 프로덕션에서는 안전한 키 관리 필요.
    private const string KeyHex =
        "536176654C6F616453747564795F4145533235365F4B65795F32303235212100";

    private static readonly byte[] Key = HexToBytes(KeyHex);

    private static byte[] HexToBytes(string hex)
    {
        int len = hex.Length / 2;
        byte[] bytes = new byte[len];
        for (int i = 0; i < len; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    // IV는 매번 바뀐다
    // 암호화: plainText → [IV 16바이트][AES-CBC 암호문]
    public static byte[] Encrypt(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }
    }

    // 복호화: [IV 16바이트][AES-CBC 암호문] → 원본 문자열
    public static string Decrypt(byte[] encryptedData)
    {
        const int ivSize = 16;

        byte[] iv = new byte[ivSize];
        Buffer.BlockCopy(encryptedData, 0, iv, 0, ivSize);

        byte[] cipherBytes = new byte[encryptedData.Length - ivSize];
        Buffer.BlockCopy(encryptedData, ivSize, cipherBytes, 0, cipherBytes.Length);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (MemoryStream ms = new MemoryStream(cipherBytes))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cs, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
