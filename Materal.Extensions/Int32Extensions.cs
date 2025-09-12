using System.Collections.ObjectModel;

namespace Materal.Extensions;

/// <summary>
/// 整数扩展
/// </summary>
public static class Int32Extensions
{
    /// <summary>
    /// 简体中文
    /// </summary>
    private readonly static IntConvertModel SimplifiedChineseModel = new(
        new Dictionary<int, string>
        {
            [0] = "零",
            [1] = "一",
            [2] = "二",
            [3] = "三",
            [4] = "四",
            [5] = "五",
            [6] = "六",
            [7] = "七",
            [8] = "八",
            [9] = "九",
        },
        [
            "",
            "万",
            "亿"
        ],
        new Dictionary<int, string>
        {
            [0] = "千",
            [1] = "百",
            [2] = "十"
        }
    );
    /// <summary>
    /// 大写中文
    /// </summary>
    private readonly static IntConvertModel CapitalChineseModel = new(
        new Dictionary<int, string>
        {
            [0] = "零",
            [1] = "壹",
            [2] = "贰",
            [3] = "叁",
            [4] = "肆",
            [5] = "伍",
            [6] = "陆",
            [7] = "柒",
            [8] = "捌",
            [9] = "玖",
        },
        [
            "",
            "万",
            "亿"
        ],
        new Dictionary<int, string>
        {
            [0] = "仟",
            [1] = "佰",
            [2] = "拾"
        }
    );
    /// <summary>
    /// 转换为简体中文
    /// </summary>
    /// <param name="inputNumber">输入数字</param>
    /// <returns>简体中文表示</returns>
    public static string ConvertToSimplifiedChinese(this int inputNumber) => inputNumber.ConvertToChinese(SimplifiedChineseModel);
    /// <summary>
    /// 转换为大写中文
    /// </summary>
    /// <param name="inputNumber">输入数字</param>
    /// <returns>大写中文表示</returns>
    public static string ConvertToCapitalChinese(this int inputNumber) => inputNumber.ConvertToChinese(CapitalChineseModel);
    /// <summary>
    /// 转换为中文
    /// </summary>
    /// <param name="inputNumber">输入数字</param>
    /// <param name="model">转换模型</param>
    /// <returns>中文表示</returns>
    private static string ConvertToChinese(this int inputNumber, IntConvertModel model)
    {
        int number = inputNumber;
        StringBuilder result = new();
        if (number < 0)
        {
            result.Append('负');
            number *= -1;
        }
        else if (number == 0) return "零";

        ReadOnlyDictionary<int, string> chineseNumberDictionary = model.Numbers;
        List<int> digits = [];
        while (number > 0)
        {
            digits.Add(number % 10);
            number /= 10;
        }
        while (digits.Count % 4 != 0)
        {
            digits.Add(0);
        }

        List<string> chineseNumberGroups = [];
        string currentChineseNumber = string.Empty;
        int digitCount = 0;
        for (int i = digits.Count - 1; i >= 0; i--)
        {
            currentChineseNumber += chineseNumberDictionary[digits[i]];
            digitCount++;
            if (digitCount % 4 == 0)
            {
                chineseNumberGroups.Add(currentChineseNumber);
                currentChineseNumber = string.Empty;
            }
        }

        int groupIndex = 0;
        for (int i = chineseNumberGroups.Count - 1; i >= 0; i--)
        {
            string chineseNumberGroup = chineseNumberGroups[i];
            chineseNumberGroups[i] = ConvertToChineseHandler(chineseNumberGroup, groupIndex++, model);
        }

        result.Append(string.Join("", chineseNumberGroups));
        if (result.Length > 0 && result[^1] == '零')
        {
            result.Remove(result.Length - 1, 1);
        }
        return result.ToString();
    }
    /// <summary>
    /// 处理中文数字组
    /// </summary>
    /// <param name="digitGroup">数字组</param>
    /// <param name="groupIndex">组索引</param>
    /// <param name="model">转换模型</param>
    /// <returns>处理后的中文数字组</returns>
    private static string ConvertToChineseHandler(string digitGroup, int groupIndex, IntConvertModel model)
    {
        // 预构建扩展字典以提高性能，避免重复创建
        ReadOnlyDictionary<int, string> extendMapping = new(new Dictionary<int, string>
        {
            [0] = model.Extend[0],
            [1] = model.Extend[1],
            [2] = model.Extend[2],
            [3] = model.Units[groupIndex]
        });

        StringBuilder result = new();
        for (int i = 0; i < digitGroup.Length; i++)
        {
            string processedDigit = digitGroup[i] + extendMapping[i];
            if (processedDigit[0] == '零')
            {
                // 处理连续的零
                if (result.Length > 0 && result[^1] == '零') continue;
                // 处理开头的零
                if (result.Length == 0) continue;
                processedDigit = digitGroup[i].ToString();
            }
            result.Append(processedDigit);
        }

        // 处理结尾为零的情况
        if (result.Length > 0 && result[^1] == '零')
        {
            result.Remove(result.Length - 1, 1);
            result.Append(extendMapping[digitGroup.Length - 1] + "零");
        }
        return result.ToString();
    }
    /// <summary>
    /// 转换为二进制字符串
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static string GetBinaryString(this int buffer) => Convert.ToString(buffer, 2);
}
