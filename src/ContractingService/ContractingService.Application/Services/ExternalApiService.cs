namespace ContractingService.Application.Services;

using ContractingService.Application.Interfaces;

public class ExternalApiService : IExternalApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetDataAsync()
    {
        var client = _httpClientFactory.CreateClient("ResilientClient");
        var response = await client.GetAsync("https://api.externa.com/data");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
