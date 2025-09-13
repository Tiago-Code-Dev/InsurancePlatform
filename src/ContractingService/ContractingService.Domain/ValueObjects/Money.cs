namespace ContractingService.Domain.ValueObjects;

using ContractingService.Domain.Exceptions;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        Amount = amount;
        Currency = currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
