using System.Reactive;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
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
    public async Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken = default)
    {
        
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLEL_FILES);
        var fileList = fileData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(fileList, cancellationToken);
            
            var tasks = fileList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));
            
            var pathResult = await Task.WhenAll(tasks);
            if (pathResult.Any(p => p.IsFailure))
                return pathResult.First().Error;
            
            var result = pathResult.Select(p => p.Value).ToList();
            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, 
                "Fail to upload files in minio, files amount: {amount}", fileList.Count);
            
            var error = Error.Failure("file.upload", "Fail to upload file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
            
        }
    }

    private async Task<Result<FilePath, ErrorList>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FilePath.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload file with path {path} in minio bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);
            var error = Error.Failure("file.upload", "Fail to upload file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }

        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
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