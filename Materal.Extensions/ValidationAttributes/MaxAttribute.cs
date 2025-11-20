namespace Materal.Extensions.ValidationAttributes;

/// <summary>
/// 最大
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MaxAttribute : ValidationAttribute
{
    /// <summary>
    /// 最大值
    /// </summary>
    public object MaxValue { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="maxValue"></param>
    public MaxAttribute(object maxValue) => MaxValue = maxValue;

    /// <summary>
    /// 构造方法<see cref="DateTime"/>
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    /// <param name="millisecond"></param>
    public MaxAttribute(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
    {
        MaxValue = new DateTime(year, month, day, hour, minute, second, millisecond);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(object? value)
    {
        if (MaxValue is not IComparable max) throw new ArgumentException("MaxValue必须实现IComparable接口");
        if (value is null || value.GetType() != MaxValue.GetType()) return false;
        bool result = max.CompareTo(value) >= 0;
        return result;
    }
}
