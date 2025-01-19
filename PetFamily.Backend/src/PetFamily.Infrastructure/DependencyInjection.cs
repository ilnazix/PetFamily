using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<DeleteExpiredVolunteersService>();
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddHostedService<DeleteExpiredVolunteersBackgroundService>();

            return services;
        }
    }
}
