namespace Materal.Extensions.ValidationAttributes;

/// <summary>
/// 最小
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MinAttribute : ValidationAttribute
{
    /// <summary>
    /// 最小值
    /// </summary>
    public object MinValue { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="minValue">最小值</param>
    public MinAttribute(object minValue) => MinValue = minValue;

    /// <summary>
    /// 构造方法(DateTime)
    /// </summary>
    public MinAttribute(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
    {
        MinValue = new DateTime(year, month, day, hour, minute, second, millisecond);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(object? value)
    {
        if (MinValue is not IComparable min) throw new ArgumentException("MinValue必须实现IComparable接口");
        if (value is null || value.GetType() != MinValue.GetType()) return false;
        bool result = min.CompareTo(value) <= 0;
        return result;
    }
}