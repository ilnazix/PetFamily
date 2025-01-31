using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;


namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFilesProvider
    {
        private const int MAX_DEGREE_OF_PARALLELISM = 5; 

        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(
            IMinioClient minioClient,
            ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<string, Error>> UploadFile(
            FileData file,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await CreateBucketIfNotExist(file.BucketName, cancellationToken);

                var path = Guid.NewGuid().ToString() + "_" + file.Path;

                var putObjectargs = new PutObjectArgs()
                    .WithBucket(file.BucketName)
                    .WithStreamData(file.Content)
                    .WithObjectSize(file.Content.Length)
                    .WithObject(path);

                var result = await _minioClient.PutObjectAsync(putObjectargs, cancellationToken);

                return result.ObjectName;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Fail to upload file in MinIO");

                return Error.Failure("file.upload", "Fail to upload file in MinIO");
            }
        }

        public async Task<Result<string, Error>> GetPresignedUrl(
            string bucketName, 
            string fileName, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithExpiry(60 * 60 * 24);

                var presignedUrl = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

                return presignedUrl;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to get presigned url file from MinIO");

                return Error.Failure("file.presigned", "Fail to get presigned url file from MinIO");
            }
        }

        public async Task<UnitResult<Error>> DeleteFile(
            string bucketName, 
            string fileName, 
            CancellationToken cancellationToken)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return UnitResult.Success<Error>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Fail to delete file from MinIO");

                return Error.Failure("file.presigned", "Fail to delete file from MinIO");
            }
        }

        public async Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            IEnumerable<FileData> files, 
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
            var filesList = files.ToList();

            try
            {
                await CreateBucketsIfNotExist(filesList, cancellationToken);

                var tasks = filesList.Select(async file => await PutObject(file, semaphoreSlim, cancellationToken));

                var pathsResult = await Task.WhenAll(tasks);

                if(pathsResult.Any(result => result.IsFailure))
                {
                    return pathsResult.First(result => result.IsFailure).Error;
                }

                return pathsResult
                    .Select(r => r.Value)
                    .ToList()
                    .AsReadOnly();

            }
            catch (Exception ex) {
                _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

                return Error.Failure("file.upload", "Fail to upload files in minio");
            }
        }

        private async Task<Result<string, Error>> PutObject(
            FileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.Content)
                .WithObjectSize(fileData.Content.Length)
                .WithObject(fileData.Path);

            try
            {
                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
                return fileData.Path;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.Path, 
                    fileData.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task CreateBucketsIfNotExist(List<FileData> files, CancellationToken cancellationToken = default)
        {
            HashSet<string> bucketNames = [.. files.Select(f => f.BucketName)];

            foreach (var bucket in bucketNames)
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucket);

                var isExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if(isExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucket);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }
            }
        }

        private async Task CreateBucketIfNotExist(string bucketName, CancellationToken cancellationToken)
        {
            var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}
