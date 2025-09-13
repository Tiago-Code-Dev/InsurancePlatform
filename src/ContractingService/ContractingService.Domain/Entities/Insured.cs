namespace ContractingService.Domain.Entities;

using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Exceptions;

public class Insured
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }

    private Insured() { } // EF Core

    public Insured(string name, Document document, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Insured name cannot be empty.");

        Id = Guid.NewGuid();
        Name = name;
        Document = document ?? throw new DomainException("Document is required.");
        Email = email ?? throw new DomainException("Email is required.");
    }
}
