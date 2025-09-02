namespace Materal.Extensions;

/// <summary>
/// 程序集扩展
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// 根据过滤条件获取程序集中的第一个匹配类型
    /// </summary>
    /// <param name="assembly">要搜索的程序集</param>
    /// <param name="filter">类型过滤器函数</param>
    /// <returns>第一个匹配的类型，如果未找到或发生异常则返回null</returns>
    public static Type? GetTypeByFilter(this Assembly assembly, Func<Type, bool> filter)
    {
        try
        {
            return assembly.GetTypes().FirstOrDefault(m => filter(m));
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 根据过滤条件获取程序集中所有匹配的类型
    /// </summary>
    /// <param name="assembly">要搜索的程序集</param>
    /// <param name="filter">类型过滤器函数</param>
    /// <returns>所有匹配的类型集合，如果发生异常则返回空集合</returns>
    public static IEnumerable<Type> GetTypesByFilter(this Assembly assembly, Func<Type, bool> filter)
    {
        try
        {
            return assembly.GetTypes().Where(m => filter(m));
        }
        catch
        {
            return [];
        }
    }
    /// <summary>
    /// 获取程序集中所有实现了指定类型 T 的公共具体类（非抽象类）
    /// </summary>
    /// <typeparam name="T">要查找的基类型或接口</typeparam>
    /// <param name="assembly">要搜索的程序集</param>
    /// <returns>所有匹配的类型集合</returns>
    public static IEnumerable<Type> GetTypes<T>(this Assembly assembly)
        => assembly.GetTypesByFilter(type => type.IsAssignableTo(typeof(T)) && type.IsPublic && type.IsClass && !type.IsAbstract);
    /// <summary>
    /// 获取程序集所在的文件夹路径
    /// </summary>
    /// <param name="assembly">要获取路径的程序集</param>
    /// <returns>程序集所在的文件夹路径</returns>
    /// <exception cref="MateralException">当无法获取文件夹路径时抛出</exception>
    public static string GetDirectoryPath(this Assembly assembly)
        => Path.GetDirectoryName(assembly.Location) ?? throw new MateralException("获取所在文件夹路径失败");
    /// <summary>
    /// 检查程序集是否包含指定的自定义特性
    /// </summary>
    /// <typeparam name="T">要检查的特性类型</typeparam>
    /// <param name="assembly">要检查的程序集</param>
    /// <returns>如果程序集包含指定特性则返回true，否则返回false</returns>
    public static bool HasCustomAttribute<T>(this Assembly assembly)
        where T : Attribute => assembly.GetCustomAttribute<T>() is not null;
}
