namespace ContractingService.Api.Contracts.Requests;

using ContractingService.Application.DTOs;

public record CreateContractRequest(InsuredDto Insured, List<CoverageDto> Coverages);
