namespace ContractingService.Api.Validation;

using FluentValidation;
using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.Validators;

public class CreateContractRequestValidator : AbstractValidator<CreateContractRequest>
{
    public CreateContractRequestValidator()
    {

        RuleFor(x => x.Insured)
           .NotNull().WithMessage("The insured is mandatory")
           .SetValidator(new InsuredDtoValidator());

        RuleFor(x => x.Coverages)
            .NotEmpty().WithMessage("At least one coverage must be reported.")
            .ForEach(c => c.SetValidator(new CoverageDtoValidator()));
    }
}
