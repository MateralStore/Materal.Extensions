namespace Materal.Extensions;

/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举类型的所有值
    /// </summary>
    /// <param name="enum">枚举实例</param>
    /// <returns>包含枚举类型所有值的列表</returns>
    /// <exception cref="ExtensionException">当类型不是枚举类型时抛出</exception>
    public static List<Enum> GetAllEnum(this Enum @enum) => @enum.GetType().GetAllEnum();
    /// <summary>
    /// 获取指定类型的所有枚举值
    /// </summary>
    /// <param name="type">枚举类型</param>
    /// <returns>包含指定枚举类型所有值的列表</returns>
    /// <exception cref="ExtensionException">当类型不是枚举类型时抛出</exception>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static List<Enum> GetAllEnum(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (!type.IsEnum) throw new ExtensionException("该类型不是枚举类型");
        List<Enum> result = [];
        Array allEnums = Enum.GetValues(type);
        foreach (Enum item in allEnums)
        {
            result.Add(item);
        }
        return result;
    }
    /// <summary>
    /// 获取枚举类型的值总数
    /// </summary>
    /// <param name="enum">枚举实例</param>
    /// <returns>枚举类型的值总数</returns>
    /// <exception cref="ExtensionException">当类型不是枚举类型时抛出</exception>
    public static int GetEnumCount(this Enum @enum) => @enum.GetType().GetEnumCount();
}
