namespace PetFamily.VolunteerRequest.Contracts.Requests;
public record GetUnassignedVolunteerRequestsRequest(
    int PageNumber = 1,
    int PageSize = 10);


public record GetAllAdminVolunteerRequestsRequest(
    string? Status,
    int PageNumber = 1,
    int PageSize = 10);
