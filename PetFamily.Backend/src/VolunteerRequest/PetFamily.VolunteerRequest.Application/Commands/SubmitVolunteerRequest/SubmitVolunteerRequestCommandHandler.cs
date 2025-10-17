using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Application.Commands.SubmitVolunteerRequest;

public class SubmitVolunteerRequestCommandHandler
    : ICommandHandler<Guid, SubmitVolunteerRequestCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IValidator<SubmitVolunteerRequestCommand> _validator;
    private readonly ILogger<SubmitVolunteerRequestCommandHandler> _logger;

    public SubmitVolunteerRequestCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IValidator<SubmitVolunteerRequestCommand> validator,
        ILogger<SubmitVolunteerRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SubmitVolunteerRequestCommand command,
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _unitOfWork.VolunteerRequestsRepository.GetById(requestId, cancelationToken);

        if (requestResult.IsFailure)
            return requestResult.Error.ToErrorList();

        var request = requestResult.Value;

        var result = request.Submit(command.UserId);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Volunteer request (id = {RequestId}) has been submitted by user {UserId}",
            request.Id.Value,
            request.UserId);

        return request.Id.Value;
    }
}
