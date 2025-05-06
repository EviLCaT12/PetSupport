using FileService.Contracts;
using FileService.Contracts.Requests;
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
            app.MapPost("files/complete-mupltipart", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        CompleteMultipartUploadRequest request,
        IFileRepository repository,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.NewGuid();
        
        BackgroundJob.Schedule<ConfirmConsistencyJob>(job => 
            job.Execute(fileId, request.Key),
            TimeSpan.FromHours(24));
        
        var response = await fileProvider.CompleteMultipartUploadAsync(
            request.Key,
            request.UploadId,
            request.Parts,
            cancellationToken);
        
        var fileMetaData = await fileProvider.GetObjectMetaDataAsync(request.Key, cancellationToken);

        var fileData = new FileData
        {
            Id = fileId,
            StoragePath = request.Key,
            FileSize = response.ContentLength,
            ContentType = fileMetaData.Headers.ContentType,
            UploadDate = DateTime.UtcNow
        };
        
        await repository.AddAsync(fileData, cancellationToken);
        
        return Results.Ok(new
        {
            request.Key,
            location = response?.Location
        });
    }
}