namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 栅栏加密
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>加密后字符串</returns>
        public static string ToFenceEncode(this string inputStr)
        {
            string outPutStr = string.Empty;
            string outPutStr2 = string.Empty;
            int count = inputStr.Length;
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                {
                    outPutStr += inputStr[i];
                }
                else
                {
                    outPutStr2 += inputStr[i];
                }
            }
            return outPutStr + outPutStr2;
        }
        /// <summary>
        /// 栅栏解密
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>解密后字符串</returns>
        public static string FenceDecode(this string inputStr)
        {
            int count = inputStr.Length;
            string outPutStr = string.Empty;
            string outPutStr1;
            string outPutStr2;
            int num1 = 0;
            int num2 = 0;
            if (count % 2 == 0)
            {
                outPutStr1 = inputStr[..(count / 2)];
                outPutStr2 = inputStr[(count / 2)..];
            }
            else
            {
                outPutStr1 = inputStr[..((count / 2) + 1)];
                outPutStr2 = inputStr[((count / 2) + 1)..];
            }
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                {
                    outPutStr += outPutStr1[num1++];
                }
                else
                {
                    outPutStr += outPutStr2[num2++];
                }
            }
            return outPutStr;
        }
    }
}
