using PetFamily.Application.Species.UpdateBreed;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record UpdateBreedRequest(string Title)
    {
        public UpdateBreedCommand ToCommand(Guid speciesId, Guid breedId) 
            => new(speciesId, breedId, Title);
    };
}
