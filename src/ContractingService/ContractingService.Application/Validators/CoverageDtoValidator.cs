namespace ContractingService.Application.Validators;

using FluentValidation;
using ContractingService.Application.DTOs;
using ContractingService.Domain.Enums; // para validar o Type

public class CoverageDtoValidator : AbstractValidator<CoverageDto>
{
    public CoverageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(t => Enum.TryParse<CoverageType>(t, out _))
            .WithMessage($"Type must be one of: {string.Join(", ", Enum.GetNames<CoverageType>())}");

        RuleFor(x => x.Premium)
            .GreaterThanOrEqualTo(0);
    }
}
