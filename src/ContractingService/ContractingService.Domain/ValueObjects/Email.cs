namespace ContractingService.Domain.ValueObjects;

using ContractingService.Domain.Exceptions;
using System.Text.RegularExpressions;

public record Email
{
    public string Address { get; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Email cannot be empty.");

        if (!Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new DomainException("Invalid email format.");

        Address = address;
    }

    public override string ToString() => Address;
}
