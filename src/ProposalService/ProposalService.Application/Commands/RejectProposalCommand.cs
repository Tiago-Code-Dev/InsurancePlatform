namespace ProposalService.Application.Commands;

public class RejectProposalCommand
{
    public Guid ProposalId { get; }

    public RejectProposalCommand(Guid proposalId)
    {
        ProposalId = proposalId;
    }
}
