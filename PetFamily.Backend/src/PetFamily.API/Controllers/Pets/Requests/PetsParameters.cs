using PetFamily.API.Shared;
using PetFamily.Application.Volunteers.Queries.GetPetsWithFilters;

namespace PetFamily.API.Controllers.Pets.Requests
{
    public record PetsParameters : RequestParameters
    {
        public GetPetsWithFiltersQuery ToQuery() => new(PageNumber, PageSize, OrderBy);
    }
}
