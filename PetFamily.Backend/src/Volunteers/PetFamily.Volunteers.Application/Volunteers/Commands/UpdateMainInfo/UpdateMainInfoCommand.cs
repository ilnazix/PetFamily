using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid Id,
    FullNameDto FullName,
    int Experience,
    string PhoneNumber,
    string Email,
    string Description
    ) : ICommand;
