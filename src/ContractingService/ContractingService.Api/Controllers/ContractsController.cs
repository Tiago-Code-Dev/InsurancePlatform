using ContractingService.Api.Contracts.Requests;
using ContractingService.Api.Extensions.SwaggerExample;
using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CrossCutting.Auth;
using Shared.CrossCutting.Controllers;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ContractingService.API.Controllers;

[Route("api/v1/contracts")]
[Produces("application/json")]
public class ContractsController : MainController
{
    private readonly IContractAppService _contractAppService;

    public ContractsController(
        IContractAppService contractAppService,
        INotifier notifier) : base(notifier)
    {
        _contractAppService = contractAppService;
    }

    // ============================================================
    // POST /api/v1/contracts
    // ============================================================
    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Cria um contrato a partir de uma proposta aprovada")]
    [SwaggerRequestExample(typeof(CreateContractRequest), typeof(CreateContractRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateContractResponse201Example))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateContractResponse400Example))]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(CreateContractResponse500Example))]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request) =>
        !ModelState.IsValid
            ? CustomResponse(ModelState)
            : CustomResponse(await _contractAppService.CreateAsync(request.Insured, request.Coverages));



    // ============================================================
    // GET /api/v1/contracts/{id}
    // ============================================================
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Consulta contrato por Id")]
    [ProducesResponseType(typeof(CustomResponse<ContractDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ContractingService.Api.SwaggerExamples.GetContractByIdResponse200Example))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ContractingService.Api.SwaggerExamples.GetContractByIdResponse404Example))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _contractAppService.GetByIdAsync(id);

        if (result == null)
        {
            NotifyError("Contract not found.");
            return NotFound(CustomResponse());
        }
        return CustomResponse(result); 
    }

    // ============================================================
    // PUT /api/v1/contracts/{id}/terminate
    // ============================================================
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}/terminate")]
    [SwaggerOperation(Summary = "Encerra (termina) um contrato existente")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ContractingService.Api.SwaggerExamples.TerminateContractResponse200Example))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ContractingService.Api.SwaggerExamples.TerminateContractResponse404Example))]
    public async Task<IActionResult> Terminate(Guid id) =>
        CustomResponse(await _contractAppService.TerminateAsync(id)); 
}
