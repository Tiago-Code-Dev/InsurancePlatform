using AuthenticationService.Application.DTOs;
using MediaBrowser.Model.Dto;
using MediatR;
using Shared.CrossCutting.Response;

namespace AuthenticationService.Application.Commands
{
    public class RegisterUserCommand : IRequest<CustomResponse<(DTOs.UserDto, TokenDto)>>
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public IList<string> Roles { get; private set; } = new List<string>();

        public RegisterUserCommand(string email, string password, IList<string> roles)
        {
            Email = email;
            Password = password;
            Roles = roles;
        }

        public RegisterUserCommand() { }
    }
}
