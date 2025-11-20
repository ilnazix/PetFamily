using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Restore;

public record RestoreVolunteerCommand(Guid Id) : ICommand;
