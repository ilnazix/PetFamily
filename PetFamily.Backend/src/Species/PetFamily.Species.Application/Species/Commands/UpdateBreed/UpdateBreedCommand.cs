using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Commands.UpdateBreed;

public record UpdateBreedCommand(Guid SpeciesId, Guid BreedId, string Title) : ICommand;
