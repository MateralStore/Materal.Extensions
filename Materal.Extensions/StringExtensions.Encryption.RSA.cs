using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 获取RSA
        /// </summary>
        /// <returns></returns>
        public static (string publicKey, string privateKey) GetRSAKey()
        {
            using RSACryptoServiceProvider RSA = new();
            string publicKey = RSA.ToXmlString(false);
            string privateKey = RSA.ToXmlString(true);
            return (publicKey, privateKey);
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKey">加密key</param>
        /// <returns></returns>
        public static string ToRSAEncode(this string content, string publicKey)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(publicKey);
            UnicodeEncoding ByteConverter = new();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateKey">解密key</param>
        /// <returns></returns>
        public static string RSADecode(this string content, string privateKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            using RSACryptoServiceProvider RSA = new();
            RSA.FromXmlString(privateKey);
            byte[] resultBytes = RSA.Decrypt(dataToDecrypt, false);
            UnicodeEncoding ByteConverter = new();
            return ByteConverter.GetString(resultBytes);
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKey">公开密钥</param>
        /// <param name="privateKey">私有密钥</param>
        /// <returns>加密后结果</returns>
        public static string ToRSAEncode(this string content, out string publicKey, out string privateKey)
        {
            (publicKey, privateKey) = GetRSAKey();
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(publicKey);
            UnicodeEncoding ByteConverter = new();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }
        /// <summary>
        /// RSA数字签名
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名结果（Base64字符串）</returns>
        public static string ToRSASign(this string content, string privateKey)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(privateKey);
            UnicodeEncoding ByteConverter = new();
            byte[] dataToSign = ByteConverter.GetBytes(content);
#if NETSTANDARD
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(dataToSign);
#else
            byte[] hash = SHA256.HashData(dataToSign);
#endif
            byte[] signedBytes = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signedBytes);
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="content">原始内容</param>
        /// <param name="signature">签名（Base64字符串）</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyRSASign(this string content, string signature, string publicKey)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(publicKey);
            UnicodeEncoding ByteConverter = new();
            byte[] dataToVerify = ByteConverter.GetBytes(content);
#if NETSTANDARD
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(dataToVerify);
#else
            byte[] hash = SHA256.HashData(dataToVerify);
#endif
            byte[] signedBytes = Convert.FromBase64String(signature);
            return rsa.VerifyHash(hash, signedBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        /// <summary>
        /// RSA数字签名
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <returns>签名结果（Base64字符串）</returns>
        public static string ToRSASign(this string content, out string publicKey, out string privateKey)
        {
            (publicKey, privateKey) = GetRSAKey();
            return content.ToRSASign(privateKey);
        }
#if NET8_0_OR_GREATER
        #region PEM格式RSA加解密
        /// <summary>
        /// 获取PEM格式的RSA密钥对
        /// </summary>
        /// <returns>(公钥, 私钥)</returns>
        public static (string publicKey, string privateKey) GetRSAKeyPEM()
        {
            using RSA rsa = RSA.Create();
            string privateKey = ExportPrivateKeyToPEM(rsa);
            string publicKey = ExportPublicKeyToPEM(rsa);
            return (publicKey, privateKey);
        }

        /// <summary>
        /// 导出RSA私钥为PEM格式
        /// </summary>
        /// <param name="rsa">RSA对象</param>
        /// <returns>PEM格式私钥</returns>
        public static string ExportPrivateKeyToPEM(RSA rsa)
        {
            byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            return FormatPEM(privateKeyBytes, "PRIVATE KEY");
        }

        /// <summary>
        /// 导出RSA公钥为PEM格式
        /// </summary>
        /// <param name="rsa">RSA对象</param>
        /// <returns>PEM格式公钥</returns>
        public static string ExportPublicKeyToPEM(RSA rsa)
        {
            byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            return FormatPEM(publicKeyBytes, "PUBLIC KEY");
        }
        /// <summary>
        /// 从XML格式密钥获取二进制数据
        /// </summary>
        /// <param name="key">XML格式的密钥</param>
        /// <returns>密钥的二进制数据</returns>
        public static string GetXmlKeyFromPEM(string key)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(key);
            if (key.Contains("<P>")) // 判断是否为私钥
            {
                byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();
                return FormatPEM(privateKeyBytes, "PRIVATE KEY");
            }
            else
            {
                byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
                return FormatPEM(publicKeyBytes, "PUBLIC KEY");
            }
        }
        /// <summary>
        /// 格式化PEM字符串
        /// </summary>
        /// <param name="data">密钥数据</param>
        /// <param name="keyType">密钥类型</param>
        /// <returns>PEM格式字符串</returns>
        public static string FormatPEM(byte[] data, string keyType)
        {
            string base64 = Convert.ToBase64String(data);
            StringBuilder sb = new();
            sb.AppendLine($"-----BEGIN {keyType}-----");

            // 按照64个字符一行进行分割
            for (int i = 0; i < base64.Length; i += 64)
            {
                int length = Math.Min(64, base64.Length - i);
                sb.AppendLine(base64.Substring(i, length));
            }

            sb.Append($"-----END {keyType}-----");
            return sb.ToString();
        }

        /// <summary>
        /// 从PEM字符串中提取密钥数据
        /// </summary>
        /// <param name="pemString">PEM格式字符串</param>
        /// <returns>密钥数据</returns>
        private static byte[] GetBytesFromPEM(string pemString)
        {
            string base64 = Regex.Replace(pemString, "-----BEGIN .+?-----|\r?\n|-----END .+?-----", "");
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// 使用PEM格式公钥进行RSA加密
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="publicKeyPEM">PEM格式公钥</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string ToRSAEncodePEM(this string content, string publicKeyPEM)
        {
            using RSA rsa = RSA.Create();
            byte[] publicKeyBytes = GetBytesFromPEM(publicKeyPEM);
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            UnicodeEncoding byteConverter = new();
            byte[] dataToEncrypt = byteConverter.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// 使用PEM格式私钥进行RSA解密
        /// </summary>
        /// <param name="content">要解密的Base64字符串</param>
        /// <param name="privateKeyPEM">PEM格式私钥</param>
        /// <returns>解密后的内容</returns>
        public static string RSADecodePEM(this string content, string privateKeyPEM)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            using RSA rsa = RSA.Create();
            byte[] privateKeyBytes = GetBytesFromPEM(privateKeyPEM);
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            byte[] resultBytes = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.Pkcs1);
            UnicodeEncoding byteConverter = new();
            return byteConverter.GetString(resultBytes);
        }

        /// <summary>
        /// RSA加密并输出PEM格式密钥对
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="publicKey">输出的PEM格式公钥</param>
        /// <param name="privateKey">输出的PEM格式私钥</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string ToRSAEncodePEM(this string content, out string publicKey, out string privateKey)
        {
            (publicKey, privateKey) = GetRSAKeyPEM();
            return content.ToRSAEncodePEM(publicKey);
        }

        /// <summary>
        /// 使用PEM格式私钥进行RSA数字签名
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="privateKeyPEM">PEM格式私钥</param>
        /// <returns>签名结果（Base64字符串）</returns>
        public static string ToRSASignPEM(this string content, string privateKeyPEM)
        {
            using RSA rsa = RSA.Create();
            byte[] privateKeyBytes = GetBytesFromPEM(privateKeyPEM);
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            UnicodeEncoding byteConverter = new();
            byte[] dataToSign = byteConverter.GetBytes(content);
#if NETSTANDARD
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(dataToSign);
#else
            byte[] hash = SHA256.HashData(dataToSign);
#endif
            byte[] signedBytes = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signedBytes);
        }

        /// <summary>
        /// 使用PEM格式公钥验证RSA签名
        /// </summary>
        /// <param name="content">原始内容</param>
        /// <param name="signature">签名（Base64字符串）</param>
        /// <param name="publicKeyPEM">PEM格式公钥</param>
        /// <returns>验证是否通过</returns>
        public static bool VerifyRSASignPEM(this string content, string signature, string publicKeyPEM)
        {
            using RSA rsa = RSA.Create();
            byte[] publicKeyBytes = GetBytesFromPEM(publicKeyPEM);
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            UnicodeEncoding byteConverter = new();
            byte[] dataToVerify = byteConverter.GetBytes(content);
#if NETSTANDARD
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(dataToVerify);
#else
            byte[] hash = SHA256.HashData(dataToVerify);
#endif
            byte[] signedBytes = Convert.FromBase64String(signature);
            return rsa.VerifyHash(hash, signedBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// RSA数字签名并输出PEM格式密钥对
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="publicKey">输出的PEM格式公钥</param>
        /// <param name="privateKey">输出的PEM格式私钥</param>
        /// <returns>签名结果（Base64字符串）</returns>
        public static string ToRSASignPEM(this string content, out string publicKey, out string privateKey)
        {
            (publicKey, privateKey) = GetRSAKeyPEM();
            return content.ToRSASignPEM(privateKey);
        }
        #endregion
#endif
    }
}
