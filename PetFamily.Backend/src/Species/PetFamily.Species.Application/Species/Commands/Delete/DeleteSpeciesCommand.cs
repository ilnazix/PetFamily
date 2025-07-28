using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}
