using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Options.Jwt;
using PetFamily.Accounts.Infrastructure.Options.JwtBearer;
using PetFamily.Accounts.Infrastructure.Providers;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddIdentity()
            .AddAuth()
            .AddDbContext(configuration)
            .AddOptions()
            .AddProviders();

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
        
        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddProviders(
       this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();

        return services;
    }
}
