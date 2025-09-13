namespace ProposalService.Domain.Entities;

using ProposalService.Domain.ValueObjects;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Exceptions;

public class Proposal
{
    public Guid Id { get; private set; }
    public Customer Customer { get; private set; }
    public Contract Contract { get; private set; }
    public ProposalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }

    private Proposal() { } // EF Core

    public Proposal(Customer customer, Contract contract)
    {
        Id = Guid.NewGuid();
        Customer = customer ?? throw new DomainException("Customer is required.");
        Contract = contract ?? throw new DomainException("Contract is required.");
        Status = ProposalStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status != ProposalStatus.Pending)
            throw new DomainException("Only pending proposals can be approved.");

        Status = ProposalStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != ProposalStatus.Pending)
            throw new DomainException("Only pending proposals can be rejected.");

        Status = ProposalStatus.Rejected;
    }
}