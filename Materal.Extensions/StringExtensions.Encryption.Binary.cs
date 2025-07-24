namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 文本转换为二进制字符
        /// </summary>
        /// <param name="inputStr">文本</param>
        /// <param name="digit">位数</param>
        /// <returns>二进制字符串</returns>
        public static string ToBinaryStr(this string inputStr, int digit = 8)
        {
            byte[] data = Encoding.UTF8.GetBytes(inputStr);
            StringBuilder resStr = new(data.Length * digit);
            foreach (byte item in data)
            {
                resStr.Append(Convert.ToString(item, 2).PadLeft(digit, '0'));
            }
            return resStr.ToString();
        }
        /// <summary>
        /// 二进制字符转换为文本
        /// </summary>
        /// <param name="inputStr">二进制字符串</param>
        /// <param name="digit">位数</param>
        /// <returns>文本</returns>
        public static string BinaryToStr(this string inputStr, int digit = 8)
        {
            int numOfBytes = inputStr.Length / digit;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                bytes[i] = Convert.ToByte(inputStr.Substring(digit * i, digit), 2);
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
