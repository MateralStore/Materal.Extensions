namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 转换为Base64字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static string ToBase64Encode(this string inputStr)
        {
            byte[] input = Encoding.UTF8.GetBytes(inputStr);
            return Convert.ToBase64String(input);
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string Base64Decode(this string inputStr)
        {
            try
            {
                byte[] input = Convert.FromBase64String(inputStr);
                return Encoding.UTF8.GetString(input);
            }
            catch (Exception ex)
            {
                throw new ExtensionException("解密错误", ex);
            }
        }
    }
}
