using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Application.Commands.AddMessage;

public class AddMessageCommandHandler : ICommandHandler<Guid, AddMessageCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<AddMessageCommand> _validator;
    private readonly ILogger<AddMessageCommandHandler> _logger;

    public AddMessageCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<AddMessageCommand> validator,
        ILogger<AddMessageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddMessageCommand command, 
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var discussionId = DiscussionId.Create(command.DiscussionId);
        var discussionResult = await _unitOfWork.DiscussionsRepository.GetById(discussionId, cancelationToken);

        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var discussion = discussionResult.Value;

        var messageId = MessageId.NewMessageId();
        var message = Message.Create(messageId, command.UserId, command.Text).Value;
        var result = discussion.AddMessage(message);

        if(result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "User {UserId} added a message ({MessageId}) to discussion {DiscussionId}",
            command.UserId, messageId.Value, discussionId.Value);

        return discussionId.Value;
    }
}
