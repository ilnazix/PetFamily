using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Providers;
using FileInfo = PetFamily.Application.Providers.FileMetadata;

namespace PetFamily.API.Controllers
{
    [Route("[controller]")]
    public class FilesController : ApplicationController
    {
        private const string BUCKET_NAME = "test";

        private readonly IFilesProvider _fileProvider;

        public FilesController(IFilesProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }


        [HttpPost]
        public async Task<ActionResult<Envelope>> CreateFile(IFormFile file, CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            var fileInfo = new FileInfo(BUCKET_NAME, file.FileName);
            var result = await _fileProvider.UploadFile(new FileData(fileInfo, stream), cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{fileName}/presigned-url")]
        public async Task<ActionResult<Envelope>> GetPresignedUrl([FromRoute] string fileName, CancellationToken cancellationToken)
        {
            var result = await _fileProvider.GetPresignedUrl(BUCKET_NAME, fileName, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value); 
        }

        [HttpDelete("{fileName}")]
        public async Task<ActionResult> DeleteFile([FromRoute] string fileName, CancellationToken cancellationToken)
        {
            var fileMetadata = new FileMetadata(BUCKET_NAME, fileName);
            var result = await _fileProvider.DeleteFile(fileMetadata, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return NoContent();
        }
    }
}
