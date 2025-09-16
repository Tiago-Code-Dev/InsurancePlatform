using System;
using System.Collections.Generic;
using ContractingService.Application.DTOs;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ContractingService.Api.SwaggerExamples
{
    public class GetContractByIdResponse200Example : IExamplesProvider<CustomResponse<ContractDto>>
    {
        public CustomResponse<ContractDto> GetExamples()
        {
            var dto = new ContractDto(
                Id: Guid.NewGuid(),
                InsuredName: "Maria Oliveira",
                InsuredDocument: "98765432100",
                InsuredEmail: "maria.oliveira@email.com",
                Coverages: new List<CoverageDto>
                {
                    new CoverageDto("Cobertura Básica", "Vida", 250.00m),
                    new CoverageDto("Cobertura Acidente", "Acidente", 100.00m)
                },
                Status: "Ativo",
                ProposalId: Guid.NewGuid()
            );

            return CustomResponse<ContractDto>.Ok(dto);
        }
    }

    public class GetContractByIdResponse404Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail("Contract not found.");
        }
    }
}
