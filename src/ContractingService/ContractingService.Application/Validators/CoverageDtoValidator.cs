namespace ContractingService.Application.Validators;

using FluentValidation;
using ContractingService.Application.DTOs;
using ContractingService.Domain.Enums; 

public class CoverageDtoValidator : AbstractValidator<CoverageDto>
{
    public CoverageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Coverage name is required.")
            .MaximumLength(200).WithMessage("Coverage name must be at most 200 characters.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Coverage type is required.")
            .Must(t => Enum.TryParse<CoverageType>(t, out _))
            .WithMessage($"Type must be one of the following: {string.Join(", ", Enum.GetNames<CoverageType>())}");

        RuleFor(x => x.Premium)
            .GreaterThan(0).WithMessage("Premium must be greater than 0.");
    }
}
