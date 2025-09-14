using Serilog;

namespace ContractingService.Api.Extensions;

public static class HostExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder host, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        return host.UseSerilog();
    }
}
