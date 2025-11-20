using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionByRelationIdCommandHandler
    : ICommandHandler<Guid, CloseDiscussionByRelationIdCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<CloseDiscussionByRelationIdCommand> _validator;
    private readonly ILogger<CloseDiscussionByRelationIdCommandHandler> _logger;

    public CloseDiscussionByRelationIdCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<CloseDiscussionByRelationIdCommand> validator,
        ILogger<CloseDiscussionByRelationIdCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CloseDiscussionByRelationIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var discussionResult = await _unitOfWork.DiscussionsRepository.GetByRelationId(command.RelationId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var discussion = discussionResult.Value;

        var result = discussion.Close();
        
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Discussion {DiscussionId} was closed", discussion.Id.Value);

        return discussion.Id.Value;
    }
}

