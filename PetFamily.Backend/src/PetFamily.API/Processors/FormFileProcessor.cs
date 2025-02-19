using PetFamily.Application.Volunteers.Commands.AddPetPhoto;

namespace PetFamily.API.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        public List<UploadFileCommand> _fileCommands = [];

        public List<UploadFileCommand> Process(IFormFileCollection files)
        {
            foreach(var file in files)
            {
                var stream = file.OpenReadStream();
                var uploadFileCommand = new UploadFileCommand(stream, file.FileName);

                _fileCommands.Add(uploadFileCommand);
            }

            return _fileCommands;
        }

        public async ValueTask DisposeAsync()
        {
            foreach(var file in _fileCommands)
            {
                await file.Content.DisposeAsync();
            }
        }
    }
}
