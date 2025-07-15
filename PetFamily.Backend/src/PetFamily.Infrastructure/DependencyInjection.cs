using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.Species.Commands;
using PetFamily.Application.Volunteers.Commands;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;
using static CSharpFunctionalExtensions.Result;

namespace PetFamily.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddMinio(configuration)
                .AddHostedServices()
                .AddMessaging();

            services.AddScoped<DeleteExpiredVolunteersService>();
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
           

            return services;
        }

        private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

            services.AddMinio(options =>
            {
                var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                    ?? throw new ApplicationException("Missing minio configuration");

                options.WithEndpoint(minioOptions.Endpoint);
                options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
                options.WithSSL(minioOptions.WithSSL);
            });

            services.AddScoped<IFilesProvider, MinioProvider>();

            return services;
        }
        
        private static IServiceCollection AddDbContexts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationWriteDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
            });

            services.AddDbContext<IReadDbContext, ApplicationReadDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }

        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<DeleteExpiredVolunteersBackgroundService>();
            services.AddHostedService<FilesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageQueue<IEnumerable<FileMetadata>>, 
                InMemoryMessageQueue<IEnumerable<FileMetadata>>>();

            return services;
        }
    }
}
