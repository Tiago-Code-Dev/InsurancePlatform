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
            .HasConversion<string>(); // grava como string no banco

        builder.HasOne(p => p.Customer)
            .WithMany() // uma proposta tem um cliente
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Contract)
            .WithMany() // uma proposta tem um contrato
            .HasForeignKey("ContractId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
