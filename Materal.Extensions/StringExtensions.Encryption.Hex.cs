namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] ToHexByte(this string hexString)
        {
            if (!hexString.IsHexNumber()) throw new ExtensionException("16进制字符串有误");
            try
            {
                hexString = hexString.Replace(" ", "");
                if ((hexString.Length % 2) != 0)
                {
                    hexString += " ";
                }
                byte[] returnBytes = new byte[hexString.Length / 2];
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
                return returnBytes;
            }
            catch (Exception ex)
            {
                throw new ExtensionException("16进制字符串有误", ex);
            }
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexStr(this byte[] bytes) => bytes is null ? string.Empty : bytes.Aggregate(string.Empty, (current, item) => current + item.ToString("X2"));
    }
}
