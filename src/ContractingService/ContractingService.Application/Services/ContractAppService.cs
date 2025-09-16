using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.Interfaces;
using ContractingService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.CrossCutting.Response;

namespace ContractingService.Application.Services;

public class ContractAppService : IContractAppService
{
    private readonly IContractRepository _contractRepository;
    private readonly IExternalApiService _externalApiService;
    private readonly ILogger<ContractAppService> _logger;


    public ContractAppService(IContractRepository contractRepository, IExternalApiService externalApiService,
        ILogger<ContractAppService> logger)
    {
        _contractRepository = contractRepository;
        _externalApiService = externalApiService;
        _logger = logger;
    }

    public async Task<CustomResponse<ContractDto>> CreateAsync(InsuredDto insuredDto, List<CoverageDto> coverageDtos)
    {
        _logger.LogInformation("Starting contract creation for insured {InsuredName}, document {Document}",
            insuredDto.Name, insuredDto.Document);

        try
        {
            var insured = new Insured(insuredDto.Name,
                                      new Document(insuredDto.Document),
                                      new Email(insuredDto.Email), insuredDto.ProposalId);

            var contract = new Contract(insured);

            foreach (var coverageDto in coverageDtos)
            {
                if (!Enum.TryParse<CoverageType>(coverageDto.Type, out var type))
                {
                    _logger.LogWarning("Invalid coverage type {CoverageType} for insured {InsuredName}",
                        coverageDto.Type, insuredDto.Name);

                    return CustomResponse<ContractDto>.Fail($"Invalid coverage type: {coverageDto.Type}", "Type");
                }

                var coverage = new Coverage(coverageDto.Name, type, new Money(coverageDto.Premium));
                contract.AddCoverage(coverage);
            }

            await _contractRepository.AddAsync(contract);

            _logger.LogInformation("Contract {ContractId} created successfully for insured {InsuredName}",
                contract.Id, insuredDto.Name);

            return CustomResponse<ContractDto>.Created(MapToDto(contract));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating contract for insured {InsuredName}", insuredDto.Name);
            return CustomResponse<ContractDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<ContractDto>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Retrieving contract {ContractId}", id);

        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);

            if (contract is null)
            {
                _logger.LogWarning("Contract {ContractId} not found", id);
                return CustomResponse<ContractDto>.Fail("Contract not found.");
            }

            return CustomResponse<ContractDto>.Ok(MapToDto(contract));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving contract {ContractId}", id);
            return CustomResponse<ContractDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<Result>> ActivateAsync(Guid contractId) =>
      await ChangeContractStateAsync(contractId, c => c.Activate(), "Contract activated successfully.");

    public async Task<CustomResponse<Result>> CancelAsync(Guid contractId) =>
        await ChangeContractStateAsync(contractId, c => c.Cancel(), "Contract canceled successfully.");

    public async Task<CustomResponse<Result>> TerminateAsync(Guid contractId) =>
        await ChangeContractStateAsync(contractId, c => c.Terminate(), "Contract terminated successfully.");


    private async Task<CustomResponse<Result>> ChangeContractStateAsync(Guid id, Action<Contract> action, string actionName)
    {
        _logger.LogInformation("Changing state of contract {ContractId} to {Action}", id, actionName);

        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract is null)
            {
                _logger.LogWarning("Attempt to {Action} contract {ContractId} failed: not found", actionName, id);
                return CustomResponse<Result>.Fail("Contract not found.");
            }

            action(contract);
            await _contractRepository.UpdateAsync(contract);

            _logger.LogInformation("Contract {ContractId} {Action} successfully", contract.Id, actionName);

            return CustomResponse<Result>.Ok(Result.Ok($"Contract {actionName} successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while changing contract {ContractId} state to {Action}", id, actionName);
            return CustomResponse<Result>.InternalServerError();
        }
    }

    //public async Task<CustomResponse<Result>> ActivateAsync(Guid contractId)
    //{
    //    try
    //    {
    //        var contract = await _contractRepository.GetByIdAsync(contractId);

    //        if (contract is null)
    //            return CustomResponse<Result>.Fail("Contract not found.");

    //        contract.Activate();
    //        await _contractRepository.UpdateAsync(contract);

    //        return CustomResponse<Result>.Ok(Result.Ok("Contract activated successfully."));
    //    }
    //    catch (Exception)
    //    {
    //        return CustomResponse<Result>.InternalServerError();
    //    }
    //}


    //public async Task<CustomResponse<Result>> CancelAsync(Guid contractId)
    //{
    //    try
    //    {
    //        var contract = await _contractRepository.GetByIdAsync(contractId);

    //        if (contract is null)
    //            return CustomResponse<Result>.Fail("Contract not found.");

    //        contract.Cancel();
    //        await _contractRepository.UpdateAsync(contract);

    //        return CustomResponse<Result>.Ok(Result.Ok("Contract canceled successfully."));
    //    }
    //    catch (Exception)
    //    {
    //        return CustomResponse<Result>.InternalServerError();
    //    }
    //}

    //public async Task<CustomResponse<Result>> TerminateAsync(Guid contractId)
    //{
    //    try
    //    {
    //        var contract = await _contractRepository.GetByIdAsync(contractId);

    //        if (contract is null)
    //            return CustomResponse<Result>.Fail("Contract not found.");

    //        contract.Terminate();
    //        await _contractRepository.UpdateAsync(contract);

    //        return CustomResponse<Result>.Ok(Result.Ok("Contract terminated successfully."));
    //    }
    //    catch (Exception)
    //    {
    //        return CustomResponse<Result>.InternalServerError();
    //    }
    //}

    private static ContractDto MapToDto(Contract contract) =>
         new(
             Id: contract.Id,
             InsuredName: contract.Insured.Name,
             InsuredDocument: contract.Insured.Document.Number,
             InsuredEmail: contract.Insured.Email.Address,
             Coverages: contract.Coverages.Select(c => new CoverageDto(
                 c.Name,
                 c.Type.ToString(),
                 c.Premium.Amount
             )).ToList(),
             Status: contract.Status.ToString(),
             ProposalId: contract.Insured.ProposalId
         );
}
