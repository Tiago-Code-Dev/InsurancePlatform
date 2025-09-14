namespace ProposalService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces;
using ProposalService.Infrastructure.Context;

public class ProposalRepository : IProposalRepository
{
    private readonly ProposalDbContext _context;

    public ProposalRepository(ProposalDbContext context)
    {
        _context = context;
    }

    public async Task<Proposal?> GetByIdAsync(Guid id)
    {
        return await _context.Proposals
            .Include(p => p.Customer)
            .Include(p => p.Contract)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Proposal proposal)
    {
        await _context.Proposals.AddAsync(proposal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Proposal proposal)
    {
        _context.Proposals.Update(proposal);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Proposal>> GetAllAsync()
    {
        return await _context.Proposals
            .Include(p => p.Customer)
            .Include(p => p.Contract)
            .ToListAsync();
    }

}
