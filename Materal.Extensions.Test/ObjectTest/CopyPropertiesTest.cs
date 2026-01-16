using Materal.Utils.Test.AutoMapperTest;

namespace Materal.Extensions.Test.ObjectTest;

[TestClass]
public class CopyPropertiesTest : MateralTestBase
{
    /// <summary>
    /// 复制到同类型测试
    /// </summary>
    [TestMethod]
    public void CopySameTypeTest()
    {
        ModelA source = new()
        {
            Name = "测试",
            Age = 18,
            Sub = new()
            {
                Age = 18
            },
            Subs = [
                new(){ Age = 18 },
                new(){ Age = 19 },
                new(){ Age = 20 }
            ]
        };
        ModelA? result = source.CopyProperties<ModelA>();
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Name, source.Name);
        Assert.AreEqual(result.Sub, source.Sub);
        Assert.AreEqual(result.Subs, source.Subs);
    }
    /// <summary>
    /// 复制到同类型测试
    /// </summary>
    [TestMethod]
    public void CopyDifferentTypeTest()
    {
        ModelA source = new()
        {
            Name = "测试",
            Sub = new()
            {
                Age = 18
            },
            Subs = [
                new(){ Age = 18 },
                new(){ Age = 19 },
                new(){ Age = 20 }
            ]
        };
        ModelB? result = source.CopyProperties<ModelB>();
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Name, source.Name);
        Assert.AreEqual(result.Sub, source.Sub);
        Assert.AreEqual(result.Subs, source.Subs);
    }
}
