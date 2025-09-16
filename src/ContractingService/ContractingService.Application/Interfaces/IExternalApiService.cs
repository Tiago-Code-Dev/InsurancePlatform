using Shared.Contracts.Proposals;

namespace ContractingService.Application.Interfaces

{
    public interface IExternalApiService
    {
        Task<ProposalResponse?> GetProposalDetailsAsync(Guid proposaId);
    }
}
