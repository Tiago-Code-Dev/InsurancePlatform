using AuthenticationService.Domain.Entities;


namespace AuthenticationService.Domain.Interfaces;
public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(Role role, CancellationToken ct = default);
}