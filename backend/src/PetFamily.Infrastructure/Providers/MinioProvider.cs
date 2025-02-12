using System.Reactive;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    public const int MAX_DEGREE_OF_PARALLEL_FILES = 10;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> UploadFiles(FileData fileData,
        CancellationToken cancellationToken = default)
    {
        
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLEL_FILES);

        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(fileData.BucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(fileData.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            List<Task> tasks = [];
            foreach (var file in fileData.Files)
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(file.Stream)
                    .WithObjectSize(file.Stream.Length)
                    .WithObject(file.ObjectName);
                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                var task = _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                semaphoreSlim.Release();

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload file in minio");
            var error = Error.Failure("file.upload", "Fail to upload file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }
        finally
        {
            semaphoreSlim.Release();
        }

        return Result.Success<ErrorList>();
    }
    
}