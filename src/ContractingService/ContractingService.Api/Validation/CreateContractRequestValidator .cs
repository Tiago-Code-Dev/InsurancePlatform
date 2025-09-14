namespace ContractingService.Api.Validation;

using FluentValidation;
using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.Validators;

public class CreateContractRequestValidator : AbstractValidator<CreateContractRequest>
{
    public CreateContractRequestValidator()
    {
        RuleFor(x => x.Insured).SetValidator(new InsuredDtoValidator());
        RuleForEach(x => x.Coverages).SetValidator(new CoverageDtoValidator());
    }
}
