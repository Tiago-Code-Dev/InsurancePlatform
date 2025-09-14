namespace ContractingService.Api.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapControllers();
        app.MapHealthChecks("/health");

        return app;
    }
}
