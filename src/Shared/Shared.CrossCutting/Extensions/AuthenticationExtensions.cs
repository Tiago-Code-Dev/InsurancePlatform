namespace Shared.CrossCutting.Extensions;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.CrossCutting.Auth;

public static class AuthenticationExtensions
{
    /// <summary>
    /// Lê a seção "Jwt" do appsettings, registra JwtSettings,
    /// configura AddAuthentication/AddJwtBearer e AddAuthorization.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(section);

        var settings = section.Get<JwtSettings>() ?? new JwtSettings();
        var keyBytes = Encoding.UTF8.GetBytes(settings.Key);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // habilite true em prod
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.Zero // tokens expiram no minuto exato
                };
            });

        services.AddAuthorization();

        // Serviço para emissão de tokens
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
