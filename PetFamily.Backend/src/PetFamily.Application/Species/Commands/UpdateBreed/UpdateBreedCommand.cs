using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.UpdateBreed
{
    public record UpdateBreedCommand(Guid SpeciesId, Guid BreedId, string Title) : ICommand;
}
