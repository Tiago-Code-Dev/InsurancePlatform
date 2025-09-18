using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Services
{
    public sealed class TokenAppService : ITokenAppService
    {
        private readonly IUserRepository _users;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _uow;

        public TokenAppService(IUserRepository users, ITokenGenerator tokenGenerator, IUnitOfWork uow)
        {
            _users = users;
            _tokenGenerator = tokenGenerator;
            _uow = uow;
        }

        public async Task<Result<string>> IssueRefreshTokenAsync(IssueRefreshTokenCommand cmd, CancellationToken ct)
        {
            if (cmd.UserId == Guid.Empty) return Result<string>.Fail("Invalid user id.");
            if (cmd.Ttl <= TimeSpan.Zero) return Result<string>.Fail("TTL must be greater than zero.");

            var user = await _users.GetByIdAsync(cmd.UserId, ct);
            if (user is null) return Result<string>.Fail("User not found.");

            var token = _tokenGenerator.GenerateRefreshToken();
            user.IssueRefreshToken(token, cmd.Ttl);

            await _users.UpdateAsync(user, ct);
            await _uow.CommitAsync(ct);

            return Result<string>.Ok(token);
        }
    }
}
