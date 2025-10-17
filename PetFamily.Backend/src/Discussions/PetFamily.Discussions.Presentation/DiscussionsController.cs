using Microsoft.AspNetCore.Mvc;
using PetFamily.Discussions.Application.Commands.AddMessage;
using PetFamily.Discussions.Application.Commands.CloseDiscussion;
using PetFamily.Discussions.Application.Commands.DeleteMessage;
using PetFamily.Discussions.Application.Commands.EditMessage;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.Discussions.Presentation.Extensions;
using PetFamily.Framework;
using PetFamily.Framework.Auth;

namespace PetFamily.Discussions.Presentation;

[Route("[controller]")]
public class DiscussionsController : ApplicationController
{
    private readonly IUserContext _userContext;

    public DiscussionsController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost("{discussionId}/messages")]
    [HasPermission(Permissions.Discussions.AddMessage)]
    public async Task<ActionResult> AddMessage(
        [FromRoute] Guid discussionId,
        [FromBody] AddMessageRequest request,
        [FromServices] AddMessageCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = request.ToCommand(discussionId, userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{discussionId}/close")]
    public async Task<ActionResult> CloseDiscussion(
        [FromRoute] Guid discussionId,
        [FromServices] CloseDiscussionCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CloseDiscussionCommand(discussionId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{discussionId}/messages/{messageId}")]
    [HasPermission(Permissions.Discussions.DeleteMessage)]
    public async Task<ActionResult> DeleteMessage(
    [FromRoute] Guid discussionId,
    [FromRoute] Guid messageId,
    [FromServices] DeleteMessageCommandHandler handler,
    CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = new DeleteMessageCommand(discussionId, messageId, userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{discussionId}/messages/{messageId}")]
    [HasPermission(Permissions.Discussions.EditMessage)]
    public async Task<ActionResult> EditMessage(
    [FromRoute] Guid discussionId,
    [FromRoute] Guid messageId,
    [FromBody] EditMessageRequest request,
    [FromServices] EditMessageCommandHandler handler,
    CancellationToken cancellationToken)
    {
        var userId = _userContext.Current.UserId;
        var command = new EditMessageCommand(discussionId, messageId, userId, request.Text);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
