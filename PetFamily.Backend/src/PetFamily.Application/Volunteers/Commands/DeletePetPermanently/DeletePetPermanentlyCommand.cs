using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePetPermanently
{
    public record DeletePetPermanentlyCommand(
        Guid VolunteerId,
        Guid PetId) : ICommand;
}
