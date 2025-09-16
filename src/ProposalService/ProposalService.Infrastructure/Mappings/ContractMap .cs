namespace ProposalService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;
using ProposalService.Domain.ValueObjects;

public class ContractMap : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
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
