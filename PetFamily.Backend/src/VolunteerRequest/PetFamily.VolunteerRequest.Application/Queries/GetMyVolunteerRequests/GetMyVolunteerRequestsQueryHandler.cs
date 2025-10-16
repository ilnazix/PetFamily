using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Application.DTOs;

namespace PetFamily.VolunteerRequest.Application.Queries.GetMyVolunteerRequests;

public class GetMyVolunteerRequestsQueryHandler
    : IQueryHandler<PagedList<VolunteerRequestDto>, GetMyVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestsReadDbContext _readDbContext;

    public GetMyVolunteerRequestsQueryHandler(IVolunteerRequestsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<VolunteerRequestDto>> Handle(
        GetMyVolunteerRequestsQuery query,
        CancellationToken cancelationToken = default)
    {
        var requestsQuery = _readDbContext.VolunteerRequests;

        requestsQuery = requestsQuery.Where(r => r.UserId == query.UserId);

        requestsQuery = requestsQuery.WhereIf(
            string.IsNullOrWhiteSpace(query.Status),
            r => r.Status == r.Status
            );


        return requestsQuery.ToPagedList(query.PageNumber, query.PageSize, cancelationToken);
    }
}
