using System;
using System.Collections.Generic;
using ProposalService.Application.DTOs;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ProposalService.Api.Extensions.SwaggerExample
{
    public class CreateProposalResponse201Example : IExamplesProvider<CustomResponse<ProposalDto>>
    {
        public CustomResponse<ProposalDto> GetExamples()
        {
            var dto = new ProposalDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "João da Silva",
                CustomerDocument = "12345678901",
                ContractType = "Vida",
                Premium = 149.90m,
                Status = "Em Análise"
            };

            return CustomResponse<ProposalDto>.Created(dto);
        }
    }

    public class CreateProposalResponse400Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail(new List<MessageResponse>
            {
                new MessageResponse("document", "CPF inválido"),
                new MessageResponse("premium", "Valor deve ser maior que zero")
            });
        }
    }

    public class CreateProposalResponse500Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.InternalServerError(
                "Ocorreu um erro interno. Tente novamente mais tarde."
            );
        }
    }
}
