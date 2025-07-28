using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangePetPosition
{
    public record ChangePetPositionCommand(
        Guid VolunteerId,
        Guid PetId,
        int NewPosition) : ICommand;
}
