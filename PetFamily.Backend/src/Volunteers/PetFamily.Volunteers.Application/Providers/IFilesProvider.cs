using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Application.Providers
{
    public interface IFilesProvider
    {
        Task<UnitResult<Error>> DeleteFile(FileMetadata file, CancellationToken cancellationToken);
        Task<Result<string, Error>> GetPresignedUrl(string bucketName, string fileName, CancellationToken cancellationToken = default);
        Task<Result<string, Error>> UploadFile(FileData file, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<string>, Error>> UploadFiles(IEnumerable<FileData> files, CancellationToken cancellationToken = default);
    }
}
