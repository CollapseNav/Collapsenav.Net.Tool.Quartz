using Quartz;

namespace Collapsenav.Net.Tool.Ext;

public class TriggerConfig
{
    private int repeatTimes;
    public object Obj { get; private set; }
    public TriggerKey TKey { get; private set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? Start { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? End { get; set; }
    /// <summary>
    /// 重复次数
    /// </summary>
    /// <remarks>只有当 obj 是int时可用</remarks>
    public int RepeatTimes
    {
        get => repeatTimes; set
        {
            repeatTimes = Obj is string ? throw new Exception("obj need int") : value;
        }
    }
    public TriggerConfig()
    {
        // 默认每秒执行一次
        Obj = 1000;
        // 随机赋值名称
        TKey = new TriggerKey(SnowFlake.NextId().ToString(), SnowFlake.NextId().ToString());
    }
    public TriggerConfig(object obj, TriggerKey tKey)
    {
        if (obj is not (string or int or DateTime))
            throw new ArgumentException("obj need int or string or datetime");
        Obj = obj;
        TKey = tKey;
    }

    public ITrigger InitTriggerBuilder(TriggerBuilder? builder = null)
    {
        builder ??= TriggerBuilder.Create();

        builder.WithIdentity(TKey);
        if (Start.HasValue)
            builder.StartAt(Start.Value);
        else
            builder.StartNow();
        if (End.HasValue)
            builder.EndAt(End.Value);

        if (Obj is string cron)
            builder.WithCronSchedule(cron);
        else if (Obj is int len)
            builder.WithSimpleSchedule(b => b.WithIntervalInSeconds(len).RepeatForever());
        return builder.Build();
    }
}