using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Discussions.Application.Commands.DeleteMessage;

public class DeleteMessageCommandHandler
    : ICommandHandler<Guid, DeleteMessageCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteMessageCommand> _validator;
    private readonly ILogger<DeleteMessageCommandHandler> _logger;

    public DeleteMessageCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<DeleteMessageCommand> validator,
        ILogger<DeleteMessageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteMessageCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var discussionId = DiscussionId.Create(command.DiscussionId);
        var discussionResult = await _unitOfWork.DiscussionsRepository.GetById(discussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var discussion = discussionResult.Value;

        var messageId = MessageId.Create(command.MessageId);
        var result = discussion.DeleteMessage(command.UserId, messageId);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation(
            "Message {MessageId} was deleted from discussion {DiscussionId} by user {UserId}",
            messageId.Value, discussionId.Value, command.UserId);

        return discussionId.Value;
    }
}
