using PetFamily.Species.Application.Species.Commands.AddBreed;
using PetFamily.Species.Application.Species.Commands.Create;
using PetFamily.Species.Application.Species.Commands.Update;
using PetFamily.Species.Application.Species.Commands.UpdateBreed;
using PetFamily.Species.Application.Species.Queries.CheckIfBreedsExistsQuery;
using PetFamily.Species.Application.Species.Queries.GetFilteredBreedsWithPagination;
using PetFamily.Species.Application.Species.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation.Extensions
{
    public static class RequestExtensions
    {
        public static CheckBreedsExistenceQuery ToQuery(this CheckBreedExistenceRequest request)
        {
            return new(request.SpeciesId, request.BreedId);
        }

        public static AddBreedCommand ToCommand(this AddBreedRequest request, Guid speciesId)
        {
            return new AddBreedCommand(speciesId, request.Title);
        }

        public static CreateSpeciesCommand ToCommand(this CreateSpeciesRequest request)
        {
            return new CreateSpeciesCommand(request.Title);
        }

        public static GetFilteredBreedsWithPaginationQuery ToQuery(this GetFilteredBreedsWithPaginationRequest request, Guid speciesId)
        {
            return new GetFilteredBreedsWithPaginationQuery(
                request.Page,
                request.PageSize,
                speciesId,
                request.Title
            );
        }

        public static GetFilteredSpeciesWithPaginationQuery ToQuery(this GetFilteredSpeciesWithPaginationRequest request)
        {
            return new GetFilteredSpeciesWithPaginationQuery(
                request.Page,
                request.PageSize,
                request.Title
            );
        }

        public static UpdateBreedCommand ToCommand(this UpdateBreedRequest request, Guid speciesId, Guid breedId)
        {
            return new UpdateBreedCommand(speciesId, breedId, request.Title);
        }

        public static UpdateSpeciesCommand ToCommand(this UpdateSpeciesRequest request, Guid speciesId)
        {
            return new UpdateSpeciesCommand(speciesId, request.Title);
        }
    }
}
