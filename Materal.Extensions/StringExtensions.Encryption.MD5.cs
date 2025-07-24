using System.Security.Cryptography;

namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 转换为32位Md5加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="isLower">小写</param>
        /// <returns></returns>
        public static string ToMd5_32Encode(this string inputStr, bool isLower = false)
        {
#if NETSTANDARD
            if (inputStr is null) throw new ArgumentNullException(nameof(inputStr));
            using MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(Encoding.Default.GetBytes(inputStr));
#else
            ArgumentNullException.ThrowIfNull(inputStr);
            byte[] output = MD5.HashData(Encoding.Default.GetBytes(inputStr));
#endif
            string outputStr = BitConverter.ToString(output).Replace("-", "");
            outputStr = isLower ? outputStr.ToLower() : outputStr.ToUpper();
            return outputStr;
        }
        /// <summary>
        /// 转换为16位Md5加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="isLower">小写</param>
        /// <returns></returns>
        public static string ToMd5_16Encode(this string inputStr, bool isLower = false) => ToMd5_32Encode(inputStr, isLower).Substring(8, 16);
    }
}
