namespace ContractingService.Application.Interfaces;

using ContractingService.Application.DTOs;
using Shared.CrossCutting.Response;

public interface IContractAppService
{
    Task<CustomResponse<ContractDto>> CreateAsync(InsuredDto insured, List<CoverageDto> coverages);
    Task<CustomResponse<ContractDto>> GetByIdAsync(Guid id);
    Task<CustomResponse<Result>> ActivateAsync(Guid contractId);
    Task<CustomResponse<Result>> CancelAsync(Guid contractId);
    Task<CustomResponse<Result>> TerminateAsync(Guid id);

}
