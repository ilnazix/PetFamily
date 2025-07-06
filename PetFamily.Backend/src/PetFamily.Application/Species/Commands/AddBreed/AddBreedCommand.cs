using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.AddBreed
{
    public record AddBreedCommand(Guid SpeciesId, string Title) : ICommand;
}
