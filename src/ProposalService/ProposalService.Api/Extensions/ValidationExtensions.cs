using FluentValidation;
using FluentValidation.AspNetCore;
using ProposalService.Api.Validation;
using ProposalService.Application.Validators;

namespace ProposalService.Api.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateProposalRequestValidator>();

        return services;
    }
}
