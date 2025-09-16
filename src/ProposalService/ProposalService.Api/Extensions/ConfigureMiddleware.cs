using ProposalService.Api.Extensions;
using Serilog;
using Shared.CrossCutting.Middleware;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwaggerDocumentation(env);
        }

        app.UseCorrelationId();              
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseJwtAuthenticationConfig();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        return app;
    }

    public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapControllers();
        app.MapHealthChecks("/health");
        return app;
    }
}
