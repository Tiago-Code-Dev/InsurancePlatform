using AuthenticationService.Domain.Entities;


namespace AuthenticationService.Domain.Interfaces;
public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string roleName);
    Task<List<Role>> GetAllAsync();
    Task<bool> ExistsAsync(string roleName);
    Task CreateAsync(Role role);
}