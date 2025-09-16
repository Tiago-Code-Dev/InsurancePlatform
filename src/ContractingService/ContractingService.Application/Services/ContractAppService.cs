using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.Interfaces;
using ContractingService.Domain.ValueObjects;
using Shared.CrossCutting.Response;

namespace ContractingService.Application.Services;

public class ContractAppService : IContractAppService
{
    private readonly IContractRepository _contractRepository;
    private readonly IExternalApiService _externalApiService;

    public ContractAppService(IContractRepository contractRepository, IExternalApiService externalApiService)
    {
        _contractRepository = contractRepository;
        _externalApiService = externalApiService;
    }

    public async Task<CustomResponse<ContractDto>> CreateAsync(InsuredDto insuredDto, List<CoverageDto> coverageDtos)
    {
        try
        {

            var proposal = await _externalApiService.GetProposalDetailsAsync(insuredDto.ProposalId);

            if (proposal is null)
                return CustomResponse<ContractDto>.Fail("Proposal not found.");

            if (!proposal.Status.Equals("Aprovada", StringComparison.OrdinalIgnoreCase))
                return CustomResponse<ContractDto>.Fail("The proposal is not approved for contracting.");

            var insured = new Insured(insuredDto.Name, new Document(insuredDto.Document), new Email(insuredDto.Email), insuredDto.ProposalId);
            var contract = new Contract(insured);

            foreach (var coverageDto in coverageDtos)
            {
                if (!Enum.TryParse<CoverageType>(coverageDto.Type, out var type))
                    return CustomResponse<ContractDto>.Fail($"Invalid coverage type: {coverageDto.Type}");

                var coverage = new Coverage(coverageDto.Name, type, new Money(coverageDto.Premium));
                contract.AddCoverage(coverage);
            }

            await _contractRepository.AddAsync(contract);

            return CustomResponse<ContractDto>.Created(MapToDto(contract));
        }
        catch (ArgumentException ex)
        {
            return CustomResponse<ContractDto>.Fail(ex.Message);
        }
        catch (Exception)
        {
            return CustomResponse<ContractDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<ContractDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);

            return (contract is null)
                ? CustomResponse<ContractDto>.Fail("Contract not found.")
                : CustomResponse<ContractDto>.Ok(MapToDto(contract));
        }
        catch (Exception)
        {
            return CustomResponse<ContractDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<Result>> ActivateAsync(Guid contractId) =>
      await ChangeContractStateAsync(contractId, c => c.Activate(), "Contract activated successfully.");

    public async Task<CustomResponse<Result>> CancelAsync(Guid contractId) =>
        await ChangeContractStateAsync(contractId, c => c.Cancel(), "Contract canceled successfully.");

    public async Task<CustomResponse<Result>> TerminateAsync(Guid contractId) =>
        await ChangeContractStateAsync(contractId, c => c.Terminate(), "Contract terminated successfully.");


    private async Task<CustomResponse<Result>> ChangeContractStateAsync(Guid id, Action<Contract> action, string successMessage)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract is null)
                return CustomResponse<Result>.Fail("Contract not found.");

            action(contract);
            await _contractRepository.UpdateAsync(contract);

            return CustomResponse<Result>.Ok(Result.Ok(successMessage));
        }
        catch (Exception)
        {
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
