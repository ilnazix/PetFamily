using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Consumers;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Messaging;
using PetFamily.Accounts.Infrastructure.Options.Admin;
using PetFamily.Accounts.Infrastructure.Options.Jwt;
using PetFamily.Accounts.Infrastructure.Options.RefreshSession;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Infrastructure.Utilities;
using PetFamily.Core.Database;
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
            .AddMessageBus(configuration)
            .AddOptions()
            .AddProviders()
            .AddSeeding()
            .AddManagers();

        services.AddScoped<IDbMigrator, AccountsDbMigrator>();
        return services;
    }

    private static IServiceCollection AddMessageBus(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddMassTransit<IAccountsBus>(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<VolunteerRequestApprovedEventConsumer>()
                .Endpoint(x => x.InstanceId = "accounts");

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:HostName"]!), cfg =>
                {
                    cfg.Username(configuration["RabbitMQ:UserName"]!);
                    cfg.Password(configuration["RabbitMQ:Password"]!);
                });
                cfg.Durable = true;
                cfg.ConfigureEndpoints(context);
            });
        });

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
            .AddEntityFrameworkStores<AccountsWriteDbContext>();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AccountsWriteDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<IAccountsReadDbContext, AccountsReadDbContext>(options =>
        {
            options
                   .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                   .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                   .UseSnakeCaseNamingConvention()
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<RefreshSessionManager>();

        return services;
    }
}
