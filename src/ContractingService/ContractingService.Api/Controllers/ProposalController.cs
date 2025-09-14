namespace ContractingService.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;

[ApiController]
[Route("api/v1/contracts")]
public class ContractController : ControllerBase
{
    private readonly IContractAppService _contractAppService;

    public ContractController(IContractAppService contractAppService)
    {
        _contractAppService = contractAppService;
    }

    // POST /api/v1/contracts
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContractDto contract)
    {
        var result = await _contractAppService.CreateAsync(contract);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET /api/v1/contracts/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _contractAppService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    // PUT /api/v1/contracts/{id}/activate
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _contractAppService.ActivateAsync(id);
        return NoContent();
    }

    // PUT /api/v1/contracts/{id}/terminate
    [HttpPut("{id:guid}/terminate")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        await _contractAppService.TerminateAsync(id);
        return NoContent();
    }
}
