namespace Materal.Extensions;

/// <summary>
/// 字典扩展
/// </summary>
public static class DictionaryExtensions
{
#if NETSTANDARD
    /// <summary>
    /// 尝试向字典中添加键值对
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    /// <param name="dictionary">要操作的字典</param>
    /// <param name="key">要添加的键</param>
    /// <param name="value">要添加的值</param>
    /// <returns>添加成功返回true，如果键已存在则返回false</returns>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return false;
        dictionary.Add(key, value);
        return true;
    }
#endif
}
