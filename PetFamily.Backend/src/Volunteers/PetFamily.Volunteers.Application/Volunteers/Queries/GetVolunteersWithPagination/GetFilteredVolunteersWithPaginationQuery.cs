using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        string? FirstName,
        string? LastName,
        string? MiddleName,
        string? Email,
        int Page,
        int PageSize) : IQuery;

}
