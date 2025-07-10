using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.DeletePetPermanently
{
    public class DeletePetPermanentlyCommandHandler : ICommandHandler<DeletePetPermanentlyCommand>
    {
        private const string BUCKET_NAME = Constants.Buckets.PetPhotos;

        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IMessageQueue<IEnumerable<FileMetadata>> _messageQueue;
        private readonly ILogger<DeletePetPermanentlyCommandHandler> _logger;

        public DeletePetPermanentlyCommandHandler(
            IVolunteersRepository volunteersRepository,
            IMessageQueue<IEnumerable<FileMetadata>> messageQueue,
            ILogger<DeletePetPermanentlyCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _messageQueue = messageQueue;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeletePetPermanentlyCommand command, 
            CancellationToken cancelationToken = default)
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancelationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;
            var petId = PetId.Create(command.PetId);

            var petResult = volunteer.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var pet = petResult.Value; 
            volunteer.DeletePetPermanently(petId);

            var photosToDelete = pet.Photos
                .Select(p => new FileMetadata(BUCKET_NAME, p.Path));

            await _messageQueue.WriteAsync(photosToDelete, cancelationToken);

            await _volunteersRepository.Save(volunteer, cancelationToken);

            _logger.LogInformation(
                "Pet with ID {PetId} was permanently deleted by volunteer {VolunteerId}",
                petId.Value, 
                volunteerId.Value);

            return UnitResult.Success<ErrorList>();
        }
    }
}
