namespace ContractingService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContractingService.Domain.Entities;

public class CoverageMap : IEntityTypeConfiguration<Coverage>
{
    public void Configure(EntityTypeBuilder<Coverage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Type)
            .IsRequired();

        builder.OwnsOne(c => c.Premium, m =>
        {
            m.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            m.Property(p => p.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}
