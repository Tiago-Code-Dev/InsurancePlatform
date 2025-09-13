namespace ContractingService.Domain.Entities;

using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Enums;
using ContractingService.Domain.Exceptions;

public class Coverage
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CoverageType Type { get; private set; }
    public Money Premium { get; private set; }

    private Coverage() { } // EF Core

    public Coverage(string name, CoverageType type, Money premium)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Coverage name cannot be empty.");

        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        Premium = premium ?? throw new DomainException("Premium is required.");
    }
}
