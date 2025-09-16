namespace ContractingService.Application.Services;

using ContractingService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Proposals;
using System.Net.Http;
using System.Net.Http.Json;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalApiService> _logger;


    public ExternalApiService(HttpClient httpClient, IConfiguration configuration,
        ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ProposalResponse?> GetProposalDetailsAsync(Guid proposalId)
    {
        var url = $"/api/v1/proposals/{proposalId}";
        _logger.LogInformation("Requesting proposal {ProposalId} from ProposalService at {Url}", proposalId, url);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var response = await _httpClient.GetAsync(url);
        sw.Stop();

        _logger.LogInformation("Response {StatusCode} for proposal {ProposalId} in {Elapsed} ms",
            response.StatusCode, proposalId, sw.ElapsedMilliseconds);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch proposal {ProposalId} - StatusCode {StatusCode}",
                proposalId, response.StatusCode);
            return null;
        }

        var proposal = await response.Content.ReadFromJsonAsync<ProposalResponse>();
        _logger.LogDebug("Proposal {ProposalId} received with status {Status}", proposal?.Id, proposal?.Status);

        return proposal;
    }
}
