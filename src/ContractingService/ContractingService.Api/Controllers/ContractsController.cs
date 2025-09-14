namespace ContractingService.Api.Controllers;

using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractAppService _contractAppService;

    public ContractsController(IContractAppService contractAppService)
    {
        _contractAppService = contractAppService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
    {
        var result = await _contractAppService.CreateAsync(
            request.Insured,
            request.Coverages ?? new List<CoverageDto>()
        );

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _contractAppService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _contractAppService.ActivateAsync(id);
        return NoContent();
    }

    [HttpPut("{id:guid}/terminate")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        await _contractAppService.TerminateAsync(id);
        return NoContent();
    }
}
