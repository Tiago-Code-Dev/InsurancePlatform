using ContractingService.Api.Validation;
using ContractingService.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ContractingService.Api.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<InsuredDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateContractRequestValidator>();

        return services;
    }
}
