namespace ContractingService.Application.Commands;

using ContractingService.Application.DTOs;
using System.Collections.Generic;

public class CreateContractCommand
{
    public InsuredDto Insured { get; }
    public List<CoverageDto> Coverages { get; }

    public CreateContractCommand(InsuredDto insured, List<CoverageDto> coverages)
    {
        Insured = insured;
        Coverages = coverages;
    }
}
