namespace ContractingService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContractingService.Domain.Entities;

public class ContractMap : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.HasOne(c => c.Insured)
            .WithMany() 
            .IsRequired();

        builder.HasMany(c => c.Coverages)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
