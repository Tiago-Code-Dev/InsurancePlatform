using Polly;
using Polly.Extensions.Http;

namespace ProposalService.Api.Extensions;

public static class HttpClientResilienceExtensions
{
    public static IServiceCollection AddResilientHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("ResilientClient")
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddHttpClient("ResilientClientWithTimeout")
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5))
            .AddPolicyHandler(Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"message\":\"Service temporarily unavailable, using fallback\"}")
                }));

        return services;
    }
}
