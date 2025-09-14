using ContractingService.Api.Extensions;
using ContractingService.Api.Validation;
using ContractingService.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Shared.CrossCutting.Middleware;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Services
builder.Services.AddContractingServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<InsuredDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.Run();
