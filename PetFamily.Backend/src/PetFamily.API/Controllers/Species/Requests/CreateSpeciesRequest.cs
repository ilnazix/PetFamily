using PetFamily.Application.Species.Create;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record CreateSpeciesRequest(string Title) 
    {
        public CreateSpeciesCommand ToCommand() => new(Title);
    };

}
