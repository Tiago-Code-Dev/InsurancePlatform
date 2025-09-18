using AuthenticationService.Application.Commands;
using FluentValidation;

namespace AuthenticationService.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("At least one role must be specified.")
                .Must(r => r.Any()).WithMessage("At least one role is required.");
        }
    }
}
