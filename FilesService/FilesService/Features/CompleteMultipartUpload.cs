using FilesService.Core;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.MongoDataAccess;

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

    public record PartETagInfo(int PartNumber, string ETag);
    
    private record CompleteMultipartUploadRequest(string UploadId, List<PartETagInfo> Parts);
    
    private static async Task<IResult> Handler(
        string key,
        CompleteMultipartUploadRequest request,
        IFileRepository repository,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        
        var response = await fileProvider.CompleteMultipartUploadAsync(
            key,
            request.UploadId,
            request.Parts,
            cancellationToken);
        
        var fileMetaData = await fileProvider.GetObjectMetaDataAsync(key, cancellationToken);

        var fileData = new FileData
        {
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