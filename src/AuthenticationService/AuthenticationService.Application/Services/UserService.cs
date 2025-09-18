using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Application.Services
{

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<(UserDto, TokenDto)> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(user.Id, user.Email, roles);
            var tokenDto = await _tokenService.GenerateTokenAsync(userDto, ct);

            return (userDto, tokenDto);
        }

        public async Task<(UserDto, TokenDto)> RegisterAsync(string email, string password, IList<string> roles, CancellationToken ct = default)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new ApplicationException("An error occurred while registering the user.");

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new Role { Name = role });

                await _userManager.AddToRoleAsync(user, role);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(user.Id, user.Email, userRoles);
            var tokenDto = await _tokenService.GenerateTokenAsync(userDto, ct);

            return (userDto, tokenDto);
        }
    }
}
