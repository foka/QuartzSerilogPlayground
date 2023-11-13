using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using Serilog.Context;

namespace QuartzSerilogPlayground;

public class HelloJob : IJob
{
    private readonly ILogger logger = Log.ForContext<HelloJob>();
    private readonly IServiceProvider serviceProvider;

    public HelloJob(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public Task Execute(IJobExecutionContext jobContext)
    {
        using var _ = LogContext.PushProperty("JobFireInstanceId", jobContext.FireInstanceId);

        logger.Information($"HelloJob executing");

        using var scope = serviceProvider.CreateScope();
        var helloService = scope.ServiceProvider.GetRequiredService<HelloService>();
        return helloService.DoStuff(jobContext.CancellationToken);
    }
}