using System.Text.RegularExpressions;
using AuthenticationService.Domain.Exceptions;

namespace AuthenticationService.Domain.ValueObjects;

public class Email
{
    public string Address { get; private set; }

    protected Email() { } 


    public Email(string address)
    {
        if (!IsValid(address))
            throw new DomainException("Invalid email.");

        Address = address.Trim().ToLower();
    }

    public static bool IsValid(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    }

    public override string ToString() => Address;
}