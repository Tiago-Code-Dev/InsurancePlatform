namespace ContractingService.Api.Extensions;

using ContractingService.Application.Interfaces;
using ContractingService.Application.Services;
using ContractingService.Domain.Interfaces;
using ContractingService.Infrastructure.Context;
using ContractingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.CrossCutting.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContractingServices(this IServiceCollection services, IConfiguration cfg)
    {
        var conn = cfg.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(conn))
        {
            services.AddDbContext<ContractDbContext>(o => o.UseInMemoryDatabase("ContractDb"));
        }
        else
        {
            services.AddDbContext<ContractDbContext>(o => o.UseSqlServer(conn));
        }

        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IContractAppService, ContractAppService>();

        return services;
    }
}
