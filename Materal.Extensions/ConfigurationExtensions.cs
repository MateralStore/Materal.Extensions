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
    public static T? Get<T>(this IConfiguration configuration, string key)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (typeof(T) == typeof(string))
        {
            string? value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                value = GetConfigItemToString(configuration, key);
            }
            return (T?)(object?)value;
        }
        else
        {
            IConfigurationSection configSection = configuration.GetSection(key);
            T? result = configSection.Get<T>();
            return result;
        }
    }

    /// <summary>
    /// 从配置节获取动态对象
    /// </summary>
    /// <param name="configSection">配置节</param>
    /// <returns>动态对象,如果配置节没有子项则返回null</returns>
    /// <exception cref="ArgumentNullException">当configSection为null时抛出</exception>
    private static object? GetConfigItem(this IConfigurationSection configSection)
    {
        if (configSection is null) throw new ArgumentNullException(nameof(configSection));
        IConfigurationSection[] sectionItems = [.. configSection.GetChildren()];
        if (sectionItems.Length == 0) return null;
        Dictionary<string, object?> propertyDic = [];
        List<object?> objects = [];
        bool isArray = sectionItems.First().Key == "0";
        foreach (IConfigurationSection sectionItem in sectionItems)
        {
            if (!string.IsNullOrWhiteSpace(sectionItem.Value))
            {
                if (!isArray)
                {
                    propertyDic.Add(sectionItem.Key, sectionItem.Value);
                }
                else
                {
                    objects.Add(sectionItem.Value);
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
    /// 根据键获取配置项的字符串值
    /// </summary>
    /// <param name="configuration">配置对象</param>
    /// <param name="key">配置项键</param>
    /// <returns>配置项的字符串值，如果不存在则返回null</returns>
    /// <exception cref="ArgumentNullException">当configuration或key为null时抛出</exception>
    private static string? GetConfigItemToString(this IConfiguration configuration, string key)
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
    private static string? GetConfigItemToString(this IConfigurationSection configSection)
    {
        if (configSection is null) throw new ArgumentNullException(nameof(configSection));

        object? value = configSection.GetConfigItem();
        return value?.ToJson();
    }
}
