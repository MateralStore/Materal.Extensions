namespace Materal.Extensions;

/// <summary>
/// 枚举帮助类
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// 根据描述获取枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="description">枚举值的描述</param>
    /// <returns>对应的枚举值</returns>
    /// <exception cref="ExtensionException">当转换失败或描述不匹配任何枚举值时抛出</exception>
    /// <exception cref="ArgumentNullException">当 description 为 null 时抛出</exception>
    public static T ConvertToEnumByDescription<T>(string description)
        where T : Enum
    {
        if (description is null) throw new ArgumentNullException(nameof(description));

        Type type = typeof(T);
        object obj = type.GetEnumByDescription(description);
        if (obj is not T result) throw new ExtensionException("转换失败");
        return result;
    }
}
