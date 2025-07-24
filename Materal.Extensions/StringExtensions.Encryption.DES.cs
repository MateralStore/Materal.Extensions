using System.Security.Cryptography;

namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        private const string InputIv = "MateralC";
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="inputString">需要解密的字符串</param>
        /// <param name="inputKey">密钥,必须为8位字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>解密后的字符串</returns>
        public static string ToDesEncode(this string inputString, string inputKey, Encoding? encoding = null) => ToDesEncode(inputString, inputKey, InputIv, encoding);
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="inputString">需要解密的字符串</param>
        /// <param name="inputKey">密钥,必须为8位字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecode(this string inputString, string inputKey, Encoding? encoding = null) => DesDecode(inputString, inputKey, InputIv, encoding);
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="inputString">需要加密的字符串</param>
        /// <param name="inputKey">密钥,必须为8位字符串</param>
        /// <param name="inputIv">向量,必须为8位字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDesEncode(this string inputString, string inputKey, string inputIv, Encoding? encoding = null)
        {
            if (inputKey.Length != 8) throw new ExtensionException("密钥必须为8位");
            if (inputIv.Length != 8) throw new ExtensionException("向量必须为8位");
            encoding ??= Encoding.UTF8;
            DES dsp = DES.Create();
            using MemoryStream memoryStream = new();
            byte[] key = encoding.GetBytes(inputKey);
            byte[] iv = encoding.GetBytes(inputIv);
            using CryptoStream cryptoStream = new(memoryStream, dsp.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            StreamWriter writer = new(cryptoStream);
            writer.Write(inputString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            memoryStream.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="inputString">需要解密的字符串</param>
        /// <param name="inputKey">密钥,必须为8位字符串</param>
        /// <param name="inputIv">向量,必须为8位字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecode(this string inputString, string inputKey, string inputIv, Encoding? encoding = null)
        {
            if (inputKey.Length != 8) throw new ExtensionException("密钥必须为8位");
            if (inputIv.Length != 8) throw new ExtensionException("向量必须为8位");
            encoding ??= Encoding.UTF8;
            DES dsp = DES.Create();
            byte[] buffer = Convert.FromBase64String(inputString);
            using MemoryStream memoryStream = new();
            byte[] key = encoding.GetBytes(inputKey);
            byte[] iv = encoding.GetBytes(inputIv);
            using CryptoStream cryptoStream = new(memoryStream, dsp.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();
            return encoding.GetString(memoryStream.ToArray());
        }
    }
}
