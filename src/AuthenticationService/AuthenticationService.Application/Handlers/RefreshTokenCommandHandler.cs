using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using InsurancePlatform.AuthenticationService.Application.Commands;
using MediatR;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, CustomResponse<TokenDto>>
    {
        private readonly ITokenService _tokenService;
        private readonly INotifier _notifier;

        public RefreshTokenCommandHandler(ITokenService tokenService, INotifier notifier)
        {
            _tokenService = tokenService;
            _notifier = notifier;
        }

        public async Task<CustomResponse<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _tokenService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
                return CustomResponse<TokenDto>.Ok(token);
            }
            catch (Exception ex)
            {
                _notifier.Handle(new Notification("RefreshToken", ex.Message));
                return CustomResponse<TokenDto>.Fail("Failed to refresh token.");
            }
        }
    }
}
