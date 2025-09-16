using System.ComponentModel.DataAnnotations;

namespace ContractingService.Domain.Enums;

public enum ContractStatus
{
    Draft = 0,
    Active = 1,
    Canceled = 2,
    Terminated = 3
}
