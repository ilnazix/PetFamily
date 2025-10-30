using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;

public class TakeRequestOnReviewCommandHandler 
    : ICommandHandler<Guid, TakeRequestOnReviewCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IValidator<TakeRequestOnReviewCommand> _validator;
    private readonly IPublisher _publisher;
    private readonly ILogger<TakeRequestOnReviewCommandHandler> _logger;

    public TakeRequestOnReviewCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IDiscussionsModule discussionsModule,
        IValidator<TakeRequestOnReviewCommand> validator,
        ILogger<TakeRequestOnReviewCommandHandler> logger,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _publisher = publisher;
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

        await _publisher.PublishDomainEvents(volunteerRequest, cancelationToken);

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Volunteer request (id = {id}) has been taken on review", volunteerRequest.Id.Value);

        return volunteerRequest.Id.Value;
    }
}
