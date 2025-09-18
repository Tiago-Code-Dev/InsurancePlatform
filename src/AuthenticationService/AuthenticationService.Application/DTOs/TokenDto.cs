using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

        public TokenDto() { }

        public TokenDto(string accessToken, string refreshToken, DateTime expiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }
    }
}
