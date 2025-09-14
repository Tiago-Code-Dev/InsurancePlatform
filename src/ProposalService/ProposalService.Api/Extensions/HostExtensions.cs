using Serilog;

namespace ProposalService.Api.Extensions;

public static class HostExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder host, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
                  .ReadFrom.Configuration(configuration)
                  .Enrich.FromLogContext()
                  .Enrich.WithMachineName()
                  .Enrich.WithThreadId()
                  .Enrich.WithProcessId()
                  .WriteTo.Console()
                  .WriteTo.File("logs/proposal-log-.txt", rollingInterval: RollingInterval.Day)
                  .WriteTo.Seq("http://localhost:5341") // opcional se usar Seq
                  .CreateLogger();

        return host.UseSerilog();
    }
}
