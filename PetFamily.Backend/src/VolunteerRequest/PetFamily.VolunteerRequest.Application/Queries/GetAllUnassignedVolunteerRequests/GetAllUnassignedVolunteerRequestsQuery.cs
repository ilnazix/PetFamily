using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Core.Shared;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Application.DTOs;
using PetFamily.VolunteerRequest.Domain;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllUnassignedVolunteerRequests;

public record GetAllUnassignedVolunteerRequestsQuery(
    int PageNumber,
    int PageSize)
    : BaseQuery(PageNumber, PageSize);

public class GetAllUnassignedVolunteerRequestsQueryHandler
    : IQueryHandler<PagedList<VolunteerRequestDto>, GetAllUnassignedVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestsReadDbContext _readDbContext;

    public GetAllUnassignedVolunteerRequestsQueryHandler(IVolunteerRequestsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<PagedList<VolunteerRequestDto>> Handle(
        GetAllUnassignedVolunteerRequestsQuery query, 
        CancellationToken cancelationToken = default)
    {
        var requestsQuery = _readDbContext.VolunteerRequests;

        requestsQuery = requestsQuery.Where(r => r.Status == VolunteerRequestStatus.Submitted.Value);

        return requestsQuery.ToPagedList(query.PageNumber, query.PageSize, cancelationToken);
    }
}
