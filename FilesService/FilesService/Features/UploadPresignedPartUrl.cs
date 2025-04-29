using FilesService.Endpoints;
using FilesService.Error.Models;
using FilesService.Infrastructure;

namespace FilesService.Features;

public class UploadPresignedPartUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-part", Handler);
        }
    }

    private record UploadPresignedPartUrlRequest(string UploadId, int PartNumber);
    
    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        
        var presignedUrl = await fileProvider.UploadPartPresignedUrlAsync(
            request.UploadId,
            request.PartNumber,
            key);
        
        if (string.IsNullOrEmpty(presignedUrl))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(new
        {
            key,
            presignedUrl
        });
    }
}