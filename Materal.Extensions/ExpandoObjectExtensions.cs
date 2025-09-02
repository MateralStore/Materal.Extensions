using System.Data;
using System.Dynamic;
using System.Text.Json;
using System.Xml;

namespace Materal.Extensions;

/// <summary>
/// 动态对象扩展类
/// 提供将各种对象类型转换为ExpandoObject的扩展方法
/// </summary>
public static class ExpandoObjectExtensions
{
    /// <summary>
    /// 将对象转换为动态对象(ExpandoObject)
    /// </summary>
    /// <param name="obj">要转换的对象，可以为null</param>
    /// <returns>转换后的动态对象，如果输入为null则返回null</returns>
    /// <remarks>
    /// 支持转换的类型包括：
    /// - 字符串
    /// - JsonElement
    /// - IEnumerable集合
    /// - DataTable
    /// - 普通类对象
    /// 
    /// 对于标记了 <see cref="IgnoreToExpandoObjectAttribute"/> 特性的对象或属性将被忽略
    /// </remarks>
    public static object? ToExpandoObject(this object? obj)
    {
        if (obj is null) return obj;
        Type objType = obj.GetType();
        if (objType.GetCustomAttribute<IgnoreToExpandoObjectAttribute>() is not null) return obj;
        if (obj is string stringValue) return stringValue.ToExpandoObject();
        if (obj is JsonElement jsonElement) return jsonElement.ToExpandoObject();
        if (obj is IEnumerable enumerable) return enumerable.ToExpandoObject();
        if (obj is DataTable dataTable) return dataTable.ToExpandoObject();
        if (!objType.IsClass) return obj;
        ExpandoObject result = new();
        foreach (PropertyInfo propertyInfo in objType.GetProperties())
        {
            if (propertyInfo.GetCustomAttribute<IgnoreToExpandoObjectAttribute>() is not null) continue;
            if (!propertyInfo.CanRead) continue;
            object? value = propertyInfo.GetValue(obj);
            value = value?.ToExpandoObject();
            result.TryAdd(propertyInfo.Name, value);
        }
        return result;
    }
    /// <summary>
    /// 将字符串转换为动态对象
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <returns>原字符串值</returns>
    public static string ToExpandoObject(this string stringValue) => stringValue;
    /// <summary>
    /// 将XML文档转换为动态对象
    /// </summary>
    /// <param name="document">要转换的XML文档</param>
    /// <returns>转换后的动态对象</returns>
    /// <exception cref="ArgumentNullException">当 document 为 null 时抛出</exception>
    public static object? ToExpandoObject(this XmlDocument document) => document.ChildNodes.ToExpandoObject();
    /// <summary>
    /// 将XML节点列表转换为动态对象
    /// </summary>
    /// <param name="xmlNodes">要转换的XML节点列表</param>
    /// <returns>转换后的动态对象或对象列表</returns>
    /// <remarks>
    /// 如果存在同名节点，则转换为列表；否则转换为动态对象
    /// </remarks>
    /// <exception cref="ArgumentNullException">当 xmlNodes 为 null 时抛出</exception>
    public static object? ToExpandoObject(this XmlNodeList xmlNodes)
    {
        if (xmlNodes is null) throw new ArgumentNullException(nameof(xmlNodes));
            
