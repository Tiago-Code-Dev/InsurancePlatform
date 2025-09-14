using ProposalService.Application.Integration;
using ProposalService.Application.Interfaces;
using ProposalService.Application.Services;
using Shared.CrossCutting.Extensions;

namespace ProposalService.Api.Extensions;

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
        services.AddProposalServices(configuration);
        services.AddRabbitMqEventBus(configuration);
        services.AddScoped<IProposalIntegrationEventPublisher, ProposalIntegrationEventPublisher>();
        services.AddProposalServices(configuration);

        services.AddScoped<IExternalApiService, ExternalApiService>();

        return services;
    }
}
