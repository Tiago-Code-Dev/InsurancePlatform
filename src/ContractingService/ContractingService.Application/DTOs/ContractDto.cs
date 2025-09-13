namespace ContractingService.Application.DTOs;

using System;
using System.Collections.Generic;

public class ContractDto
{
    public Guid Id { get; set; }
    public string InsuredName { get; set; } = string.Empty;
    public string InsuredDocument { get; set; } = string.Empty;
    public string InsuredEmail { get; set; } = string.Empty;
    public List<CoverageDto> Coverages { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}
