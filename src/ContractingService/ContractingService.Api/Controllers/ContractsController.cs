using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CrossCutting.Auth;

[ApiController]
[Route("api/v1/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractAppService _contractAppService;

    public ContractsController(IContractAppService contractAppService)
        => _contractAppService = contractAppService;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
    {
        var result = await _contractAppService.CreateAsync(request.Insured, request.Coverages);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _contractAppService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}/terminate")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        await _contractAppService.TerminateAsync(id);
        return NoContent();
    }
}
