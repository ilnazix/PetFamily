using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.SetPetMainPhoto
{
    public class SetPetMainPhotoCommandHandler : ICommandHandler<Guid, SetPetMainPhotoCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<SetPetMainPhotoCommandHandler> _logger;

        public SetPetMainPhotoCommandHandler(
            IVolunteersRepository volunteersRepository, 
            ILogger<SetPetMainPhotoCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            SetPetMainPhotoCommand command, 
            CancellationToken cancelationToken = default)
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancelationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;

            var petId = PetId.Create(command.PetId);
            var result = volunteer.SetPetMainPhoto(petId, command.ImagePath);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _volunteersRepository.Save(volunteer, cancelationToken);

            _logger.LogInformation(
                "Main photo for pet {PetId} was updated by volunteer {VolunteerId}", 
                petId.Value, 
                volunteerId.Value);

            return petId.Value;
        }
    }
}
