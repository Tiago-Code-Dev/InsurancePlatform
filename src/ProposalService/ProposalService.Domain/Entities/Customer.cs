namespace ProposalService.Domain.Entities;

using ProposalService.Domain.ValueObjects;
using ProposalService.Domain.Exceptions;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }

    private Customer() { } // EF Core

    public Customer(string name, Document document, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name cannot be empty.");

        Id = Guid.NewGuid();
        Name = name;
        Document = document ?? throw new DomainException("Document is required.");
        Email = email ?? throw new DomainException("Email is required.");
    }
}