using PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithFilters;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfBreedExists;
using PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfSpeciesExists;

namespace PetFamily.Volunteers.Presentation.Pets.Extensions;

internal static class RequestExtensions
{
    public static GetPetsWithFiltersQuery ToQuery(this PetsParameters request)
    {
        return new(
        request.Name,
        request.Description,
        request.VolunteerIds,
        request.SpeciesId,
        request.BreedId,
        request.Color,
        request.Status,
        request.Country,
        request.State,
        request.City,
        request.PageNumber,
        request.PageSize,
        request.OrderBy);
    }

    public static AnyPetOfBreedExistsQuery ToQuery(
        this AnyPetOfBreedExistsRequest request) => new(request.BreedId);

    public static AnyPetOfScpeciesExistsQuery ToQuery(
        this AnyPetOfSpeciesExistsRequest request) => new(request.SpeciesId);
}
