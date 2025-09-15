using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ContractingService.Infrastructure.Context
{
    public class ContractDbContextFactory : IDesignTimeDbContextFactory<ContractDbContext>
    {
        public ContractDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContractDbContext>();

            optionsBuilder.UseSqlServer(
               "Server=localhost;Database=Insurance_Contracting;User Id=sa;Password=Your_password123;TrustServerCertificate=True",
               sqlOptions =>
               {
                   sqlOptions.EnableRetryOnFailure(
                       maxRetryCount: 5,
                       maxRetryDelay: TimeSpan.FromSeconds(10),
                       errorNumbersToAdd: null
                   );
               });

            return new ContractDbContext(optionsBuilder.Options);
        }
    }
}
