using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerRequest.Application.Commands;
using PetFamily.VolunteerRequest.Infrastructure.Database;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using PetFamily.VolunteerRequest.Infrastructure.Repositories;
using static CSharpFunctionalExtensions.Result;

namespace PetFamily.VolunteerRequest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddDbContexts(configuration);

        return services;
    }

    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<VolunteerRequestsWriteDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
        });

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>();


        services.AddScoped<IVolunteerRequestUnitOfWork, VolunteerRequestsUnitOfWork>();

        return services;
    }
}

