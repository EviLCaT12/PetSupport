using Amazon.S3;
using Amazon.S3.Model;
using FileService.Contracts;
using FileService.Contracts.Requests;
using FilesService.Features;
using CompleteMultipartUploadRequest = Amazon.S3.Model.CompleteMultipartUploadRequest;

namespace FilesService.Infrastructure;

public class FileProvider : IFileProvider
{
    private readonly IAmazonS3 _s3Client;
    
    public FileProvider(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<string> UploadPresignedUrlAsync(string contentType, string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            ContentType = contentType,
            Expires = DateTime.Now.AddMinutes(15),
            Key = key,
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.PUT
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }

    public async Task<string> UploadPartPresignedUrlAsync(
        string uploadId,
        int partNumber,
        string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Key = key,
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
        string key,
        CancellationToken cancellationToken = default)
    {
        var request = new InitiateMultipartUploadRequest
        {
            BucketName = "photos",
            ContentType = contentType,
            Key = key
        };
        
        var response = await _s3Client.InitiateMultipartUploadAsync(request, cancellationToken);
        
        return response;
    }

    public async Task<string> DeletePresignedUrlAsync(string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Expires = DateTime.Now.AddMinutes(15),
            Key = key,
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.DELETE
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }

    public async Task<string?> GetPresignedUrlAsync(string storagePath)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "photos",
            Expires = DateTime.Now.AddMinutes(15),
            Key = storagePath,
            Protocol = Protocol.HTTP,
            Verb = HttpVerb.GET
        };
        
        var response = await _s3Client.GetPreSignedURLAsync(request);
        
        return response;
    }
    
    public async Task<CompleteMultipartUploadResponse?> CompleteMultipartUploadAsync(string key,
        string uploadId,
        List<PartETagInfo> parts,
        CancellationToken cancellationToken = default)
    {
        var request = new CompleteMultipartUploadRequest
        {
            BucketName = "photos",
            Key = key,
            UploadId = uploadId,
            PartETags = parts.Select(p => new PartETag { PartNumber = p.PartNumber, ETag = p.ETag }).ToList(),
        };
        
        var response = await _s3Client.CompleteMultipartUploadAsync(request, cancellationToken);
        
        return response;
    }

    public async Task< GetObjectMetadataResponse?> GetObjectMetaDataAsync(string key, 
        CancellationToken cancellationToken = default)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = "photos",
            Key = key
        };
        
        var response = await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
        
        return response;
    }
}