namespace ContractingService.Application.Validators;

using FluentValidation;
using ContractingService.Application.DTOs;

public class InsuredDtoValidator : AbstractValidator<InsuredDto>
{
    public InsuredDtoValidator()
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MaximumLength(200).WithMessage("Name must be at most 200 characters.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required.")
            .Length(11, 14).WithMessage("Document must be between 11 and 14 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(200).WithMessage("Email must be at most 200 characters.");

        RuleFor(x => x.ProposalId)
            .NotEqual(Guid.Empty).WithMessage("ProposalId is required.");
    }
}
