using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;

namespace PetFamily.Species.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSpeciesModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddApplication()
                .AddInfrastructure(configuration);

            return services;
        }
    }
}
