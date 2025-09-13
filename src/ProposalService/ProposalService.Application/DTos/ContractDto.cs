namespace ProposalService.Application.DTOs;

using System;

public class ContractDto
{
    public string Type { get; set; } = string.Empty;
    public decimal Premium { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
