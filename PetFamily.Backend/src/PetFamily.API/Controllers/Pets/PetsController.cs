using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
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
    }
}
