namespace ContractingService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContractingService.Domain.Entities;

public class InsuredMap : IEntityTypeConfiguration<Insured>
{
    public void Configure(EntityTypeBuilder<Insured> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(i => i.Document, d =>
        {
            d.Property(x => x.Number)
                .HasMaxLength(14)
                .IsRequired();
        });

        builder.OwnsOne(i => i.Email, e =>
        {
            e.Property(x => x.Address)
                .HasMaxLength(200)
                .IsRequired();
        });
    }
}
