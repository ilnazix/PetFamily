namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerCommand(
        string FirstName,
        string LastName, 
        string MiddleName,
        string PhoneNumber,
        string Email
        );
}