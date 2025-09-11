using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options.Admin;
using PetFamily.Accounts.Infrastructure.Options.Jwt;
using PetFamily.Accounts.Infrastructure.Options.RefreshSession;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Accounts.Infrastructure.Seeding;
using System.Text;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddIdentity()
            .AddPermissionAuthentication(configuration)
            .AddDbContext(configuration)
            .AddOptions()
            .AddProviders()
            .AddSeeding()
            .AddManagers();

        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options =>
                 {
                     options.User.RequireUniqueEmail = true;
                 }
             )
            .AddEntityFrameworkStores<AccountsDbContext>();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AccountsDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
        });

        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<AdminOptionsSetup>();
        services.ConfigureOptions<RefreshSessionOptionsSetup>();
        
        return services;
    }

    private static IServiceCollection AddPermissionAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                var _jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>()
                    ?? throw new ApplicationException("Missing jwt options");

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = _jwtOptions.ValidateIssuer,
                    ValidateAudience = _jwtOptions.ValidateAudience,
                    ValidateIssuerSigningKey = _jwtOptions.ValidateIssuerSigningKey,
                    ValidateLifetime = _jwtOptions.ValidateLifetime,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
                };
            });

        return services;
    }

    private static IServiceCollection AddProviders(
       this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();

        return services;
    }

    private static IServiceCollection AddSeeding(
       this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();

        return services;
    }

    private static IServiceCollection AddManagers(
       this IServiceCollection services)
    {
        services.AddScoped<IPermissionManager, PermissionManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<RefreshSessionManager>();

        return services;
    }
}
