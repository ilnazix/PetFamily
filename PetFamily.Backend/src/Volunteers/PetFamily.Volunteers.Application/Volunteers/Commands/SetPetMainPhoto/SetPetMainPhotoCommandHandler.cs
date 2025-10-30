using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SetPetMainPhoto;

public class SetPetMainPhotoCommandHandler : ICommandHandler<Guid, SetPetMainPhotoCommand>
{
    private readonly IVolunteersUnitOfWork _unitOfWork;
    private readonly ILogger<SetPetMainPhotoCommandHandler> _logger;

    public SetPetMainPhotoCommandHandler(
        IVolunteersUnitOfWork unitOfWork,
        ILogger<SetPetMainPhotoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetPetMainPhotoCommand command,
        CancellationToken cancelationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancelationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var volunteer = volunteerResult.Value;

        var petId = PetId.Create(command.PetId);
        var result = volunteer.SetPetMainPhoto(petId, command.ImagePath);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation(
            "Main photo for pet {PetId} was updated by volunteer {VolunteerId}",
            petId.Value,
            volunteerId.Value);

        return petId.Value;
    }
}
