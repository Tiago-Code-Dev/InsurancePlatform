using Shared.Contracts.Events;

namespace ProposalService.Application.Integration;

public interface IProposalIntegrationEventPublisher
{
    Task PublishAsync(ProposalCreatedEvent @event);
}
