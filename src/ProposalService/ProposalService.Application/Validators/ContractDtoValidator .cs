namespace ProposalService.Application.Validators;

using FluentValidation;
using ProposalService.Application.DTOs;
using ProposalService.Domain.Enums;

public class ContractDtoValidator : AbstractValidator<ContractDto>
{
    public ContractDtoValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(t => Enum.TryParse<ContractType>(t, out _))
            .WithMessage($"Type must be one of: {string.Join(", ", Enum.GetNames<ContractType>())}");

        RuleFor(x => x.Premium)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("StartDate must be before EndDate.");
    }
}
