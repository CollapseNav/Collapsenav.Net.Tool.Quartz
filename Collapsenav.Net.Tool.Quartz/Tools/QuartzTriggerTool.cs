using Quartz;

namespace Collapsenav.Net.Tool.Ext;

public static partial class QuartzTool
{
    public static ITrigger CreateTrigger(TriggerConfig config)
    {
        return config.InitTriggerBuilder();
    }
    public static ITrigger CreateTrigger(object obj, TriggerKey tkey, DateTime? start = null, DateTime? end = null)
    {
        return CreateTrigger(new TriggerConfig(obj, tkey)
        {
            Start = start,
            End = end,
        });
    }
    /// <summary>
    /// 获取一个trigger
    /// </summary>
    public static ITrigger CreateTrigger(object cron, string name, string? group = null) => CreateTrigger(cron, new TriggerKey(name, group ?? name));
    /// <summary>
    /// 使用type的name作为triggerkey创建trigger
    /// </summary>
    public static ITrigger CreateTrigger(object cron, Type type) => CreateTrigger(cron, type.Name);
    /// <summary>
    /// 使用泛型类型的name作为triggerkey创建trigger
    /// </summary>
    public static ITrigger CreateTrigger<Job>(object cron) where Job : IJob => CreateTrigger(cron, typeof(Job));
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    /// <remarks>
    /// <para>同时生成多个 triggerkey</para>
    /// <para>如: 传入 name,group</para>
    /// <para>则会生成 name_1.group, name_2.group, name_3.group ... 这样的key</para>
    /// </remarks>
    public static IEnumerable<ITrigger> CreateTriggers(IEnumerable<int> crons, string name, string? group = null)
        => crons.Select((item, index) => CreateTrigger(item, new TriggerKey($"{name}_{index}", $"{group ?? name}"))).ToList();
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    /// <remarks>
    /// <para>同时生成多个 triggerkey</para>
    /// <para>如: 传入 name,group</para>
    /// <para>则会生成 name_1.group, name_2.group, name_3.group ... 这样的key</para>
    /// </remarks>
    public static IEnumerable<ITrigger> CreateTriggers(IEnumerable<object> crons, string name, string? group = null)
        => crons.Select((item, index) => CreateTrigger(item, new TriggerKey($"{name}_{index}", $"{group.IsEmpty(name)}"))).ToList();
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    public static IEnumerable<ITrigger> CreateTriggers(Type type, params int[] crons) => CreateTriggers(crons, type.Name);
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    public static IEnumerable<ITrigger> CreateTriggers(Type type, params object[] crons) => CreateTriggers(crons, type.Name);
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    public static IEnumerable<ITrigger> CreateTriggers<Job>(params object[] crons) where Job : IJob => CreateTriggers(typeof(Job), crons);
    /// <summary>
    /// 根据cron的数量生成多个trigger
    /// </summary>
    public static IEnumerable<ITrigger> CreateTriggers<Job>(params int[] crons) where Job : IJob => CreateTriggers(typeof(Job), crons);
    public static IEnumerable<TriggerKey> CreateTriggerKeys(int count, string name, string? group = null)
    {
        if (count <= 0)
            return Enumerable.Empty<TriggerKey>();
        return Enumerable.Range(0, count).Select(item => new TriggerKey($"{name}_{item}", $"{group ?? name}"));
    }
    public static IEnumerable<TriggerKey> CreateTriggerKeys(int count, Type type, string? group = null) => CreateTriggerKeys(count, type.Name, group);
    public static IEnumerable<TriggerKey> CreateTriggerKeys<Job>(int count, string? group = null) where Job : IJob => CreateTriggerKeys(count, typeof(Job).Name, group);
}