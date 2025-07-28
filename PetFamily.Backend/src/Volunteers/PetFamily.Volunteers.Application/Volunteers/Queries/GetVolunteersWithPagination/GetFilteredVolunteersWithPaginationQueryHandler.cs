using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.DTOs;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetFilteredVolunteersWithPaginationQueryHandler 
        : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;

        public GetFilteredVolunteersWithPaginationQueryHandler(
            IVolunteersReadDbContext readDbContext)
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
