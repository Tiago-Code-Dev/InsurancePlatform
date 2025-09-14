using ContractingService.Application.Services;

namespace ContractingService.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocumentation();
        services.AddJwtAuthenticationConfig(configuration);
        services.AddValidationConfig();
        services.AddResilientHttpClients();
        services.AddHealthChecks();
        services.RegisterContractingMessaging();
        services.AddScoped<ExternalApiService>();

        return services;
    }
}
