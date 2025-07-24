using System.Security.Cryptography;

namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 转换为64位SHA256加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="isLower">小写</param>
        /// <returns></returns>
        public static string ToSHA256_64Encode(this string inputStr, bool isLower = false)
        {
#if NETSTANDARD
            if (inputStr is null) throw new ArgumentNullException(nameof(inputStr));
            using SHA256 sha256 = SHA256.Create();
            byte[] output = sha256.ComputeHash(Encoding.Default.GetBytes(inputStr));
#else
            ArgumentNullException.ThrowIfNull(inputStr);
            byte[] output = SHA256.HashData(Encoding.Default.GetBytes(inputStr));
#endif
            string outputStr = BitConverter.ToString(output).Replace("-", "");
            outputStr = isLower ? outputStr.ToLower() : outputStr.ToUpper();
            return outputStr;
        }
        /// <summary>
        /// 转换为40位SHA256加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="isLower">小写</param>
        /// <returns></returns>
        public static string ToSHA1_40Encode(this string inputStr, bool isLower = false)
        {
#if NETSTANDARD
            if (inputStr is null) throw new ArgumentNullException(nameof(inputStr));
            using SHA1 sha1 = SHA1.Create();
            byte[] output = sha1.ComputeHash(Encoding.Default.GetBytes(inputStr));
#else
            ArgumentNullException.ThrowIfNull(inputStr);
            byte[] output = SHA1.HashData(Encoding.Default.GetBytes(inputStr));
#endif
            string outputStr = BitConverter.ToString(output).Replace("-", "");
            outputStr = isLower ? outputStr.ToLower() : outputStr.ToUpper();
            return outputStr;
        }
    }
}
