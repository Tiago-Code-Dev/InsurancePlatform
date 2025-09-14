using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.CrossCutting.Auth;

namespace ProposalService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings)
        {
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 🔹 Simulação de autenticação
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtTokenService.GenerateToken(
                    subject: request.Username,
                    roles: new[] { Roles.Admin }
                );

                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public record LoginRequest(string Username, string Password);
}
