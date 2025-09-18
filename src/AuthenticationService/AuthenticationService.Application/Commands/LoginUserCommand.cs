using AuthenticationService.Application.DTOs;
using MediatR;
using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Commands
{
    public class LoginUserCommand : IRequest<CustomResponse<(UserDto, TokenDto)>>
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public LoginUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public LoginUserCommand() { }
    }
}