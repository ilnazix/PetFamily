using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Application.Providers;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.AddPetPhoto
{
    public class AddPetPhotoCommandHandler : ICommandHandler<IReadOnlyList<string>, AddPetPhotoCommand>
    {
        private const string BUCKET_NAME = Constants.Buckets.PetPhotos;

        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IFilesProvider _fileProvider;
        private readonly IValidator<AddPetPhotoCommand> _validator;
        private readonly IMessageQueue<IEnumerable<FileMetadata>> _messageQueue;
        private readonly ILogger<AddPetPhotoCommandHandler> _logger;

        public AddPetPhotoCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IFilesProvider fileProvider,
            IValidator<AddPetPhotoCommand> validator,
            IMessageQueue<IEnumerable<FileMetadata>> messageQueue,
            ILogger<AddPetPhotoCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _fileProvider = fileProvider;
            _validator = validator;
            _messageQueue = messageQueue;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<string>, ErrorList>> Handle(AddPetPhotoCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(VolunteerId.Create(command.VolunteerId));
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;

            var petId = PetId.Create(command.PetId);
            var petResult = volunteer.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var photos = new List<Photo>();
            var fileDatas = new List<FileData>();

            foreach (var file in command.Files)
            {
                var path = Guid.NewGuid().ToString() + "_" + file.FileName;
                var photoResult = Photo.Create(path, file.FileName, false);

                if (photoResult.IsFailure)
                    return photoResult.Error.ToErrorList();

                var fileMetadata = new FileMetadata(BUCKET_NAME, path);
                var fileData = new FileData(fileMetadata, file.Content);

                photos.Add(photoResult.Value);
                fileDatas.Add(fileData);
            }

            var pathsResult = await _fileProvider.UploadFiles(fileDatas, cancellationToken);

            if (pathsResult.IsFailure)
            {
                await _messageQueue.WriteAsync(fileDatas.Select(f => f.Info), cancellationToken);
                return pathsResult.Error.ToErrorList();
            }

            var result = volunteer.SetPetPhotos(petId, photos);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _unitOfWork.Commit(cancellationToken);

            return pathsResult.Value.ToList();
        }
    }
}
