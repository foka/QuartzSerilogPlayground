using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using QuartzSerilogPlayground;
using Serilog;

var host = Host.CreateDefaultBuilder()
    .UseSerilog()
    .ConfigureServices((cxt, services) =>
    {
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        services.AddScoped<HelloService>();
    })
    .Build();
    
await ConfigureQuartz(host);
ConfigureSerilog(host);

await host.RunAsync();

static async Task ConfigureQuartz(IHost host)
{
    var schedulerFactory = host.Services.GetRequiredService<ISchedulerFactory>();
    
    var scheduler = await schedulerFactory.GetScheduler();

    var job = JobBuilder.Create<HelloJob>()
        .WithIdentity("myJob", "group1")
        .Build();

    var trigger = TriggerBuilder.Create()
        .WithIdentity("myTrigger", "group1")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(3)
            .RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);
}

static void ConfigureSerilog(IHost host)
{
    var loggerConfiguration = new LoggerConfiguration()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}")
    ;

    Log.Logger = loggerConfiguration.CreateLogger();
}
