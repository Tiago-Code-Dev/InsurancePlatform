namespace ContractingService.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using ContractingService.Domain.Entities;

public class ContractDbContext : DbContext
{
    public ContractDbContext(DbContextOptions<ContractDbContext> options) : base(options) { }

    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Insured> Insureds { get; set; }
    public DbSet<Coverage> Coverages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContractDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
