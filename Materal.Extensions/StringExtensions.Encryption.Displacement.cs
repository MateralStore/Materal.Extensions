namespace Materal.Extensions
{
    /// <summary>
    /// 字符串加密解密扩展
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 移位加密
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDisplacementEncode(this string inputStr, int key = 3)
        {
            string outputStr = string.Empty;
            if (!inputStr.IsLetter()) throw new ExtensionException("只能包含英文字母");
            inputStr = inputStr.ToUpper();
            char[] alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
            int aCount = alphabet.Length;
            int count = inputStr.Length;
            for (int i = 0; i < count; i++)
            {
                if (inputStr[i] != ' ')
                {
                    for (int j = 0; j < aCount; j++)
                    {
                        if (inputStr[i] != alphabet[j]) continue;
                        int eIndex = j + key;
                        if (eIndex < 0)
                        {
                            eIndex = aCount + eIndex;
                        }
                        while (eIndex >= aCount)
                        {
                            eIndex -= aCount;
                        }
                        outputStr += alphabet[eIndex];
                        break;
                    }
                }
                else
                {
                    outputStr += " ";
                }
            }
            return outputStr;
        }
        /// <summary>
        /// 移位解密
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DisplacementDecode(this string inputStr, int key = 3) => ToDisplacementEncode(inputStr, -key);
    }
}
