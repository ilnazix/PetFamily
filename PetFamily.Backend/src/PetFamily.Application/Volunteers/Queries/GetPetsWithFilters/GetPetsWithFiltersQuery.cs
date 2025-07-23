using PetFamily.Application.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithFilters
{
    public record GetPetsWithFiltersQuery(
        string? Name,
        string? Description,
        Guid[]? VolunteerIds,
        Guid? SpeciesId,
        Guid? BreedId,
        string? Color,
        string? Status,
        string? Country,
        string? State,
        string? City,
        int PageNumber,
        int PageSize,
        string? OrderBy
        ) : BaseQuery(PageNumber, PageSize);
}
