using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Database;
using PetFamily.Discussions.Application.Commands;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Infrastructure.Database;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Discussions.Infrastructure.Repositories;
using PetFamily.Discussions.Infrastructure.Utilities;

namespace PetFamily.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddDbContexts(configuration);

        services.AddScoped<IDbMigrator, DiscussionsDbMigrator>();

        return services;
    }

    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DiscussionsWriteDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<IDiscussionsReadDbContext,  DiscussionsReadDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();


        services.AddScoped<IDiscussionsUnitOfWork, DiscussionsUnitOfWork>();

        return services;
    }
}
