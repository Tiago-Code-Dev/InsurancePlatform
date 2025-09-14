using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.CrossCutting.Auth;
using Shared.CrossCutting.Controllers;
using Shared.CrossCutting.Response;
using Shared.CrossCutting.Notifications;

namespace Shared.CrossCutting.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : MainController
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings,
            INotifier notifier) : base(notifier)
        {
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            // Simulação de autenticação
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtTokenService.GenerateToken(
                    subject: request.Username,
                    roles: new[] { Roles.Admin }
                );

                return CustomResponse(new { Token = token });
            }

            NotifyError("Invalid credentials");
            return CustomResponse();
        }
    }

    public record LoginRequest(string Username, string Password);
}
