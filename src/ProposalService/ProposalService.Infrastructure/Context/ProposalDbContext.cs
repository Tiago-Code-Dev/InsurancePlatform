namespace ProposalService.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using ProposalService.Domain.Entities;

public class ProposalDbContext : DbContext
{
    public ProposalDbContext(DbContextOptions<ProposalDbContext> options) : base(options) { }

    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProposalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
