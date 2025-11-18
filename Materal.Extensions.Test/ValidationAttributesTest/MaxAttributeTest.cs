namespace Materal.Extensions.Test.ValidationAttributesTest;

[TestClass]
public class MaxAttributeTest
{
    [TestMethod]
    public void Test()
    {
        TestModel model = new()
        {
            Number = 10,
            DateTime = new DateTime(2024, 12, 31)
        };
        if (!model.Validation(out string errorMessage))
        {
            Assert.Fail(errorMessage);
        }
    }

    /// <summary>
    /// 测试边界值 - 等于最大值应该通过验证
    /// </summary>
    [TestMethod]
    public void Test_BoundaryValue_Equal()
    {
        TestModel model = new()
        {
            Number = 10,
            DateTime = new DateTime(2024, 12, 31)
        };
        Assert.IsTrue(model.Validation(out _));
    }

    /// <summary>
    /// 测试小于最大值应该通过验证
    /// </summary>
    [TestMethod]
    public void Test_LessThanMax_ShouldPass()
    {
        TestModel model = new()
        {
            Number = 5,
            DateTime = new DateTime(2024, 12, 30)
        };
        Assert.IsTrue(model.Validation(out _));
    }

    /// <summary>
    /// 测试大于最大值应该失败
    /// </summary>
    [TestMethod]
    public void Test_GreaterThanMax_ShouldFail()
    {
        TestModel model = new()
        {
            Number = 11,
            DateTime = new DateTime(2024, 12, 31)
        };
        Assert.IsFalse(model.Validation(out string errorMessage));
        Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
    }

    /// <summary>
    /// 测试DateTime大于最大值应该失败
    /// </summary>
    [TestMethod]
    public void Test_DateTime_GreaterThanMax_ShouldFail()
    {
        TestModel model = new()
        {
            Number = 10,
            DateTime = new DateTime(2025, 1, 1)
        };
        Assert.IsFalse(model.Validation(out string errorMessage));
        Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
    }

    /// <summary>
    /// 测试完整DateTime参数（包含时分秒毫秒）
    /// </summary>
    [TestMethod]
    public void Test_DateTime_WithFullParameters()
    {
        var model = new TestModelWithFullDateTime
        {
            DateTime = new DateTime(2024, 12, 31, 23, 59, 59, 999)
        };
        Assert.IsTrue(model.Validation(out _));

        model.DateTime = new DateTime(2025, 1, 1, 0, 0, 0, 0);
        Assert.IsFalse(model.Validation(out _));
    }

    /// <summary>
    /// 测试不同数值类型
    /// </summary>
    [TestMethod]
    public void Test_DifferentNumericTypes()
    {
        var model = new TestModelWithDifferentTypes
        {
            LongValue = 100L,
            DoubleValue = 99.99,
            DecimalValue = 1000.50m
        };
        Assert.IsTrue(model.Validation(out _));

        model.LongValue = 101L;
        Assert.IsFalse(model.Validation(out _));
    }

    /// <summary>
    /// 测试字符串比较
    /// </summary>
    [TestMethod]
    public void Test_StringComparison()
    {
        var model = new TestModelWithString
        {
            Name = "Apple"
        };
        Assert.IsTrue(model.Validation(out _));

        model.Name = "Zebra";
        Assert.IsFalse(model.Validation(out _));
    }

    /// <summary>
    /// 测试nullable类型为null的情况
    /// </summary>
    [TestMethod]
    public void Test_NullableValue_Null_ShouldFail()
    {
        var model = new TestModelWithNullable
        {
            NullableNumber = null
        };
        Assert.IsFalse(model.Validation(out _));
    }

    /// <summary>
    /// 测试nullable类型有值的情况
    /// </summary>
    [TestMethod]
    public void Test_NullableValue_WithValue_ShouldPass()
    {
        var model = new TestModelWithNullable
        {
            NullableNumber = 50
        };
        Assert.IsTrue(model.Validation(out _));
    }

    /// <summary>
    /// 测试类型不匹配应该失败
    /// </summary>
    [TestMethod]
    public void Test_TypeMismatch_ShouldFail()
    {
        var attribute = new MaxAttribute(100);
        // 尝试验证字符串值，但MaxValue是int
        Assert.IsFalse(attribute.IsValid("100"));
    }

    /// <summary>
    /// 测试MaxValue不实现IComparable接口应该抛出异常
    /// </summary>
    [TestMethod]
    public void Test_NonComparableMaxValue_ShouldThrowException()
    {
        var attribute = new MaxAttribute(new object());
        try
        {
            attribute.IsValid(new object());
            Assert.Fail("应该抛出 ArgumentException");
        }
        catch (ArgumentException ex)
        {
            // 验证异常消息
            Assert.Contains("IComparable", ex.Message);
        }
    }

    public class TestModel
    {
        [Max(10)]
        public int Number { get; set; }
        [Max(2024, 12, 31)]
        public DateTime DateTime { get; set; }
    }

    public class TestModelWithFullDateTime
    {
        [Max(2024, 12, 31, 23, 59, 59, 999)]
        public DateTime DateTime { get; set; }
    }

    public class TestModelWithDifferentTypes
    {
        [Max(100L)]
        public long LongValue { get; set; }

        [Max(100.0)]
        public double DoubleValue { get; set; }

        [Max(1000.99)]
        public decimal DecimalValue { get; set; }
    }

    public class TestModelWithString
    {
        [Max("Banana")]
        public string Name { get; set; } = string.Empty;
    }

    public class TestModelWithNullable
    {
        [Max(100)]
        public int? NullableNumber { get; set; }
    }
}
