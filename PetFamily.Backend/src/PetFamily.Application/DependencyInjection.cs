using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.Restore;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
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
            services.AddScoped<UpdateRequisitesCommandHandler>();
            services.AddScoped<SoftDeleteCommandHandler>();
            services.AddScoped<HardDeleteCommandHandler>();
            services.AddScoped<RestoreVolunteerCommandHandler>();

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            
            return services;
        }
    }
}
