using AuthenticationService.Domain.Abstractions;

namespace AuthenticationService.Domain.ValueObjects;

public sealed class RefreshToken
{
    public string Token { get; }
    public DateTimeOffset ExpiresAt { get; }
    public bool Revoked { get; private set; }

    private RefreshToken(string token, DateTimeOffset expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("Refresh token cannot be empty.");

        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Issue(string token, TimeSpan ttl)
        => new(token, DateTimeOffset.UtcNow.Add(ttl));

    public bool IsActive() => !Revoked && DateTimeOffset.UtcNow < ExpiresAt;

    public void Revoke() => Revoked = true;
}
