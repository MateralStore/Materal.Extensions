using System.Text.Json;
using System.Text.Json.Serialization;

namespace Materal.Extensions.JsonConverters;

/// <summary>
/// 支持任意类型的 JSON 转换器，自动保留和还原类型信息
/// </summary>
internal class ObjectToInferredTypesConverter : JsonConverter<object>
{
    /// <inheritdoc/>
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
    {
        JsonTokenType.StartObject => ReadObject(ref reader, options),
        JsonTokenType.String => reader.GetString()!,
        JsonTokenType.Number => ReadNumber(ref reader),
        JsonTokenType.True or JsonTokenType.False => reader.GetBoolean(),
        JsonTokenType.Null => null!,
        JsonTokenType.StartArray => ReadArray(ref reader, options),
        _ => throw new JsonException($"不支持的TokenType: {reader.TokenType}"),
    };

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            Type type = value.GetType();
            // 对于基础类型，直接序列化
            if (type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateOnly)
                || type == typeof(TimeOnly))
            {
                JsonSerializer.Serialize(writer, value, type, options);
                return;
            }
            // 对于数组类型，直接序列化
            else if (type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                JsonSerializer.Serialize(writer, value, type, options);
                return;
            }
            else
            {
                // 对于复杂类型，添加类型信息
                writer.WriteStartObject();
                writer.WriteString("$type", $"{type.FullName}, {type.Assembly.GetName().Name}");
                // 序列化对象的所有属性
                string json = JsonSerializer.Serialize(value, type, options);
                using JsonDocument doc = JsonDocument.Parse(json);
                foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
                writer.WriteEndObject();
            }
        }
    }

    #region 私有方法

    /// <summary>
    /// 读取数字
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    private static object ReadNumber(ref Utf8JsonReader reader)
    {
        if (reader.TryGetInt32(out int intValue)) return intValue;
        else if (reader.TryGetInt64(out long longValue)) return longValue;
        else if (reader.TryGetDouble(out double doubleValue)) return doubleValue;
        else if (reader.TryGetDecimal(out decimal decimalValue)) return decimalValue;
        throw new JsonException($"获取数字失败: {reader.TokenType}");
    }

    /// <summary>
    /// 读取数组
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private static List<object> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        return JsonSerializer.Deserialize<List<object>>(doc.RootElement.GetRawText(), options)!;
    }

    /// <summary>
    /// 读取对象
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private static object ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement root = doc.RootElement;
        if (root.TryGetProperty("$type", out var typeElement))
        {
            string? typeName = typeElement.GetString();
            if (!string.IsNullOrEmpty(typeName))
            {
                Type? type = Type.GetType(typeName);
                if (type != null) return JsonSerializer.Deserialize(root.GetRawText(), type, options)!;
            }
        }
        return JsonSerializer.Deserialize<Dictionary<string, object>>(root.GetRawText(), options)!;
    }
    #endregion
}