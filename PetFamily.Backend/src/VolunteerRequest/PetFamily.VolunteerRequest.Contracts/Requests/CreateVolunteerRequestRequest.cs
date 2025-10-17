namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record CreateVolunteerRequestRequest(
    string FirstName,
    string LastName, 
    string MiddleName,
    string Email,
    string PhoneNumber);