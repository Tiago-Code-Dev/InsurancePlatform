using System.Text.RegularExpressions;
using AuthenticationService.Domain.Abstractions;

namespace AuthenticationService.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>{
    public string Value { get; }

    private static readonly Regex Pattern =
        new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
            throw new DomainException("Invalid email.");
        return new Email(value.Trim());
    }

    public override string ToString() => Value;

    public bool Equals(Email? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as Email);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}
