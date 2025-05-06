using FileService.Contracts;
using FileService.Contracts.Requests;
using FileService.Contracts.Responses;
using FilesService.Core;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.Jobs;
using FilesService.MongoDataAccess;
using Hangfire;

namespace FilesService.Features;

public class GetFilesPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-urls", Handler);
        }
    }
    
    private static async Task<IResult> Handler(
        GetFilesPresignedUrlRequest request,
        IFileRepository repository,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        List<FileResponse> fileResponses = [];
        var files =  await repository.GetAsync(request.FileIds, cancellationToken);

        foreach (var file in files)
        {
            var presignedUrl = await fileProvider.GetPresignedUrlAsync(file.StoragePath);
            
            fileResponses.Add(new FileResponse(file.Id, presignedUrl));
        }
        
        return Results.Ok(fileResponses);
    }
}