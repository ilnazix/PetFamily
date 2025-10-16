namespace PetFamily.VolunteerRequest.Contracts.Requests;
public record GetUnassignedVolunteerRequestsRequest(
    int PageNumber = 1,
    int PageSize = 10);
