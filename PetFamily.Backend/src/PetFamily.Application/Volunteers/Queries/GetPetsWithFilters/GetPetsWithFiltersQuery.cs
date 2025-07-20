using PetFamily.Application.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithFilters
{
    public record GetPetsWithFiltersQuery(
        int PageNumber,
        int PageSize,
        string? OrderBy
        ) : BaseQuery(PageNumber, PageSize);
}
