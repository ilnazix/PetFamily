using PetFamily.VolunteerRequest.Application.Queries.GetAllAdminVolunteerRequests;
using PetFamily.VolunteerRequest.Application.Queries.GetAllUnassignedVolunteerRequests;
using PetFamily.VolunteerRequest.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Presentation.Extensions;

internal static class QueryExtensions
{
    public static GetAllUnassignedVolunteerRequestsQuery ToQuery(
        this GetUnassignedVolunteerRequestsRequest request)
        => new(request.PageNumber, request.PageSize);

    public static GetAllAdminVolunteerRequestsQuery ToQuery(
        this GetAllAdminVolunteerRequestsRequest request, Guid adminId)
        => new(adminId, request.Status, request.PageNumber, request.PageSize);
}
