namespace ContractingService.Application.Services;

using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;
using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Enums;

public class ContractAppService : IContractAppService
{
    private readonly IContractRepository _contractRepository;

    public ContractAppService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<ContractDto> CreateAsync(InsuredDto insuredDto, List<CoverageDto> coverageDtos)
    {
        var insured = new Insured(insuredDto.Name, new Document(insuredDto.Document), new Email(insuredDto.Email));
        var contract = new Contract(insured);

        foreach (var coverageDto in coverageDtos)
        {
            var coverage = new Coverage(coverageDto.Name, Enum.Parse<CoverageType>(coverageDto.Type), new Money(coverageDto.Premium));
            contract.AddCoverage(coverage);
        }

        await _contractRepository.AddAsync(contract);

        return MapToDto(contract);
    }

    public async Task<ContractDto?> GetByIdAsync(Guid id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        return contract == null ? null : MapToDto(contract);
    }

    public async Task ActivateAsync(Guid contractId)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null) return;

        contract.Activate();
        await _contractRepository.UpdateAsync(contract);
    }

    public async Task CancelAsync(Guid contractId)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null) return;

        contract.Cancel();
        await _contractRepository.UpdateAsync(contract);
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

    public async Task TerminateAsync(Guid contractId)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract is null)
            throw new KeyNotFoundException("Contract not found.");

        contract.Terminate();
        await _contractRepository.UpdateAsync(contract);
    }


    public Task CreateAsync(ContractDto contract)
    {
        throw new NotImplementedException();
    }
}
