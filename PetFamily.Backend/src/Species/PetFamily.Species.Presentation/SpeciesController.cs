using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Auth;
using PetFamily.Species.Application.Species.Commands.AddBreed;
using PetFamily.Species.Application.Species.Commands.Create;
using PetFamily.Species.Application.Species.Commands.Delete;
using PetFamily.Species.Application.Species.Commands.DeleteBreed;
using PetFamily.Species.Application.Species.Commands.Update;
using PetFamily.Species.Application.Species.Commands.UpdateBreed;
using PetFamily.Species.Application.Species.Queries.GetFilteredBreedsWithPagination;
using PetFamily.Species.Application.Species.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Species.Presentation.Extensions;


namespace PetFamily.Species.Presentation;

[Route("api/[controller]")]
public class SpeciesController : ApplicationController
{
    [HttpGet]
    [HasPermission(Permissions.Species.Read)]
    public async Task<ActionResult> GetAllSpecies(
        [FromQuery] GetFilteredSpeciesWithPaginationRequest request,
        [FromServices] GetFilteredSpeciesWithPaginationQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{speciesId:guid}")]
    [HasPermission(Permissions.Species.Read)]
    public async Task<ActionResult> GetAllBreeds(
        [FromRoute] Guid speciesId,
        [FromQuery] GetFilteredBreedsWithPaginationRequest request,
        [FromServices] GetFilteredBreedsWithPaginationQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery(speciesId);

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [HasPermission(Permissions.Species.Create)]
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
    [HasPermission(Permissions.Species.Update)]
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
    [HasPermission(Permissions.Species.Delete)]
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
    [HasPermission(Permissions.Species.Create)]
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
    [HasPermission(Permissions.Species.Update)]
    public async Task<ActionResult> UpdateBreed(
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

    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    [HasPermission(Permissions.Species.Delete)]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return NoContent();
    }
}
