using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerRequest.Domain;
using PetFamily.Core.Extensions;

namespace PetFamily.VolunteerRequest.Application.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestCommandHandler : ICommandHandler<Guid, UpdateVolunteerRequestCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateVolunteerRequestCommand> _validator;
    private readonly ILogger<UpdateVolunteerRequestCommandHandler> _logger;

    public UpdateVolunteerRequestCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IValidator<UpdateVolunteerRequestCommand> validator,
        ILogger<UpdateVolunteerRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerRequestCommand command,
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var id = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _unitOfWork.VolunteerRequestsRepository.GetById(id, cancelationToken);
        if (requestResult.IsFailure)
            return requestResult.Error.ToErrorList();

        var request = requestResult.Value;

        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName).Value;
        var phone = PhoneNumber.Create(command.PhoneNumber).Value;
        var email = Email.Create(command.Email).Value;

        var newVolunteerInfo = VolunteerInfo.Create(fullName, phone, email).Value;

        var result = request.UpdateVolunteerInfo(command.UserId, newVolunteerInfo);

        if(result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Volunteer request (id = {Id}) updated by user {UserId}", id.Value, command.UserId);

        return id.Value;
    }
}
