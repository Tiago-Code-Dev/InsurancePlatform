namespace ContractingService.Application.Interfaces;

using ContractingService.Application.DTOs;

public interface IContractAppService
{
    Task<ContractDto> CreateAsync(InsuredDto insured, List<CoverageDto> coverages);
    Task<ContractDto?> GetByIdAsync(Guid id);
    Task ActivateAsync(Guid contractId);
    Task CancelAsync(Guid contractId);
    Task TerminateAsync(Guid id);
    Task CreateAsync(ContractDto contract);
}
