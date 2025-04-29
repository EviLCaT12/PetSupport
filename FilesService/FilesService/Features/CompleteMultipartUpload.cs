using FilesService.Endpoints;
using FilesService.Error.Models;
using FilesService.Infrastructure;

namespace FilesService.Features;

public class CompleteMultipartUpload
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/complete-mupltipart", Handler);
        }
    }

    public record PartETagInfo(int PartNumber, string ETag);
    
    private record CompleteMultipartUploadRequest(string UploadId, List<PartETagInfo> Parts);
    
    private static async Task<IResult> Handler(
        Guid key,
        CompleteMultipartUploadRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        
        var response = await fileProvider.CompleteMultipartUploadAsync(
            key,
            request.UploadId,
            request.Parts,
            cancellationToken);
        
        return Results.Ok(new
        {
            key,
            location = response?.Location
        });
    }
}