using FilesService.Core.Models;
using FilesService.Endpoints;
using FilesService.Infrastructure;

namespace FilesService.Features;

public static class DeletePresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("files/{key}/presigned", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        string key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var providerResponse = await fileProvider.DeletePresignedUrlAsync(key);
        if (string.IsNullOrEmpty(providerResponse))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(providerResponse);
    }
}