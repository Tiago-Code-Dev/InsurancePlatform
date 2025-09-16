using System;
using System.Collections.Generic;
using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ContractingService.Api.Extensions.SwaggerExample
{
    public class CreateContractRequestExample : IExamplesProvider<CreateContractRequest>
    {
        public CreateContractRequest GetExamples()
        {
            return new CreateContractRequest(
                new InsuredDto(
                    Name: "Maria Oliveira",
                    Document: "98765432100",
                    Email: "maria.oliveira@email.com",
                    ProposalId: Guid.NewGuid()
                ),
                new List<CoverageDto>
                {
                    new CoverageDto("Cobertura Básica", "Basic", 250.00m),
                    new CoverageDto("Cobertura Acidente", "Extended", 100.00m)
                }
            );
        }
    }
}
