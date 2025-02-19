using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Shared;


namespace PetFamily.Application.Volunteers.Commands.Create
{
    public record CreateVolunteerCommand(
        FullNameDto FullName,
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaInfo> SocialMedias,
        IEnumerable<RequisitesInfo> Requisites
        ) : ICommand;
}