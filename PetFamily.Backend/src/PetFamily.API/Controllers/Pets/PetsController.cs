using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.Application.Volunteers.Queries.GetPet;
using PetFamily.Application.Volunteers.Queries.GetPetsWithFilters;

namespace PetFamily.API.Controllers.Pets
{
    [Route("[controller]")]
    public class PetsController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult> GetAllPets(
            [FromQuery] PetsParameters request,
            GetPetsWithFiltersQueryHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPetById(
            [FromRoute] Guid id,
            GetPetQueryHandler handler,
            CancellationToken cancellationToken)
        {
            var query = new GetPetQuery(id);
            var result = await handler.Handle(query, cancellationToken);

            return Ok(result);
        }
    }
}
