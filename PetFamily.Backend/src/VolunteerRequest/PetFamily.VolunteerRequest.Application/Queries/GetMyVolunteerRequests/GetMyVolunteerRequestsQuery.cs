using PetFamily.Core.Shared;

namespace PetFamily.VolunteerRequest.Application.Queries.GetMyVolunteerRequests;

public record GetMyVolunteerRequestsQuery(
    Guid UserId,
    string? Status,
    int PageNumber,
    int PageSize
    ) : BaseQuery(PageNumber, PageSize);
