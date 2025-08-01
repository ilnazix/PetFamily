using Microsoft.AspNetCore.Http;
using PetFamily.Core.Models;

namespace PetFamily.Framework.Processors
{
    public class FormFileProcessor : IAsyncDisposable
    {
        public List<UploadFileCommand> _fileCommands = [];

        public List<UploadFileCommand> Process(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var uploadFileCommand = new UploadFileCommand(stream, file.FileName);

                _fileCommands.Add(uploadFileCommand);
            }

            return _fileCommands;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var file in _fileCommands)
            {
                await file.Content.DisposeAsync();
            }
        }
    }
}
