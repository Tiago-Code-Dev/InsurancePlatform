namespace ProposalService.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Application.Interfaces;

[ApiController]
[Route("api/v1/proposals")]
public class ProposalController : ControllerBase
{
    private readonly IProposalAppService _proposalAppService;

    public ProposalController(IProposalAppService proposalAppService)
    {
        _proposalAppService = proposalAppService;
    }

    // POST /api/v1/proposals
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProposalRequest request)
    {
        var result = await _proposalAppService.CreateAsync(request.Customer, request.Contract);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET /api/v1/proposals/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _proposalAppService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    // PUT /api/v1/proposals/{id}/approve
    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _proposalAppService.ApproveAsync(id);
        return NoContent();
    }

    // PUT /api/v1/proposals/{id}/reject
    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _proposalAppService.RejectAsync(id);
        return NoContent();
    }
}
