using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.VolunteerRequest.Domain;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.VolunteerRequest.Application.Commands.ApproveVolunteerRequest;

public class ApproveVolunteerRequestCommandHandler 
    : ICommandHandler<Guid, ApproveVolunteerRequestCommand>
{
    private readonly IValidator<ApproveVolunteerRequestCommand> _validator;
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IVolunteersModule _volunteersModule;
    private readonly ILogger<ApproveVolunteerRequestCommandHandler> _logger;

    public ApproveVolunteerRequestCommandHandler(
        IValidator<ApproveVolunteerRequestCommand> validator,
        IVolunteerRequestUnitOfWork unitOfWork,
        IVolunteersModule volunteersModule, 
        ILogger<ApproveVolunteerRequestCommandHandler> logger)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _volunteersModule = volunteersModule;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ApproveVolunteerRequestCommand command, 
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var id = VolunteerRequestId.Create(command.VolunteerRequestId);
        var volunteerRequestResult = await _unitOfWork.VolunteerRequestsRepository.GetById(id, cancelationToken);

        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var volunteerRequest = volunteerRequestResult.Value;

        var result = volunteerRequest.Approve(command.AdminId);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        var volunteerInfo = volunteerRequest.VolunteerInfo;
        var createVolunteerRequest = MapToCreateVolunteerRequest(volunteerInfo);

        var createVolunteerResult = await _volunteersModule.CreateVolunteer(createVolunteerRequest, cancelationToken);

        if (createVolunteerResult.IsFailure)
            return createVolunteerResult.Error;

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Volunteer request {RequestId} approved by admin {AdminId} and volunteer created successfully.",
            volunteerRequest.Id.Value,
            command.AdminId
        );


        return id.Value;
    }

    private static Volunteers.Contracts.Requests.CreateVolunteerRequest MapToCreateVolunteerRequest(VolunteerInfo volunteerInfo)
    {
        return new Volunteers.Contracts.Requests.CreateVolunteerRequest(
            volunteerInfo.FullName.FirstName,
            volunteerInfo.FullName.LastName,
            volunteerInfo.FullName.MiddleName,
            volunteerInfo.PhoneNumber.Value,
            volunteerInfo.Email.Value
            );
    }
}
