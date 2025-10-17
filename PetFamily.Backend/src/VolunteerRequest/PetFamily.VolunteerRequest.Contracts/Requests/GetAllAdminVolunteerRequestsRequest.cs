namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record GetAllAdminVolunteerRequestsRequest(
    string? Status,
    int PageNumber = 1,
    int PageSize = 10);
