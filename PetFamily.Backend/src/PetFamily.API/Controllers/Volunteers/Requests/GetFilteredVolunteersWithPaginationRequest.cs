using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests
{
    public record GetFilteredVolunteersWithPaginationRequest(
        string? FirstName,
        string? LastName,
        string? MiddleName,
        string? Email,
        int Page = 1, 
        int PageSize = 10)
    {
        public GetFilteredVolunteersWithPaginationQuery ToQuery()
        {
            var query = new GetFilteredVolunteersWithPaginationQuery(FirstName, 
                LastName, 
                MiddleName,
                Email,
                Page, 
                PageSize);

            return query;
        }
    }
}
