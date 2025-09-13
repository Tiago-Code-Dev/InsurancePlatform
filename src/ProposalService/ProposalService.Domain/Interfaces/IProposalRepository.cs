namespace ProposalService.Domain.Interfaces;

using ProposalService.Domain.Entities;

public interface IProposalRepository 
{
    Task<Proposal?> GetByIdAsync(Guid id);
    Task AddAsync(Proposal proposal);
    Task UpdateAsync(Proposal proposal);
}