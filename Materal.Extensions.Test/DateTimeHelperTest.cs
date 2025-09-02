using System;
using System.ComponentModel;
using Materal.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Materal.Extensions.Test
{
    [TestClass]
    public class DateTimeHelperTest
    {
        [TestMethod]
        public void TestGetTimeStamp()
        {
            // 测试获取时间戳
            long timeStamp = DateTimeHelper.GetTimeStamp();
            Assert.IsTrue(timeStamp > 0);
            
            // 测试UTC时间戳
            long utcTimeStamp = DateTimeHelper.GetTimeStamp(DateTimeKind.Utc);
            Assert.IsTrue(utcTimeStamp > 0);
            
            // 测试本地时间戳
            long localTimeStamp = DateTimeHelper.GetTimeStamp(DateTimeKind.Local);
            Assert.IsTrue(localTimeStamp > 0);
        }
        
        [TestMethod]
        public void TestTimeStampToDateTime()
        {
            // 获取当前时间戳
            long timeStamp = DateTimeHelper.GetTimeStamp();
            
            // 测试时间戳转换为DateTime
            DateTime dateTime = DateTimeHelper.TimeStampToDateTime(timeStamp);
            Assert.IsNotNull(dateTime);
            
            // 测试时间戳转换为DateTimeOffset
            DateTimeOffset dateTimeOffset = DateTimeHelper.TimeStampToDateTimeOffset(timeStamp);
            Assert.IsNotNull(dateTimeOffset);
        }
        
        [TestMethod]
        public void TestTimeUnitConversions()
        {
            double value = 1;
            
            // 测试年转换
            double milliseconds = DateTimeHelper.ToMilliseconds(value, DateTimeUnitEnum.YearUnit);
            double seconds = DateTimeHelper.ToSeconds(value, DateTimeUnitEnum.YearUnit);
            double minutes = DateTimeHelper.ToMinutes(value, DateTimeUnitEnum.YearUnit);
            double hours = DateTimeHelper.ToHours(value, DateTimeUnitEnum.YearUnit);
            double days = DateTimeHelper.ToDay(value, DateTimeUnitEnum.YearUnit);
            double months = DateTimeHelper.ToMonth(value, DateTimeUnitEnum.YearUnit);
            double years = DateTimeHelper.ToYear(value, DateTimeUnitEnum.YearUnit);
            
            // 验证转换结果一致性
            Assert.AreEqual(milliseconds, seconds * 1000);
            Assert.AreEqual(seconds, minutes * 60);
            Assert.AreEqual(minutes, hours * 60);
            Assert.AreEqual(hours, days * 24);
            Assert.AreEqual(days, years * 365.25, 0.01);
            Assert.AreEqual(months, years * 12);
        }
        
        [TestMethod]
        public void TestInvalidEnumArgumentException()
        {
            // 测试无效枚举值异常
            try
            {
                // 强制转换一个无效的枚举值
                DateTimeHelper.ToMilliseconds(1, (DateTimeUnitEnum)999);
                Assert.Fail("应该抛出InvalidEnumArgumentException");
            }
            catch (InvalidEnumArgumentException)
            {
                // 正确捕获异常
            }
        }
    }
}
