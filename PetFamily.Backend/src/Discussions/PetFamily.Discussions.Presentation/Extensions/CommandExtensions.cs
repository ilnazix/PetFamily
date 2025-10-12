using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Application.DTOs;
using PetFamily.Discussions.Contracts.Requests;

namespace PetFamily.Discussions.Presentation.Extensions;

internal static class CommandExtensions
{
    public static CreateDiscussionCommand ToCommand(
        this CreateDiscussionRequest request)
    {
        var participants = request.Participants
            .Select(p => new Participant(p.Id, p.Email));

        return new(request.RelationId, participants);
    }
}
