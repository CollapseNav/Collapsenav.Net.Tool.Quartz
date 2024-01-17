using Quartz;

namespace Collapsenav.Net.Tool.Ext;

/// <summary>
/// Quartz json格式的配置
/// </summary>
public interface IQuartzJsonConfig
{
    JobItem ToJobItem();
    bool CanUse();
}

public class QuartzConfigNode : IQuartzJsonConfig
{
    public QuartzConfigNode(string jobName, int len)
    {
        JobName = jobName;
        Len = len;
    }
    public QuartzConfigNode(string jobName, string cron)
    {
        JobName = jobName;
        Cron = cron;
    }

    /// <summary>
    /// Job 名称
    /// </summary>
    public string JobName { get; set; }
    /// <summary>
    /// Cron 表达式
    /// </summary>
    public string? Cron { get; set; }
    /// <summary>
    /// 间隔时间
    /// </summary>
    public int? Len { get; set; }

    /// <summary>
    /// 通过字典生成配置
    /// </summary>
    public static IEnumerable<QuartzConfigNode> ConvertFromKeyValue(IDictionary<string, string> dict)
    {
        return dict.Select(item => ConvertFromKeyValue(item.Key, item.Value));
    }
    /// <summary>
    /// 通过key,value生成配置
    /// </summary>
    public static QuartzConfigNode ConvertFromKeyValue(KeyValuePair<string, string> kv)
    {
        return ConvertFromKeyValue(kv.Key, kv.Value);
    }
    /// <summary>
    /// 通过key,value生成配置
    /// </summary>
    public static QuartzConfigNode ConvertFromKeyValue(string key, string value)
    {
        if (int.TryParse(value, out int len))
            return new QuartzConfigNode(key, len);
        return new QuartzConfigNode(key, value);
    }

    /// <summary>
    /// 判断可用
    /// </summary>
    public bool CanUse()
    {
        if (JobName.IsEmpty())
            return false;
        if (Cron.IsEmpty() && Len == null)
            return false;
        return true;
    }

    /// <summary>
    /// 将config转为jobitem
    /// </summary>
    public JobItem ToJobItem()
    {
        var type = QuartzNode.GetJobType(JobName);
        if (type == null)
            return null;
        if (Cron.NotEmpty())
            return new CronJob(type, Cron);
        if (Len.HasValue)
            return new SimpleJob(type, Len.Value);
        return null;
    }
}