using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SoftDelete;

public class SoftDeleteCommandHandler : ICommandHandler<Guid, SoftDeleteCommand>
{
    private readonly IVolunteersUnitOfWork _unitOfWork;
    private readonly IValidator<SoftDeleteCommand> _validator;
    private readonly ILogger<SoftDeleteCommandHandler> _logger;

    public SoftDeleteCommandHandler(
        IVolunteersUnitOfWork unitOfWork,
        IValidator<SoftDeleteCommand> validator,
        ILogger<SoftDeleteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(SoftDeleteCommand command, CancellationToken cancellationToken)
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
        volunteer.Delete();

        await _unitOfWork.Commit(cancellationToken);

        _logger.LogInformation("Soft delete volunteer with Id={Id}", id.Value);

        return id.Value;
    }
}
