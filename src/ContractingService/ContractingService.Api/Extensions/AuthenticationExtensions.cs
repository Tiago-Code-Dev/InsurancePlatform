using Shared.CrossCutting.Extensions;

namespace ContractingService.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthenticationConfig(this IServiceCollection services, IConfiguration configuration) =>
        services.AddJwtAuthentication(configuration);
 
    public static IApplicationBuilder UseJwtAuthenticationConfig(this IApplicationBuilder app) =>
       app.UseAuthentication()
           .UseAuthorization();
}
