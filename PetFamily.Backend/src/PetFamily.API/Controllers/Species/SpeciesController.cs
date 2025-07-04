using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.Create;
using PetFamily.Application.Species.Delete;
using PetFamily.Application.Species.Update;
using PetFamily.Application.Species.UpdateBreed;
using System.Threading;

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

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateSpecies(
            [FromRoute] Guid id,
            [FromBody] UpdateSpeciesRequest request,
            [FromServices] UpdateSpeciesCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteSpecies(
            [FromRoute] Guid id,
            [FromServices] DeleteSpeciesCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteSpeciesCommand(id);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return NoContent();
        }

        [HttpPost("{speciesId:guid}/breeds")]
        public async Task<ActionResult> AddBreed(
            [FromRoute] Guid speciesId,
            [FromBody] AddBreedRequest request,
            [FromServices] AddBreedCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(speciesId), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{speciesId:guid}/breeds/{breedId:guid}")]
        public async Task<ActionResult> AddBreed(
            [FromRoute] Guid speciesId,
            [FromRoute] Guid breedId,
            [FromBody] UpdateBreedRequest request,
            [FromServices] UpdateBreedCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToCommand(speciesId, breedId), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
