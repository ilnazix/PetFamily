using PetFamily.Application.Abstractions;


namespace PetFamily.Application.Species.Update
{
    public record UpdateSpeciesCommand(Guid Id, string Title) : ICommand;
}
