using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Api.Extensions.SwaggerExample;
using ProposalService.Application.DTOs;
using ProposalService.Application.Interfaces;
using Shared.CrossCutting.Auth;
using Shared.CrossCutting.Controllers;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ProposalService.API.Controllers
{
    [Route("api/v1/proposals")]
    [Produces("application/json")]

    public class ProposalsController : MainController
    {
        private readonly IProposalAppService _proposalAppService;

        public ProposalsController(
            IProposalAppService proposalAppService,
            INotifier notifier) : base(notifier)
        {
            _proposalAppService = proposalAppService;
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova proposta de seguro")]
        [SwaggerRequestExample(typeof(CreateProposalRequest), typeof(CreateProposalRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateProposalResponse201Example))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateProposalResponse400Example))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(CreateProposalResponse500Example))]
        [ProducesResponseType(typeof(CustomResponse<ProposalDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateProposalRequest request)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _proposalAppService.CreateAsync(request.Customer, request.Contract);
            return CustomResponse(result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as propostas")]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll() =>
            CustomResponse(await _proposalAppService.GetAllAsync());


        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Consulta uma proposta por Id")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProposalService.Api.SwaggerExamples.GetProposalByIdResponse200Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProposalService.Api.SwaggerExamples.GetProposalByIdResponse404Example))]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CustomResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _proposalAppService.GetByIdAsync(id);

            if (result is null)
            {
                NotifyError("Contract not found.");
                return NotFound(CustomResponse());
            }

            return CustomResponse(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}/approve")]
        [SwaggerOperation(Summary = "Aprova uma proposta pendente")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProposalService.Api.SwaggerExamples.ApproveProposalResponse200Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProposalService.Api.SwaggerExamples.ApproveProposalResponse404Example))]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _proposalAppService.ApproveAsync(id);
            return CustomResponse(new { message = "Proposal approved successfully." });
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}/reject")]
        [SwaggerOperation(Summary = "Rejeita uma proposta pendente")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProposalService.Api.SwaggerExamples.RejectProposalResponse200Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProposalService.Api.SwaggerExamples.RejectProposalResponse404Example))]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _proposalAppService.RejectAsync(id);
            return CustomResponse(new { message = "Proposal rejected successfully." });
        }
    }
}
