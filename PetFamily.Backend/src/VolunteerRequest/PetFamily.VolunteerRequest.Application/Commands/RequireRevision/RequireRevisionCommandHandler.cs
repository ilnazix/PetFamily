using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Application.Commands.RequireRevision;

public class RequireRevisionCommandHandler : ICommandHandler<Guid, RequireRevisionCommand>
{
    private readonly IVolunteerRequestUnitOfWork _unitOfWork;
    private readonly IValidator<RequireRevisionCommand> _validator;
    private readonly ILogger<RequireRevisionCommandHandler> _logger;

    public RequireRevisionCommandHandler(
        IVolunteerRequestUnitOfWork unitOfWork,
        IValidator<RequireRevisionCommand> validator, 
        ILogger<RequireRevisionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RequireRevisionCommand command, 
        CancellationToken cancelationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancelationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var volunteerRequestResult = await _unitOfWork.VolunteerRequestsRepository
                .GetById(volunteerRequestId, cancelationToken);

        if(volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var volunteerRequest = volunteerRequestResult.Value;

        var result = volunteerRequest.RequestRevision(command.AdminId, command.RejectionComment);

        if(result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Volunteer request (id = {id}) was sent for revision by admin {adminId}",
            volunteerRequest.Id.Value,
            command.AdminId);

        return volunteerRequest.Id.Value;
    }
}
