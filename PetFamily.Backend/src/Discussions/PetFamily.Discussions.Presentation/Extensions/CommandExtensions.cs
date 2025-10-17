using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Contracts.Requests;

namespace PetFamily.Discussions.Presentation.Extensions;

internal static class CommandExtensions
{
    public static CreateDiscussionCommand ToCommand(
        this CreateDiscussionRequest request)
    {
        return new(request.RelationId, request.ParticipantIds);
    }
}
