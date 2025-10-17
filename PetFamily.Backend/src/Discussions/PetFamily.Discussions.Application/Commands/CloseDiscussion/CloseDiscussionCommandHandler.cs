using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionCommandHandler
    : ICommandHandler<Guid, CloseDiscussionCommand>
{
    private readonly IDiscussionsUnitOfWork _unitOfWork;
    private readonly IValidator<CloseDiscussionCommand> _validator;
    private readonly ILogger<CloseDiscussionCommandHandler> _logger;

    public CloseDiscussionCommandHandler(
        IDiscussionsUnitOfWork unitOfWork,
        IValidator<CloseDiscussionCommand> validator,
        ILogger<CloseDiscussionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CloseDiscussionCommand command,
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

        var result = discussion.Close();
        
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Discussion {DiscussionId} was closed", discussionId.Value);

        return discussionId.Value;
    }
}

