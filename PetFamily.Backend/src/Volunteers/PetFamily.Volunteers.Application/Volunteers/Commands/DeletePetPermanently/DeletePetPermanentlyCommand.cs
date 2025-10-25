using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePetPermanently;

public record DeletePetPermanentlyCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
