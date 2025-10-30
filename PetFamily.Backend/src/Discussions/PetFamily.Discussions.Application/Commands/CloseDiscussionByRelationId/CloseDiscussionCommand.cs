using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public record CloseDiscussionByRelationIdCommand(
    Guid RelationId
    ) : ICommand;