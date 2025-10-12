using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public class CreateDiscussionCommandHandler : ICommandHandler<Guid, CreateDiscussionCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<CreateDiscussionCommand> _validator;
    private readonly ILogger<CreateDiscussionCommandHandler> _logger;

    public CreateDiscussionCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<CreateDiscussionCommand> validator,
        ILogger<CreateDiscussionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateDiscussionCommand command,
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var id = DiscussionId.NewDiscussionId();
        var participants = command.Participants.Select(p => User.Create(p.Id, p.Email).Value);

        var discussionResult = Discussion.Create(id, command.RelationId, participants);

        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var discussion = discussionResult.Value;

        await _unitOfWork.DiscussionsRepository.Add(discussion, cancelationToken);
        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Created discussion (id={discussionId}) for volunteer request (id = {relationId})", 
            discussion.Id.Value,
            command.RelationId);

        return id.Value;
    }
}
