using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialMedias;

namespace PetFamily.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //TODO: отрефакторить названия хендлеров с использованием command для единообразия
            services.AddScoped<CreateVolunteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateSocialMediasCommandHandler>();

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
