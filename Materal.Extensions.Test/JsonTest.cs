using Materal.Extensions.JsonConverters;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
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

    [TestMethod]
    public void TestListToJson()
    {
        List<object> models = [];
        string emptyJson = models.ToJsonWithInferredTypes();
        List<object> emptyModels = emptyJson.JsonToObjectWithInferredTypes<List<object>>();
        Assert.IsEmpty(emptyModels);
        models.Add(new TestSubModel() { String = "1" });
        models.Add(new TestModel() { String = "2" });
        string json = models.ToJsonWithInferredTypes();
        List<object> result = json.JsonToObjectWithInferredTypes<List<object>>();
        Assert.HasCount(result.Count, models);
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i] is TestSubModel resultSubModel && models[i] is TestSubModel modelSubModel)
            {
                AreEqual(resultSubModel, modelSubModel);
            }
            else if (result[i] is TestModel resultModel && models[i] is TestModel model)
            {
                AreEqual(resultModel, model);
            }
            else
            {
                Assert.Fail("不相等");
            }
        }
    }

    [TestMethod]
    public void TestObservableCollectionToJson()
    {
        ObservableCollection<object> models = [];
        string emptyJson = models.ToJsonWithInferredTypes();
        ObservableCollection<object> emptyModels = emptyJson.JsonToObjectWithInferredTypes<ObservableCollection<object>>();
        Assert.IsEmpty(emptyModels);
        models.Add(new TestSubModel() { String = "1" });
        models.Add(new TestModel() { String = "2" });
        string json = models.ToJsonWithInferredTypes();
        ObservableCollection<object> result = json.JsonToObjectWithInferredTypes<ObservableCollection<object>>();
        Assert.HasCount(result.Count, models);
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i] is TestSubModel resultSubModel && models[i] is TestSubModel modelSubModel)
            {
                AreEqual(resultSubModel, modelSubModel);
            }
            else if (result[i] is TestModel resultModel && models[i] is TestModel model)
            {
                AreEqual(resultModel, model);
            }
            else
            {
                Assert.Fail("不相等");
            }
        }
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
            if (propertyInfo.PropertyType == typeof(TestSubModel)) continue;
            Assert.AreEqual(propertyInfo.GetValue(model2), propertyInfo.GetValue(model1));
        }
        AreEqual(model1.SubModel, model2.SubModel);
    }

    private static void AreEqual(object? model1, object? model2)
    {
        if (model1 is null && model2 is null) return;
        if (model1 is null || model2 is null)
        {
            Assert.Fail("不相等");
            return;
        }
        Type actualType = model1.GetType();
        foreach (PropertyInfo propertyInfo in actualType.GetProperties())
        {
            Assert.AreEqual(propertyInfo.GetValue(model2), propertyInfo.GetValue(model1));
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