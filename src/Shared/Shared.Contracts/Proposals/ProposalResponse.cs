namespace Shared.Contracts.Proposals;

public class ProposalResponse
{
    public Guid Id { get; set; }
    public required string Customer { get; set; }
    public required string Contract { get; set; }
    public required string Status { get; set; }
}
