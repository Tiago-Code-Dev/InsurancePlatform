namespace ProposalService.Application.Commands;

using ProposalService.Application.DTOs;

public class CreateProposalCommand
{
    public CustomerDto Customer { get; set; }
    public ContractDto Contract { get; set; }

    public CreateProposalCommand(CustomerDto customer, ContractDto contract)
    {
        Customer = customer;
        Contract = contract;
    }
}
