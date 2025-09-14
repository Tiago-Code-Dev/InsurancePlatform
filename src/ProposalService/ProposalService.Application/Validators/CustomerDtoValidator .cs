namespace ProposalService.Application.Validators;

using FluentValidation;
using ProposalService.Application.DTOs;

public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MinimumLength(2).MaximumLength(200);

        RuleFor(x => x.Document)
            .NotEmpty().Length(11, 14);

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().MaximumLength(200);
    }
}
