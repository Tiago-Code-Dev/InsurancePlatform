namespace ContractingService.Application.Validators;

using FluentValidation;
using ContractingService.Application.DTOs;

public class InsuredDtoValidator : AbstractValidator<InsuredDto>
{
    public InsuredDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Document)
            .NotEmpty()
            .Length(11, 14);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);
    }
}
