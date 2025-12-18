using Materal.Extensions.JsonConverters;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Materal.Extensions.Test;

[TestClass]
public class JsonTest
{
    [TestMethod]
    public void TestObjectToJson()
    {
        TestModel model = GetTestModel();
        {
            string json = model.ToJson();
            TestModel result = json.JsonToObject<TestModel>();
            AreEqual(result, model);
        }
        {
            string json = model.ToJsonWithInferredTypes();
            TestModel result = json.JsonToObjectWithInferredTypes<TestModel>();
            AreEqual(result, model);
        }
    }

    [TestMethod]
    public void TestDicToJson()
    {
        Dictionary<string, object> data = [];
        data.Add("Int", 1);
        data.Add("Data", GetTestModel());
        string json = data.ToJsonWithInferredTypes();
        Dictionary<string, object> result = json.JsonToObjectWithInferredTypes<Dictionary<string, object>>();
        Assert.AreEqual(result["Int"], data["Int"]);
        AreEqual(result["Data"] as TestModel, data["Data"] as TestModel);
    }

    [TestMethod]
    public void TestTypeToJson()
    {
        TypeJsonModel model = new(typeof(string));
        string json = model.ToJsonWithInferredTypes();
        TypeJsonModel result = json.JsonToObjectWithInferredTypes<TypeJsonModel>();
        model.Type = result.Type;
    }

    private static void AreEqual(TestModel? model1, TestModel? model2)
    {
        if (model1 is null && model2 is null) return;
        if (model1 is null || model2 is null)
        {
            Assert.Fail("不相等");
            return;
        }
        foreach (PropertyInfo propertyInfo in typeof(TestModel).GetProperties())
        {
            if (propertyInfo.PropertyType != typeof(TestSubModel))
            {
                Assert.AreEqual(propertyInfo.GetValue(model2), propertyInfo.GetValue(model1));
            }
            else
            {
                foreach (PropertyInfo propertyInfo2 in typeof(TestSubModel).GetProperties())
                {
                    Assert.AreEqual(propertyInfo2.GetValue(model2.SubModel), propertyInfo2.GetValue(model1.SubModel));
                }
            }
        }
    }

    private static TestModel GetTestModel()
    {
        TestModel result = new()
        {
            String = "Hello World!",
            Bool = true,
            Int = 1,
            Float = 1.1f,
            Double = 1.2,
            Decimal = 1.3m,
            Enum = LogLevel.Error,
            SubModel = new()
        };
        return result;
    }
}
public class TestModel : TestSubModel
{
    public TestSubModel? SubModel { get; set; }
}
public class TestSubModel
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string String { get; set; } = string.Empty;
    public bool Bool { get; set; }
    public int Int { get; set; }
    public float Float { get; set; }
    public double Double { get; set; }
    public decimal Decimal { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.Now;
    public DateOnly DateOnly { get; set; } = DateTime.Now.ToDateOnly();
    public TimeOnly TimeOnly { get; set; } = DateTime.Now.ToTimeOnly();
    public LogLevel Enum { get; set; }
}