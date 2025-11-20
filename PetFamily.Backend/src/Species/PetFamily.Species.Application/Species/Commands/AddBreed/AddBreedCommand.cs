using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Commands.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Title) : ICommand;
