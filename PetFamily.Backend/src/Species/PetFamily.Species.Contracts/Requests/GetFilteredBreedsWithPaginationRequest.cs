namespace PetFamily.Species.Contracts.Requests;

public record GetFilteredBreedsWithPaginationRequest(
    int Page = 1,
    int PageSize = 10,
    string? Title = null);
