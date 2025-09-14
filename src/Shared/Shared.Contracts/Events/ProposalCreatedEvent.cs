namespace Shared.CrossCutting.Messaging.Events;

public class ProposalCreatedEvent
{
    public Guid ProposalId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerDocument { get; set; } = string.Empty;

    public List<CoverageMessage> Coverages { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public decimal Amount { get; set; }

    public class CoverageMessage
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Premium { get; set; }
    }
}
