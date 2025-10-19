using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;

namespace PetFamily.Discussions.Application.Commands.EditMessage;

public class EditMessageCommandHandler : ICommandHandler<Guid, EditMessageCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<EditMessageCommand> _validator;
    private readonly ILogger<EditMessageCommandHandler> _logger;

    public EditMessageCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<EditMessageCommand> validator,
        ILogger<EditMessageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        EditMessageCommand command,
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
        var result = discussion.EditMessage(command.UserId, messageId, command.NewText);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Message {MessageId} in discussion {DiscussionId} edited by user {UserId}",
            command.MessageId, command.DiscussionId, command.UserId);

        return discussionId.Value;
    }
}

