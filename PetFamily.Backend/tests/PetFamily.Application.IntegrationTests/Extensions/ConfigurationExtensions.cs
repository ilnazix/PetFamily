using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Application.IntegrationTests.Extensions;
public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureVolunteersModule(
        this IServiceCollection services, 
        string dbConnectionString)
    {
        var writeDbContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(VolunteersWriteDbContext));

        if (writeDbContext is not null)
            services.Remove(writeDbContext);

        var readDbContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(VolunteersReadDbContext));

        if (readDbContext is not null)
            services.Remove(readDbContext);

        services.AddDbContext<VolunteersWriteDbContext>(options =>
        {
            options
                .UseNpgsql(dbConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<VolunteersReadDbContext>(options =>
        {
            options
                .UseNpgsql(dbConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();

        context.Database.Migrate();
        return services;
    }
}
