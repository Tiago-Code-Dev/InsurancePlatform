using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.CrossCutting.Messaging;
using Shared.CrossCutting.Messaging.RabbitMq;

namespace Shared.CrossCutting.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
        return services;
    }
}
