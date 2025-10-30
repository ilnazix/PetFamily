namespace PetFamily.VolunteerRequest.Contracts.Messaging;

public record VolunteerRequestApprovedEvent(
    Guid VolunteerRequestId,
    Guid UserId,
    string FirstName,
    string LastName, 
    string MiddleName,
    string PhoneNumber,
    string Email
    );
