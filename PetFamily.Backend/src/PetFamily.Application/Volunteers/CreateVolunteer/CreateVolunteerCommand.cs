using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerCommand(
        //TODO: отрефакторить с использованием FullNameDto
        string FirstName,
        string LastName, 
        string MiddleName,
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaDto> SocialMedias,
        IEnumerable<CreateRequisiteCommand> Requisites
        );
}