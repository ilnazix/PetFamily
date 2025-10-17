using PetFamily.Core.Abstractions;


namespace PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;

public record CreateVolunteerRequestCommand(
    Guid UserId,
    string FirstName,
    string LastName, 
    string MiddleName,
    string Email,
    string PhoneNumber
    ) : ICommand;