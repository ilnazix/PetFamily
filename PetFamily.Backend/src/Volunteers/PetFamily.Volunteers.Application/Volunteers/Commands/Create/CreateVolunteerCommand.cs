using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string PhoneNumber,
    string Email
    ) : ICommand;