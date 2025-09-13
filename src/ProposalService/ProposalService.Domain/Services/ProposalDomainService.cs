namespace ProposalService.Domain.Services;

using ProposalService.Domain.Entities;
using ProposalService.Domain.Exceptions;

public class ProposalDomainService
{
    public void ValidateProposal(Proposal proposal)
    {
        if (proposal.Customer == null)
            throw new DomainException("Proposal must have a customer.");

        if (proposal.Contract == null)
            throw new DomainException("Proposal must have a contract.");
    }
}
