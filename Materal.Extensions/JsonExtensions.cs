using Materal.Extensions.JsonConverters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Materal.Extensions;

/// <summary>
/// Json扩展
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Json序列化配置
    /// </summary>
    private static readonly JsonSerializerOptions _jsonSerializerOptions = GetDefaultJsonSerializerOptions();

    /// <summary>
    /// 获得默认的Json序列化配置
    /// </summary>
    /// <returns></returns>
    private static JsonSerializerOptions GetDefaultJsonSerializerOptions() => new()
    {
        Converters = { new JsonStringEnumConverter() },//自定义转换器
        PropertyNameCaseInsensitive = true,// 忽略大小写
        WriteIndented = true,// 格式化输出
    };

    /// <summary>
    /// Json字符串转换对象
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns>转换后的对象</returns>
    public static object JsonToObject(this string jsonStr, Type type, JsonSerializerOptions? options = null)
    {
        try
        {
            object? model = JsonSerializer.Deserialize(jsonStr, type, options ?? _jsonSerializerOptions) ?? throw new ExtensionException("转换失败");
            return model;
        }
        catch (Exception ex)
        {
            throw new ExtensionException("Json字符串有误", ex);
        }
    }

    /// <summary>
    /// Json字符串转换对象
    /// </summary>
    /// <typeparam name="T">目标对象类型</typeparam>
    /// <param name="jsonStr">Json字符串</param>
    /// <param name="options"></param>
    /// <returns>转换后的对象</returns>
    public static T JsonToObject<T>(this string jsonStr, JsonSerializerOptions? options = null)
    {
        try
        {
            T? model = JsonSerializer.Deserialize<T>(jsonStr, options ?? _jsonSerializerOptions) ?? throw new ExtensionException("转换失败");
            return model;
        }
        catch (Exception ex)
        {
            throw new ExtensionException("Json字符串有误", ex);
        }
    }

    /// <summary>
    /// Json字符串转换对象
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <param name="options"></param>
    /// <returns>转换后的对象</returns>
    public static object JsonToObject(this string jsonStr, JsonSerializerOptions? options = null) => JsonToObject<object>(jsonStr, options);

    /// <summary>
    /// 对象转换为Json
    /// </summary>
    /// <param name="obj">要转换的对象</param>
    /// <param name="options"></param>
    /// <returns>转换后的Json字符串</returns>
    public static string ToJson(this object obj, JsonSerializerOptions? options = null) => JsonSerializer.Serialize(obj, options ?? _jsonSerializerOptions);

    /// <summary>
    /// 对象转换为Json，保留类型信息以便还原
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string ToJsonWithInferredTypes(this object obj, JsonSerializerOptions? options = null)
    {
        JsonSerializerOptions serializerOptions = options ?? GetDefaultJsonSerializerOptions();
        serializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        return JsonSerializer.Serialize(obj, serializerOptions);
    }

    /// <summary>
    /// Json字符串转换对象，自动还原类型信息
    /// </summary>
    /// <param name="jsonStr"></param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="ExtensionException"></exception>
    public static object JsonToObjectWithInferredTypes(this string jsonStr, Type type, JsonSerializerOptions? options = null)
    {
        try
        {
            JsonSerializerOptions serializerOptions = options ?? GetDefaultJsonSerializerOptions();
            serializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
            object model = JsonSerializer.Deserialize(jsonStr, type, serializerOptions) ?? throw new ExtensionException("转换失败");
            return model;
        }
        catch (Exception ex)
        {
            throw new ExtensionException("Json字符串有误", ex);
        }
    }

    /// <summary>
    /// Json字符串转换对象，自动还原类型信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonStr"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="ExtensionException"></exception>
    public static T JsonToObjectWithInferredTypes<T>(this string jsonStr, JsonSerializerOptions? options = null)
    {
        try
        {
            JsonSerializerOptions serializerOptions = options ?? GetDefaultJsonSerializerOptions();
            serializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
            T? model = JsonSerializer.Deserialize<T>(jsonStr, serializerOptions) ?? throw new ExtensionException("转换失败");
            return model;
        }
        catch (Exception ex)
        {
            throw new ExtensionException("Json字符串有误", ex);
        }
    }

    /// <summary>
    /// Json字符串转换对象，自动还原类型信息
    /// </summary>
    /// <param name="jsonStr"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object JsonToObjectWithInferredTypes(this string jsonStr, JsonSerializerOptions? options = null) => JsonToObjectWithInferredTypes<object>(jsonStr, options);
}