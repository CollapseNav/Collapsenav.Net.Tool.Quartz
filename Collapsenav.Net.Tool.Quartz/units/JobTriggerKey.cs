using Quartz;

namespace Collapsenav.Net.Tool.Ext;

public class JobTriggerKey
{
    public JobTriggerKey(JobKey jKey, TriggerKey tKey)
    {
        JKey = jKey;
        TKey = tKey;
    }
    public JobKey JKey { get; set; }
    public TriggerKey TKey { get; set; }
}