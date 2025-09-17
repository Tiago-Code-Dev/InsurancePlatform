using Shared.CrossCutting.Response;

namespace ContractingService.Tests.Unit.Support
{
    internal static class ResponseDumpExtensions
    {
        public static string Dump<T>(this CustomResponse<T> resp)
            => (resp.Messages == null || resp.Messages.Count == 0)
                ? "No messages"
                : string.Join(" | ", resp.Messages.Select(m => m.Message));
    }
}
