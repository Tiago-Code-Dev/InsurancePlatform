namespace ContractingService.Domain.Entities;

using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Exceptions;

public class Insured
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }

 
    public Guid ProposalId { get; private set; }

    private Insured() { }

    public Insured(string name, Document document, Email email, Guid proposalId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Insured name cannot be empty.");

        if (name.Length > 200)
            throw new DomainException("Insured name must be 200 characters or fewer.");

        Document = document ?? throw new DomainException("Document is required.");
        Email = email ?? throw new DomainException("Email is required.");
        ProposalId = proposalId == Guid.Empty ? throw new DomainException("ProposalId is required.") : proposalId;

        Id = Guid.NewGuid();
        Name = name;
    }
}
