using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Features;

namespace FilesService.Infrastructure;

public class FileProvider : IFileProvider
{
    private readonly IAmazonS3 _s3Client;
    
    public FileProvider(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<string> UploadPresignedUrlAsync(string contentType, Guid key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            ContentType = contentType,
            Expires = DateTime.Now.AddMinutes(15),
            Key = key.ToString(),
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.PUT
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }

    public async Task<string> UploadPartPresignedUrlAsync(
        string uploadId,
        int partNumber,
        Guid key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Key = key.ToString(),
            Expires = DateTime.Now.AddMinutes(15),
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.PUT,
            UploadId = uploadId,
            PartNumber = partNumber
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }

    public async Task<InitiateMultipartUploadResponse?> GetInitialMuplipartUploadPresignedUrlAsync(
        string contentType, 
        Guid key,
        CancellationToken cancellationToken = default)
    {
        var request = new InitiateMultipartUploadRequest
        {
            BucketName = "photos",
            ContentType = contentType,
            Key = key.ToString(),
        };
        
        var response = await _s3Client.InitiateMultipartUploadAsync(request, cancellationToken);
        
        return response;
    }

    public async Task<string> DeletePresignedUrlAsync(Guid key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Expires = DateTime.Now.AddMinutes(15),
            Key = key.ToString(),
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.DELETE
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }

    public async Task<string> GetPresignedUrlAsync(Guid key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Expires = DateTime.Now.AddMinutes(15),
            Key = key.ToString(),
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.GET
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }
    
    public async Task< CompleteMultipartUploadResponse?> CompleteMultipartUploadAsync(
        Guid key,
        string uploadId,
        List<CompleteMultipartUpload.PartETagInfo> parts,
        CancellationToken cancellationToken = default)
    {
        var request = new CompleteMultipartUploadRequest
        {
            BucketName = "photos",
            Key = key.ToString(),
            UploadId = uploadId,
            PartETags = parts.Select(p => new PartETag { PartNumber = p.PartNumber, ETag = p.ETag }).ToList(),
        };
        
        var response = await _s3Client.CompleteMultipartUploadAsync(request, cancellationToken);
        
        return response;
    }
}