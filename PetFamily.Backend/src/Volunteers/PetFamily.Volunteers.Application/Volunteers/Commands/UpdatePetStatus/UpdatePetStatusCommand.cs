using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetStatus
{
    public record UpdatePetStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        string Status) : ICommand;
}
