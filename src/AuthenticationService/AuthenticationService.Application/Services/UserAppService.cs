using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Common;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.InterfacesServices;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Interfaces;
using AuthenticationService.Domain.ValueObjects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Application.Services
{
    public class UserAppService : IUserAppService
    {

        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IPasswordHasher _hasher;
        private readonly IUnitOfWork _uow;

        public UserAppService(IUserRepository users, IRoleRepository roles, IPasswordHasher hasher, IUnitOfWork uow)
        {
            _users = users;
            _roles = roles;
            _hasher = hasher;
            _uow = uow;
        }

        public Task<Result> AssignRoleAsync(AssignRoleCommand cmd, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Guid>> RegisterAsync(RegisterUserCommand cmd, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(cmd.Email))
                return Result<Guid>.Fail("Email is required.");
            if (string.IsNullOrWhiteSpace(cmd.Password) || cmd.Password.Length < 8)
                return Result<Guid>.Fail("Password must be at least 8 characters.");

            var email = Email.Create(cmd.Email);

            if (await _users.ExistsByEmailAsync(email, ct))
                return Result<Guid>.Fail("Email already registered.");

            var user = User.Register(email, cmd.FullName);

            var _ = _hasher.Hash(cmd.Password);

            await _users.AddAsync(user, ct);
            await _uow.CommitAsync(ct);

            return Result<Guid>.Ok(user.Id);
        }

        public async Task<Result> AssignRoleAsync(AssignRoleCommand cmd, CancellationToken ct)
        {
            if (cmd.UserId == Guid.Empty) return Result.Fail("Invalid user id.");
            if (string.IsNullOrWhiteSpace(cmd.RoleName)) return Result.Fail("Role name is required.");

            var user = await _users.GetByIdAsync(cmd.UserId, ct);
            if (user is null) return Result.Fail("User not found.");

            var roleName = cmd.RoleName.Trim();
            var role = await _roles.GetByNameAsync(roleName, ct) ?? Role.Create(roleName);

            user.AddRole(role);
            if (role.Id == Guid.Empty) await _roles.AddAsync(role, ct);

            await _users.UpdateAsync(user, ct);
            await _uow.CommitAsync(ct);

            return Result.Ok();
        }
    }
}
