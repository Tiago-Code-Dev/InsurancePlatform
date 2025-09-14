namespace ProposalService.Api.Validation;

using FluentValidation;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Application.Validators;

public class CreateProposalRequestValidator : AbstractValidator<CreateProposalRequest>
{
    public CreateProposalRequestValidator()
    {
        RuleFor(x => x.Customer).SetValidator(new CustomerDtoValidator());
        RuleFor(x => x.Contract).SetValidator(new ContractDtoValidator());
    }
}
