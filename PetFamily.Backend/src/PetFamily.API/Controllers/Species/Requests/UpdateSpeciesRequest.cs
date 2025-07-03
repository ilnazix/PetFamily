using PetFamily.Application.Species.Update;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record UpdateSpeciesRequest(string Title) 
    {
        public UpdateSpeciesCommand ToCommand(Guid id) => new(id, Title);
    };
}
