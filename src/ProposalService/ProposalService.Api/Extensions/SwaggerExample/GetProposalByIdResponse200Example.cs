using System;
using System.Collections.Generic;
using ProposalService.Application.DTOs;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ProposalService.Api.SwaggerExamples
{
    public class GetProposalByIdResponse200Example : IExamplesProvider<CustomResponse<ProposalDto>>
    {
        public CustomResponse<ProposalDto> GetExamples()
        {
            var dto = new ProposalDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "João da Silva",
                CustomerDocument = "12345678901",
                ContractType = "Vida",
                Premium = 199.90m,
                Status = "Aprovada"
            };

            return CustomResponse<ProposalDto>.Ok(dto);
        }
    }

    public class GetProposalByIdResponse404Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail("Proposal not found.");
        }
    }
}
