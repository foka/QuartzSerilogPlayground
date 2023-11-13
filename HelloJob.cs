using Quartz;
using Serilog;
using Serilog.Context;

namespace QuartzSerilogPlayground;

public class HelloJob : IJob
{
    private readonly ILogger logger = Log.ForContext<HelloJob>();
    private readonly HelloService helloService;

    public HelloJob(HelloService helloService)
    {
        this.helloService = helloService;
    }
    
    public Task Execute(IJobExecutionContext jobContext)
    {
        using var _ = LogContext.PushProperty("JobFireInstanceId", jobContext.FireInstanceId);
        
        logger.Information($"HelloJob executing");
        
        return helloService.DoStuff(jobContext.CancellationToken);
    }
}