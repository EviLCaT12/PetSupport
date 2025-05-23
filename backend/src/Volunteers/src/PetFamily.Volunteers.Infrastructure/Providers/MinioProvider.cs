using CSharpFunctionalExtensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.Files;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using FileInfo = PetFamily.Core.Files.FileInfo;
using IFileProvider = PetFamily.Core.Providers.IFileProvider;
using Providers_IFileProvider = PetFamily.Core.Providers.IFileProvider;

namespace PetFamily.Volunteer.Infrastructure.Providers;

public class MinioProvider : Providers_IFileProvider
{
    public const int MAX_DEGREE_OF_PARALLEL_FILES = 10;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IMinioClient minioClient, 
        ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> UploadFilesAsync(
        IEnumerable<FileData> fileData, CancellationToken cancellationToken)
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
            

            return Result.Success<ErrorList>();
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

    public async Task<Result<IEnumerable<string>, ErrorList>> RemoveFilesAsync(
        IEnumerable<FileInfo> files, CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLEL_FILES);
        var fileList = files.ToList();

        try
        {
            await CheckBucketsForExistAsync(fileList, cancellationToken);
            await CheckObjectsExisted(fileList, cancellationToken);

            var tasks = fileList.Select(async file =>
                await RemoveObject(file, semaphoreSlim, cancellationToken));
            
            var removeResult = await Task.WhenAll(tasks);
            
            if (removeResult.Any(r => r.IsFailure))
                return removeResult.First().Error;

            var removeFiles = removeResult.Select(p => p.Value).ToList();
            
            return removeFiles;
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to remove files in minio, files amount: {amount}", fileList.Count);
            var error = Error.Failure("file.remove", "Fail to remove file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }

    }

    public async Task<Result<string, ErrorList>> GetFilePresignedUrl(
        FileInfo file, CancellationToken cancellationToken)
    {
        try
        {
            await CheckBucketsForExistAsync([file], cancellationToken);

            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(file.BucketName)
                .WithObject(file.FilePath.Path)
                .WithExpiry(60 * 60 * 24);

            var getUrlResult = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
            
            // Если вернулась пустая стркоа, значит такого файла нет. А если файла нет, то логируем и кидаем соответствующею ошибку
            if (string.IsNullOrEmpty(getUrlResult))
            {
                _logger.LogError("Can't get file presigned url with path {path} in bucket {bucketName}",
                    file.FilePath.Path,
                    file.BucketName);

                var error = Error.Failure("file.get.presigned", "Fail to get file in minio");
                var errorList = new ErrorList([error]);
                return errorList;
            }
            
            return getUrlResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't get file presigned url with path {path} in bucket {bucketName}",
                file.FilePath.Path,
                file.BucketName);

            var error = Error.Failure("file.get.presigned", "Fail to get file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }
    }
    
    

    private async Task<Result<string, ErrorList>> RemoveObject(
        FileInfo fileInfo,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(fileInfo.BucketName)
            .WithObject(fileInfo.FilePath.Path);

        try
        {
            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileInfo.FilePath.Path;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload file with path {path} in minio bucket {bucket}",
                fileInfo.FilePath.Path,
                fileInfo.BucketName);

            var error = Error.Failure("file.remove", "Fail to remove file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    

    private async Task<Result<FilePath, ErrorList>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.Info.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.Info.FilePath.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.Info.FilePath;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload file with path {path} in minio bucket {bucket}",
                fileData.Info.FilePath.Path,
                fileData.Info.BucketName);
            var error = Error.Failure("file.upload", "Fail to upload file in minio");
            var errorList = new ErrorList([error]);
            return errorList;
        }

        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<UnitResult<ErrorList>> CheckObjectsExisted(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        foreach (var file in files)
        {
            try
            {
                var stateObjectArgs = new StatObjectArgs()
                    .WithBucket(file.BucketName)
                    .WithObject(file.FilePath.Path);
        
                var objectStat = await _minioClient.StatObjectAsync(stateObjectArgs, cancellationToken);
                if (objectStat == null)
                    return Result.Success<ErrorList>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cannot get file with path {path}", file.FilePath.Path);
                var error = Error.Failure("file.exist", "Fail to get file`s metadata");
                return new ErrorList([error]);
            }
        }

        return Result.Success<ErrorList>();
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.Info.BucketName)];

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

    private async Task<UnitResult<ErrorList>> CheckBucketsForExistAsync(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..files.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            
            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (bucketExist == false)
            {
                _logger.LogError("Bucket {bucketName} does not exist, cannot delete object", bucketName);
                var error = Error.NotFound("file.remove", 
                    "Bucket {bucketName} does not exist, cannot delete object");
                return new ErrorList([error]);
            }
        }
        
        return Result.Success<ErrorList>();
    }
    
}