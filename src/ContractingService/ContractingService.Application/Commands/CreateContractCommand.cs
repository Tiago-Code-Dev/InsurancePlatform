namespace ContractingService.Application.Commands;

using ContractingService.Application.DTOs;
using System.Collections.Generic;

public record CreateContractCommand(InsuredDto Insured, List<CoverageDto> Coverages);
