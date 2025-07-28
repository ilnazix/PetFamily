using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create
{
    public record CreateVolunteerCommand(
        FullNameDto FullName,
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaInfo> SocialMedias,
        IEnumerable<RequisitesInfo> Requisites
        ) : ICommand;
}