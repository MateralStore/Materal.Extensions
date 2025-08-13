namespace Materal.Utils.Test.AutoMapperTest;

public class ModelA
{
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
    public SubModelA Sub { get; set; } = new();
    public List<SubModelA> Subs { get; set; } = [];
    public DateTime CreateTime { get; set; } = DateTime.Now;
}
public class ModelB
{
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
    public SubModelA Sub { get; set; } = new();
    public List<SubModelA> Subs { get; set; } = [];
    public DateTime CreateTime { get; set; } = DateTime.Now;
}
public class SubModelA
{
    public int Age { get; set; }
}