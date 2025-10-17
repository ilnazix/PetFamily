using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.VolunteerRequest.Application;
using PetFamily.VolunteerRequest.Infrastructure;

namespace PetFamily.VolunteerRequest.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddInfrastructure(configuration);

        return services;
    } 
}
