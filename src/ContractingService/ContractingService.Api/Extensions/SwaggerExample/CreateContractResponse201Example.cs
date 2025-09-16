using System;
using System.Collections.Generic;
using ContractingService.Application.DTOs;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ContractingService.Api.Extensions.SwaggerExample
{
    public class CreateContractResponse201Example : IExamplesProvider<CustomResponse<ContractDto>>
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
                Status: "Em Análise",
                ProposalId: Guid.NewGuid()
            );

            return CustomResponse<ContractDto>.Created(dto);
        }
    }

    public class CreateContractResponse400Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail(new List<MessageResponse>
            {
                new MessageResponse("cpf", "CPF inválido"),
                new MessageResponse("valorSeguro", "Valor deve ser maior que zero")
            });
        }
    }

    public class CreateContractResponse500Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.InternalServerError(
                "Ocorreu um erro interno. Tente novamente mais tarde."
            );
        }
    }
}
