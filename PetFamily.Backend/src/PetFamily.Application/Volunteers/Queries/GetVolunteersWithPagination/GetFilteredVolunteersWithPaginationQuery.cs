using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public record GetFilteredVolunteersWithPaginationQuery(
        string? FirstName,
        string? LastName,
        string? MiddleName,
        string? Email,
        int Page,
        int PageSize) : IQuery;

}
