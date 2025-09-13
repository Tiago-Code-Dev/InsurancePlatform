namespace ProposalService.Domain.ValueObjects;

using ProposalService.Domain.Exceptions;

public record Document
{
    public string Number { get; }

    public Document(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Document number cannot be empty.");

        if (number.Length < 11 || number.Length > 14)
            throw new DomainException("Invalid document number length.");

        Number = number;
    }

    public override string ToString() => Number;
}
