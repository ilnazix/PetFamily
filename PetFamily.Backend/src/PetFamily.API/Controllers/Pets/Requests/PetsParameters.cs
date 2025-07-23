using PetFamily.API.Shared;
using PetFamily.Application.Volunteers.Queries.GetPetsWithFilters;

namespace PetFamily.API.Controllers.Pets.Requests
{
    public record PetsParameters : RequestParameters
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public Guid[]? VolunteerIds { get; init; }
        public Guid? SpeciesId { get; init; }
        public Guid? BreedId { get; init; }
        public string? Color { get; init; }
        public string? Status { get; init; }
        public string? Country { get; init; }
        public string? State { get; init; }
        public string? City { get; init; }

        public GetPetsWithFiltersQuery ToQuery() => new(
            Name,
            Description,
            VolunteerIds,
            SpeciesId,
            BreedId,
            Color,
            Status,
            Country,
            State,
            City,
            PageNumber,
            PageSize,
            OrderBy);
    }

}
