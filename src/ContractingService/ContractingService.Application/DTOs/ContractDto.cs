namespace ContractingService.Application.DTOs;

using System;
using System.Collections.Generic;

public record ContractDto(
    Guid Id,
    string InsuredName,
    string InsuredDocument,
    string InsuredEmail,
    List<CoverageDto> Coverages,
    string Status,
    Guid ProposalId);
