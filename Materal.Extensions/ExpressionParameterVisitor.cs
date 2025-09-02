namespace Materal.Extensions
{
    /// <summary>
    /// 表达式参数访问器
    /// </summary>
    /// <param name="map"></param>
    public class ExpressionParameterVisitor(Dictionary<ParameterExpression, ParameterExpression> map) : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map = map ?? [];

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="map"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp) => new ExpressionParameterVisitor(map).Visit(exp);
        /// <inheritdoc/>
        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            if (_map.TryGetValue(parameterExpression, out ParameterExpression? replacement))
            {
                parameterExpression = replacement;
            }
            return base.VisitParameter(parameterExpression);
        }
    }
}
