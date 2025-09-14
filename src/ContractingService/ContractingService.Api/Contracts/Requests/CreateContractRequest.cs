namespace ContractingService.Api.Contracts.Requests;

using ContractingService.Application.DTOs;

public class CreateContractRequest
{
    public InsuredDto Insured { get; set; } = default!;
    public List<CoverageDto> Coverages { get; set; } = new();
}
