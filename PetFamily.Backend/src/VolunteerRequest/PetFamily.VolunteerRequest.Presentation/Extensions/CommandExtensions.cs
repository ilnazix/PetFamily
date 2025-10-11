using PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;
using PetFamily.VolunteerRequest.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Presentation.Extensions;

internal static class CommandExtensions
{
    public static CreateVolunteerRequestCommand ToCommand(
        this CreateVolunteerRequestRequest r,
        Guid userId)
        => new (userId, r.FirstName, r.LastName, r.MiddleName, r.Email, r.PhoneNumber);
    
}
