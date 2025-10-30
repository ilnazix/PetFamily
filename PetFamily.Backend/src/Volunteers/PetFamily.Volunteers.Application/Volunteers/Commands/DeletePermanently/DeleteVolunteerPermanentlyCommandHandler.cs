using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePermanently;

public class DeleteVolunteerPermanentlyCommandHandler : ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand>
{
    private readonly IVolunteersUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteVolunteerPermanentlyCommand> _validator;
    private readonly ILogger<DeleteVolunteerPermanentlyCommandHandler> _logger;

    public DeleteVolunteerPermanentlyCommandHandler(
        IVolunteersUnitOfWork unitOfWork,
        IValidator<DeleteVolunteerPermanentlyCommand> validator,
        ILogger<DeleteVolunteerPermanentlyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerPermanentlyCommand command,
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

        await _unitOfWork.VolunteersRepository.Delete(volunteerResult.Value, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Volunteers with Id={Id} permanently deleted", id.Value);

        return id.Value;
    }
}
