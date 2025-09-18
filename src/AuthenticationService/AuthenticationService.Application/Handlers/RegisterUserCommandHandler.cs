using AuthenticationService.Application.Commands;
using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using MediatR;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, CustomResponse<(UserDto, TokenDto)>>
    {
        private readonly IUserService _userService;
        private readonly INotifier _notifier;

        public RegisterUserCommandHandler(IUserService userService, INotifier notifier)
        {
            _userService = userService;
            _notifier = notifier;
        }

        public async Task<CustomResponse<(UserDto, TokenDto)>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.RegisterAsync(request.Email, request.Password, request.Roles, cancellationToken);
                return CustomResponse<(UserDto, TokenDto)>.Created(result);
            }
            catch (Exception ex)
            {
                _notifier.Handle(new Notification("Register", ex.Message));
                return CustomResponse<(UserDto, TokenDto)>.Fail("User registration failed.");
            }
        }
    }
}
