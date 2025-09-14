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
            .HasConversion<string>(); // enum → string

        builder.Property(c => c.StartDate).IsRequired();
        builder.Property(c => c.EndDate).IsRequired();

        builder.OwnsOne(c => c.Premium, premium =>
        {
            premium.Property(p => p.Amount)
                .HasColumnName("PremiumAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            premium.Property(p => p.Currency)
                .HasColumnName("PremiumCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}
