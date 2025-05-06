using FileService.Contracts;
using FilesService.Core.Models;
using FilesService.Endpoints;
using FilesService.Infrastructure;

namespace FilesService.Features;

public class UploadPresignedPartUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/presigned-part", Handler);
        }
    }

    
    
    private static async Task<IResult> Handler(
        string key,
        UploadPresignedPartUrlRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
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