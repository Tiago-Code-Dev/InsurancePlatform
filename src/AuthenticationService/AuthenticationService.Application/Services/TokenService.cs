using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<TokenDto> GenerateTokenAsync(UserDto user, CancellationToken ct = default)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString();

            return Task.FromResult(new TokenDto(accessToken, refreshToken, tokenDescriptor.Expires.Value));
        }

        public Task<TokenDto> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            throw new NotImplementedException("Refresh token logic not implemented yet.");
        }
    }
}
