using PetFamily.Application.Abstractions;


namespace PetFamily.Application.Species.Commands.Update
{
    public record UpdateSpeciesCommand(Guid Id, string Title) : ICommand;
}
