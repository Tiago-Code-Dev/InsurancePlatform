namespace ContractingService.Domain.Interfaces;

using ContractingService.Domain.Entities;

public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(Guid id);
    Task AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
}
