namespace ProposalService.Api.Contracts.Requests;

using ProposalService.Application.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreateProposalRequest
{
    [Required]
    public CustomerDto Customer { get; set; } = new();

    [Required]
    public ContractDto Contract { get; set; } = new();
}
