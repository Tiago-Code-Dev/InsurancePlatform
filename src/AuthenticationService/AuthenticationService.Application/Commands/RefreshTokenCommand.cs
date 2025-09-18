using MediatR;
using Shared.CrossCutting.Response;
using AuthenticationService.Application.DTOs;

namespace InsurancePlatform.AuthenticationService.Application.Commands
{
    public class RefreshTokenCommand : IRequest<CustomResponse<TokenDto>>
    {
        public string RefreshToken { get; private set; }

        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public RefreshTokenCommand() { }
    }
}
