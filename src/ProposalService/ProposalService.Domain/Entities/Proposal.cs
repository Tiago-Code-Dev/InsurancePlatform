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
       
        Customer = customer ?? throw new DomainException("Customer is required.");
        Contract = contract ?? throw new DomainException("Contract is required.");

        Id = Guid.NewGuid();
        Status = ProposalStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        EnsureStatus(ProposalStatus.Pending, "Only pending proposals can be approved.");
        Status = ProposalStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        EnsureStatus(ProposalStatus.Pending, "Only pending proposals can be rejected.");
        Status = ProposalStatus.Rejected;
    }

    private void EnsureStatus(ProposalStatus expected, string message)
    {
        if (Status != expected)
            throw new DomainException(message);
    }
}