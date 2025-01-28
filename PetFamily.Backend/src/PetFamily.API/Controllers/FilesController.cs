using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Minio.DataModel.Args;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Providers;

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

            var result = await _fileProvider.UploadFile(stream, BUCKET_NAME, file.FileName, cancellationToken);

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
            var result = await _fileProvider.DeleteFile(BUCKET_NAME, fileName, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return NoContent();
        }
    }
}
