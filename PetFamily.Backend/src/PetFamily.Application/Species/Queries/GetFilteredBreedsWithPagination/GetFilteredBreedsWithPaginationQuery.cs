using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetFilteredBreedsWithPagination
{
    public record GetFilteredBreedsWithPaginationQuery(
        int Page, 
        int PageSize,
        Guid SpeciesId,
        string? Title) : IQuery;
}
