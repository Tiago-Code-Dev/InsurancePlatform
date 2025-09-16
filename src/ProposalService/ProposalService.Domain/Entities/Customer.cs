namespace ProposalService.Domain.Entities;

using ProposalService.Domain.ValueObjects;
using ProposalService.Domain.Exceptions;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }

    private Customer() { } 

    public Customer(string name, Document document, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name cannot be empty.");

        if (name.Length > 200)
            throw new DomainException("Customer name must be 200 characters or fewer.");

        Id = Guid.NewGuid();
        Name = name;
        Document = document ?? throw new DomainException("Document is required.");
        Email = email ?? throw new DomainException("Email is required.");
    }
}