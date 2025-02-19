using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto
{
    public class AddPetPhotoCommandHandler : ICommandHandler<IReadOnlyList<string>, AddPetPhotoCommand>
    {
        public const string BUCKET_NAME = "pet-photos";

        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IFilesProvider _fileProvider;
        private readonly IValidator<AddPetPhotoCommand> _validator;
        private readonly IMessageQueue<IEnumerable<FileMetadata>> _messageQueue;
        private readonly ILogger<AddPetPhotoCommandHandler> _logger;

        public AddPetPhotoCommandHandler(
            IVolunteersRepository volunteersRepository,
            IFilesProvider fileProvider,
            IValidator<AddPetPhotoCommand> validator,
            IMessageQueue<IEnumerable<FileMetadata>> messageQueue,
            ILogger<AddPetPhotoCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
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

            var volunteerResult = await _volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId));
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
                var photoResult = Photo.Create(path, file.FileName);

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

            var saveResult = await _volunteersRepository.Save(volunteer, cancellationToken);

            return pathsResult.Value.ToList();
        }
    }
}
