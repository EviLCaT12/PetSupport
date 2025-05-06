using FileService.Contracts;
using FileService.Contracts.Requests;
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
            app.MapPost("files/upload-presigned-part", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var presignedUrl = await fileProvider.UploadPartPresignedUrlAsync(
            request.UploadId,
            request.PartNumber,
            request.Key);
        
        if (string.IsNullOrEmpty(presignedUrl))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(new
        {
            request.Key,
            presignedUrl
        });
    }
}