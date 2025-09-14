namespace ProposalService.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProposalService.Application.DTOs;
using ProposalService.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IProposalAppService _proposalAppService;

    public ProposalController(IProposalAppService proposalAppService)
    {
        _proposalAppService = proposalAppService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerDto customer, [FromBody] ContractDto contract)
    {
        var result = await _proposalAppService.CreateAsync(customer, contract);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _proposalAppService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _proposalAppService.ApproveAsync(id);
        return NoContent();
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _proposalAppService.RejectAsync(id);
        return NoContent();
    }
}
