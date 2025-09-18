using System.Collections.ObjectModel;
using AuthenticationService.Domain.Abstractions;
using AuthenticationService.Domain.ValueObjects;

namespace AuthenticationService.Domain.Entities;

public sealed class User : Entity, IAggregateRoot
{
    private readonly List<Role> _roles = new();
    private readonly List<RefreshToken> _refreshTokens = new();

    public Email Email { get; private set; }
    public string? FullName { get; private set; }
    public bool Active { get; private set; }

    public ReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    public ReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() {}

    private User(Email email, string? fullName)
    {
        Email = email;
        FullName = fullName?.Trim();
        Active = true;
    }

    public static User Register(Email email, string? fullName) => new(email, fullName);

    public void Activate() => Active = true;
    public void Deactivate() => Active = false;

    public void ChangeEmail(Email email) => Email = email;
    public void ChangeName(string? fullName) => FullName = fullName?.Trim();

    public void AddRole(Role role)
    {
        if (_roles.Any(r => string.Equals(r.Name, role.Name, StringComparison.OrdinalIgnoreCase)))
            return;
        _roles.Add(role);
    }

    public void RemoveRole(string roleName) =>
        _roles.RemoveAll(r => string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));

    public RefreshToken IssueRefreshToken(string token, TimeSpan ttl)
    {
        var rt = RefreshToken.Issue(token, ttl);
        _refreshTokens.Add(rt);
        return rt;
    }

    public void RevokeRefreshToken(string token)
    {
        var rt = _refreshTokens.FirstOrDefault(x => x.Token == token);
        if (rt is null) throw new DomainException("Refresh token not found.");
        rt.Revoke();
    }
}
