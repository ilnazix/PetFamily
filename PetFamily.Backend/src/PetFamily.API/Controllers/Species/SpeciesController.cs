using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.Create;

namespace PetFamily.API.Controllers.Species
{
    [Route("[controller]")]
    public class SpeciesController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> CreateSpecies(
            [FromBody] CreateSpeciesRequest request,
            [FromServices] CreateSpeciesCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
