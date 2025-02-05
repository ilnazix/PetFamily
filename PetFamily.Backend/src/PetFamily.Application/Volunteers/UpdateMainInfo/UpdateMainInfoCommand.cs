using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid Id,
        FullNameDto FullName,
        int Experience,
        string PhoneNumber,
        string Email,
        string Description
        );
}
