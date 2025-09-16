using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using Shared.CrossCutting.Messaging.Events;

namespace ContractingService.Application.EventHandlers;

public class ProposalCreatedEventHandler
{
    private readonly IContractAppService _contractAppService;

    public ProposalCreatedEventHandler(IContractAppService contractAppService)
    {
        _contractAppService = contractAppService;
    }

    public async Task HandleAsync(ProposalCreatedEvent evt)
    {
        var insured = new InsuredDto(evt.CustomerName, evt.CustomerDocument, string.Empty, evt.ProposalId);
        var coverages = new List<CoverageDto>();

        await _contractAppService.CreateAsync(insured, coverages);
    }
}
