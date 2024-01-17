using Quartz;

namespace Collapsenav.Net.Tool.Ext;

public abstract class JobItem
{
    public JobItem(Type jobtype)
    {
        JobType = jobtype;
    }
    public Type JobType { get; set; }
    public JobKey JKey { get => jKey ?? new JobKey(JobType.Name, JobType.Name); set => jKey = value; }
    private JobKey? jKey;
    public TriggerKey TKey { get => tKey ?? new TriggerKey(JobType.Name, JobType.Name); set => tKey = value; }
    private TriggerKey? tKey;
    public abstract ITrigger GetTrigger();
    public virtual IJobDetail GetJobDetail() => QuartzTool.CreateJob(JobType, JKey);
}

/// <summary>
/// 使用cron的job
/// </summary>
public class CronJob : JobItem
{
    public CronJob(Type jobtype, string cron) : base(jobtype)
    {
        Cron = cron;
    }
    public string Cron { get; set; }
    public override ITrigger GetTrigger() => QuartzTool.CreateTrigger(Cron, TKey);
}
/// <summary>
/// 使用len的job
/// </summary>
public class SimpleJob : JobItem
{
    public SimpleJob(Type jobtype, int len) : base(jobtype)
    {
        Len = len;
    }
    public int Len { get; set; }
    public override ITrigger GetTrigger() => QuartzTool.CreateTrigger(Len, TKey);
}
