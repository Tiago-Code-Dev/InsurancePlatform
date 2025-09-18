using AuthenticationService.Domain.Abstractions;

namespace AuthenticationService.Domain.Entities;

public sealed class Role : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    private Role() { Name = string.Empty; }
    private Role(string name) => SetName(name);

    public static Role Create(string name) => new(name);

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty.");
        Name = name.Trim();
    }
}
