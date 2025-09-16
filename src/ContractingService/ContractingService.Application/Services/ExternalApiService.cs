namespace ContractingService.Application.Services;

using ContractingService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.Proposals;
using System.Net.Http.Json;

public class ExternalApiService : IExternalApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExternalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<ProposalResponse?> GetProposalDetailsAsync(Guid proposalId)
    {
        var client = _httpClientFactory.CreateClient("ProposalService");
        var response = await client.GetAsync($"/api/v1/proposals/{proposalId}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ProposalResponse>(); ;
    }
}
