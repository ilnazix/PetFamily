using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Infrastructure;

namespace PetFamily.Discussions.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddInfrastructure(configuration);

        services.AddScoped<IDiscussionsModule, DiscussionsModule>();

        return services;
    }
}
