using PetFamily.Discussions.Application.Commands.AddMessage;
using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Contracts.Requests;

namespace PetFamily.Discussions.Presentation.Extensions;

internal static class CommandExtensions
{
    public static CreateDiscussionCommand ToCommand(
        this CreateDiscussionRequest request)
        => new(request.RelationId, request.ParticipantIds);

    public static AddMessageCommand ToCommand(
        this AddMessageRequest request, 
        Guid discussionId,
        Guid userId)
        => new(discussionId, userId, request.Text);
}
