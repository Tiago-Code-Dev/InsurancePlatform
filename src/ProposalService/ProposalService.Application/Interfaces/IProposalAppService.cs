namespace ProposalService.Application.Interfaces;

using ProposalService.Application.DTOs;

public interface IProposalAppService
{
    Task<ProposalDto> CreateAsync(CustomerDto customer, ContractDto contract);
    Task<ProposalDto?> GetByIdAsync(Guid id);
    Task ApproveAsync(Guid proposalId);
    Task RejectAsync(Guid proposalId);
}
