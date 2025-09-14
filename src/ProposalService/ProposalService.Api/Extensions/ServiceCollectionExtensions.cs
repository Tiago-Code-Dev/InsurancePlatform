namespace ProposalService.Api.Extensions;

using Microsoft.EntityFrameworkCore;
using ProposalService.Application.Interfaces;
using ProposalService.Application.Services;
using ProposalService.Domain.Interfaces;
using ProposalService.Infrastructure.Context;
using ProposalService.Infrastructure.Repositories;
using Shared.CrossCutting.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProposalServices(this IServiceCollection services, IConfiguration cfg)
    {
        var conn = cfg.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(conn))
        {
            services.AddDbContext<ProposalDbContext>(o => o.UseInMemoryDatabase("ProposalDb"));
        }
        else
        {
            services.AddDbContext<ProposalDbContext>(o => o.UseSqlServer(conn));
        }
        
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<IProposalAppService, ProposalAppService>();

        return services;
    }
}
