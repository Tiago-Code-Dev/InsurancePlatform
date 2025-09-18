using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokenAsync(UserDto user, CancellationToken ct = default);
        Task<TokenDto> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    }
}
