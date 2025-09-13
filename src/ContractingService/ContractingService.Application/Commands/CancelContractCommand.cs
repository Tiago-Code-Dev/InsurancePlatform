namespace ContractingService.Application.Commands;

public class CancelContractCommand
{
    public Guid ContractId { get; }

    public CancelContractCommand(Guid contractId)
    {
        ContractId = contractId;
    }
}
