using ContractingService.Api.Contracts.Requests;
using ContractingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CrossCutting.Auth;
using Shared.CrossCutting.Controllers;
using Shared.CrossCutting.Notifications;

namespace ContractingService.API.Controllers;

[Route("api/v1/contracts")]
public class ContractsController : MainController
{
    private readonly IContractAppService _contractAppService;

    public ContractsController(
        IContractAppService contractAppService,
        INotifier notifier) : base(notifier)
    {
        _contractAppService = contractAppService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _contractAppService.CreateAsync(request.Insured, request.Coverages);

        return CustomResponse(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _contractAppService.GetByIdAsync(id);

        if (result == null)
        {
            NotifyError("Contract not found.");
            return CustomResponse();
        }

        return CustomResponse(result); 
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}/terminate")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        var result = await _contractAppService.TerminateAsync(id);
        return CustomResponse(result); 
    }
}
