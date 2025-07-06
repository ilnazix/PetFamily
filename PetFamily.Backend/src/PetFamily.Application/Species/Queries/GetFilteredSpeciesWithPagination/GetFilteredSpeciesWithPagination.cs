using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetFilteredSpeciesWithPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        int Page, 
        int PageSize, 
        string? Title) : IQuery;
}
