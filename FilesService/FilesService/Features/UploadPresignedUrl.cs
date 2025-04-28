using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Endpoints;

namespace FilesService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(
        string FileName,
        string ContentType,
        long Size);
    
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var key = Guid.NewGuid();

            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "bucket",
                Key = $"photos/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = request.ContentType,
                Protocol = Protocol.HTTP,
                Metadata =
                {
                    ["file-name"] = request.FileName
                }
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            return Results.Ok(new
            {
                key,
                url = presignedUrl
            });
        }
        catch (AmazonS3Exception e)
        {
            return Results.BadRequest($"S3 error generating presigned URL: {e.Message}");
        }
    }
}