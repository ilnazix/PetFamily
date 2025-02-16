using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Shared;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid Id,
        FullNameDto FullName,
        int Experience,
        string PhoneNumber,
        string Email,
        string Description
        ) : ICommand;
}
