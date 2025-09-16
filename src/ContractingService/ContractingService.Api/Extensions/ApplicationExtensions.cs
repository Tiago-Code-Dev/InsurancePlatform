using Shared.CrossCutting.Middleware;

namespace ContractingService.Api.Extensions;

public static class ApplicationExtensions
{
    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env) =>

         app.UseSwaggerDocumentation(env)
           .UseHttpsRedirection()
           .UseJwtAuthenticationConfig()
           .UseMiddleware<ErrorHandlingMiddleware>();
}
