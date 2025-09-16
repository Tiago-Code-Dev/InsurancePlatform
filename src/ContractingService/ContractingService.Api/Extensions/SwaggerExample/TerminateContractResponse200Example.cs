using Shared.CrossCutting.Response;
using Swashbuckle.AspNetCore.Filters;

namespace ContractingService.Api.SwaggerExamples
{
    public class TerminateContractResponse200Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Ok(new { message = "Contract terminated successfully." });
        }
    }

    public class TerminateContractResponse404Example : IExamplesProvider<CustomResponse<object>>
    {
        public CustomResponse<object> GetExamples()
        {
            return CustomResponse<object>.Fail("Contract not found.");
        }
    }
}
