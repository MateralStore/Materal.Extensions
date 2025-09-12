namespace Materal.Extensions;

/// <summary>
/// MemberInfo扩展方法类
/// 提供对MemberInfo类型（包括PropertyInfo和FieldInfo）的扩展功能
/// </summary>
public static class MemberInfoExtensions
{
    /// <summary>
    /// 获取成员的值
    /// 支持PropertyInfo（属性）和FieldInfo（字段）类型的成员
    /// </summary>
    /// <param name="memberInfo">成员信息对象（PropertyInfo或FieldInfo）</param>
    /// <param name="obj">包含该成员的对象实例</param>
    /// <returns>返回成员的值，如果成员不是PropertyInfo或FieldInfo类型则抛出异常</returns>
    /// <exception cref="ExtensionException">当memberInfo不是PropertyInfo或FieldInfo类型时抛出</exception>
    public static object? GetValue(this MemberInfo memberInfo, object obj)
    {
        object? memberValue = memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.GetValue(obj, null),
            FieldInfo fieldInfo => fieldInfo.GetValue(obj),
            _ => throw new ExtensionException($"{memberInfo.Name}不能获取值"),
        };
        return memberValue;
    }
    /// <summary>
    /// 检查成员是否具有指定类型的自定义特性
    /// </summary>
    /// <typeparam name="T">要检查的特性类型，必须继承自Attribute</typeparam>
    /// <param name="propertyInfo">成员信息对象</param>
    /// <returns>如果成员具有指定类型的特性则返回true，否则返回false</returns>
    public static bool HasCustomAttribute<T>(this MemberInfo propertyInfo)
        where T : Attribute
    {
        Attribute? attr = propertyInfo.GetCustomAttribute<T>();
        return attr is not null;
    }
}
