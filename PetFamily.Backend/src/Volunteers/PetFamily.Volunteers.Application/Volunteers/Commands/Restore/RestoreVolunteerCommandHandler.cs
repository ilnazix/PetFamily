using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Restore;

public class RestoreVolunteerCommandHandler : ICommandHandler<Guid, RestoreVolunteerCommand>
{
    private readonly IVolunteersUnitOfWork _unitOfWork;
    private readonly IValidator<RestoreVolunteerCommand> _validator;
    private readonly ILogger<RestoreVolunteerCommandHandler> _logger;

    public RestoreVolunteerCommandHandler(
        IVolunteersUnitOfWork unitOfWork,
        IValidator<RestoreVolunteerCommand> validator,
        ILogger<RestoreVolunteerCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RestoreVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var id = VolunteerId.Create(command.Id);
        var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(id, cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteer = volunteerResult.Value;
        volunteer.Restore();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Restore volunteer with Id={Id}", id.Value);

        return id.Value;
    }
}
