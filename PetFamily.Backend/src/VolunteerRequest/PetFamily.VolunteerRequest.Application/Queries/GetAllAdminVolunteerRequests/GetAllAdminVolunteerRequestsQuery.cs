using PetFamily.Core.Shared;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllAdminVolunteerRequests;

public record GetAllAdminVolunteerRequestsQuery(
    Guid AdminId,
    string? Status,
    int PageNumber,
    int PageSize) : BaseQuery(PageNumber, PageSize);
