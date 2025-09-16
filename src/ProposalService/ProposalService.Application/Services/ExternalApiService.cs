using ProposalService.Application.Interfaces;

namespace ProposalService.Application.Services

{
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

            if (!response.IsSuccessStatusCode)
            {
                return $"{{\"success\": false, \"messages\": [{{\"message\": \"API error: {response.StatusCode}\"}}]}}";
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
