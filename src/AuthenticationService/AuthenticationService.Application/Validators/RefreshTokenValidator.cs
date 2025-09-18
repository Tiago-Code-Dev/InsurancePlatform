using FluentValidation;
using InsurancePlatform.AuthenticationService.Application.Commands;

namespace AuthenticationService.Application.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
