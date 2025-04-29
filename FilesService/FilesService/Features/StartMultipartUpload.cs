using FilesService.Endpoints;
using FilesService.Error.Models;
using FilesService.Infrastructure;

namespace FilesService.Features;

public class StartMultipartUpload
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart", Handler);
        }
    }

    private record StartMultipartUploadRequest(string ContentType);
    
    private static async Task<IResult> Handler(
        StartMultipartUploadRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        
        var response = await fileProvider.GetInitialMuplipartUploadPresignedUrlAsync(
            request.ContentType,
            key,
            cancellationToken);
        
        return Results.Ok(new
        {
            key,
            uploadId = response?.UploadId
        });
    }
}