using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts;
using PetFamily.Species.Infrastructure;

namespace PetFamily.Species.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddInfrastructure(configuration);

        services.AddScoped<ISpeciesModule, SpeciesModule>();

        return services;
    }
}
