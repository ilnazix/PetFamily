using PetFamily.VolunteerRequest.Application.Queries.GetAllUnassignedVolunteerRequests;
using PetFamily.VolunteerRequest.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Presentation.Extensions;

internal static class QueryExtensions
{
    public static GetAllUnassignedVolunteerRequestsQuery ToQuery(
        this GetUnassignedVolunteerRequestsRequest request)
        => new(request.PageNumber, request.PageSize);
}
