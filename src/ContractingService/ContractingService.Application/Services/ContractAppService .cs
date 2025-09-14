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

    public ContractAppService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<CustomResponse<ContractDto>> CreateAsync(InsuredDto insuredDto, List<CoverageDto> coverageDtos)
    {
        try
        {
            var insured = new Insured(insuredDto.Name, new Document(insuredDto.Document), new Email(insuredDto.Email));
            var contract = new Contract(insured);

            foreach (var coverageDto in coverageDtos)
            {
                var coverage = new Coverage(coverageDto.Name, Enum.Parse<CoverageType>(coverageDto.Type), new Money(coverageDto.Premium));
                contract.AddCoverage(coverage);
            }

            await _contractRepository.AddAsync(contract);

            return CustomResponse<ContractDto>.Created(MapToDto(contract));
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

            if (contract is null)
                return CustomResponse<ContractDto>.Fail("Contract not found.");

            return CustomResponse<ContractDto>.Ok(MapToDto(contract));
        }
        catch (Exception)
        {
            return CustomResponse<ContractDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<Result>> ActivateAsync(Guid contractId)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(contractId);

            if (contract is null)
                return CustomResponse<Result>.Fail("Contract not found.");

            contract.Activate();
            await _contractRepository.UpdateAsync(contract);

            return CustomResponse<Result>.Ok(Result.Ok("Contract activated successfully."));
        }
        catch (Exception)
        {
            return CustomResponse<Result>.InternalServerError();
        }
    }

    public async Task<CustomResponse<Result>> CancelAsync(Guid contractId)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(contractId);

            if (contract is null)
                return CustomResponse<Result>.Fail("Contract not found.");

            contract.Cancel();
            await _contractRepository.UpdateAsync(contract);

            return CustomResponse<Result>.Ok(Result.Ok("Contract canceled successfully."));
        }
        catch (Exception)
        {
            return CustomResponse<Result>.InternalServerError();
        }
    }

    public async Task<CustomResponse<Result>> TerminateAsync(Guid contractId)
    {
        try
        {
            var contract = await _contractRepository.GetByIdAsync(contractId);

            if (contract is null)
                return CustomResponse<Result>.Fail("Contract not found.");

            contract.Terminate();
            await _contractRepository.UpdateAsync(contract);

            return CustomResponse<Result>.Ok(Result.Ok("Contract terminated successfully."));
        }
        catch (Exception)
        {
            return CustomResponse<Result>.InternalServerError();
        }
    }

    private static ContractDto MapToDto(Contract contract) =>
        new()
        {
            Id = contract.Id,
            InsuredName = contract.Insured.Name,
            InsuredDocument = contract.Insured.Document.Number,
            InsuredEmail = contract.Insured.Email.Address,
            Coverages = contract.Coverages.Select(c => new CoverageDto
            {
                Name = c.Name,
                Type = c.Type.ToString(),
                Premium = c.Premium.Amount
            }).ToList(),
            Status = contract.Status.ToString()
        };
}
