namespace Materal.Extensions;

/// <summary>
/// 表达式扩展
/// 提供对 LINQ 表达式树的操作扩展方法，主要用于动态构建和组合表达式条件
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// 组合两个表达式
    /// 将两个具有相同参数类型的表达式按照指定的合并方式进行组合
    /// </summary>
    /// <typeparam name="T">表达式的类型</typeparam>
    /// <param name="first">第一个表达式</param>
    /// <param name="second">第二个表达式</param>
    /// <param name="merge">表达式合并函数，如 Expression.AndAlso（与）或 Expression.OrElse（或）</param>
    /// <returns>合并后的新表达式</returns>
    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        // 创建参数映射字典，将第二个表达式的参数映射到第一个表达式的参数
        Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
            .Select((f, i) => new { First = f, Second = second.Parameters[i] })
            .ToDictionary(p => p.Second, p => p.First);

        // 替换第二个表达式中的参数
        Expression secondBody = ExpressionParameterVisitor.ReplaceParameters(map, second.Body);

        // 使用合并函数组合两个表达式的主体，并创建新的 Lambda 表达式
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    /// <summary>
    /// 使用 AND 逻辑组合两个表达式条件
    /// 相当于 first &amp;&amp; second
    /// </summary>
    /// <typeparam name="T">表达式参数的类型</typeparam>
    /// <param name="first">第一个条件表达式</param>
    /// <param name="second">第二个条件表达式</param>
    /// <returns>组合后的 AND 条件表达式</returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) =>
        first.Compose(second, Expression.AndAlso);

    /// <summary>
    /// 使用 OR 逻辑组合两个表达式条件
    /// 相当于 first 或 second 条件满足其一
    /// </summary>
    /// <typeparam name="T">表达式参数的类型</typeparam>
    /// <param name="first">第一个条件表达式</param>
    /// <param name="second">第二个条件表达式</param>
    /// <returns>组合后的 OR 条件表达式</returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) =>
        first.Compose(second, Expression.OrElse);
}
