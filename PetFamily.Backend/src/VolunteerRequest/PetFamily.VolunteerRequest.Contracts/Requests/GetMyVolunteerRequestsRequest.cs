namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record GetMyVolunteerRequestsRequest(
    string? Status,
    int PageNumber = 1,
    int PageSize = 10);
