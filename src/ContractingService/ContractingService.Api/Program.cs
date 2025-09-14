using ContractingService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware(app.Environment);
app.ConfigureEndpoints();

app.Run();
