using Serilog;

namespace QuartzSerilogPlayground;

public class HelloService
{
    private readonly ILogger logger = Log.ForContext<HelloService>();
    
    public Task DoStuff(CancellationToken cancellationToken = default)
    {
        logger.Information("HelloService is doing stuff");
        
        return Task.CompletedTask;
    }    
}
