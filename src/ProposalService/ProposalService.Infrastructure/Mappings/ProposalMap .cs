namespace ProposalService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;

public class ProposalMap : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ApprovedAt)
            .IsRequired(false);

        builder.HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Contract)
            .WithMany()
            .HasForeignKey("ContractId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
