using FileService.Contracts;
using FilesService.Core;
using FilesService.Core.Models;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.Jobs;
using FilesService.MongoDataAccess;
using Hangfire;
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
    
    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IFileProvider fileProvider,
        IFileRepository fileRepository,
        CancellationToken cancellationToken = default)
    {
        var key = $"{request.ContentType}/{Guid.NewGuid()}";
        var fileId = Guid.NewGuid();
        
        BackgroundJob.Schedule<ConfirmConsistencyJob>(job => 
            job.Execute(fileId, key),
            TimeSpan.FromHours(24));
        
        var presignedUrl = await fileProvider.UploadPresignedUrlAsync(request.ContentType, key);
        if (string.IsNullOrEmpty(presignedUrl))
            return Results.BadRequest(Errors.FileProviderErrors.EmptyPresignedUrl());
        

        var fileData = new FileData
        {
            Id = fileId,
            StoragePath = key,
            FileSize = request.FileSize,
            ContentType = request.ContentType,
            UploadDate = DateTime.UtcNow
        };
        
        await fileRepository.AddAsync(fileData, cancellationToken);
        
        return Results.Ok(new
        {
            key,
            presignedUrl
        });
    }
}