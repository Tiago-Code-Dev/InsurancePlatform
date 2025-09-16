using System;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ProposalService.Api.Extensions.SwaggerExample
{
    public class CreateProposalRequestExample : IExamplesProvider<CreateProposalRequest>
    {
        public CreateProposalRequest GetExamples()
        {
            return new CreateProposalRequest
            {
                Customer = new CustomerDto
                {
                    Name = "João da Silva",
                    Document = "12345678901",
                    Email = "joao.silva@email.com"
                },
                Contract = new ContractDto
                {
                    Type = "Vida",
                    Premium = 149.90m,
                    StartDate = DateTime.UtcNow.Date.AddDays(1),
                    EndDate = DateTime.UtcNow.Date.AddYears(1)
                }
            };
        }
    }
}
