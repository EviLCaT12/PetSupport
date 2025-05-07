using FileService.Contracts.Requests;
using FilesService.Core.Models;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.MongoDataAccess;

namespace FilesService.Features;

public static class DeletePresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("files/delete-presigned", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        DeletePresignedUrlRequest request,
        IFileProvider fileProvider,
        IFileRepository fileRepository,
        CancellationToken cancellationToken = default)
    {
        var files = await fileRepository.GetAsync(request.FileIds, cancellationToken);
        if (files is { Count: 0 })
            return Results.NotFound();
        
        await fileRepository.RemoveManyAsync(request.FileIds, cancellationToken);
        
        var providerResponse = await fileProvider.DeletePresignedUrlAsync(request.Key);
        if (string.IsNullOrEmpty(providerResponse))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        
        return Results.Ok(providerResponse);
    }
}