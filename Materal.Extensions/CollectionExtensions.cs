using System.Collections.ObjectModel;

namespace Materal.Extensions;

/// <summary>
/// 集合扩展
/// </summary>
public static partial class CollectionExtensions
{
    /// <summary>
    /// 将列表转换为动态数据集合
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    /// <param name="listM">要转换的列表</param>
    /// <returns>包含相同元素的ObservableCollection集合</returns>
    /// <exception cref="ArgumentNullException">当listM为null时抛出</exception>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> listM)
    {
        if (listM is null) throw new ArgumentNullException(nameof(listM));
        return new ObservableCollection<T>(listM);
    }

    /// <summary>
    /// 比较两个集合，获取需要新增的元素和需要删除的元素
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    /// <param name="sourceArray">新的集合</param>
    /// <param name="oldArray">旧的集合</param>
    /// <returns>包含需要新增元素和需要删除元素的元组</returns>
    /// <exception cref="ArgumentNullException">当sourceArray或oldArray为null时抛出</exception>
    public static (ICollection<T> addArray, ICollection<T> removeArray) GetAddArrayAndRemoveArray<T>(this ICollection<T> sourceArray, ICollection<T> oldArray)
    {
        if (sourceArray is null) throw new ArgumentNullException(nameof(sourceArray));
        if (oldArray is null) throw new ArgumentNullException(nameof(oldArray));

        ICollection<T> addArray = [.. sourceArray.Except(oldArray)];
        ICollection<T> removeArray = [.. oldArray.Except(sourceArray)];
        return (addArray, removeArray);
    }

    /// <summary>
    /// 使用自定义比较器对集合进行去重
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    /// <param name="source">要去重的集合</param>
    /// <param name="comparer">自定义比较器函数</param>
    /// <returns>去重后的集合</returns>
    /// <exception cref="ArgumentNullException">当source或comparer为null时抛出</exception>
    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer) where T : class
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return source.Distinct(new DynamicEqualityComparer<T>(comparer));
    }

    /// <summary>
    /// 动态比较器
    /// </summary>
    /// <typeparam name="T">比较元素类型</typeparam>
    /// <param name="func">比较函数</param>
    private sealed class DynamicEqualityComparer<T>(Func<T, T, bool> func) : IEqualityComparer<T> where T : class
    {
        public bool Equals(T? x, T? y)
        {
            if (x is null && y is null) return true;
            else if (x is null || y is null) return false;
            return func(x, y);
        }

        public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
    }
}
