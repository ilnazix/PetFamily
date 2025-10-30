using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Queries.GetFilteredBreedsWithPagination;

public record GetFilteredBreedsWithPaginationQuery(
    int Page,
    int PageSize,
    Guid SpeciesId,
    string? Title) : IQuery;