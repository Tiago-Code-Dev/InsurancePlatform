using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProposalService.Infrastructure.Context
{
    public class ProposalDbContextFactory : IDesignTimeDbContextFactory<ProposalDbContext>
    {
        public ProposalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProposalDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=Insurance_Proposal;User Id=sa;Password=Your_password123;TrustServerCertificate=True",
               sqlOptions =>
               {
                   sqlOptions.EnableRetryOnFailure(
                       maxRetryCount: 5,
                       maxRetryDelay: TimeSpan.FromSeconds(10),
                       errorNumbersToAdd: null
                   );
               });

            return new ProposalDbContext(optionsBuilder.Options);
        }
    }
}
