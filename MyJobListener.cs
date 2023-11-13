using Quartz;
using Quartz.Listener;
using Serilog.Context;

namespace QuartzSerilogPlayground;

public class MyJobListener : JobListenerSupport
{
    public override string Name => nameof(MyJobListener);

    public override async Task JobToBeExecuted(IJobExecutionContext jobContext, CancellationToken cancellationToken = default)
    {
        await base.JobToBeExecuted(jobContext, cancellationToken);
        
        var fireInstanceIdProperty = LogContext.PushProperty("JobFireInstanceId", jobContext.FireInstanceId);
        jobContext.JobDetail.JobDataMap.Add("JobFireInstanceIdSerilogProperty", fireInstanceIdProperty);
    }
    
    public override async Task JobWasExecuted(IJobExecutionContext jobContext, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {        
        if (jobContext.MergedJobDataMap["JobFireInstanceIdSerilogProperty"] is IDisposable fireInstanceIdProperty)
        {
            fireInstanceIdProperty.Dispose();
        }
        
        await base.JobWasExecuted(jobContext, jobException, cancellationToken);
    }
}
