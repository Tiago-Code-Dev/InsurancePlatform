namespace ProposalService.Application.Interfaces;

using ProposalService.Application.DTOs;
using Shared.CrossCutting.Response;

public interface IProposalAppService
{
    Task<CustomResponse<ProposalDto>> CreateAsync(CustomerDto customer, ContractDto contract);
    Task<CustomResponse<ProposalDto>> GetByIdAsync(Guid id);
    Task<CustomResponse<IEnumerable<ProposalDto>>> GetAllAsync();
    Task<CustomResponse<Result>> ApproveAsync(Guid proposalId);
    Task<CustomResponse<Result>> RejectAsync(Guid proposalId);

}
