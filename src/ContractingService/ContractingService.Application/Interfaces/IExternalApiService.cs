namespace ContractingService.Application.Interfaces

{
    public interface IExternalApiService
    {
        Task<string> GetDataAsync();
    }
}
