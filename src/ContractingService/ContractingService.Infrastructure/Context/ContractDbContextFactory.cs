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
                "Server=localhost;Database=Insurance_Contracting;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
            );

            return new ContractDbContext(optionsBuilder.Options);
        }
    }
}
