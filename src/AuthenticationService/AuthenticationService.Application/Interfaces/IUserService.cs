using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Interfaces
{
    public interface IUserService
    {
        Task<(UserDto, TokenDto)> LoginAsync(string email, string password, CancellationToken ct = default);
        Task<(UserDto, TokenDto)> RegisterAsync(string email, string password, IList<string> roles, CancellationToken ct = default);
    }
}
