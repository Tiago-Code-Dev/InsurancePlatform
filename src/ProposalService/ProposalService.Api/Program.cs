using ProposalService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
app.ConfigureMiddleware(app.Environment);
app.ConfigureEndpoints();

if (builder.Configuration.GetValue<bool>("HttpsRedirection:Enabled"))
{
    app.UseHttpsRedirection();
}

app.Run();
