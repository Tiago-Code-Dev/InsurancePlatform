namespace ProposalService.Domain.Services;

using ProposalService.Domain.Entities;
using ProposalService.Domain.Exceptions;

public class ProposalDomainService
{
    public void ValidateProposal(Proposal proposal)
    {
        if (proposal is null)
            throw new DomainException("Proposal cannot be null.");

        if (proposal.Customer is null)
            throw new DomainException("Proposal must have a customer.");

        if (proposal.Contract is null)
            throw new DomainException("Proposal must have a contract.");

        if (proposal.Contract.EndDate <= proposal.Contract.StartDate)
            throw new DomainException("Contract end date must be after start date.");

        if (proposal.Contract.Premium is null || proposal.Contract.Premium.Amount <= 0)
            throw new DomainException("Contract premium must be greater than zero.");
    }
}
