using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Queries.GetFilteredSpeciesWithPagination
{
    public record GetFilteredSpeciesWithPaginationQuery(
        int Page,
        int PageSize,
        string? Title) : IQuery;
}
