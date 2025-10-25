using PetFamily.Core.Abstractions;


namespace PetFamily.Species.Application.Species.Commands.Update;

public record UpdateSpeciesCommand(Guid Id, string Title) : ICommand;
