using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddCommandHandlers()
                .AddQueryHandlers()
                .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            
            return services;
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }
        
        private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
