using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.DTOs;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public record CreateDiscussionCommand(
    Guid RelationId,
    IEnumerable<Participant> Participants
    ) : ICommand;
