namespace PetFamily.API.Controllers.Volunteers
{
    public record CreateVolunteerRequest(
        string FirstName,
        string LastName, 
        string MiddleName, 
        string PhoneNumber,
        string Email,
        IEnumerable<SocialMediaDto> SocialMedias,
        IEnumerable<RequisitesDto> Requisites
        ); 
}