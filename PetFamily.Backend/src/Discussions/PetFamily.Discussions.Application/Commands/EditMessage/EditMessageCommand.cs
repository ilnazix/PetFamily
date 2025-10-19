using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.EditMessage;

public record EditMessageCommand(
    Guid DiscussionId,
    Guid MessageId,
    Guid UserId,
    string NewText
) : ICommand;