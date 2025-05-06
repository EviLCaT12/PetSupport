using FileService.Contracts;
using FilesService.Core;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.Jobs;
using FilesService.MongoDataAccess;
using Hangfire;

namespace FilesService.Features;

public class CompleteMultipartUpload
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/complete-mupltipart", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        string key,
        CompleteMultipartUploadRequest request,
        IFileRepository repository,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.NewGuid();
        
        BackgroundJob.Schedule<ConfirmConsistencyJob>(job => 
            job.Execute(fileId, key),
            TimeSpan.FromHours(24));
        
        var response = await fileProvider.CompleteMultipartUploadAsync(
            key,
            request.UploadId,
            request.Parts,
            cancellationToken);
        
        var fileMetaData = await fileProvider.GetObjectMetaDataAsync(key, cancellationToken);

        var fileData = new FileData
        {
            Id = fileId,
            StoragePath = key,
            FileSize = response.ContentLength,
            ContentType = fileMetaData.Headers.ContentType,
            UploadDate = DateTime.UtcNow
        };
        
        await repository.AddAsync(fileData, cancellationToken);
        
        return Results.Ok(new
        {
            key,
            location = response?.Location
        });
    }
}