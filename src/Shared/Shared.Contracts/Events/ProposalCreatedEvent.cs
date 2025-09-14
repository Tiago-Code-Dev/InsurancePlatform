namespace Shared.Contracts.Events;

public class ProposalCreatedEvent
{
    public Guid ProposalId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
