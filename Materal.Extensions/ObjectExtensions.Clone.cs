using System.Xml.Serialization;

namespace Materal.Extensions;

/// <summary>
/// Object扩展方法类
/// 提供对对象的克隆扩展功能，支持多种克隆方式
/// </summary>
public static partial class ObjectExtensions
{
    /// <summary>
    /// 通过JSON序列化克隆对象
    /// 将对象序列化为JSON字符串，然后再反序列化为新对象
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象</returns>
    /// <remarks>
    /// 适用于支持JSON序列化的对象
    /// </remarks>
    public static T CloneByJson<T>(this T inputObj)
        where T : notnull
    {
        string jsonStr = inputObj.ToJson();
        return jsonStr.JsonToObject<T>();
    }
    /// <summary>
    /// 通过XML序列化克隆对象
    /// 将对象序列化为XML格式，然后再反序列化为新对象
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象，如果克隆失败则返回默认值</returns>
    /// <remarks>
    /// 该方法使用XmlSerializer进行序列化和反序列化
    /// 适用于支持XML序列化的对象
    /// </remarks>
    public static T? CloneByXml<T>(this T inputObj)
        where T : notnull
    {
        object? result;
        using (MemoryStream ms = new())
        {
            XmlSerializer xml = new(typeof(T));
            xml.Serialize(ms, inputObj);
            ms.Seek(0, SeekOrigin.Begin);
            result = xml.Deserialize(ms);
            ms.Close();
        }
        return result is null ? default : (T)result;
    }
    /// <summary>
    /// 通过反射克隆对象
    /// 使用反射获取对象的所有属性，并逐个复制属性值到新对象
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象，如果克隆失败则返回默认值</returns>
    /// <remarks>
    /// 该方法使用反射机制获取和设置属性值
    /// 对于引用类型属性，会递归调用Clone方法进行深度克隆
    /// </remarks>
    public static T? CloneByReflex<T>(this T inputObj)
        where T : notnull
    {
        Type tType = inputObj.GetType();
        T resM = tType.Instantiation<T>();
        PropertyInfo[] propertyInfos = tType.GetProperties();
        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            object? value = propertyInfo.GetValue(inputObj);
            if (value is null) continue;
            propertyInfo.SetValue(resM, value is ValueType ? value : Clone(value));
        }
        return resM;
    }
#if NETSTANDARD
    /// <summary>
    /// 通过二进制序列化克隆对象
    /// 使用BinaryFormatter将对象序列化为二进制流，然后再反序列化为新对象
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象，如果克隆失败则返回默认值</returns>
    /// <exception cref="ExtensionException">当对象未标记为可序列化时抛出</exception>
    /// <remarks>
    /// 该方法要求对象必须标记[Serializable]特性
    /// 使用BinaryFormatter进行二进制序列化和反序列化
    /// </remarks>
    public static T? CloneBySerializable<T>(this T inputObj)
        where T : notnull
    {
        if (!inputObj.GetType().HasCustomAttribute<SerializableAttribute>()) throw new ExtensionException("未标识为可序列化");
        MemoryStream stream = new();
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new();
        formatter.Serialize(stream, inputObj);
        stream.Position = 0;
        object obj = formatter.Deserialize(stream);
        if (obj is T result) return result;
        return default;
    }
    /// <summary>
    /// 克隆对象
    /// 根据对象是否标记为可序列化自动选择克隆方式
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象，如果克隆失败则返回默认值</returns>
    /// <remarks>
    /// 该方法会检查对象是否标记了[Serializable]特性：
    /// - 如果标记了，则使用CloneBySerializable方法进行克隆
    /// - 如果未标记，则使用CloneByJson方法进行克隆
    /// </remarks>
    public static T? Clone<T>(this T inputObj)
        where T : notnull
    {
        SerializableAttribute? serializableAttribute = inputObj.GetType().GetCustomAttribute<SerializableAttribute>();
        return serializableAttribute is not null ? CloneBySerializable(inputObj) : CloneByJson(inputObj);
    }
#else
    /// <summary>
    /// 克隆对象
    /// 使用JSON序列化方式进行克隆
    /// </summary>
    /// <typeparam name="T">要克隆的对象类型，必须是非空类型</typeparam>
    /// <param name="inputObj">要克隆的输入对象</param>
    /// <returns>返回克隆后的新对象，如果克隆失败则返回默认值</returns>
    /// <remarks>
    /// 在非NETSTANDARD环境下，直接使用JSON序列化方式进行克隆
    /// </remarks>
    public static T? Clone<T>(this T inputObj)
        where T : notnull => CloneByJson(inputObj);
#endif
}
