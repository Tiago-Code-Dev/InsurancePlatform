using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProposalService.Infrastructure.Context
{
    public class ProposalDbContextFactory : IDesignTimeDbContextFactory<ProposalDbContext>
    {
        public ProposalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProposalDbContext>();

            // Usa a mesma connection string do appsettings.json
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=Insurance_Proposal;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
            );

            return new ProposalDbContext(optionsBuilder.Options);
        }
    }
}
