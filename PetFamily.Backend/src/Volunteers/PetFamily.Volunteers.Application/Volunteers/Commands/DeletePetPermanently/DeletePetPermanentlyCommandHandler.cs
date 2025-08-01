using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePetPermanently
{
    public class DeletePetPermanentlyCommandHandler : ICommandHandler<DeletePetPermanentlyCommand>
    {
        private const string BUCKET_NAME = Constants.Buckets.PetPhotos;

        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IMessageQueue<IEnumerable<FileMetadata>> _messageQueue;
        private readonly ILogger<DeletePetPermanentlyCommandHandler> _logger;

        public DeletePetPermanentlyCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IMessageQueue<IEnumerable<FileMetadata>> messageQueue,
            ILogger<DeletePetPermanentlyCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _messageQueue = messageQueue;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeletePetPermanentlyCommand command,
            CancellationToken cancelationToken = default)
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancelationToken);

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

            await _unitOfWork.Commit(cancelationToken);

            _logger.LogInformation(
                "Pet with ID {PetId} was permanently deleted by volunteer {VolunteerId}",
                petId.Value,
                volunteerId.Value);

            return UnitResult.Success<ErrorList>();
        }
    }
}
