using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Extensions;
using PetFamily.Application.Abstractions;


namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetFilteredVolunteersWithPaginationQueryHandler : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetFilteredVolunteersWithPaginationQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerDto>> Handle(
            GetFilteredVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var volunteersQuery = _readDbContext.Volunteers.AsQueryable();

            volunteersQuery = volunteersQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.FirstName),
                v => v.FirstName.Contains(query.FirstName!));

            volunteersQuery = volunteersQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.MiddleName),
                v => v.MiddleName.Contains(query.MiddleName!));

            volunteersQuery = volunteersQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.LastName),
                v => v.LastName.Contains(query.LastName!));

            volunteersQuery = volunteersQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.Email),
                v => v.Email.Contains(query.Email!));

            var pagedList = await volunteersQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
