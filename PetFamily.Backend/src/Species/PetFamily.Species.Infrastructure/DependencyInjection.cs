using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Database;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.Species.Commands;
using PetFamily.Species.Infrastructure.Database;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Species.Infrastructure.Repositories;
using PetFamily.Species.Infrastructure.Utilities;

namespace PetFamily.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration);

        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<ISpeciesUnitOfWork, SpeciesUnitOfWork>();

        services.AddScoped<IDbMigrator, SpeciesDbMigrator>();

        return services;
    }

    private static IServiceCollection AddDbContexts(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddDbContext<SpeciesWriteDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<ISpeciesReadDbContext, SpeciesReadDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
