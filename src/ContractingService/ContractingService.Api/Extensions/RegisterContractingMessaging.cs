using ContractingService.Application.EventHandlers;
using ContractingService.Infrastructure.Messaging;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterContractingMessaging(this IServiceCollection services)
    {
        services.AddScoped<ProposalCreatedEventHandler>();
        services.AddHostedService<RabbitMqProposalCreatedConsumer>();

        return services;
    }
}
