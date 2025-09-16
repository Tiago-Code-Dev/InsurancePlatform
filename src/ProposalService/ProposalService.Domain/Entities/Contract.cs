namespace ProposalService.Domain.Entities;

using ProposalService.Domain.ValueObjects;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Exceptions;
public class Contract
{
    public Guid Id { get; private set; }
    public ContractType Type { get; private set; }
    public Money Premium { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private Contract() { } // EF Core

    public Contract(ContractType type, Money premium, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new DomainException("Contract end date must be after start date.");

        if (premium is null)
            throw new DomainException("Premium is required.");

        if (premium.Amount <= 0)
            throw new DomainException("Premium must be greater than zero.");


        Id = Guid.NewGuid();
        Type = type;
        Premium = premium;
        StartDate = startDate;
        EndDate = endDate;
    }
}
