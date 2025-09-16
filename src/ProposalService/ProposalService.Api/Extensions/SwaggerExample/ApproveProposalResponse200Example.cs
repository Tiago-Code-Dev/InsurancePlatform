using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ProposalService.Api.SwaggerExamples
{
    public class ApproveProposalResponse200Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Ok(new { message = "Proposal approved successfully." });
        }
    }

    public class ApproveProposalResponse404Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail("Proposal not found.");
        }
    }

    public class RejectProposalResponse200Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Ok(new { message = "Proposal rejected successfully." });
        }
    }

    public class RejectProposalResponse404Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail("Proposal not found.");
        }
    }
}
