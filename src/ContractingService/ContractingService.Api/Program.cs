using ContractingService.Api.Extensions;
using ContractingService.Api.Validation;
using ContractingService.Application.Services;
using ContractingService.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Shared.CrossCutting.Extensions;
using Shared.CrossCutting.Middleware;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Services
builder.Services.AddContractingServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddJwtAuthentication(builder.Configuration); // JWT

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<InsuredDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractRequestValidator>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Insurance Platform - Contracting Service", Version = "v1" });

    // autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpClient("ResilientClient")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))) // Retry com backoff exponencial
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); // Circuit breaker

builder.Services.AddHttpClient("ResilientClientWithTimeout")
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5)) // timeout de 5 segundos
    .AddPolicyHandler(Policy<HttpResponseMessage>
        .Handle<Exception>()
        .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"message\":\"Service temporarily unavailable, using fallback\"}")
        }));


builder.Services.AddHealthChecks(); //healthcheck

builder.Services.AddScoped<ExternalApiService>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.MapHealthChecks("/health"); // health

app.Run();
