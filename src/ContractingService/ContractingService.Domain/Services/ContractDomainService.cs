namespace ContractingService.Domain.Services;

using ContractingService.Domain.Entities;
using ContractingService.Domain.Exceptions;

public class ContractDomainService
{
    public void ValidateContract(Contract contract)
    {
        if (contract is null)
            throw new DomainException("Contract cannot be null.");

        if (contract.Insured is null)
            throw new DomainException("Contract must have an insured person.");

        if (!contract.Coverages.Any())
            throw new DomainException("Contract must contain at least one coverage.");
    }
}
