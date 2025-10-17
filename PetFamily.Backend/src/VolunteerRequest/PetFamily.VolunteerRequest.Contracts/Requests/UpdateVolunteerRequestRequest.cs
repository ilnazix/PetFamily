namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record UpdateVolunteerRequestRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber);