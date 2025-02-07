using PetFamily.API.Controllers.Volunteers;
using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerCommand(
        FullNameDto FullName,
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaInfo> SocialMedias,
        IEnumerable<RequisitesInfo> Requisites
        );
}