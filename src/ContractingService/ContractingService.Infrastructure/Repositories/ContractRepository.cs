namespace ContractingService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;
using ContractingService.Infrastructure.Context;

public class ContractRepository : IContractRepository
{
    private readonly ContractDbContext _context;

    public ContractRepository(ContractDbContext context)
    {
        _context = context;
    }

    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        return await _context.Contracts
            .Include(c => c.Insured)
            .Include(c => c.Coverages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }
}
