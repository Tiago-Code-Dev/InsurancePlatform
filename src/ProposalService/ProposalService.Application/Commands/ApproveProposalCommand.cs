namespace ProposalService.Application.Commands;

public class ApproveProposalCommand
{
    public Guid ProposalId { get; }

    public ApproveProposalCommand(Guid proposalId)
    {
        ProposalId = proposalId;
    }
}
