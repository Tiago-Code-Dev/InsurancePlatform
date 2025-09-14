namespace ProposalService.Api.Contracts.Requests;

using ProposalService.Application.DTOs;

public class CreateProposalRequest
{
    public CustomerDto Customer { get; set; } = new();
    public ContractDto Contract { get; set; } = new();
}
