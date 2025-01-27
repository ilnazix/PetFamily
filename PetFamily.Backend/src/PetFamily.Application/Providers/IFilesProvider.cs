using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers
{
    public interface IFilesProvider
    {
        Task<UnitResult<Error>> DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken);
        Task<Result<string, Error>> GetPresignedUrl(string bucketName, string fileName, CancellationToken cancellationToken = default);
        Task<Result<string, Error>> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default);
    }
}
