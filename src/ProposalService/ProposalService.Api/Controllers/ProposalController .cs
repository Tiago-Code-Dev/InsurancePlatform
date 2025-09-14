using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Application.Interfaces;
using Shared.CrossCutting.Auth;

[ApiController]
[Route("api/v1/proposals")]
public class ProposalsController : ControllerBase
{
    private readonly IProposalAppService _proposalAppService;

    public ProposalsController(IProposalAppService proposalAppService)
        => _proposalAppService = proposalAppService;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProposalRequest request)
    {
        var result = await _proposalAppService.CreateAsync(request.Customer, request.Contract);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _proposalAppService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _proposalAppService.ApproveAsync(id);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _proposalAppService.RejectAsync(id);
        return NoContent();
    }
}
