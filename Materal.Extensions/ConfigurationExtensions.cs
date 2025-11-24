namespace Materal.Extensions;

/// <summary>
/// 配置对象扩展
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// 根据键获取配置项的值
    /// </summary>
    /// <typeparam name="T">配置项类型</typeparam>
    /// <param name="configuration">配置对象</param>
    /// <param name="key">配置项键</param>
    /// <returns>配置项的值，如果不存在则返回默认值</returns>
    /// <exception cref="ArgumentNullException">当configuration或key为null时抛出</exception>
    public static T? GetConfigItem<T>(this IConfiguration configuration, string key)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (key is null) throw new ArgumentNullException(nameof(key));

        IConfigurationSection configSection = configuration.GetSection(key);
        return configSection.GetConfigItem<T>();
    }

    /// <summary>
    /// 从配置节获取指定类型的配置项值
    /// </summary>
    /// <typeparam name="T">配置项类型</typeparam>
    /// <param name="configSection">配置节</param>
    /// <returns>配置项的值，如果不存在则返回默认值</returns>
    /// <exception cref="ArgumentNullException">当configSection为null时抛出</exception>
    public static T? GetConfigItem<T>(this IConfigurationSection configSection)
    {
        if (configSection is null) throw new ArgumentNullException(nameof(configSection));

        string? value = configSection.GetConfigItemToString();
        if (string.IsNullOrEmpty(value)) return default;

        T? result = default;
        if (value is null) return result;
        Type tType = typeof(T);

        // 处理可空类型
        if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            tType = tType.GetGenericArguments()[0];
        }
        if (value.CanConvertTo(tType))
        {
            result = (T?)value.ConvertTo(tType);
        }
        else if (tType.IsEnum)
        {
            result = (T?)Enum.Parse(tType, value);
        }
        else if (value.IsJson())
        {
            result = value.JsonToObject<T>();
        }

        return result;
    }

    /// <summary>
    /// 从配置节获取动态对象
    /// </summary>
    /// <param name="configSection">配置节</param>
    /// <returns>动态对象,如果配置节没有子项则返回null</returns>
    /// <exception cref="ArgumentNullException">当configSection为null时抛出</exception>
    public static object? GetConfigItem(this IConfigurationSection configSection)
    {
        if (configSection is null) throw new ArgumentNullException(nameof(configSection));

        IConfigurationSection[] sectionItems = configSection.GetChildren().ToArray();
        if (sectionItems.Length == 0) return null;

        Dictionary<string, object?> propertyDic = [];
        List<object?> objects = [];
        bool isArray = sectionItems.First().Key == "0";

        foreach (IConfigurationSection sectionItem in sectionItems)
        {
            if (!string.IsNullOrWhiteSpace(sectionItem.Value))
            {
                // 尝试将字符串值转换为实际类型
                object? actualValue = ParseConfigValue(sectionItem.Value);

                if (!isArray)
                {
                    propertyDic.Add(sectionItem.Key, actualValue);
                }
                else
                {
                    objects.Add(actualValue);
                }
            }
            else
            {
                object? value = sectionItem.GetConfigItem();
                if (value is null) continue;
                if (!isArray)
                {
                    propertyDic.Add(sectionItem.Key, value);
                }
                else
                {
                    objects.Add(value);
                }
            }
        }

        return isArray ? objects : propertyDic;
    }

    /// <summary>
    /// 解析配置值为实际类型
    /// </summary>
    /// <param name="value">配置值字符串</param>
    /// <returns>解析后的实际类型值</returns>
    private static object? ParseConfigValue(string? value)
    {
        // 尝试解析为布尔值
        if (bool.TryParse(value, out bool boolValue))
        {
            return boolValue;
        }

        // 尝试解析为长整型
        if (long.TryParse(value, out long longValue))
        {
            return longValue;
        }

        // 尝试解析为双精度浮点数
        if (double.TryParse(value, out double doubleValue))
        {
            return doubleValue;
        }

        // 默认返回字符串
        return value;
    }

    /// <summary>
    /// 根据键获取配置项的字符串值
    /// </summary>
    /// <param name="configuration">配置对象</param>
    /// <param name="key">配置项键</param>
    /// <returns>配置项的字符串值，如果不存在则返回null</returns>
    /// <exception cref="ArgumentNullException">当configuration或key为null时抛出</exception>
    public static string? GetConfigItemToString(this IConfiguration configuration, string key)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (key is null) throw new ArgumentNullException(nameof(key));

        IConfigurationSection configSection = configuration.GetSection(key);
        if (!string.IsNullOrWhiteSpace(configSection.Value)) return configSection.Value;
        return configSection.GetConfigItemToString();
    }

    /// <summary>
    /// 从配置节获取对象的JSON字符串表示
    /// </summary>
    /// <param name="configSection">配置节</param>
    /// <returns>对象的JSON字符串表示，如果对象为null则返回null</returns>
    /// <exception cref="ArgumentNullException">当configSection为null时抛出</exception>
    public static string? GetConfigItemToString(this IConfigurationSection configSection)
    {
        if (configSection is null) throw new ArgumentNullException(nameof(configSection));

        object? value = configSection.GetConfigItem();
        return value?.ToJson();
    }
}
