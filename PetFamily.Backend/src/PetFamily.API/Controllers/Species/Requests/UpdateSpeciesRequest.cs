using PetFamily.Application.Species.Commands.Update;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record UpdateSpeciesRequest(string Title) 
    {
        public UpdateSpeciesCommand ToCommand(Guid id) => new(id, Title);
    };
}
