using Microsoft.Extensions.DependencyInjection;

namespace Materal.Extensions;

/// <summary>
/// IServiceCollection扩展方法类
/// 提供对依赖注入服务集合的扩展功能
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// 从服务集合中获取指定类型的单例实例
    /// 该方法用于在服务注册阶段获取已经注册为单例的服务实例
    /// </summary>
    /// <typeparam name="T">要获取实例的服务类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns>返回指定类型的单例实例，如果未找到或不是单例实例则返回默认值</returns>
    /// <remarks>
    /// 注意：此方法仅适用于通过ImplementationInstance注册的单例服务
    /// 对于通过ImplementationFactory或ImplementationType注册的服务将返回默认值
    /// </remarks>
    public static T? GetSingletonInstance<T>(this IServiceCollection services)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(m => m.ServiceType == typeof(T));
        if (serviceDescriptor is null || serviceDescriptor.ImplementationInstance is not T result) return default;
        return result;
    }
}