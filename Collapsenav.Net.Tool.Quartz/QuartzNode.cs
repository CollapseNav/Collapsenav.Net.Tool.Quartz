using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Collapsenav.Net.Tool.Ext;

public class QuartzNode
{
    private static IServiceCollection? Services;
    public static IScheduler? Scheduler;
    public static QuartzJobBuilder Builder = new();
    public static void SetService(IServiceCollection services)
    {
        Services = services;
    }
#if NETCOREAPP2_0_OR_GREATER
    [MemberNotNull("Scheduler")]
#endif
    public static async Task InitSchedulerAsync(IScheduler? scheduler = null)
    {
        Scheduler = scheduler ?? await new StdSchedulerFactory().GetScheduler();
    }

    public static void InitFactory(IJobFactory factory)
    {
        if (Scheduler != null && factory != null)
            Scheduler.JobFactory = factory;
    }
    public static async Task InitFromBuilderAsync(QuartzJobBuilder builder)
    {
        await InitSchedulerAsync();
        Builder = builder;
        await Builder.Build();
    }

    /// <summary>
    /// 使用 QuartzJobBuilder 构建任务
    /// </summary>
    public static async Task Build(QuartzJobBuilder? builder = null)
    {
        if (builder != null)
            Builder ??= builder;
        await Builder.Build();
    }

    public static Type GetJobType(string typeName)
    {
        // 当 service 为空时自动扫描程序集查找type
        if (Services == null)
        {
            var types = AppDomain.CurrentDomain.GetCustomerTypesByPrefix(typeName);
            if (types.IsEmpty())
                throw new Exception();
            return types.First();
        }
        // 当 service 非空时使用注册的type
        var type = Services.FirstOrDefault(item => item.ServiceType.Name == typeName || item.ServiceType.FullName == typeName)?.ServiceType;
        if (type == null)
            throw new Exception();
        return type;
    }
}