using Microsoft.Extensions.Options;
using Shared.Contracts.Events;
using Shared.CrossCutting.Messaging;
using Shared.CrossCutting.Messaging.RabbitMq;

namespace ProposalService.Application.Integration;

public class ProposalIntegrationEventPublisher : IProposalIntegrationEventPublisher
{
    private readonly IEventBus _eventBus;
    private readonly RabbitMqOptions _options;

    public ProposalIntegrationEventPublisher(IEventBus eventBus, IOptions<RabbitMqOptions> options)
    {
        _eventBus = eventBus;
        _options = options.Value;
    }

    public Task PublishAsync(ProposalCreatedEvent @event)
        => _eventBus.PublishAsync(@event, _options.Queues.ProposalCreated);
}
