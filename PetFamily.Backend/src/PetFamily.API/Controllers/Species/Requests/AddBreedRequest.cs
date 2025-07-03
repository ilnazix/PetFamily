using PetFamily.Application.Species.AddBreed;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record AddBreedRequest(string Title)
    {
        public AddBreedCommand ToCommand(Guid id) => new(id, Title);
    };
}
