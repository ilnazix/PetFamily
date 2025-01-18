using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.API.Controllers.Volunteers
{
    public record CreateVolunteerRequest(
        //TODO: отрефакторить с использованием FullNameDTO
        string FirstName,
        string LastName, 
        string MiddleName, 
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaDto> SocialMedias,
        IEnumerable<RequisitesDto> Requisites
        ); 
}