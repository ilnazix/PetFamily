using Microsoft.AspNetCore.Mvc;
using PetFamily.Discussions.Application.Commands.AddMessage;
using PetFamily.Discussions.Application.Commands.CloseDiscussion;
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

    [HttpPost("{discussionId}")]
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
}
