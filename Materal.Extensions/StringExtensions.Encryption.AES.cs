using System.Security.Cryptography;

namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 获取AES密钥和IV
        /// </summary>
        /// <returns>(密钥, IV)</returns>
        public static (string key, string iv) GetAESKey()
        {
            using Aes aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            string key = Convert.ToBase64String(aes.Key);
            string iv = Convert.ToBase64String(aes.IV);
            return (key, iv);
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string ToAESEncode(this string content, string key, string iv)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(content);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(dataToEncrypt, 0, dataToEncrypt.Length);
            cs.FlushFinalBlock();
            byte[] resultBytes = ms.ToArray();
            return Convert.ToBase64String(resultBytes);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="content">要解密的Base64字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns>解密后的内容</returns>
        public static string AESDecode(this string content, string key, string iv)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using MemoryStream ms = new(dataToDecrypt);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new(cs);
            return reader.ReadToEnd();
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="key">输出的密钥</param>
        /// <param name="iv">输出的初始化向量</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string ToAESEncode(this string content, out string key, out string iv)
        {
            (key, iv) = GetAESKey();
            return content.ToAESEncode(key, iv);
        }
        /// <summary>
        /// AES加密（使用默认IV）
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string ToAESEncode(this string content, string key)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV();
            string iv = Convert.ToBase64String(aes.IV);
            string encryptedContent = content.ToAESEncode(key, iv);
            byte[] ivBytes = aes.IV;
            byte[] encryptedBytes = Convert.FromBase64String(encryptedContent);
            byte[] resultBytes = new byte[ivBytes.Length + encryptedBytes.Length];
            Buffer.BlockCopy(ivBytes, 0, resultBytes, 0, ivBytes.Length);
            Buffer.BlockCopy(encryptedBytes, 0, resultBytes, ivBytes.Length, encryptedBytes.Length);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// AES解密（使用默认IV）
        /// </summary>
        /// <param name="content">要解密的Base64字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的内容</returns>
        public static string AESDecode(this string content, string key)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            using Aes aes = Aes.Create();
            int ivLength = aes.BlockSize / 8;
            byte[] ivBytes = new byte[ivLength];
            byte[] encryptedBytes = new byte[dataToDecrypt.Length - ivLength];
            Buffer.BlockCopy(dataToDecrypt, 0, ivBytes, 0, ivLength);
            Buffer.BlockCopy(dataToDecrypt, ivLength, encryptedBytes, 0, encryptedBytes.Length);
            string iv = Convert.ToBase64String(ivBytes);
            string encryptedContent = Convert.ToBase64String(encryptedBytes);
            return encryptedContent.AESDecode(key, iv);
        }

        /// <summary>
        /// AES加密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        public static void EncryptFileAES(string sourceFilePath, string destinationFilePath, string key, string iv)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using FileStream sourceFile = new(sourceFilePath, FileMode.Open, FileAccess.Read);
            using FileStream destinationFile = new(destinationFilePath, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new(destinationFile, encryptor, CryptoStreamMode.Write);
            sourceFile.CopyTo(cryptoStream);
        }

        /// <summary>
        /// AES解密文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destinationFilePath">目标文件路径</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        public static void DecryptFileAES(string sourceFilePath, string destinationFilePath, string key, string iv)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using FileStream sourceFile = new(sourceFilePath, FileMode.Open, FileAccess.Read);
            using FileStream destinationFile = new(destinationFilePath, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new(sourceFile, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(destinationFile);
        }
    }
}
