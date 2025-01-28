using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using System.Runtime.InteropServices;
using System.Threading;

namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFilesProvider
    {
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
            Stream stream, 
            string bucketName, 
            string fileName, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await CreateBucketIfNotExist(bucketName, cancellationToken);

                var path = Guid.NewGuid().ToString() + "_" + fileName;

                var putObjectargs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
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
