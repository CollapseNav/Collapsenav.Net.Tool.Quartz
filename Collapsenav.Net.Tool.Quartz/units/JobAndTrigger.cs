using Quartz;

namespace Collapsenav.Net.Tool.Ext;

public class JobAndTrigger
{
    public JobAndTrigger(IJobDetail job, ITrigger trigger)
    {
        Job = job;
        Trigger = trigger;
    }
    public IJobDetail Job { get; set; }
    public ITrigger Trigger { get; set; }
}