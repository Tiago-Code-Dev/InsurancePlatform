namespace Shared.CrossCutting.Auth;

using System.Security.Claims;

public interface IJwtTokenService
{
    string GenerateToken(
        string subject,
        IEnumerable<Claim>? extraClaims = null,
        IEnumerable<string>? roles = null);
}
