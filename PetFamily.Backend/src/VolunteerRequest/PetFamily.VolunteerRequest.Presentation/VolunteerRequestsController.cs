using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Auth;
using PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;
using PetFamily.VolunteerRequest.Contracts.Requests;
using PetFamily.VolunteerRequest.Presentation.Extensions;

namespace PetFamily.VolunteerRequest.Presentation;

[Route("volunteer-requests")]
public class VolunteerRequestsController : ApplicationController
{
    protected readonly IUserContext _userContext;

    public VolunteerRequestsController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost]
    [HasPermission(Permissions.VolunteerRequests.Create)]
    public async Task<ActionResult> CreateVolunteerRequest(
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] CreateVolunteerRequestCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = request.ToCommand(userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id}/review")]
    [HasPermission(Permissions.VolunteerRequests.TakeOnReview)]
    public async Task<ActionResult> TakeVolunteerRequestOnReview(
        [FromRoute] Guid id,
        [FromServices] TakeRequestOnReviewCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var adminId = _userContext.Current.UserId;
        var adminEmail = _userContext.Current.Email;

        var command = new TakeRequestOnReviewCommand(id, adminId, adminEmail);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
