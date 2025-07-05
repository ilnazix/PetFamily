using PetFamily.Application.Species.Queries.GetFilteredBreedsWithPagination;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record GetFilteredBreedsWithPaginationRequest(
        int Page = 1,
        int PageSize = 10,
        string? Title = null)
    {
        public GetFilteredBreedsWithPaginationQuery ToQuery(Guid speciesId) => new(Page, PageSize, speciesId, Title);
    };
}
