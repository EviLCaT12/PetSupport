using FilesService.Core.Models;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace FilesService.Features;

public static class UploadPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }

    private record UploadPresignedUrlRequest(string ContentType);
    
    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var key = $"{request.ContentType}/{Guid.NewGuid()}";
        
        var presignedUrl = await fileProvider.UploadPresignedUrlAsync(request.ContentType, key);
        if (string.IsNullOrEmpty(presignedUrl))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(new
        {
            key,
            presignedUrl
        });
    }
}