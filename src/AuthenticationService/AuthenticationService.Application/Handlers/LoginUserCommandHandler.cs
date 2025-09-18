using AuthenticationService.Application.Commands;
using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using MediatR;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, CustomResponse<(UserDto, TokenDto)>>
    {
        private readonly IUserService _userService;
        private readonly INotifier _notifier;

        public LoginUserCommandHandler(IUserService userService, INotifier notifier)
        {
            _userService = userService;
            _notifier = notifier;
        }

        public async Task<CustomResponse<(UserDto, TokenDto)>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.LoginAsync(request.Email, request.Password, cancellationToken);
                return CustomResponse<(UserDto, TokenDto)>.Ok(result);
            }
            catch (Exception ex)
            {
                _notifier.Handle(new Notification("Login", ex.Message));
                return CustomResponse<(UserDto, TokenDto)>.Fail("Login failed.");
            }
        }
    }
}
