namespace ProposalService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;

public class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(c => c.Document, d =>
        {
            d.Property(x => x.Number)
             .HasMaxLength(14)
             .IsRequired();
        });

        builder.OwnsOne(c => c.Email, e =>
        {
            e.Property(x => x.Address)
             .HasMaxLength(200)
             .IsRequired();
        });
    }
}
