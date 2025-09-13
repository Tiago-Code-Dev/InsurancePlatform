namespace ProposalService.Infrastructure.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;

public class ProposalMap : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Status).IsRequired();
        builder.HasOne(p => p.Customer);
        builder.HasOne(p => p.Contract);
    }
}
