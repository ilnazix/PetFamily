using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Database;
using PetFamily.VolunteerRequest.Application.Commands;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Infrastructure.Database;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using PetFamily.VolunteerRequest.Application.Messaging;
using PetFamily.VolunteerRequest.Infrastructure.Repositories;
using PetFamily.VolunteerRequest.Infrastructure.Utilities;
using PetFamily.VolunteerRequest.Infrastructure.Outbox;
using Quartz;
using Polly;

namespace PetFamily.VolunteerRequest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddDbContexts(configuration)
            .AddMessageBus(configuration)
            .AddQuartzServices()
            .AddResilience()
            .AddOutbox();

        services.AddScoped<IDbMigrator, VolunteerRequestsDbMigrator>();

        return services;
    }

    private static IServiceCollection AddResilience(
        this IServiceCollection services)
    {
        services.AddResiliencePipeline(Constants.Resilience.BasicPipeline, (builder, context) =>
        {
            var loggerFactory = context.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(Constants.Resilience.BasicPipeline);

            builder.AddRetry(new()
            {
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(e => e is not NullReferenceException),
                UseJitter = true,
                MaxRetryAttempts = 10,
                MaxDelay = TimeSpan.FromSeconds(10),
                OnRetry = (args) =>
                {
                    var level = args.AttemptNumber >= 3 ? LogLevel.Warning : LogLevel.Debug;
                    logger.Log(level,
                        "Retry {Attempt} after {Delay} due to {ExceptionType}: {Message}",
                        args.AttemptNumber,
                        args.Duration,
                        args.Outcome.Exception?.GetType().Name,
                        args.Outcome.Exception?.Message);

                    return ValueTask.CompletedTask;
                }
            });
            builder.AddTimeout(TimeSpan.FromSeconds(10));
        });

        return services;
    }

    private static IServiceCollection AddOutbox(
       this IServiceCollection services)
    {
        services.AddScoped<ProcessOutboxMessagesService>();

        return services;
    }

    private static IServiceCollection AddMessageBus(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddMassTransit<IVolunteerRequestsBus>(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:HostName"]!), cfg =>
                {
                    cfg.Username(configuration["RabbitMQ:UserName"]!);
                    cfg.Password(configuration["RabbitMQ:Password"]!);
                });
                cfg.Durable = true;
                cfg.ConfigureEndpoints(context);
            });
        });

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

        services.AddDbContext<IVolunteerRequestsReadDbContext, VolunteerRequestsReadDbContext>(options =>
        {
            options
                    .UseNpgsql(configuration.GetConnectionString(Constants.DB_CONFIGURATION_SECTION))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<IVolunteerRequestUnitOfWork, VolunteerRequestsUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddQuartzServices(
        this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey, (Action<IJobConfigurator>?)null)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule
                            .WithIntervalInSeconds(10)
                            .RepeatForever()));
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
}

