using FilesService.Endpoints;
using FilesService.Error.Models;
using FilesService.Infrastructure;

namespace FilesService.Features;

public static class GetPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{key:guid}/presigned", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        Guid key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var providerResponse = await fileProvider.GetPresignedUrlAsync(key);
        if (string.IsNullOrEmpty(providerResponse))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(providerResponse);
    }
}