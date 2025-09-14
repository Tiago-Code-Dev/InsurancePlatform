using Shared.CrossCutting.Middleware;

namespace ContractingService.Api.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwaggerDocumentation(env);

        if (!env.IsProduction()) // <--- Apenas em produção
            app.UseHttpsRedirection(); // Evita forçar HTTPS no Docker em dev

        app.UseJwtAuthenticationConfig();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        return app;
    }
}
