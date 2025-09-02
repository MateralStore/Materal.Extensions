namespace Materal.Extensions;

/// <summary>
/// Byte扩展
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// 转换为二进制字符串
    /// </summary>
    /// <param name="buffer">要转换的字节数组</param>
    /// <returns>表示字节数组的二进制字符串</returns>
    public static string GetBinaryString(this byte[] buffer)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));

        IEnumerable<string> binaryStrings = buffer.Select(m => Convert.ToString(m, 2).PadLeft(8, '0'));
        string result = string.Join("", binaryStrings);
        return result;
    }

    /// <summary>
    /// 获得指定位数的二进制字符串
    /// </summary>
    /// <param name="buffer">要处理的字节数组</param>
    /// <param name="startBitIndex">起始位索引（从0开始）</param>
    /// <param name="endBitIndex">结束位索引（包含）</param>
    /// <returns>指定范围内的二进制字符串</returns>
    /// <exception cref="ArgumentOutOfRangeException">当索引超出范围时抛出</exception>
    public static string GetBinaryStringByBitIndex(this byte[] buffer, int startBitIndex, int endBitIndex)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        string result = buffer.GetBinaryString();
        // 边界检查
        if (startBitIndex < 0 || endBitIndex < 0) throw new ArgumentOutOfRangeException(nameof(startBitIndex), "索引不能为负数");
        if (startBitIndex > endBitIndex) throw new ArgumentOutOfRangeException(nameof(startBitIndex), "起始索引不能大于结束索引");
        if (endBitIndex >= result.Length) throw new ArgumentOutOfRangeException(nameof(endBitIndex), "索引超出范围");
        result = result[startBitIndex..(endBitIndex + 1)];
        return result;
    }

    /// <summary>
    /// 获得指定位数的值
    /// </summary>
    /// <param name="buffer">要处理的字节数组</param>
    /// <param name="startBitIndex">起始位索引（从0开始）</param>
    /// <param name="endBitIndex">结束位索引（包含）</param>
    /// <returns>指定范围内的字节数组</returns>
    /// <exception cref="ArgumentOutOfRangeException">当索引超出范围时抛出</exception>
    public static byte[] GetValueByBitIndex(this byte[] buffer, int startBitIndex, int endBitIndex)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        int intValue = buffer.GetIntValueByBitIndex(startBitIndex, endBitIndex);
        byte[] result = BitConverter.GetBytes(intValue);
        return result;
    }

    /// <summary>
    /// 获得指定位数的值
    /// </summary>
    /// <param name="buffer">要处理的字节数组</param>
    /// <param name="startBitIndex">起始位索引（从0开始）</param>
    /// <param name="endBitIndex">结束位索引（包含）</param>
    /// <returns>指定范围内的整数值</returns>
    /// <exception cref="ArgumentOutOfRangeException">当索引超出范围时抛出</exception>
    public static int GetIntValueByBitIndex(this byte[] buffer, int startBitIndex, int endBitIndex)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        string binaryString = buffer.GetBinaryStringByBitIndex(startBitIndex, endBitIndex);
        int result = Convert.ToInt32(binaryString, 2);
        return result;
    }
}
