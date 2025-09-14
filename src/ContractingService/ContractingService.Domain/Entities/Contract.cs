namespace ContractingService.Domain.Entities;

using ContractingService.Domain.Enums;
using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Exceptions;

public class Contract
{
    public Guid Id { get; private set; }
    public Insured Insured { get; private set; }
    public IReadOnlyCollection<Coverage> Coverages => _coverages.AsReadOnly();
    private readonly List<Coverage> _coverages = new();
    public ContractStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ActivatedAt { get; private set; }

    private Contract() { } // EF Core

    public Contract(Insured insured)
    {
        Id = Guid.NewGuid();
        Insured = insured ?? throw new DomainException("Insured is required.");
        Status = ContractStatus.Draft;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddCoverage(Coverage coverage)
    {
        if (Status != ContractStatus.Draft)
            throw new DomainException("Coverages can only be added while contract is in Draft status.");

        _coverages.Add(coverage);
    }

    public void Activate()
    {
        if (!_coverages.Any())
            throw new DomainException("A contract must have at least one coverage to be activated.");

        Status = ContractStatus.Active;
        ActivatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != ContractStatus.Active)
            throw new DomainException("Only active contracts can be canceled.");

        Status = ContractStatus.Canceled;
    }

    public void Terminate()
    {
        if (Status == ContractStatus.Terminated)
            throw new InvalidOperationException("Contract is already terminated.");

        Status = ContractStatus.Terminated;
    }
}
