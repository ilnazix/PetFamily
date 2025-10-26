using MassTransit;
using PetFamily.Discussions.Infrastructure.Consumers;

namespace PetFamily.Web.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<VolunteerRequestTakenForReviewEventConsumer>();

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
}
