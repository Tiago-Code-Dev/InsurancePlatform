namespace ProposalService.Application.Validators;

using FluentValidation;
using ProposalService.Application.DTOs;
using ProposalService.Domain.Enums;

public class ContractDtoValidator : AbstractValidator<ContractDto>
{
    public ContractDtoValidator()
    {
        RuleFor(x => x.Type)
           .NotEmpty().WithMessage("Contract type is required.")
           .Must(t => Enum.TryParse<ContractType>(t, out _))
           .WithMessage($"Type must be one of the following: {string.Join(", ", Enum.GetNames<ContractType>())}");

        RuleFor(x => x.Premium)
            .GreaterThan(0).WithMessage("Premium must be greater than 0.");

        RuleFor(x => x.StartDate)
            .NotEqual(default(DateTime)).WithMessage("StartDate is required.")
            .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");

        RuleFor(x => x.EndDate)
            .NotEqual(default(DateTime)).WithMessage("EndDate is required.");
    }
}
