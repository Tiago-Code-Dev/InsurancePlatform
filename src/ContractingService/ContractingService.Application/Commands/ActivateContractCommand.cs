namespace ContractingService.Application.Commands;

public class ActivateContractCommand
{
    public Guid ContractId { get; }

    public ActivateContractCommand(Guid contractId)
    {
        ContractId = contractId;
    }
}
