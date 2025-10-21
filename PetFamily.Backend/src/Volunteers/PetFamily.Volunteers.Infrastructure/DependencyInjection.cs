using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.Volunteers.Commands;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Options;
using PetFamily.Volunteers.Infrastructure.Providers;
using PetFamily.Volunteers.Infrastructure.Repositories;
using Minio;
using PetFamily.Core.Database;
using PetFamily.Volunteers.Infrastructure.Services;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Core.Messaging;
using PetFamily.Volunteers.Infrastructure.MessageQueues;
using PetFamily.Volunteers.Infrastructure.Database;
using PetFamily.Volunteers.Infrastructure.Utilities;

namespace PetFamily.Volunteers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<VolunteerEntityOptions>(configuration.GetSection("VolunteerEntityOptions"));

            services
                .AddDbContexts(configuration)
                .AddRepositories()
                .AddMinio(configuration)
                .AddHostedServices()
                .AddMessaging();

            services.AddScoped<DeleteExpiredVolunteersService>();

            services.AddScoped<IVolunteersUnitOfWork, VolunteersUnitOfWork>();

            services.AddScoped<IDbMigrator, VolunteersDbMigrator>();

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<VolunteersWriteDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention();
            });

            services.AddDbContext<IVolunteersReadDbContext, VolunteersReadDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }

        private static IServiceCollection AddMinio(
            this IServiceCollection services, 
            IConfiguration configuration)
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
