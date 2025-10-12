using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Models;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;

public class TakeRequestOnReviewCommandHandler 
    : ICommandHandler<Guid, TakeRequestOnReviewCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IDiscussionsModule _discussionsModule;
    private readonly IValidator<TakeRequestOnReviewCommand> _validator;
    private readonly ILogger<TakeRequestOnReviewCommandHandler> _logger;

    public TakeRequestOnReviewCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IDiscussionsModule discussionsModule,
        IValidator<TakeRequestOnReviewCommand> validator,
        ILogger<TakeRequestOnReviewCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _discussionsModule = discussionsModule;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeRequestOnReviewCommand command, 
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerRequestId = VolunteerRequestId.Create(command.RequestId);
        var volunteerRequestResult = await _unitOfWork.VolunteerRequestsRepository
            .GetById(volunteerRequestId, cancelationToken);

        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var volunteerRequest = volunteerRequestResult.Value;

        var result = volunteerRequest.TakeOnReview(command.AdminId);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        var createDiscussionResult = await CreateDiscussion(command, volunteerRequest, cancelationToken);

        if (createDiscussionResult.IsFailure)
            return createDiscussionResult.Error;

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Volunteer request (id = {id}) has been taken on review", volunteerRequest.Id.Value);

        return volunteerRequest.Id.Value;
    }

    private async Task<Result<Guid, ErrorList>> CreateDiscussion(TakeRequestOnReviewCommand command, Domain.VolunteerRequest volunteerRequest, CancellationToken cancelationToken)
    {
        var relationId = volunteerRequest.Id;
        var user = new DiscussionParticipant(volunteerRequest.UserId, volunteerRequest.VolunteerInfo.Email.Value);
        var admin = new DiscussionParticipant(command.AdminId, command.AdminEmail);
        var createDiscussionRequest = new CreateDiscussionRequest(relationId, [user, admin]);
        var createDiscussionResult = await _discussionsModule.CreateDiscussion(createDiscussionRequest, cancelationToken);
        return createDiscussionResult;
    }
}
