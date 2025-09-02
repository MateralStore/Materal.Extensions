using System.Globalization;

namespace Materal.Extensions;

/// <summary>
/// 日期时间扩展
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 为TimeOnly对象添加指定的秒数
    /// </summary>
    /// <param name="time">要添加秒数的时间对象</param>
    /// <param name="seconds">要添加的秒数</param>
    /// <returns>添加秒数后的新TimeOnly对象</returns>
    public static TimeOnly AddSeconds(this TimeOnly time, int seconds) => time.Add(TimeSpan.FromSeconds(seconds));
    /// <summary>
    /// 将DateTimeOffset转换为DateTime
    /// </summary>
    /// <param name="dateTime">要转换的DateTimeOffset对象</param>
    /// <param name="dateTimeKind">目标DateTimeKind，默认为Local</param>
    /// <returns>转换后的DateTime对象</returns>
    public static DateTime ToDateTime(this DateTimeOffset dateTime, DateTimeKind dateTimeKind = DateTimeKind.Local)
        => DateTime.SpecifyKind(dateTime.DateTime, dateTimeKind);
    /// <summary>
    /// 将DateTime转换为DateTimeOffset
    /// </summary>
    /// <param name="dateTime">要转换的DateTime对象</param>
    /// <param name="dateTimeKind">目标DateTimeKind，如果为null则使用原DateTime的Kind</param>
    /// <returns>转换后的DateTimeOffset对象</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, DateTimeKind? dateTimeKind = null) => new(DateTime.SpecifyKind(dateTime, dateTimeKind ?? dateTime.Kind));
    /// <summary>
    /// 将TimeOnly和DateOnly组合转换为DateTime
    /// </summary>
    /// <param name="time">时间部分</param>
    /// <param name="date">日期部分</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>组合后的DateTime对象</returns>
    public static DateTime ToDateTime(this TimeOnly time, DateOnly date, DateTimeKind dateTimeKind = DateTimeKind.Local)
        => new(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, dateTimeKind);
    /// <summary>
    /// 将TimeOnly和DateOnly组合转换为DateTimeOffset
    /// </summary>
    /// <param name="time">时间部分</param>
    /// <param name="date">日期部分</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>组合后的DateTimeOffset对象</returns>
    public static DateTimeOffset ToDateTimeOffset(this TimeOnly time, DateOnly date, DateTimeKind dateTimeKind = DateTimeKind.Local) => time.ToDateTime(date, dateTimeKind).ToDateTimeOffset();
    /// <summary>
    /// 将DateOnly转换为DateTime（时间为00:00:00）
    /// </summary>
    /// <param name="date">要转换的DateOnly对象</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>转换后的DateTime对象</returns>
    public static DateTime ToDateTime(this DateOnly date, DateTimeKind dateTimeKind = DateTimeKind.Local) => date.ToDateTime(new TimeOnly(0, 0, 0), dateTimeKind);
    /// <summary>
    /// 将DateOnly和TimeOnly组合转换为DateTime
    /// </summary>
    /// <param name="date">日期部分</param>
    /// <param name="time">时间部分</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>组合后的DateTime对象</returns>
    public static DateTime ToDateTime(this DateOnly date, TimeOnly time, DateTimeKind dateTimeKind = DateTimeKind.Local) => time.ToDateTime(date, dateTimeKind);
    /// <summary>
    /// 将DateOnly转换为DateTimeOffset（时间为00:00:00）
    /// </summary>
    /// <param name="date">要转换的DateOnly对象</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>转换后的DateTimeOffset对象</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateOnly date, DateTimeKind dateTimeKind = DateTimeKind.Local) => date.ToDateTimeOffset(new TimeOnly(0, 0, 0), dateTimeKind);
    /// <summary>
    /// 将DateOnly和TimeOnly组合转换为DateTimeOffset
    /// </summary>
    /// <param name="date">日期部分</param>
    /// <param name="time">时间部分</param>
    /// <param name="dateTimeKind">DateTimeKind，默认为Local</param>
    /// <returns>组合后的DateTimeOffset对象</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateOnly date, TimeOnly time, DateTimeKind dateTimeKind = DateTimeKind.Local) => date.ToDateTime(time, dateTimeKind).ToDateTimeOffset();
    /// <summary>
    /// 将DateTime转换为DateOnly
    /// </summary>
    /// <param name="dateTime">要转换的DateTime对象</param>
    /// <returns>转换后的DateOnly对象</returns>
    public static DateOnly ToDateOnly(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, dateTime.Day);
    /// <summary>
    /// 将DateTimeOffset转换为DateOnly
    /// </summary>
    /// <param name="dateTimeOffset">要转换的DateTimeOffset对象</param>
    /// <returns>转换后的DateOnly对象</returns>
    public static DateOnly ToDateOnly(this DateTimeOffset dateTimeOffset) => new(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day);
    /// <summary>
    /// 将DateTime转换为TimeOnly（仅包含小时、分钟、秒）
    /// </summary>
    /// <param name="dateTime">要转换的DateTime对象</param>
    /// <returns>转换后的TimeOnly对象</returns>
    public static TimeOnly ToTimeOnly(this DateTime dateTime) => new(dateTime.Hour, dateTime.Minute, dateTime.Second);
    /// <summary>
    /// 将DateTimeOffset转换为TimeOnly（仅包含小时、分钟、秒）
    /// </summary>
    /// <param name="dateTimeOffset">要转换的DateTimeOffset对象</param>
    /// <returns>转换后的TimeOnly对象</returns>
    public static TimeOnly ToTimeOnly(this DateTimeOffset dateTimeOffset) => new(dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second);
    /// <summary>
    /// 获取DateOnly对象在年份中的季度
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>季度数（1-4）</returns>
    public static int GetQuarterOfYear(this DateOnly date) => date.ToDateTime().GetQuarterOfYear();
    /// <summary>
    /// 获取DateOnly对象在季度中的月份
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>季度中的月份（1-3）</returns>
    public static int GetMonthOfQuarter(this DateOnly date) => date.ToDateTime().GetMonthOfQuarter();
    /// <summary>
    /// 获取DateOnly对象在季度中的周数
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>季度中的周数</returns>
    public static int GetWeekOfQuarter(this DateOnly date) => date.ToDateTime().GetWeekOfQuarter();
    /// <summary>
    /// 获取DateOnly对象在季度中的天数
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>季度中的天数</returns>
    public static int GetDayOfQuarter(this DateOnly date) => date.ToDateTime().GetDayOfQuarter();
    /// <summary>
    /// 获取DateOnly对象在年份中的周数
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>年份中的周数</returns>
    public static int GetWeekOfYear(this DateOnly date) => date.ToDateTime().GetWeekOfYear();
    /// <summary>
    /// 获取DateOnly对象在月份中的周数
    /// </summary>
    /// <param name="date">要计算的DateOnly对象</param>
    /// <returns>月份中的周数</returns>
    public static int GetWeekOfMonth(this DateOnly date) => date.ToDateTime().GetWeekOfMonth();
    /// <summary>
    /// 获取DateTime对象在年份中的季度
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>季度数（1-4）</returns>
    public static int GetQuarterOfYear(this DateTime dateTime)
    {
        return (dateTime.Month + 2) / 3;
    }
    /// <summary>
    /// 获取DateTime对象在季度中的月份
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>季度中的月份（1-3）</returns>
    public static int GetMonthOfQuarter(this DateTime dateTime)
    {
        int quarter = dateTime.GetQuarterOfYear();
        return dateTime.Month - (quarter - 1) * 3;
    }
    /// <summary>
    /// 获取DateTime对象在季度中的周数
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>季度中的周数</returns>
    public static int GetWeekOfQuarter(this DateTime dateTime)
    {
        int quarter = dateTime.GetQuarterOfYear();
        DateTime startDate = new(dateTime.Year, (quarter - 1) * 3 + 1, 1);
        // 找到季度开始日期所在周的周一
        while (startDate.DayOfWeek != DayOfWeek.Monday)
        {
            startDate = startDate.AddDays(-1);
        }
        // 计算两个日期之间的天数差
        int differenceDays = (dateTime.Date - startDate).Days;
        // 计算周数（从0开始）
        return differenceDays / 7 + 1;
    }
    /// <summary>
    /// 获取DateTime对象在季度中的天数
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>季度中的天数</returns>
    public static int GetDayOfQuarter(this DateTime dateTime)
    {
        int quarter = dateTime.GetQuarterOfYear();
        DateTime startDate = new(dateTime.Year, (quarter - 1) * 3 + 1, 1);
        return dateTime.DayOfYear - startDate.DayOfYear + 1;
    }
    /// <summary>
    /// 获取DateTime对象在年份中的周数
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <param name="firstDay">每周的第一天，默认为Monday</param>
    /// <returns>年份中的周数</returns>
    public static int GetWeekOfYear(this DateTime dateTime, DayOfWeek firstDay = DayOfWeek.Monday)
    {
        GregorianCalendar gregorianCalendar = new();
        return gregorianCalendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, firstDay);
    }
    /// <summary>
    /// 获取DateTime对象在月份中的周数
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>月份中的周数</returns>
    public static int GetWeekOfMonth(this DateTime dateTime)
    {
        DateTime monthFirstDay = new(dateTime.Year, dateTime.Month, 1);
        DayOfWeek monthFirstDayOfWeek = monthFirstDay.DayOfWeek;

        // 计算月第一天是星期几（Sunday=0, Monday=1, ...）
        int firstDayOffset = (int)monthFirstDayOfWeek;

        // 计算当前日期在月份中的周数
        return (dateTime.Day + firstDayOffset - 1) / 7 + 1;
    }

    /// <summary>
    /// 获得时间戳
    /// 1970年1月1日 0点0分0秒以来的秒数
    /// </summary>
    /// <param name="dateTime">要计算时间戳的DateTime对象</param>
    /// <returns>时间戳（秒数）</returns>
    public static long GetTimeStamp(this DateTime dateTime)
    {
        TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, dateTime.Kind);
        return (long)timeSpan.TotalSeconds;
    }
    /// <summary>
    /// 获得时间戳
    /// 1970年1月1日 0点0分0秒以来的秒数
    /// </summary>
    /// <param name="dateTime">要计算时间戳的DateTimeOffset对象</param>
    /// <returns>时间戳（秒数）</returns>
    public static long GetTimeStamp(this DateTimeOffset dateTime) => dateTime.ToDateTime().GetTimeStamp();
    /// <summary>
    /// 获得当天第一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当天的第一秒（00:00:00）</returns>
    public static DateTime GetDayFirstSecond(this DateTime dateTime) => dateTime.Date;
    /// <summary>
    /// 获得当天第一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当天的第一毫秒（00:00:00.000）</returns>
    public static DateTime GetDayFirstMillisecond(this DateTime dateTime) => dateTime.Date;
    /// <summary>
    /// 获得当天最后一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当天的最后一毫秒（23:59:59.999）</returns>
    public static DateTime GetDayLastMillisecond(this DateTime dateTime) => dateTime.Date.AddDays(1).AddMilliseconds(-1);
    /// <summary>
    /// 获得当天最后一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当天的最后一秒（23:59:59）</returns>
    public static DateTime GetDayLastSecond(this DateTime dateTime) => dateTime.Date.AddDays(1).AddSeconds(-1);
    /// <summary>
    /// 获得当月第一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当月的第一秒（1号 00:00:00）</returns>
    public static DateTime GetMonthFirstSecond(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);
    /// <summary>
    /// 获得当月第一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当月的第一毫秒（1号 00:00:00.000）</returns>
    public static DateTime GetMonthFirstMillisecond(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);
    /// <summary>
    /// 获得当月最后一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当月的最后一毫秒</returns>
    public static DateTime GetMonthLastMillisecond(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddMilliseconds(-1);
    /// <summary>
    /// 获得当月最后一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当月的最后一秒</returns>
    public static DateTime GetMonthLastSecond(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddSeconds(-1);
    /// <summary>
    /// 获得当年第一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当年的第一秒（1月1日 00:00:00）</returns>
    public static DateTime GetYearFirstSecond(this DateTime dateTime) => new(dateTime.Year, 1, 1);
    /// <summary>
    /// 获得当年第一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当年的第一毫秒（1月1日 00:00:00.000）</returns>
    public static DateTime GetYearFirstMillisecond(this DateTime dateTime) => new(dateTime.Year, 1, 1);
    /// <summary>
    /// 获得当年最后一毫秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当年的最后一毫秒</returns>
    public static DateTime GetYearLastMillisecond(this DateTime dateTime) => new(dateTime.Year, 12, 31, 23, 59, 59, 999);
    /// <summary>
    /// 获得当年最后一秒
    /// </summary>
    /// <param name="dateTime">要计算的DateTime对象</param>
    /// <returns>当年的最后一秒</returns>
    public static DateTime GetYearLastSecond(this DateTime dateTime) => new(dateTime.Year, 12, 31, 23, 59, 59);
}
