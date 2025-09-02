namespace Materal.Extensions
{
    /// <summary>
    /// Enumerable扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 通过HashSet去重，使用元素本身的相等比较器
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="sources">要去重的元素序列</param>
        /// <returns>去重后的元素序列</returns>
        /// <exception cref="ArgumentNullException">当 sources 为 null 时抛出</exception>
        public static IEnumerable<T> DistinctByHashSet<T>(this IEnumerable<T> sources)
            => DistinctByHashSet(sources, null);

        /// <summary>
        /// 通过HashSet去重，使用指定的相等比较器
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="sources">要去重的元素序列</param>
        /// <param name="comparer">用于比较元素的相等比较器</param>
        /// <returns>去重后的元素序列</returns>
        /// <exception cref="ArgumentNullException">当 sources 为 null 时抛出</exception>
        public static IEnumerable<T> DistinctByHashSet<T>(this IEnumerable<T> sources, IEqualityComparer<T>? comparer)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));

            HashSet<T> hashSet = comparer is null ? [] : new HashSet<T>(comparer);
            foreach (T source in sources)
            {
                if (!hashSet.Add(source)) continue;
                yield return source;
            }
        }

        /// <summary>
        /// 通过HashSet去重，使用指定的键选择器函数
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="sources">要去重的元素序列</param>
        /// <param name="keySelector">键选择器函数，用于提取每个元素的比较键</param>
        /// <returns>去重后的元素序列</returns>
        /// <exception cref="ArgumentNullException">当 sources 或 keySelector 为 null 时抛出</exception>
        public static IEnumerable<T> DistinctByHashSet<T, TKey>(this IEnumerable<T> sources, Func<T, TKey> keySelector)
            => DistinctByHashSet(sources, keySelector, null);

        /// <summary>
        /// 通过HashSet去重，使用指定的键选择器函数和相等比较器
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="sources">要去重的元素序列</param>
        /// <param name="keySelector">键选择器函数，用于提取每个元素的比较键</param>
        /// <param name="comparer">用于比较键的相等比较器</param>
        /// <returns>去重后的元素序列</returns>
        /// <exception cref="ArgumentNullException">当 sources 或 keySelector 为 null 时抛出</exception>
        public static IEnumerable<T> DistinctByHashSet<T, TKey>(this IEnumerable<T> sources, Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            HashSet<TKey> hashSet = comparer is null ? [] : new HashSet<TKey>(comparer);
            foreach (T source in sources)
            {
                if (!hashSet.Add(keySelector(source))) continue;
                yield return source;
            }
        }
    }
}
