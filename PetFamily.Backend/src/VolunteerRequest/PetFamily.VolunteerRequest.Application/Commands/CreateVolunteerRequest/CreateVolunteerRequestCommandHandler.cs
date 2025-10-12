using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.VolunteerRequest.Domain;


namespace PetFamily.VolunteerRequest.Application.Commands.CreateVolunteerRequest;

public class CreateVolunteerRequestCommandHandler : ICommandHandler<Guid, CreateVolunteerRequestCommand>
{
    private readonly IValidator<CreateVolunteerRequestCommand> _validator;
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVolunteerRequestCommandHandler> _logger;

    public CreateVolunteerRequestCommandHandler(
        IValidator<CreateVolunteerRequestCommand> validator,
        IVolunteerRequestUnitOfWork unitOfWork,
        ILogger<CreateVolunteerRequestCommandHandler> logger)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerRequestCommand command, 
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var id = VolunteerRequestId.NewVolunteerId();

        var fullName = FullName.Create(
            command.FirstName,
            command.LastName,
            command.MiddleName).Value;

        var email = Email.Create(command.Email).Value;
        var phone = PhoneNumber.Create(command.PhoneNumber).Value;
        var volunteerInfo = VolunteerInfo.Create(fullName, phone, email).Value;

        var volunteerRequest = Domain.VolunteerRequest.Create(
            id, 
            command.UserId,
            volunteerInfo
            ).Value;

        await _unitOfWork.VolunteerRequestsRepository.Add(volunteerRequest, cancelationToken);
        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("User with id={id} created volunteer request", command.UserId);

        return id.Value;
    }
}
