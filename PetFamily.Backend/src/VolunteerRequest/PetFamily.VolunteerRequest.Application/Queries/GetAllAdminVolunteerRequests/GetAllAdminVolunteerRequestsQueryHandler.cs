using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Application.DTOs;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllAdminVolunteerRequests;

public class GetAllAdminVolunteerRequestsQueryHandler :
    IQueryHandler<PagedList<VolunteerRequestDto>, GetAllAdminVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestsReadDbContext _readDbContext;

    public GetAllAdminVolunteerRequestsQueryHandler(
        IVolunteerRequestsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<VolunteerRequestDto>> Handle(
        GetAllAdminVolunteerRequestsQuery query, 
        CancellationToken cancelationToken = default)
    {
        var requestsQuery = _readDbContext.VolunteerRequests;

        requestsQuery = requestsQuery.Where(r => r.AdminId == query.AdminId);

        requestsQuery = requestsQuery.WhereIf(
            string.IsNullOrWhiteSpace(query.Status),
            r => r.Status == r.Status
            );

        return requestsQuery.ToPagedList(query.PageNumber, query.PageSize);
    }
}
