using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithFilters;
using PetFamily.Volunteers.Contracts.Requests;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace PetFamily.Volunteers.Presentation.Pets.Extensions
{
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
    }
}