        HashSet<string> names = [];
        bool isArray = false;
        foreach (XmlNode xmlNode in xmlNodes)
        {
            if (!names.Add(xmlNode.Name)) // 如果添加失败，说明已存在
            {
                isArray = true;
                break;
            }
        }
        if (!isArray)
        {
            ExpandoObject result = new();
            foreach (XmlNode xmlNode in xmlNodes)
            {
                result.TryAdd(xmlNode.Name, xmlNode.ToExpandoObject());
            }
            return result;
        }
        else
        {
            List<object?> result = [];
            foreach (XmlNode xmlNode in xmlNodes)
            {
                result.Add(xmlNode.ToExpandoObject());
            }
            return result;
        }
    }
    /// <summary>
    /// 将XML节点转换为动态对象
    /// </summary>
    /// <param name="xmlNode">要转换的XML节点</param>
    /// <returns>转换后的动态对象</returns>
    /// <exception cref="ArgumentNullException">当 xmlNode 为 null 时抛出</exception>
    /// <remarks>
    /// 转换规则：
    /// - 如果有子节点且只有一个文本节点，则返回文本值
    /// - 如果有子节点且不只一个，则递归转换子节点
    /// - 如果没有子节点，则返回节点值
    /// </remarks>
    public static object? ToExpandoObject(this XmlNode xmlNode)
    {
        if (xmlNode is null) throw new ArgumentNullException(nameof(xmlNode));
        if (xmlNode.ChildNodes.Count > 0)
        {
            if (xmlNode.ChildNodes.Count == 1 && xmlNode.FirstChild is XmlText xmlText) return xmlText.Value?.ToExpandoObject();
            object? value = xmlNode.ChildNodes.ToExpandoObject();
            return value;
        }
        else
        {
            return xmlNode.Value?.ToExpandoObject();
        }
    }
    /// <summary>
    /// 将JsonElement转换为动态对象
    /// </summary>
    /// <param name="jsonElement">要转换的JsonElement</param>
    /// <returns>转换后的动态对象</returns>
    /// <remarks>
    /// 根据JsonElement的值类型进行转换：
    /// - 字符串：转换为字符串
    /// - 数字：转换为decimal
    /// - 布尔值：转换为bool
    /// - Undefined/Null：返回null
    /// - 其他：转换为字符串后再转换为动态对象
    /// </remarks>
    public static object? ToExpandoObject(this JsonElement jsonElement) => jsonElement.ValueKind switch
    {
        JsonValueKind.String => jsonElement.GetString()?.ToExpandoObject(),
        JsonValueKind.Number => jsonElement.GetDecimal(),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Undefined => null,
        JsonValueKind.Null => null,
        _ => jsonElement.ToString().ToExpandoObject(),
    };
    /// <summary>
    /// 将动态对象转换为动态对象（直接返回）
    /// </summary>
    /// <param name="expandoObject">要转换的动态对象</param>
    /// <returns>原动态对象</returns>
    public static object? ToExpandoObject(this ExpandoObject expandoObject) => expandoObject;
    /// <summary>
    /// 将可枚举对象转换为动态对象列表
    /// </summary>
    /// <param name="enumerable">要转换的可枚举对象</param>
    /// <returns>转换后的动态对象列表</returns>
    /// <remarks>
    /// 转换规则：
    /// - 如果为null，返回null
    /// - 如果是ExpandoObject，直接返回
    /// - 如果是IDictionary，转换为动态对象
    /// - 如果是长度为16的字节数组，尝试转换为Guid（Guid标准格式为16字节）
    /// - 其他情况，转换为对象列表
    /// </remarks>
    public static object? ToExpandoObject(this IEnumerable enumerable)
    {
        if (enumerable is null) return null;
        if (enumerable is ExpandoObject expandoObject) return expandoObject.ToExpandoObject();
        if (enumerable is IDictionary dictionary) return dictionary.ToExpandoObject();
        // Guid标准格式为16字节，只处理长度为16的字节数组
        if (enumerable is byte[] bytes && bytes.Length == 16)
        {
            try
            {
                return new Guid(bytes);
            }
            catch { } // 如果转换失败，继续执行默认逻辑
        }
        List<object?> result = [];
        foreach (object item in enumerable)
        {
            if (item is null) continue;
            result.Add(item.ToExpandoObject());
        }
        return result;
    }
    /// <summary>
    /// 将字典对象转换为动态对象
    /// </summary>
    /// <param name="dictionary">要转换的字典对象</param>
    /// <returns>转换后的动态对象</returns>
    /// <remarks>
    /// 遍历字典中的所有键值对，将键转换为字符串，值转换为动态对象
    /// </remarks>
    public static object? ToExpandoObject(this IDictionary dictionary)
    {
        if (dictionary is null) return null;
        ExpandoObject result = new();
        foreach (object? item in dictionary)
        {
            if (item is null || item is not DictionaryEntry dictionaryEntry) continue;
            object keyObj = dictionaryEntry.Key;
            string? key = keyObj is string keyValue ? keyValue : keyObj.ToString();
            if (key is null) continue;
            result.TryAdd(key, dictionaryEntry.Value.ToExpandoObject());
        }
        return result;
    }
    /// <summary>
    /// 将数据表转换为动态对象列表
    /// </summary>
    /// <param name="dataTable">要转换的数据表</param>
    /// <returns>转换后的动态对象列表</returns>
    /// <exception cref="ArgumentNullException">当 dataTable 为 null 时抛出</exception>
    public static object? ToExpandoObject(this DataTable dataTable)
    {
        if (dataTable is null) throw new ArgumentNullException(nameof(dataTable));
            
        List<ExpandoObject> list = [];
        foreach (DataRow row in dataTable.Rows)
        {
            ExpandoObject value = row.ToExpandoObject(dataTable.Columns);
            list.Add(value);
        }
        return list;
    }
    /// <summary>
    /// 将数据行转换为动态对象
    /// </summary>
    /// <param name="row">要转换的数据行</param>
    /// <param name="dataColumns">数据列集合</param>
    /// <returns>转换后的动态对象</returns>
    /// <exception cref="ArgumentNullException">当 row 或 dataColumns 为 null 时抛出</exception>
    public static ExpandoObject ToExpandoObject(this DataRow row, DataColumnCollection dataColumns)
    {
        if (row is null) throw new ArgumentNullException(nameof(row));
        if (dataColumns is null) throw new ArgumentNullException(nameof(dataColumns));
            
        ExpandoObject result = new();
        for (int i = 0; i < dataColumns.Count; i++)
        {
            object value = row[i];
            result.TryAdd(dataColumns[i].ColumnName, value.ToExpandoObject());
        }
        return result;
    }
#if NETSTANDARD
    /// <summary>
    /// 尝试向动态对象添加键值对
    /// </summary>
    /// <param name="expandoObject">要添加键值对的动态对象</param>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns>如果添加成功返回true，如果键已存在返回false</returns>
    /// <exception cref="ArgumentNullException">当 expandoObject 或 key 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 expandoObject 无法转换为 IDictionary&lt;string, object?&gt; 时抛出</exception>
    public static bool TryAdd(this ExpandoObject expandoObject, string key, object? value)
    {
        if (expandoObject is null) throw new ArgumentNullException(nameof(expandoObject));
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (expandoObject is not IDictionary<string, object?> expandoDictionary) throw new ArgumentException("无法将expandoObject转换为IDictionary<string, object?>", nameof(expandoObject));
        if (expandoDictionary.ContainsKey(key)) return false;
        expandoDictionary.Add(key, value);
        return true;
    }
#endif
}
