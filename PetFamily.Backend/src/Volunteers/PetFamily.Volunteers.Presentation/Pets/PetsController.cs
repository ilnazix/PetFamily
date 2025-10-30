using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Auth;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetPet;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetPetsWithFilters;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Presentation.Pets.Extensions;

namespace PetFamily.API.Controllers.Pets;

[Route("api/[controller]")]
public class PetsController : ApplicationController
{
    [HttpGet]
    [HasPermission(Permissions.Pets.Read)]
    public async Task<ActionResult> GetAllPets(
        [FromQuery] PetsParameters request,
        GetPetsWithFiltersQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.Pets.Read)]
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
