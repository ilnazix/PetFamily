using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;
using PetFamily.VolunteerRequest.Application.Database;
using MediatR;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectVolunteerRequest;

public class RejectVolunteerRequestCommandHandler : ICommandHandler<Guid, RejectVolunteerRequestCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IValidator<RejectVolunteerRequestCommand> _validator;
    private readonly IPublisher _publisher;
    private readonly ILogger<RejectVolunteerRequestCommandHandler> _logger;

    public RejectVolunteerRequestCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IValidator<RejectVolunteerRequestCommand> validator,
        IPublisher publisher,
        ILogger<RejectVolunteerRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RejectVolunteerRequestCommand command,
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var volunteerRequestResult = await _unitOfWork.VolunteerRequestsRepository
                .GetById(volunteerRequestId, cancelationToken);

        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var volunteerRequest = volunteerRequestResult.Value;

        var result = volunteerRequest.Reject(command.AdminId, command.RejectionComment);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _publisher.PublishDomainEvents(volunteerRequest, cancelationToken);

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Volunteer request (id = {id}) was rejected by admin {adminId}",
            volunteerRequest.Id.Value,
            command.AdminId);

        return volunteerRequest.Id.Value;
    }
}
