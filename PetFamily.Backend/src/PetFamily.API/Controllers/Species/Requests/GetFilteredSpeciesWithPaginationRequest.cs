using PetFamily.Application.Species.Queries.GetFilteredSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record GetFilteredSpeciesWithPaginationRequest(
        int Page = 1, 
        int PageSize = 10,
        string? Title = null)
    {
        public GetFilteredSpeciesWithPaginationQuery ToQuery() => new(Page, PageSize, Title);
    };
}
