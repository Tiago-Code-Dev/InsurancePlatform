namespace ProposalService.Application.DTOs;

public class ProposalDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerDocument { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public decimal Premium { get; set; }
    public string Status { get; set; } = string.Empty;
}
