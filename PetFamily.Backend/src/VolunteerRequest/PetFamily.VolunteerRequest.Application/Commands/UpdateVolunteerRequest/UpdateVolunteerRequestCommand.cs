using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.UpdateVolunteerRequest;

public record UpdateVolunteerRequestCommand(
    Guid VolunteerRequestId,
    Guid UserId,
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber
    ) : ICommand;