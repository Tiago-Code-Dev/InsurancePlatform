using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shared.CrossCutting.Logging;

public static class LoggingExtensions
{
   
    public static IHostBuilder UseAppSerilog(this IHostBuilder host)
    {
        return host.UseSerilog((context, services, cfg) =>
        {
            cfg.ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext()
              .Enrich.WithMachineName()
              .Enrich.WithThreadId()
              .Enrich.WithProperty("ServiceName", context.HostingEnvironment.ApplicationName);
        });
    }

    public static WebApplication UseAppRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(opts =>
        {
            // Template amigável; não loga corpo (mantém performance)
            opts.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            opts.EnrichDiagnosticContext = (ctx, http) =>
            {
                ctx.Set("ClientIP", http.Connection.RemoteIpAddress?.ToString());
                ctx.Set("UserAgent", http.Request.Headers.UserAgent.ToString());
                ctx.Set("RequestId", http.TraceIdentifier);
                ctx.Set("CorrelationId", http.Items.TryGetValue("CorrelationId", out var cid) ? cid : null);
            };
        });
        return app;
    }
}
