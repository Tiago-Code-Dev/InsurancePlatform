namespace ProposalService.Application.Interfaces

{
    public interface IExternalApiService
    {
        Task<string> GetDataAsync();
    }
}
