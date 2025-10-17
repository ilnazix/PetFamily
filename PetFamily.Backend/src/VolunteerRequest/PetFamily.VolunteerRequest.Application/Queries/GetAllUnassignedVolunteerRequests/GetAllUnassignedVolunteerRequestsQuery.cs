using PetFamily.Core.Shared;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllUnassignedVolunteerRequests;

public record GetAllUnassignedVolunteerRequestsQuery(
    int PageNumber,
    int PageSize)
    : BaseQuery(PageNumber, PageSize);