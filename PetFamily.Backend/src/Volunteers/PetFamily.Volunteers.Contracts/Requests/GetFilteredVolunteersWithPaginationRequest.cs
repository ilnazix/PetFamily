namespace PetFamily.Volunteers.Contracts.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? Email,
    int Page = 1,
    int PageSize = 10);
