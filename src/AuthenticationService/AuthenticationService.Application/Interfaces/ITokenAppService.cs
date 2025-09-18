using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Interfaces;
public interface ITokenAppService
{
    Task<CustomResponse<string>> IssueRefreshTokenAsync(IssueRefreshTokenCommand cmd, CancellationToken ct);
}