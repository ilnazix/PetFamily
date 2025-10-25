using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Auth;
using PetFamily.VolunteerRequest.Application.Commands.ApproveVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Commands.RejectVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Commands.RequireRevision;
using PetFamily.VolunteerRequest.Application.Commands.SubmitVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;
using PetFamily.VolunteerRequest.Application.Commands.UpdateVolunteerRequest;
using PetFamily.VolunteerRequest.Application.Queries.GetAllAdminVolunteerRequests;
using PetFamily.VolunteerRequest.Application.Queries.GetAllUnassignedVolunteerRequests;
using PetFamily.VolunteerRequest.Application.Queries.GetMyVolunteerRequests;
using PetFamily.VolunteerRequest.Contracts.Requests;
using PetFamily.VolunteerRequest.Presentation.Extensions;

namespace PetFamily.VolunteerRequest.Presentation;

[Route("api/volunteer-requests")]
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

    [HttpPut("{id}")]
    [HasPermission(Permissions.VolunteerRequests.Update)]
    public async Task<ActionResult> UpdateVolunteerRequest(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequestRequest request,
        [FromServices] UpdateVolunteerRequestCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = request.ToCommand(id, userId);

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
        
        var command = new TakeRequestOnReviewCommand(id, adminId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id}/submit")]
    [HasPermission(Permissions.VolunteerRequests.Submit)]
    public async Task<ActionResult> SubmitVolunteerRequest(
        [FromRoute] Guid id,
        [FromServices] SubmitVolunteerRequestCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = new SubmitVolunteerRequestCommand(id, userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id}/require-revision")]
    [HasPermission(Permissions.VolunteerRequests.RequireRevision)]
    public async Task<ActionResult> RequireVolunteerRequestRevision(
        [FromRoute] Guid id,
        [FromBody] RequireVolunteerRequestRevisionRequest request,
        [FromServices] RequireRevisionCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var adminId = _userContext.Current.UserId;
        var command = new RequireRevisionCommand(id, adminId, request.RejectionComment);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id}/reject")]
    [HasPermission(Permissions.VolunteerRequests.RequireRevision)]
    public async Task<ActionResult> RejectVolunteerRequest(
        [FromRoute] Guid id,
        [FromBody] RejectVolunteerRequestRequest request,
        [FromServices] RejectVolunteerRequestCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var adminId = _userContext.Current.UserId;
        var command = new RejectVolunteerRequestCommand(id, adminId, request.RejectionComment);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id}/approve")]
    [HasPermission(Permissions.VolunteerRequests.Approve)]
    public async Task<ActionResult> TakeVolunteerRequestOnReview(
        [FromRoute] Guid id,
        [FromServices] ApproveVolunteerRequestCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var adminId = _userContext.Current.UserId;

        var command = new ApproveVolunteerRequestCommand(id, adminId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet()]
    [HasPermission(Permissions.VolunteerRequests.ReadUnassigned)]
    public async Task<ActionResult> GetSubmittedVolunteerRequests(
        [FromQuery] GetUnassignedVolunteerRequestsRequest request,
        [FromServices] GetAllUnassignedVolunteerRequestsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("admin")]
    [HasPermission(Permissions.VolunteerRequests.ReadAdmin)]
    public async Task<ActionResult> GetAllAdminVolunteerRequests(
        [FromQuery] GetAllAdminVolunteerRequestsRequest request,
        [FromServices] GetAllAdminVolunteerRequestsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var adminId = _userContext.Current.UserId;
        var query = request.ToQuery(adminId);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("my-requests")]
    [HasPermission(Permissions.VolunteerRequests.ReadOwn)]
    public async Task<ActionResult> GetMyVolunteerRequests(
            [FromQuery] GetMyVolunteerRequestsRequest request,
            [FromServices] GetMyVolunteerRequestsQueryHandler handler,
            CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var query = request.ToQuery(userId);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }
}
