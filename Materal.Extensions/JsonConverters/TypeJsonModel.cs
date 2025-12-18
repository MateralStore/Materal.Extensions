using System.Text.Json.Serialization;

namespace Materal.Extensions.JsonConverters;

/// <summary>
/// Type转换为Json的模型
/// </summary>
public class TypeJsonModel
{
    /// <summary>
    /// 类型描述
    /// </summary>
    public string TypeDescription
    {
        get => field;
        set
        {
            field = value;
            Type? type = Type.GetType(value);
            if (Type == type) return;
            Type = type;
        }
    } = string.Empty;

    /// <summary>
    /// 类型
    /// </summary>
    [JsonIgnore]
    public Type? Type
    {
        get => field;
        set
        {
            field = value;
            string typeDescription = value is null ? string.Empty : $"{value.FullName}, {value.Assembly.GetName().Name}";
            if (TypeDescription == typeDescription) return;
            TypeDescription = typeDescription;
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    public TypeJsonModel()
    {

    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="type"></param>
    public TypeJsonModel(Type type) : this()
    {
        Type = type;
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="objectType"></param>
    public TypeJsonModel(object objectType) : this(objectType.GetType())
    {

    }
}
